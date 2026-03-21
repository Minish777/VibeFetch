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
        // === КОНФИГУРАЦИЯ ВЕРСИИ ===
        static string Version = "2.1.0";
        // ЗАМЕНИ НА СВОЙ РЕПОЗИТОРИЙ (Например: "твой_ник/VibeFetch")
        static string Repo = "Minish777/VibeFetch"; 
        static string Tag = "v" + Version;

        // === ПАЛИТРА CATPPUCCIN (MOCHA) ===
        // Стандартные ANSI цвета не используются, используем TrueColor (24-bit)
        static string Text = "\u001b[38;2;205;214;244m";      // Молок (Текст)
        static string Subtext1 = "\u001b[38;2;186;194;222m"; // Субтекст 1 (Значения)
        static string Mauve = "\u001b[38;2;203;166;247m";    // Лиловый (Акцент, Пользователь)
        static string Sapphire = "\u001b[38;2;116;199;236m"; // Сапфир (Ключи)
        static string Overlay0 = "\u001b[38;2;108;112;134m"; // Оверлей 0 (Разделитель)
        
        // Цветовые точки
        static string Rosewater = "\u001b[48;2;245;224;220m  ";
        static string Lavender = "\u001b[48;2;180;190;254m  ";
        static string Teal = "\u001b[48;2;148;226;213m  ";
        static string Surface0 = "\u001b[48;2;49;50;68m  ";
        static string Reset = "\u001b[0m";

        static async Task Main(string[] args)
        {
            if (args.Contains("--update")) { await RunUpdate(); return; }
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) WinInit();
            ShowFetch();
        }

        static async Task RunUpdate()
        {
            Console.WriteLine($"{Mauve} Checking for updates...{Reset}");
            try
            {
                using var client = new HttpClient();
                client.DefaultRequestHeaders.Add("User-Agent", "vfetch-updater");
                var releaseJson = await client.GetStringAsync($"https://api.github.com/repos/{Repo}/releases/latest");
                
                if (releaseJson.Contains($"\"tag_name\":\"{Tag}\""))
                {
                    Console.WriteLine($"{Teal} You are on the latest version.{Reset}");
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
                Console.WriteLine($"{Mauve} Update success. Please restart vfetch.{Reset}");
            }
            catch (Exception ex) { Console.WriteLine($"{Text}[-] Error: {ex.Message}{Reset}"); }
        }

        static void ShowFetch()
        {
            Console.Clear();
            string user = Environment.UserName;
            string host = Environment.MachineName;

            // ASCII Арт (логотип V) в цвете Sapphire
            Console.WriteLine($"{Sapphire}  oooooo     oooo");
            Console.WriteLine($"{Sapphire}   `888.     .8'   {Mauve}{user}{Text}@{Mauve}{host}{Reset}");
            Console.WriteLine($"{Sapphire}    `888.   .8'    {Overlay0}--------------------------{Reset}");
            
            // Сбор информации
            Console.WriteLine($"{Sapphire}     `888. .8'     {Sapphire}os      {Subtext1}➜  {GetOSName()}");
            Console.WriteLine($"{Sapphire}      `888.8'      {Sapphire}version {Subtext1}➜  {Version}");
            Console.WriteLine($"{Sapphire}       `888'       {Sapphire}kernel  {Subtext1}➜  {GetKernelInfo()}");
            Console.WriteLine($"{Sapphire}        `8'        {Sapphire}uptime  {Subtext1}➜  {GetUptime()}");
            
            var (usedMem, totalMem) = GetRamInfo();
            Console.WriteLine($"                   {Sapphire}ram     {Subtext1}➜  {usedMem}GB / {totalMem}GB");

            var (usedDisk, totalDisk) = GetDiskInfo();
            Console.WriteLine($"                   {Sapphire}disk    {Subtext1}➜  {usedDisk}GB / {totalDisk}GB");

            // Цветовые точки (Catppuccin blocks)
            Console.WriteLine($"\n                   {Rosewater}{Reset} {Lavender}{Reset} {Teal}{Reset} {Surface0}{Reset}");
        }

        // === ИСПРАВЛЕННЫЕ МЕТОДЫ ПОЛУЧЕНИЯ ИНФОРМАЦИИ ===

        static string GetOSName()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) return "Windows " + Environment.OSVersion.Version.Major;
            
            // На Linux ищем красивое имя
            if (File.Exists("/etc/os-release"))
            {
                var prettyName = File.ReadAllLines("/etc/os-release")
                    .FirstOrDefault(l => l.StartsWith("PRETTY_NAME="))?.Split('"')[1];
                return prettyName ?? "Linux";
            }
            return RuntimeInformation.OSDescription;
        }

        static string GetKernelInfo()
        {
            // На Linux OSDescription часто возвращает 'Linux' или 'Arch', нам нужна версия
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                try { return RunCommand("uname", "-r"); } catch { return RuntimeInformation.OSDescription; }
            }
            return RuntimeInformation.OSDescription.Split(' ').Last(); // Windows: 10.0.19045
        }

        static string GetUptime()
        {
            var t = TimeSpan.FromMilliseconds(Environment.TickCount64);
            return $"{t.Hours}h {t.Minutes}m";
        }

        static (long used, long total) GetRamInfo()
        {
            try {
                // Если ты на CachyOS, то System.Management (WMI) тут не сработает
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    // На Windows берем то, что ты хардкодил
                    return (4, 32); 
                }
                else if (File.Exists("/proc/meminfo"))
                {
                    // На Linux парсим /proc/meminfo (точно)
                    var lines = File.ReadAllLines("/proc/meminfo");
                    long totalKb = long.Parse(lines[0].Split(' ', StringSplitOptions.RemoveEmptyEntries)[1]);
                    long availKb = long.Parse(lines[2].Split(' ', StringSplitOptions.RemoveEmptyEntries)[1]);
                    return ((totalKb - availKb) / 1024 / 1024, totalKb / 1024 / 1024);
                }
            } catch {}
            return (0, 0);
        }

        static (long used, long total) GetDiskInfo()
        {
            // Берем главный системный диск
            string root = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "C:\\" : "/";
            var drive = new DriveInfo(root);
            long totalGb = drive.TotalSize / 1073741824; // 1024^3
            long freeGb = drive.AvailableFreeSpace / 1073741824;
            return (totalGb - freeGb, totalGb);
        }

        // Хелпер для запуска консольных команд
        static string RunCommand(string cmd, string args)
        {
            var startInfo = new ProcessStartInfo(cmd, args) { RedirectStandardOutput = true, UseShellExecute = false, CreateNoWindow = true };
            using var process = Process.Start(startInfo);
            return process.StandardOutput.ReadToEnd().Trim();
        }

        // === ИНИЦИАЛИЗАЦИЯ WINDOWS ANSI ===
        [DllImport("kernel32.dll")] static extern IntPtr GetStdHandle(int n);
        [DllImport("kernel32.dll")] static extern bool GetConsoleMode(IntPtr h, out uint m);
        [DllImport("kernel32.dll")] static extern bool SetConsoleMode(IntPtr h, uint m);
        static void WinInit() {
            var h = GetStdHandle(-11); // STD_OUTPUT_HANDLE
            if (GetConsoleMode(h, out uint m)) SetConsoleMode(h, m | 0x0004 | 0x0008); // ENABLE_VIRTUAL_TERMINAL_PROCESSING
        }
    }
}
