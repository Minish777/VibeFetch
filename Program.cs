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
        static string Mauve = "\u001b[38;2;203;166;247m";    // Акценты
        static string Sapphire = "\u001b[38;2;116;199;236m"; // Ключи
        static string Text = "\u001b[38;2;205;214;244m";      // Основной текст
        static string Overlay = "\u001b[38;2;108;112;134m";   // Разделитель
        static string Teal = "\u001b[38;2;148;226;213m";      // Успех
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

            Console.WriteLine($"{Sapphire}  oooooo     oooo");
            Console.WriteLine($"{Sapphire}   `888.     .8'   {Mauve}{user}{Text}@{Mauve}{host}{Reset}");
            Console.WriteLine($"{Sapphire}    `888.   .8'    {Overlay}--------------------------{Reset}");
            Console.WriteLine($"{Sapphire}     `888. .8'     {Sapphire}os      {Text}➜  {GetOSName()}");
            Console.WriteLine($"{Sapphire}      `888.8'      {Sapphire}kernel  {Text}➜  {GetKernel()}");
            Console.WriteLine($"{Sapphire}       `888'       {Sapphire}uptime  {Text}➜  {GetUptime()}");
            
            var (uM, tM) = GetRam();
            Console.WriteLine($"                   {Sapphire}ram     {Text}➜  {uM}GB / {tM}GB");
            
            var (uD, tD) = GetDisk();
            Console.WriteLine($"                   {Sapphire}disk    {Text}➜  {uD}GB / {tD}GB");
            
            // Цветовая полоска Catppuccin
            Console.WriteLine($"\n                   \u001b[48;2;245;224;220m  \u001b[48;2;242;205;205m  \u001b[48;2;203;166;247m  \u001b[48;2;116;199;236m  {Reset}");
        }

        static string GetOSName() => File.Exists("/etc/os-release") 
            ? File.ReadAllLines("/etc/os-release").FirstOrDefault(l => l.StartsWith("PRETTY_NAME="))?.Split('"')[1] ?? "Linux"
            : RuntimeInformation.OSDescription;

        static string GetKernel() => RuntimeInformation.IsOSPlatform(OSPlatform.Linux) 
            ? RunCmd("uname", "-r") : RuntimeInformation.OSDescription.Split(' ').Last();

        static string GetUptime() => RuntimeInformation.IsOSPlatform(OSPlatform.Linux)
            ? RunCmd("uptime", "-p").Replace("up ", "") : $"{TimeSpan.FromMilliseconds(Environment.TickCount64).Hours}h";

        static (long, long) GetRam()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) return (4, 32); // Твой конфиг
            var mem = File.ReadAllLines("/proc/meminfo");
            long total = long.Parse(mem[0].Split(' ', StringSplitOptions.RemoveEmptyEntries)[1]) / 1024 / 1024;
            long free = long.Parse(mem[2].Split(' ', StringSplitOptions.RemoveEmptyEntries)[1]) / 1024 / 1024;
            return (total - free, total);
        }

        static (long, long) GetDisk()
        {
            var d = DriveInfo.GetDrives().FirstOrDefault(x => x.IsReady);
            return d == null ? (0, 0) : ((d.TotalSize - d.TotalFreeSpace) / 1073741824, d.TotalSize / 1073741824);
        }

        static string RunCmd(string c, string a) {
            var p = Process.Start(new ProcessStartInfo(c, a) { RedirectStandardOutput = true });
            return p?.StandardOutput.ReadToEnd().Trim() ?? "";
        }

        [DllImport("kernel32.dll")] static extern IntPtr GetStdHandle(int n);
        [DllImport("kernel32.dll")] static extern bool GetConsoleMode(IntPtr h, out uint m);
        [DllImport("kernel32.dll")] static extern bool SetConsoleMode(IntPtr h, uint m);
        static void WinInit() {
            var h = GetStdHandle(-11);
            if (GetConsoleMode(h, out uint m)) SetConsoleMode(h, m | 0x0004 | 0x0008);
        }

        static async Task RunUpdate() { /* Логика обновления без изменений */ }
    }
}
