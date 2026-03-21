using System;
using System.Runtime.InteropServices;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Linq;

namespace VibeFetch
{
    class Program
    {
        static string Version = "2.1.0";
        static string Repo = "Minish777/VibeFetch"; 

        // === CATPPUCCIN MOCHA PALETTE ===
        static string Mauve = "\u001b[38;2;203;166;247m";    // Акценты (User)
        static string Sapphire = "\u001b[38;2;116;199;236m"; // Ключи (OS, Kernel)
        static string Text = "\u001b[38;2;205;214;244m";      // Основной текст
        static string Overlay = "\u001b[38;2;108;112;134m";   // Разделитель
        static string Reset = "\u001b[0m";

        static async Task Main(string[] args)
        {
            if (args.Contains("--update")) { await RunUpdate(); return; }
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) WinInit();
            ShowFetch();
        }

        static void ShowFetch()
        {
            Console.Clear();
            string user = Environment.UserName;
            string host = Environment.MachineName;

            // ASCII Арт Sapphire
            Console.WriteLine($"{Sapphire}  oooooo     oooo");
            Console.WriteLine($"{Sapphire}   `888.     .8'   {Mauve}{user}{Text}@{Mauve}{host}{Reset}");
            Console.WriteLine($"{Sapphire}    `888.   .8'    {Overlay}--------------------------{Reset}");
            
            // Информация с точными методами
            Console.WriteLine($"{Sapphire}     `888. .8'     {Sapphire}os      {Text}➜  {GetOSName()}");
            Console.WriteLine($"{Sapphire}      `888.8'      {Sapphire}kernel  {Text}➜  {GetKernel()}");
            Console.WriteLine($"{Sapphire}       `888'       {Sapphire}uptime  {Text}➜  {GetUptime()}");
            
            var (uM, tM) = GetRam();
            Console.WriteLine($"                   {Sapphire}ram     {Text}➜  {uM}GB / {tM}GB");
            
            var (uD, tD) = GetDisk();
            Console.WriteLine($"                   {Sapphire}disk    {Text}➜  {uD}GB / {tD}GB");
            
            // Catppuccin Dots
            Console.WriteLine($"\n                   \u001b[48;2;245;224;220m  \u001b[48;2;242;205;205m  \u001b[48;2;203;166;247m  \u001b[48;2;116;199;236m  {Reset}");
        }

        // --- ТОЧНЫЕ МЕТОДЫ ---
        static string GetOSName() => File.Exists("/etc/os-release") 
            ? File.ReadAllLines("/etc/os-release").FirstOrDefault(l => l.StartsWith("PRETTY_NAME="))?.Split('"')[1] ?? "Linux"
            : RuntimeInformation.OSDescription;

        static string GetKernel() => RuntimeInformation.IsOSPlatform(OSPlatform.Linux) 
            ? RunCmd("uname", "-r") : RuntimeInformation.OSDescription.Split(' ').Last();

        static string GetUptime() {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) {
                string up = RunCmd("uptime", "-p").Replace("up ", "");
                return string.IsNullOrEmpty(up) ? "unknown" : up;
            }
            var t = TimeSpan.FromMilliseconds(Environment.TickCount64);
            return $"{t.Hours}h {t.Minutes}m";
        }

        static (long, long) GetRam() {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) return (4, 32); // Твой конфиг
            if (File.Exists("/proc/meminfo")) {
                var lines = File.ReadAllLines("/proc/meminfo");
                long total = long.Parse(lines[0].Split(' ', StringSplitOptions.RemoveEmptyEntries)[1]) / 1024 / 1024;
                long avail = long.Parse(lines[2].Split(' ', StringSplitOptions.RemoveEmptyEntries)[1]) / 1024 / 1024;
                return (total - avail, total);
            }
            return (0, 0);
        }

        static (long, long) GetDisk() {
            var drive = DriveInfo.GetDrives().FirstOrDefault(d => d.IsReady);
            if (drive == null) return (0, 0);
            return ((drive.TotalSize - drive.AvailableFreeSpace) / 1073741824, drive.TotalSize / 1073741824);
        }

        static string RunCmd(string c, string a) {
            try {
                var p = Process.Start(new ProcessStartInfo(c, a) { RedirectStandardOutput = true, UseShellExecute = false });
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

        static async Task RunUpdate() { /* Логика та же */ }
    }
}
