﻿using CommandLine;
using System;
using WMI;

namespace winsw.CLI
{
    [Verb("testwait", HelpText = "starts the service and waits until a key is pressed then stops the service")]
    public class TestWaitOption : CliOption
    {
        public override void Run(ServiceDescriptor descriptor, Win32Services svcs, Win32Service? svc)
        {
            if (!Program.elevated)
            {
                Elevate();
                return;
            }

            var arguments = Parser.Default.FormatCommandLine(Program.cliOption).Split(' ');

            WrapperService wsvc = new WrapperService(descriptor);
            wsvc.RaiseOnStart(arguments);
            Console.WriteLine("Press any key to stop the service...");
            _ = Console.Read();
            wsvc.RaiseOnStop();
        }
    }
}
