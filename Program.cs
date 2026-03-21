using System;
using System.IO;
using System.Linq;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace VibeFetch
{
    class Program
    {
        static readonly string C1 = "\u001b[38;2;116;199;236m";
        static readonly string C2 = "\u001b[38;2;203;166;247m";
        static readonly string C3 = "\u001b[38;2;205;214;244m";
        static readonly string C4 = "\u001b[38;2;108;112;134m";
        static readonly string R = "\u001b[0m";

        static void Main()
        {
            EnableAnsi();
            SafeClear();

            Header();

            Print("os", GetOS());
            Print("kernel", GetKernel());
            Print("uptime", GetUptime());

            var ram = GetRam();
            Print("ram", $"{ram.used}GB / {ram.total}GB");

            var disk = GetDisk();
            Print("disk", $"{disk.used}GB / {disk.total}GB");

            Colors();
        }

        // ---------------- UI ----------------

        static void Header()
        {
            Console.WriteLine($"{C1}  oooooo     oooo");
            Console.WriteLine($"{C1}   `888.     .8'   {C2}{Environment.UserName}{C3}@{C2}{Environment.MachineName}{R}");
            Console.WriteLine($"{C1}    `888.   .8'    {C4}--------------------------{R}");
        }

        static void Print(string key, string value)
        {
            Console.WriteLine($"                   {C1}{key,-7}{C3}➜  {value}");
        }

        static void Colors()
        {
            Console.WriteLine($"\n                   " +
                "\u001b[48;2;245;224;220m  " +
                "\u001b[48;2;242;205;205m  " +
                "\u001b[48;2;203;166;247m  " +
                "\u001b[48;2;116;199;236m  " + R);
        }

        // ---------------- SYSTEM ----------------

        static string GetOS()
        {
            try
            {
                if (File.Exists("/etc/os-release"))
                {
                    var l = File.ReadAllLines("/etc/os-release")
                        .FirstOrDefault(x => x.StartsWith("PRETTY_NAME="));
                    if (l != null) return l.Split('"')[1];
                }
            } catch {}

            return RuntimeInformation.OSDescription;
        }

        static string GetKernel()
        {
            return TryRun("uname", "-r") ??
                   RuntimeInformation.OSDescription;
        }

        static string GetUptime()
        {
            var linux = TryRun("uptime", "-p");
            if (!string.IsNullOrWhiteSpace(linux))
                return linux.Replace("up ", "");

            try
            {
                var t = TimeSpan.FromMilliseconds(Environment.TickCount64);
                return $"{(int)t.TotalHours}h {t.Minutes}m";
            } catch {}

            return "unknown";
        }

        static (long used, long total) GetRam()
        {
            // Linux
            try
            {
                if (File.Exists("/proc/meminfo"))
                {
                    var l = File.ReadAllLines("/proc/meminfo");
                    long t = Parse(l, "MemTotal");
                    long a = Parse(l, "MemAvailable");
                    return (t - a, t);
                }
            } catch {}

            // Windows fallback
            var outp = TryRun("wmic", "OS get FreePhysicalMemory,TotalVisibleMemorySize /Value");
            if (!string.IsNullOrEmpty(outp))
            {
                long total = 0, free = 0;

                foreach (var line in outp.Split('\n'))
                {
                    if (line.Contains("TotalVisibleMemorySize"))
                        total = Extract(line) / 1024 / 1024;
                    if (line.Contains("FreePhysicalMemory"))
                        free = Extract(line) / 1024 / 1024;
                }

                return (total - free, total);
            }

            return (0, 0);
        }

        static (long used, long total) GetDisk()
        {
            try
            {
                var root = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "C:\\" : "/";
                var d = new DriveInfo(root);

                long total = d.TotalSize / 1073741824;
                long used = (d.TotalSize - d.AvailableFreeSpace) / 1073741824;

                return (used, total);
            } catch { return (0, 0); }
        }

        // ---------------- UTILS ----------------

        static string TryRun(string cmd, string args)
        {
            try
            {
                var p = Process.Start(new ProcessStartInfo
                {
                    FileName = cmd,
                    Arguments = args,
                    RedirectStandardOutput = true,
                    UseShellExecute = false
                });

                if (p == null) return null;

                string o = p.StandardOutput.ReadToEnd();
                p.WaitForExit(2000);

                return o.Trim();
            }
            catch { return null; }
        }

        static long Parse(string[] lines, string key)
        {
            var l = lines.FirstOrDefault(x => x.StartsWith(key));
            if (l == null) return 0;

            return long.Parse(l.Split(' ', StringSplitOptions.RemoveEmptyEntries)[1]) / 1024 / 1024;
        }

        static long Extract(string line)
        {
            var parts = line.Split('=');
            return parts.Length > 1 ? long.Parse(parts[1].Trim()) : 0;
        }

        static void EnableAnsi()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                try
                {
                    var h = GetStdHandle(-11);
                    GetConsoleMode(h, out uint m);
                    SetConsoleMode(h, m | 0x0004);
                } catch {}
            }
        }

        static void SafeClear()
        {
            try { Console.Clear(); } catch {}
        }

        [DllImport("kernel32.dll")] static extern IntPtr GetStdHandle(int n);
        [DllImport("kernel32.dll")] static extern bool GetConsoleMode(IntPtr h, out uint m);
        [DllImport("kernel32.dll")] static extern bool SetConsoleMode(IntPtr h, uint m);
    }
}
