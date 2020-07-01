using System;
using System.IO;
using winsw.Configuration;
using YamlDotNet.Serialization;

namespace winsw
{
    public class ServiceDescriptorYaml
    {
        public YamlConfiguration configurations;

        public static DefaultWinSWSettings Defaults { get; } = new DefaultWinSWSettings();

        readonly DirectoryInfo d = new DirectoryInfo(Path.GetDirectoryName(Defaults.ExecutablePath));

        public ServiceDescriptorYaml()
        {
            var baseName = Defaults.BaseName;
            var basePath = Defaults.BasePath;

            while (true)
            {
                if (File.Exists(Path.Combine(d.FullName, baseName + ".yml")))
                    break;

                if (d.Parent is null)
                    throw new FileNotFoundException("Unable to locate " + baseName + ".yml file within executable directory or any parents");

                d = d.Parent;
            }

            Deserialize(basePath + ".yml");
            SetEnvs();
        }

        //ServiceDescriptorYaml from Configuration File Path
        public ServiceDescriptorYaml(string configFilePath)
        {
            Deserialize(configFilePath);
            SetEnvs();
        }

        private void SetEnvs()
        {
            Environment.SetEnvironmentVariable("BASE", d.FullName);
            Environment.SetEnvironmentVariable("SERVICE_ID", configurations.Id);
            Environment.SetEnvironmentVariable(WinSWSystem.ENVVAR_NAME_EXECUTABLE_PATH, Defaults.ExecutablePath);
            Environment.SetEnvironmentVariable(WinSWSystem.ENVVAR_NAME_SERVICE_ID, configurations.Id);
        }

        //Deserialize YAML File
        private void Deserialize(string ymlFilePath)
        {
            try
            {
                using (var reader = new StreamReader(ymlFilePath))
                {
                    var file = reader.ReadToEnd();
                    var deserializer = new DeserializerBuilder().Build();

                    configurations = deserializer.Deserialize<YamlConfiguration>(file);
                }
            }
            catch (FileNotFoundException)
            {
                throw new FileNotFoundException(ymlFilePath + "is not exist");
            }
        }


        public ServiceDescriptorYaml(YamlConfiguration configs)
        {
            configurations = configs;
        }

        public static ServiceDescriptorYaml FromYaml(string yaml)
        {
            var deserializer = new DeserializerBuilder().Build();
            var configs = deserializer.Deserialize<YamlConfiguration>(yaml);
            return new ServiceDescriptorYaml(configs);
        }

    }
}
