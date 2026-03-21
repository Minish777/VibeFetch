using System;
using System.IO;
using System.Linq;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace VibeFetch
{
    class Program
    {
        // Catppuccin Mocha palette
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

            PrintHeader(user, host);
            PrintSystemInfo();
            PrintColors();
        }

        // ---------------- UI ----------------

        static void PrintHeader(string user, string host)
        {
            Console.WriteLine($"{Sapphire}  oooooo     oooo");
            Console.WriteLine($"{Sapphire}   `888.     .8'   {Mauve}{user}{Text}@{Mauve}{host}{Reset}");
            Console.WriteLine($"{Sapphire}    `888.   .8'    {Overlay}--------------------------{Reset}");
        }

        static void PrintSystemInfo()
        {
            WriteLine("os", GetOS());
            WriteLine("kernel", GetKernel());
            WriteLine("uptime", GetUptime());

            var (usedRam, totalRam) = GetRam();
            WriteLine("ram", $"{usedRam}GB / {totalRam}GB");

            var (usedDisk, totalDisk) = GetDisk();
            WriteLine("disk", $"{usedDisk}GB / {totalDisk}GB");
        }

        static void WriteLine(string key, string value)
        {
            Console.WriteLine($"                   {Sapphire}{key,-7}{Text}➜  {value}");
        }

        static void PrintColors()
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
            catch
            {
                return "Unknown";
            }
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
                {
                    var result = Run("uptime", "-p").Replace("up ", "");
                    return string.IsNullOrWhiteSpace(result) ? "just started" : result;
                }

                var t = TimeSpan.FromMilliseconds(Environment.TickCount64);
                return $"{(int)t.TotalHours}h {t.Minutes}m";
            }
            catch
            {
                return "Unknown";
            }
        }

        static (long used, long total) GetRam()
        {
            try
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    var ci = new Microsoft.VisualBasic.Devices.ComputerInfo();

                    long total = (long)ci.TotalPhysicalMemory / 1073741824;
                    long available = (long)ci.AvailablePhysicalMemory / 1073741824;

                    return (total - available, total);
                }

                var lines = File.ReadAllLines("/proc/meminfo");

                long totalMem = ParseMeminfo(lines, "MemTotal");
                long availableMem = ParseMeminfo(lines, "MemAvailable");

                return (totalMem - availableMem, totalMem);
            }
            catch
            {
                return (0, 0);
            }
        }

        static long ParseMeminfo(string[] lines, string key)
        {
            var line = lines.FirstOrDefault(l => l.StartsWith(key));
            if (line == null) return 0;

            var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            return long.Parse(parts[1]) / 1024 / 1024;
        }

        static (long used, long total) GetDisk()
        {
            try
            {
                string root = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "C:\\" : "/";
                var d = new DriveInfo(root);

                long total = d.TotalSize / 1073741824;
                long used = (d.TotalSize - d.AvailableFreeSpace) / 1073741824;

                return (used, total);
            }
            catch
            {
                return (0, 0);
            }
        }

        // ---------------- UTILS ----------------

        static string Run(string command, string args)
        {
            try
            {
                var p = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = command,
                        Arguments = args,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };

                p.Start();
                string output = p.StandardOutput.ReadToEnd();
                p.WaitForExit();

                return output.Trim();
            }
            catch
            {
                return "";
            }
        }

        static void InitConsole()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                try
                {
                    var handle = GetStdHandle(-11);
                    if (GetConsoleMode(handle, out uint mode))
                    {
                        SetConsoleMode(handle, mode | 0x0004 | 0x0008);
                    }
                }
                catch { }
            }
        }

        // Windows ANSI fix
        [DllImport("kernel32.dll")] static extern IntPtr GetStdHandle(int nStdHandle);
        [DllImport("kernel32.dll")] static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);
        [DllImport("kernel32.dll")] static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);
    }
}
