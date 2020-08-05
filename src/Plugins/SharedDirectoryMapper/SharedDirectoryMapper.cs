﻿using System.Collections.Generic;
using log4net;
using WinSW.Configuration;
using WinSW.Extensions;
using WinSW.Util;

namespace WinSW.Plugins.SharedDirectoryMapper
{
    public class SharedDirectoryMapper : AbstractWinSWExtension
    {
        private readonly SharedDirectoryMappingHelper _mapper = new SharedDirectoryMappingHelper();
        private readonly List<SharedDirectoryMapperConfig> _entries = new List<SharedDirectoryMapperConfig>();

        public override string DisplayName => "Shared Directory Mapper";

        private static readonly ILog Logger = LogManager.GetLogger(typeof(SharedDirectoryMapper));

        public SharedDirectoryMapper()
        {
        }

        public SharedDirectoryMapper(bool enableMapping, string directoryUNC, string driveLabel)
        {
            SharedDirectoryMapperConfig config = new SharedDirectoryMapperConfig(enableMapping, driveLabel, directoryUNC);
            this._entries.Add(config);
        }

        public override void Configure(IWinSWConfiguration descriptor, object settings)
        {
            var configObject = new ObjectQuery(settings);

            var maps = configObject.On("mapping").ToList<object>();

            foreach (var map in maps)
            {
                var mapObject = new ObjectQuery(map);
                var enable = mapObject.On("enabled").ToBoolean();
                var label = mapObject.On("label").ToString();
                var uncpath = mapObject.On("uncpath").ToString();

                var config = new SharedDirectoryMapperConfig(enable, label, uncpath);
                this._entries.Add(config);
            }
        }

        public override void OnWrapperStarted()
        {
            foreach (SharedDirectoryMapperConfig config in this._entries)
            {
                if (config.EnableMapping)
                {
                    Logger.Info(this.DisplayName + ": Mapping shared directory " + config.UNCPath + " to " + config.Label);
                    try
                    {
                        this._mapper.MapDirectory(config.Label, config.UNCPath);
                    }
                    catch (MapperException ex)
                    {
                        this.HandleMappingError(config, ex);
                    }
                }
                else
                {
                    Logger.Warn(this.DisplayName + ": Mapping of " + config.Label + " is disabled");
                }
            }
        }

        public override void BeforeWrapperStopped()
        {
            foreach (SharedDirectoryMapperConfig config in this._entries)
            {
                if (config.EnableMapping)
                {
                    try
                    {
                        this._mapper.UnmapDirectory(config.Label);
                    }
                    catch (MapperException ex)
                    {
                        this.HandleMappingError(config, ex);
                    }
                }
            }
        }

        private void HandleMappingError(SharedDirectoryMapperConfig config, MapperException ex)
        {
            Logger.Error("Mapping of " + config.Label + " failed. STDOUT: " + ex.Process.StandardOutput.ReadToEnd()
                + " \r\nSTDERR: " + ex.Process.StandardError.ReadToEnd(), ex);
            throw new ExtensionException(this.Descriptor.Id, this.DisplayName + ": Mapping of " + config.Label + "failed", ex);
        }
    }
}
