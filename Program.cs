using System;
using System.Runtime.InteropServices;
using System.IO;
using System.Diagnostics;
using System.Linq;

namespace VibeFetch
{
    class Program
    {
        // Палитра Catppuccin Mocha
        static string Mauve = "\u001b[38;2;203;166;247m";    
        static string Sapphire = "\u001b[38;2;116;199;236m"; 
        static string Text = "\u001b[38;2;205;214;244m";      
        static string Overlay = "\u001b[38;2;108;112;134m";   
        static string Reset = "\u001b[0m";

        static void Main()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) WinInit();
            
            Console.Clear();
            string user = Environment.UserName;
            string host = Environment.MachineName;

            Console.WriteLine($"{Sapphire}  oooooo     oooo");
            Console.WriteLine($"{Sapphire}   `888.     .8'   {Mauve}{user}{Text}@{Mauve}{host}{Reset}");
            Console.WriteLine($"{Sapphire}    `888.   .8'    {Overlay}--------------------------{Reset}");
            Console.WriteLine($"{Sapphire}     `888. .8'     {Sapphire}os      {Text}➜  {GetOS()}");
            Console.WriteLine($"{Sapphire}      `888.8'      {Sapphire}kernel  {Text}➜  {GetKernel()}");
            Console.WriteLine($"{Sapphire}       `888'       {Sapphire}uptime  {Text}➜  {GetUptime()}");
            
            var (uM, tM) = GetRam();
            Console.WriteLine($"                   {Sapphire}ram     {Text}➜  {uM}GB / {tM}GB");
            
            var (uD, tD) = GetDisk();
            Console.WriteLine($"                   {Sapphire}disk    {Text}➜  {uD}GB / {tD}GB");
            
            Console.WriteLine($"\n                   \u001b[48;2;245;224;220m  \u001b[48;2;242;205;205m  \u001b[48;2;203;166;247m  \u001b[48;2;116;199;236m  {Reset}");
        }

        static string GetOS() => File.Exists("/etc/os-release") 
            ? File.ReadAllLines("/etc/os-release").FirstOrDefault(l => l.StartsWith("PRETTY_NAME="))?.Split('"')[1] ?? "Linux" 
            : RuntimeInformation.OSDescription;

        static string GetKernel() => RuntimeInformation.IsOSPlatform(OSPlatform.Linux) 
            ? Run("uname", "-r") : "Windows NT";

        static string GetUptime() {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) {
                var up = Run("uptime", "-p").Replace("up ", "");
                return string.IsNullOrEmpty(up) ? "just started" : up;
            }
            var t = TimeSpan.FromMilliseconds(Environment.TickCount64);
            return $"{t.Hours}h {t.Minutes}m";
        }

        static (long, long) GetRam() {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) return (4, 32); // Твой конфиг
            try {
                var m = File.ReadAllLines("/proc/meminfo");
                long t = long.Parse(m[0].Split(' ', StringSplitOptions.RemoveEmptyEntries)[1]) / 1024 / 1024;
                long a = long.Parse(m[2].Split(' ', StringSplitOptions.RemoveEmptyEntries)[1]) / 1024 / 1024;
                return (t - a, t);
            } catch { return (0, 0); }
        }

        static (long, long) GetDisk() {
            try {
                // В Linux ищем корень, в Windows диск C
                string root = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "C:\\" : "/";
                var d = new DriveInfo(root);
                return ((d.TotalSize - d.AvailableFreeSpace) / 1073741824, d.TotalSize / 1073741824);
            } catch { return (0, 0); }
        }

        static string Run(string c, string a) {
            try {
                var p = Process.Start(new ProcessStartInfo(c, a) { RedirectStandardOutput = true });
                return p?.StandardOutput.ReadToEnd().Trim() ?? "";
            } catch { return ""; }
        }

        [DllImport("kernel32.dll")] static extern IntPtr GetStdHandle(int n);
        [DllImport("kernel32.dll")] static extern bool GetConsoleMode(IntPtr h, out uint m);
        [DllImport("kernel32.dll")] static extern bool SetConsoleMode(IntPtr h, uint m);
        static void WinInit() {
            var h = GetStdHandle(-11);
            if (GetConsoleMode(h, out uint m)) SetConsoleMode(h, m | 0x0004 | 0x0008);
        }
    }
}
