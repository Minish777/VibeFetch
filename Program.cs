using System;
using System.IO;
using System.Linq;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace VibeFetch
{
    class Program
    {
        static readonly string Mauve = "\u001b[38;2;203;166;247m";
        static readonly string Sapphire = "\u001b[38;2;116;199;236m";
        static readonly string Text = "\u001b[38;2;205;214;244m";
        static readonly string Overlay = "\u001b[38;2;108;112;134m";
        static readonly string Reset = "\u001b[0m";

        static void Main()
        {
            InitConsole();
            Console.Clear();

            string user = Environment.UserName;
            string host = Environment.MachineName;

            Header(user, host);

            Line("os", GetOS());
            Line("kernel", GetKernel());
            Line("uptime", GetUptime());

            var (ramUsed, ramTotal) = GetRam();
            Line("ram", $"{ramUsed}GB / {ramTotal}GB");

            var (diskUsed, diskTotal) = GetDisk();
            Line("disk", $"{diskUsed}GB / {diskTotal}GB");

            Colors();
        }

        static void Header(string user, string host)
        {
            Console.WriteLine($"{Sapphire}  oooooo     oooo");
            Console.WriteLine($"{Sapphire}   `888.     .8'   {Mauve}{user}{Text}@{Mauve}{host}{Reset}");
            Console.WriteLine($"{Sapphire}    `888.   .8'    {Overlay}--------------------------{Reset}");
        }

        static void Line(string key, string value)
        {
            Console.WriteLine($"                   {Sapphire}{key,-7}{Text}➜  {value}");
        }

        static void Colors()
        {
            Console.WriteLine($"\n                   " +
                "\u001b[48;2;245;224;220m  " +
                "\u001b[48;2;242;205;205m  " +
                "\u001b[48;2;203;166;247m  " +
                "\u001b[48;2;116;199;236m  " +
                Reset);
        }

        // ---------------- SYSTEM ----------------

        static string GetOS()
        {
            try
            {
                if (File.Exists("/etc/os-release"))
                {
                    var line = File.ReadAllLines("/etc/os-release")
                        .FirstOrDefault(l => l.StartsWith("PRETTY_NAME="));

                    if (line != null)
                        return line.Split('"')[1];
                }

                return RuntimeInformation.OSDescription;
            }
            catch { return "Unknown"; }
        }

        static string GetKernel()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                return Run("uname", "-r");

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return Environment.OSVersion.VersionString;

            return "Unknown";
        }

        static string GetUptime()
        {
            try
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                    return Run("uptime", "-p").Replace("up ", "");

                var t = TimeSpan.FromMilliseconds(Environment.TickCount64);
                return $"{(int)t.TotalHours}h {t.Minutes}m";
            }
            catch { return "Unknown"; }
        }

        static (long, long) GetRam()
        {
            try
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    string output = Run("wmic", "OS get FreePhysicalMemory,TotalVisibleMemorySize /Value");

                    long total = 0;
                    long free = 0;

                    foreach (var line in output.Split('\n'))
                    {
                        if (line.StartsWith("TotalVisibleMemorySize="))
                            total = long.Parse(line.Split('=')[1]) / 1024 / 1024;

                        if (line.StartsWith("FreePhysicalMemory="))
                            free = long.Parse(line.Split('=')[1]) / 1024 / 1024;
                    }

                    return (total - free, total);
                }

                var lines = File.ReadAllLines("/proc/meminfo");

                long totalMem = Parse(lines, "MemTotal");
                long freeMem = Parse(lines, "MemAvailable");

                return (totalMem - freeMem, totalMem);
            }
            catch { return (0, 0); }
        }

        static long Parse(string[] lines, string key)
        {
            var line = lines.FirstOrDefault(l => l.StartsWith(key));
            if (line == null) return 0;

            return long.Parse(line.Split(' ', StringSplitOptions.RemoveEmptyEntries)[1]) / 1024 / 1024;
        }

        static (long, long) GetDisk()
        {
            try
            {
                string root = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "C:\\" : "/";
                var d = new DriveInfo(root);

                long total = d.TotalSize / 1073741824;
                long used = (d.TotalSize - d.AvailableFreeSpace) / 1073741824;

                return (used, total);
            }
            catch { return (0, 0); }
        }

        // ---------------- UTILS ----------------

        static string Run(string cmd, string args)
        {
            try
            {
                var p = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = cmd,
                        Arguments = args,
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };

                p.Start();
                string output = p.StandardOutput.ReadToEnd();
                p.WaitForExit();

                return output.Trim();
            }
            catch { return ""; }
        }

        static void InitConsole()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                try
                {
                    var h = GetStdHandle(-11);
                    if (GetConsoleMode(h, out uint mode))
                        SetConsoleMode(h, mode | 0x0004 | 0x0008);
                }
                catch { }
            }
        }

        [DllImport("kernel32.dll")] static extern IntPtr GetStdHandle(int n);
        [DllImport("kernel32.dll")] static extern bool GetConsoleMode(IntPtr h, out uint m);
        [DllImport("kernel32.dll")] static extern bool SetConsoleMode(IntPtr h, uint m);
    }
}
