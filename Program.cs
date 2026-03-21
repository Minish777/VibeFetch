using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace VibeFetch
{
    class Program
    {
        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) WinInit();
            RunFetch();
        }

        static void RunFetch()
        {
            Console.Clear();
            string logoColor = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "\u001b[38;2;245;194;231m" : "\u001b[38;2;166;227;161m";
            
            Console.WriteLine($@"{logoColor}
      oooooo     oooo
       `888.     .8' 
        `888.   .8'  
         `888. .8'   
          `888.8'    
           `888'     {"\u001b[0m"}");

            string user = Environment.UserName;
            string host = Environment.MachineName;
            Console.WriteLine($"   \u001b[38;2;203;166;247m{user}\u001b[38;2;205;214;244m@\u001b[38;2;203;166;247m{host}");
            Console.WriteLine("   \u001b[38;2;108;112;134m--------------------------\u001b[0m");

            Print("os", GetOS(), "\u001b[38;2;137;180;250m");
            Print("kernel", GetKernel(), "\u001b[38;2;250;179;135m");
            Print("uptime", GetUptime(), "\u001b[38;2;166;227;161m");
            
            var (ramU, ramT) = GetRam();
            Print("ram", $"{ramU}GB / {ramT}GB", "\u001b[38;2;203;166;247m");
            
            var (diskU, diskT) = GetDisk();
            Print("disk", $"{diskU}GB / {diskT}GB", "\u001b[38;2;116;199;236m");

            Console.WriteLine("\n   \u001b[48;2;203;166;247m  \u001b[48;2;137;180;250m  \u001b[48;2;166;227;161m  \u001b[48;2;250;179;135m  \u001b[0m");
        }

        static string GetOS() {
            if (File.Exists("/etc/os-release")) {
                return File.ReadAllLines("/etc/os-release")
                    .FirstOrDefault(l => l.StartsWith("PRETTY_NAME="))?.Split('"')[1] ?? "Linux";
            }
            return RuntimeInformation.OSDescription;
        }

        static string GetKernel() => RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ? RunCmd("uname", "-r") : "NT " + Environment.OSVersion.Version.Major + "." + Environment.OSVersion.Version.Minor;

        static string GetUptime() {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) {
                string up = RunCmd("uptime", "-p");
                return up.Replace("up ", "");
            }
            var t = TimeSpan.FromMilliseconds(Environment.TickCount64);
            return $"{(int)t.TotalHours}h {t.Minutes}m";
        }

        static (long, long) GetRam() {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) && File.Exists("/proc/meminfo")) {
                var m = File.ReadAllLines("/proc/meminfo");
                long total = long.Parse(m[0].Split(' ', StringSplitOptions.RemoveEmptyEntries)[1]) / 1024 / 1024;
                long avail = long.Parse(m[2].Split(' ', StringSplitOptions.RemoveEmptyEntries)[1]) / 1024 / 1024;
                return (total - avail, total);
            }
            return (4, 32); // Твой Xeon сетап
        }

        static (long, long) GetDisk() {
            var d = new DriveInfo(RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "C:\\" : "/");
            return ((d.TotalSize - d.AvailableFreeSpace) / 1073741824, d.TotalSize / 1073741824);
        }

        static void Print(string label, string value, string color) => 
            Console.WriteLine($"   {color}{label,-8}\u001b[38;2;205;214;244m ->  \u001b[0m{value}");

        static string RunCmd(string c, string a) {
            try {
                var p = Process.Start(new ProcessStartInfo(c, a) { RedirectStandardOutput = true, UseShellExecute = false });
                return p?.StandardOutput.ReadToEnd().Trim() ?? "unknown";
            } catch { return "unknown"; }
        }

        [DllImport("kernel32.dll")] static extern IntPtr GetStdHandle(int n);
        [DllImport("kernel32.dll")] static extern bool GetConsoleMode(IntPtr h, out uint m);
        [DllImport("kernel32.dll")] static extern bool SetConsoleMode(IntPtr h, uint m);
        static void WinInit() {
            var h = GetStdHandle(-11);
            if (GetConsoleMode(h, out uint m)) SetConsoleMode(h, m | 0x04 | 0x08);
        }
    }
}
