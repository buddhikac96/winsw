using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml;
using winsw.Configuration;
using winsw.Native;
using WMI;
using YamlDotNet.Serialization;
using System.Collections;

namespace winsw
{
    public class ServiceDescriptorYAML : IWinSWConfiguration
    {
        protected readonly string dom;

        public static DefaultWinSWSettings Defaults { get; } = new DefaultWinSWSettings();

        public string BasePath { get; set; }

        public string BaseName { get; set; }

        private readonly ServiceDescriptorConfigs configurations;

        public virtual string ExecutablePath => Defaults.ExecutablePath;


        public ServiceDescriptorYAML()
        {
            string p = ExecutablePath;
            string baseName = Path.GetFileNameWithoutExtension(p);

            if (baseName.EndsWith(".vshost"))
                baseName = baseName.Substring(0, baseName.Length - 7);

            DirectoryInfo d = new DirectoryInfo(Path.GetDirectoryName(p));
            while (true)
            {
                if (File.Exists(Path.Combine(d.FullName, baseName + ".yaml")))
                    break;

                if (d.Parent == null)
                    throw new FileNotFoundException("Unable to locate " + baseName + ".yaml file within executable directory or any parents");

                d = d.Parent;
            }

            BaseName = baseName + ".yaml";
            BasePath = Path.Combine(d.FullName, BaseName);

            //load yaml file
            using (var reader = new StreamReader(BasePath))
            {
                dom = reader.ReadToEnd();
            }

            //Initialize the Deserailizer
            var deserializer = new DeserializerBuilder().Build();

            //deserialize the yaml
            configurations = deserializer.Deserialize<ServiceDescriptorConfigs>(dom);

            // register the base directory as environment variable so that future expansions can refer to this.
            Environment.SetEnvironmentVariable("BASE", d.FullName);

            // ditto for ID
            Environment.SetEnvironmentVariable("SERVICE_ID", configurations.basicConfigs.Id);

            // New name
            Environment.SetEnvironmentVariable(WinSWSystem.ENVVAR_NAME_EXECUTABLE_PATH, ExecutablePath);

            // Also inject system environment variables
            Environment.SetEnvironmentVariable(WinSWSystem.ENVVAR_NAME_SERVICE_ID, configurations.basicConfigs.Id);
        }





        public string Id => configurations.basicConfigs.Id;

        public string Caption => configurations.basicConfigs.Caption;

        public string Description => configurations.basicConfigs.Description;

        public string Executable => configurations.basicConfigs.Executable;

        public bool HideWindow => throw new NotImplementedException();

        public bool AllowServiceAcountLogonRight => throw new NotImplementedException();

        public string? ServiceAccountPassword => throw new NotImplementedException();

        public string? ServiceAccountUser => throw new NotImplementedException();

        public List<SC_ACTION> FailureActions => throw new NotImplementedException();

        public TimeSpan ResetFailureAfter => throw new NotImplementedException();

        public string Arguments => throw new NotImplementedException();

        public string? StartArguments => throw new NotImplementedException();

        public string? StopExecutable => throw new NotImplementedException();

        public string? StopArguments => throw new NotImplementedException();

        public string WorkingDirectory => throw new NotImplementedException();

        public ProcessPriorityClass Priority => throw new NotImplementedException();

        public TimeSpan StopTimeout => throw new NotImplementedException();

        public bool StopParentProcessFirst => throw new NotImplementedException();

        public StartMode StartMode => throw new NotImplementedException();

        public string[] ServiceDependencies => throw new NotImplementedException();

        public TimeSpan WaitHint => throw new NotImplementedException();

        public TimeSpan SleepTime => throw new NotImplementedException();

        public bool Interactive => throw new NotImplementedException();

        public string LogDirectory => throw new NotImplementedException();

        public string LogMode => throw new NotImplementedException();

        public List<Download> Downloads => throw new NotImplementedException();

        public Dictionary<string, string> EnvironmentVariables => throw new NotImplementedException();

        public bool BeepOnShutdown => throw new NotImplementedException();

        public XmlNode? ExtensionsConfiguration => throw new NotImplementedException();

        
    }
}
