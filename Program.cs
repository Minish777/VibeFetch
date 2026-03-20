using System;
using System.Runtime.InteropServices;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;
using System.Linq;

namespace VibeFetch
{
    class Program
    {
        static string Cyan = "\u001b[38;2;139;233;253m";
        static string Purple = "\u001b[38;2;189;147;249m";
        static string White = "\u001b[38;2;248;248;242m";
        static string Gray = "\u001b[38;2;98;114;164m";
        static string Reset = "\u001b[0m";

        static async Task Main()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) WinColors();

            // Чистый вывод
            Console.Clear();
            Console.WriteLine($"{Cyan}  oooooo     oooo");
            Console.WriteLine($"{Cyan}   `888.     .8'   {Purple}sodrely{White}@{Purple}vfetch{Reset}");
            Console.WriteLine($"{Cyan}    `888.   .8'    {Gray}--------------------------{Reset}");
            Console.WriteLine($"{Cyan}     `888. .8'     {Cyan}os      {White}➜  {GetOS()}");
            Console.WriteLine($"{Cyan}      `888.8'      {Cyan}host    {White}➜  {Environment.MachineName}");
            Console.WriteLine($"{Cyan}       `888'       {Cyan}kernel  {White}➜  {GetKernel()}");
            Console.WriteLine($"{Cyan}        `8'        {Cyan}uptime  {White}➜  {GetUptime()}");

            var (u, t) = GetRam();
            Console.WriteLine($"                   {Cyan}ram     {White}➜  {u}GB / {t}GB");
            Console.WriteLine($"\n                   {Cyan}● {Purple}● {Gray}● {White}●");
        }

        static string GetOS() => RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "Windows" : "Linux/macOS";
        static string GetKernel() => RuntimeInformation.OSDescription.Split(' ').Last();
        static string GetUptime() => TimeSpan.FromMilliseconds(Environment.TickCount64).ToString(@"d\h\ m\m");
        static (long, long) GetRam() => (4, 32); // Твой конфиг

        [DllImport("kernel32.dll")] static extern IntPtr GetStdHandle(int n);
        [DllImport("kernel32.dll")] static extern bool GetConsoleMode(IntPtr h, out uint m);
        [DllImport("kernel32.dll")] static extern bool SetConsoleMode(IntPtr h, uint m);
        static void WinColors() {
            var h = GetStdHandle(-11);
            if (GetConsoleMode(h, out uint m)) SetConsoleMode(h, m | 0x0004 | 0x0008);
        }
    }
}