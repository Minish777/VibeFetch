using System;
using System.Runtime.InteropServices;
using System.IO;
using System.Linq;
using System.Diagnostics;

namespace VibeFetch
{
    class Program
    {
        static string Cyan = "\u001b[38;2;139;233;253m";
        static string Purple = "\u001b[38;2;189;147;249m";
        static string White = "\u001b[38;2;248;248;242m";
        static string Gray = "\u001b[38;2;98;114;164m";
        static string Reset = "\u001b[0m";

        static void Main()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) WinInit();

            Console.Clear();
            Console.WriteLine($"{Cyan}  oooooo     oooo");
            Console.WriteLine($"{Cyan}   `888.     .8'   {Purple}sodrely{White}@{Purple}vfetch{Reset}");
            Console.WriteLine($"{Cyan}    `888.   .8'    {Gray}--------------------------{Reset}");
            Console.WriteLine($"{Cyan}     `888. .8'     {Cyan}os      {White}➜  {GetOSName()}");
            Console.WriteLine($"{Cyan}      `888.8'      {Cyan}host    {White}➜  {Environment.MachineName}");
            Console.WriteLine($"{Cyan}       `888'       {Cyan}kernel  {White}➜  {GetKernelInfo()}");
            Console.WriteLine($"{Cyan}        `8'        {Cyan}uptime  {White}➜  {GetUptime()}");

            var (used, total) = GetRamInfo();
            Console.WriteLine($"                   {Cyan}ram     {White}➜  {used}GB / {total}GB");
            Console.WriteLine($"\n                   {Cyan}● {Purple}● {Gray}● {White}●");
        }

        static string GetOSName()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) return "Windows " + Environment.OSVersion.Version.Major;
            
            // Читаем название дистрибутива в Linux (например, CachyOS)
            if (File.Exists("/etc/os-release"))
            {
                var line = File.ReadAllLines("/etc/os-release").FirstOrDefault(l => l.StartsWith("PRETTY_NAME="));
                if (line != null) return line.Split('"')[1];
            }
            return "Linux";
        }

        static string GetKernelInfo()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) return Environment.OSVersion.Version.ToString();

            // Получаем точную версию ядра в Linux
            try {
                if (File.Exists("/proc/version"))
                {
                    var parts = File.ReadAllText("/proc/version").Split(' ');
                    return parts[2]; // Обычно третья часть — это версия ядра
                }
            } catch {}
            return "Unknown";
        }

        static string GetUptime()
        {
            var t = TimeSpan.FromMilliseconds(Environment.TickCount64);
            return $"{t.Hours}h {t.Minutes}m";
        }

        static (long used, long total) GetRamInfo()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) return (4, 32); // Твой конфиг

            try {
                if (File.Exists("/proc/meminfo"))
                {
                    var lines = File.ReadAllLines("/proc/meminfo");
                    long totalKb = long.Parse(lines[0].Split(' ', StringSplitOptions.RemoveEmptyEntries)[1]);
                    long freeKb = long.Parse(lines[1].Split(' ', StringSplitOptions.RemoveEmptyEntries)[1]);
                    return ((totalKb - freeKb) / 1024 / 1024, totalKb / 1024 / 1024);
                }
            } catch {}
            return (0, 0);
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
