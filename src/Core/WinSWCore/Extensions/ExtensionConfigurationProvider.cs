using System;
using System.Collections.Generic;
using System.Text;
using WinSW.Configuration;
using WinSW.Util;

namespace WinSW.Extensions
{
    public class ExtensionConfigurationProvider
    {
        public IDictionary<string, object> FromXML(IWinSWConfiguration configs)
        {
            // Method intentionally left empty.
        }

        public IDictionary<string, object> FromYaml(IWinSWConfiguration configs)
        {
            // Method intentionally left empty.
        }

        public List<ExtensionConfigurations> ExtensionConfigs { get; set; }
    }
}
