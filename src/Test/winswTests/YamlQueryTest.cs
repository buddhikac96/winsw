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
    }

    
}
