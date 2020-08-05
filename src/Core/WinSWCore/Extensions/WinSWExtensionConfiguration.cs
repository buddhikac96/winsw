namespace WinSW.Extensions
{
    public class WinSWExtensionConfiguration
    {
        public string ID { get; set; }

        public bool Enabled { get; set; }

        public string ClassName { get; set; }

        public object Settings { get; set; }

        public WinSWExtensionConfiguration(string id, bool enabled, string className, object settings)
        {
            this.ID = id;
            this.Enabled = enabled;
            this.ClassName = className;
            this.Settings = settings;
        }
    }
}
