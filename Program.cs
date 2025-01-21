using Microsoft.VisualBasic.Devices;
using System.Diagnostics;

namespace envmonitor
{
    struct ResourceInfo
    {
        public string OSFullName;
        public string OSVersion;
        public string OSPlatform;
        public float CPUUtilization;
        public ulong TotalPhysicalMemory;
        public ulong AvailablePhysicalMemory;
        public ulong TotalVirtualMemory;
        public ulong AvailableVirtualMemory;
    }
    class Program
    {
        static bool stopMonitoring = false;
        static void Main(string[] args)
        {
            Thread keyListenerThread = new Thread(KeyListener);
            keyListenerThread.Start();

            PerformanceCounter cpuCounter = new("Processor Information", "% Processor Time", "_Total"); 

            while(!stopMonitoring) 
            {
                ResourceInfo resourceInfo = GetResourceInfo(cpuCounter);

                Console.Clear();
                Console.WriteLine("Operating System: " + resourceInfo.OSFullName);
                Console.WriteLine("OS Version: " + resourceInfo.OSVersion);
                Console.WriteLine("Platform: " + resourceInfo.OSPlatform);
                Console.WriteLine("CPU Utilization (%):\t" + GenerateProgressBar((int)resourceInfo.CPUUtilization, 100, 100, "%"));
                Console.WriteLine("Physical Memory (MiB):\t" + GenerateProgressBar((int)resourceInfo.TotalPhysicalMemory - (int)resourceInfo.AvailablePhysicalMemory, (int)resourceInfo.TotalPhysicalMemory, 100, "MiB"));
                Console.WriteLine("Virtual Memory (MiB):\t" + GenerateProgressBar((int)resourceInfo.TotalVirtualMemory - (int)resourceInfo.AvailableVirtualMemory, (int)resourceInfo.TotalVirtualMemory, 100, "MiB"));
                Console.WriteLine("\nPress 'X' to stop monitoring the environment.");

                Thread.Sleep(1000);
            }

            keyListenerThread.Join();
            Console.WriteLine("Monitoring stopped.");
        }

        static ResourceInfo GetResourceInfo(PerformanceCounter cpuPerformanceCounter)
        {
            Console.WriteLine("Getting resource information...");
            ComputerInfo computerInfo = new();
            ResourceInfo resourceInfo = new()
            {
                OSFullName = computerInfo.OSFullName,
                OSVersion = computerInfo.OSVersion,
                OSPlatform = computerInfo.OSPlatform,
                CPUUtilization = cpuPerformanceCounter.NextValue(),
                TotalPhysicalMemory = computerInfo.TotalPhysicalMemory / 1048576,
                AvailablePhysicalMemory = computerInfo.AvailablePhysicalMemory / 1048576,
                TotalVirtualMemory = computerInfo.TotalVirtualMemory / 1048576,
                AvailableVirtualMemory = computerInfo.AvailableVirtualMemory / 1048576
            };

            return resourceInfo;
        }

        static void KeyListener()
        {
            while (!stopMonitoring)
            {
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(intercept: true).Key;
                    if (key ==  ConsoleKey.X)
                    {
                        stopMonitoring = true;
                    }
                }
                Thread.Sleep(100);
            }
        }
    
        static String GenerateProgressBar(int value, int total, int length, String label)
        {
            int progress = (int)((double)value / total * length);
            return value.ToString() + " " + label + " [" + new String('#', progress) + new String('-', length - progress) + "] " + total.ToString() + " " + label;
        }
    }
}
