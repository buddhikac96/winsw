using NUnit.Framework;
using WinSW;
using WinSW.Util;

namespace winswTests
{
    public class YamlQueryTest
    {
        private readonly string YamlPluginConfig = @"plugins:
    RunawayProcessKiller:
        id: killOnStartup
        enabled: true
        classname: winsw.Plugins.RunawayProcessKiller.RunawayProcessKillerExtension

    SharedDirectoryMapper:
        id: mapNetworDirs
        enabled: true
        classname: winsw.Plugins.SharedDirectoryMapper.SharedDirectoryMapper
    configs:
        -
            id: killOnStartup
            settings:
                pidfile: '%BASE%\pid.txt'
                stopTimeOut: 5000
                StopParentFirst: false
        -
            id: mapNetworDirs
            settings: 
                mapping:
                    -
                        map:
                            enabled: false
                            label: N 
                            uncpath: \\UNC
                    -
                        map:
                            enabled: false
                            label: M
                            uncpath: \\UNC2";

        [Test]
        public void Get_string_element_test()
        {
            var configs = ServiceDescriptorYaml.FromYaml(YamlPluginConfig).Configurations.Plugin;
            var data = new YamlQuery(configs);

            Assert.AreEqual("killOnStartup", data.On("RunawayProcessKiller").Get("id").ToString());
            Assert.AreEqual("mapNetworDirs", data.On("SharedDirectoryMapper").Get("id").ToString());
            Assert.AreEqual(true, data.On("SharedDirectoryMapper").Get("enabled").ToBoolean());
            Assert.AreEqual("winsw.Plugins.SharedDirectoryMapper.SharedDirectoryMapper", data.On("SharedDirectoryMapper").Get("classname").ToString());
        }

        [Test]
        public void Get_data_inside_complex_objects()
        {
            var configs = ServiceDescriptorYaml.FromYaml(YamlPluginConfig).Configurations.Plugin;
            var data = new YamlQuery(configs);

            var killOnStartupConfig = data.On("configs").ToList<object>()[0];
            var data1 = new YamlQuery(killOnStartupConfig);

            var pidfile = data1.On("settings").Get("pidfile").ToString();

            Assert.AreEqual(@"%BASE%\pid.txt", pidfile);
        }

        [Test]
        public void Get_data_from_list()
        {
            var configs = ServiceDescriptorYaml.FromYaml(YamlPluginConfig).Configurations.Plugin;
            var data = new YamlQuery(configs);

            var value = data.On("configs").At(0).Get("settings").Get("pidfile").ToString();

            Assert.AreEqual(@"%BASE%\pid.txt", value);
        }

        [Test]
        public void Go_deep()
        {
            var configs = ServiceDescriptorYaml.FromYaml(YamlPluginConfig).Configurations.Plugin;
            var data = new YamlQuery(configs);

            var value = data.On("configs").At(1).Get("settings").Get("mapping").At(0).Get("map").Get("label").ToString();
            
            Assert.AreEqual("N", value);
        }
    }
}
