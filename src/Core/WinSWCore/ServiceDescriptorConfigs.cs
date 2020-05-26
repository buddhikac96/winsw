using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using WMI;

namespace winsw
{
    public class ServiceDescriptorConfigs
    {
        public BasicConfigs basicConfigs;
        public InstallationConfigs installationConfigs;
        public ExecutableManagementConfigs executableManagementConfigs;
        public ServiceManagementConfigs serviceManagementConfigs;
        public LoggingConfigs loggingConfigs;
        public EnvironmentConfigs environmentConfigs;
    }

    public class BasicConfigs
    {
        public string Id { get; }
        public string Caption { get; }
        public string Description { get; }
        public string Executable { get; }
        public string ExecutablePath { get; }
        public bool HideWindow { get; }
    }

    public class InstallationConfigs
    {
        public bool AllowServiceAcountLogonRight { get; }
        public string? ServiceAccountPassword { get; }
        public string? ServiceAccountUser { get; }
        public List<Native.SC_ACTION> FailureActions { get; }
        public TimeSpan ResetFailureAfter { get; }
    }

    public class ExecutableManagementConfigs
    {
        public string Arguments { get; }
        public string? StartArguments { get; }
        public string? StopExecutable { get; }
        public string? StopArguments { get; }
        public string WorkingDirectory { get; }
        public ProcessPriorityClass Priority { get; }
        public TimeSpan StopTimeout { get; }
        public bool StopParentProcessFirst { get; }
    }

    public class ServiceManagementConfigs
    {
        public StartMode StartMode { get; }
        public string[] ServiceDependencies { get; }
        public TimeSpan WaitHint { get; }
        public TimeSpan SleepTime { get; }
        public bool Interactive { get; }
    }

    public class LoggingConfigs
    {
        public string LogDirectory { get; }
        // TODO: replace by enum
        public string LogMode { get; }
    }

    public class EnvironmentConfigs
    {
        public List<Download> Downloads { get; }
        public Dictionary<string, string> EnvironmentVariables { get; }
    }
}
