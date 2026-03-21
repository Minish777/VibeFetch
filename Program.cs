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
        static string Version = "2.0.0";
        static string Repo = "Minish777/VibeFetch"; // Убедись, что ник твой!
        static string Tag = "v" + Version;

        // Dracula Palette
        static string Cyan = "\u001b[38;2;139;233;253m";
        static string Purple = "\u001b[38;2;189;147;249m";
        static string White = "\u001b[38;2;248;248;242m";
        static string Gray = "\u001b[38;2;98;114;164m";
        static string Green = "\u001b[38;2;80;250;123m";
        static string Reset = "\u001b[0m";

        static async Task Main(string[] args)
        {
            if (args.Contains("--update")) { await RunUpdate(); return; }
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) WinInit();
            ShowFetch();
        }

        static async Task RunUpdate()
        {
            Console.WriteLine($"{Purple}[*] Checking for updates...{Reset}");
            try
            {
                using var client = new HttpClient();
                client.DefaultRequestHeaders.Add("User-Agent", "vfetch-updater");
                var releaseJson = await client.GetStringAsync($"https://api.github.com/repos/{Repo}/releases/latest");
                
                if (releaseJson.Contains($"\"tag_name\":\"{Tag}\""))
                {
                    Console.WriteLine($"{Green}[+] vfetch is up to date.{Reset}");
                    return;
                }

                string assetName = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "vfetch.exe" : "vfetch";
                string downloadUrl = $"https://github.com/{Repo}/releases/latest/download/{assetName}";
                var data = await client.GetByteArrayAsync(downloadUrl);
                string currentExe = Process.GetCurrentProcess().MainModule.FileName;
                string tempExe = currentExe + ".tmp";
                await File.WriteAllBytesAsync(tempExe, data);

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    string cmd = $"/c timeout /t 2 & del \"{currentExe}\" & move \"{tempExe}\" \"{currentExe}\"";
                    Process.Start(new ProcessStartInfo("cmd.exe", cmd) { CreateNoWindow = true });
                }
                else
                {
                    File.Move(tempExe, currentExe, true);
                    Process.Start("chmod", $"+x \"{currentExe}\"").WaitForExit();
                }
                Console.WriteLine($"{Purple}[!] Success. Please restart vfetch.{Reset}");
            }
            catch (Exception ex) { Console.WriteLine($"{White}[-] Update error: {ex.Message}{Reset}"); }
        }

        static void ShowFetch()
        {
            Console.Clear();
            Console.WriteLine($"{Cyan}  oooooo     oooo");
            Console.WriteLine($"{Cyan}   `888.     .8'   {Purple}{Environment.UserName}{White}@{Purple}{Environment.MachineName}{Reset}");
            Console.WriteLine($"{Cyan}    `888.   .8'    {Gray}--------------------------{Reset}");
            Console.WriteLine($"{Cyan}     `888. .8'     {Cyan}os      {White}➜  {GetOSName()}");
            Console.WriteLine($"{Cyan}      `888.8'      {Cyan}kernel  {White}➜  {RuntimeInformation.OSDescription.Split(' ').Last()}");
            Console.WriteLine($"{Cyan}       `888'       {Cyan}uptime  {White}➜  {GetUptime()}");
            
            var (uM, tM) = GetRamInfo();
            Console.WriteLine($"                   {Cyan}ram     {White}➜  {uM}GB / {tM}GB");
            
            var (uD, tD) = GetDiskInfo();
            Console.WriteLine($"                   {Cyan}disk    {White}➜  {uD}GB / {tD}GB");
            Console.WriteLine($"\n                   {Cyan}● {Purple}● {Green}● {Gray}●");
        }

        static string GetOSName()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) return "Windows " + Environment.OSVersion.Version.Major;
            if (File.Exists("/etc/os-release"))
            {
                var prettyName = File.ReadAllLines("/etc/os-release")
                    .FirstOrDefault(l => l.StartsWith("PRETTY_NAME="))?.Split('"')[1];
                return prettyName ?? "Linux";
            }
            return RuntimeInformation.OSDescription;
        }

        static string GetUptime()
        {
            var t = TimeSpan.FromMilliseconds(Environment.TickCount64);
            return $"{t.Hours}h {t.Minutes}m";
        }

        static (long, long) GetRamInfo()
        {
            try {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) return (4, 32); // Твой конфиг
                if (File.Exists("/proc/meminfo"))
                {
                    var lines = File.ReadAllLines("/proc/meminfo");
                    long total = long.Parse(lines[0].Split(' ', StringSplitOptions.RemoveEmptyEntries)[1]);
                    long avail = long.Parse(lines[2].Split(' ', StringSplitOptions.RemoveEmptyEntries)[1]);
                    return ((total - avail) / 1024 / 1024, total / 1024 / 1024);
                }
            } catch {}
            return (0, 0);
        }

        static (long, long) GetDiskInfo()
        {
            try {
                var drive = DriveInfo.GetDrives().FirstOrDefault(d => d.IsReady);
                if (drive == null) return (0, 0);
                return ((drive.TotalSize - drive.AvailableFreeSpace) / 1073741824, drive.TotalSize / 1073741824);
            } catch { return (0, 0); }
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
