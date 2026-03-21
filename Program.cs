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
        static string Version = "1.9.8";
        static string Repo = "Minish777/VibeFetch"; // Убедись, что имя репо совпадает!
        
        static string Cyan = "\u001b[38;2;139;233;253m";
        static string Purple = "\u001b[38;2;189;147;249m";
        static string White = "\u001b[38;2;248;248;242m";
        static string Reset = "\u001b[0m";

        static async Task Main(string[] args)
        {
            // Проверка флага обновления
            if (args.Contains("--update"))
            {
                await RunUpdate();
                return;
            }

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

                // 1. Получаем инфу о последнем релизе
                var release = await client.GetStringAsync($"https://api.github.com/repos/{Repo}/releases/latest");
                
                if (release.Contains(Version))
                {
                    Console.WriteLine($"{White}[+] You are already using the latest version ({Version}).{Reset}");
                    return;
                }

                // 2. Определяем имя файла для скачивания
                string assetName = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "vfetch.exe" : "vfetch";
                string downloadUrl = $"https://github.com/Root/VibeFetch/releases/latest/download/{assetName}";

                Console.WriteLine($"{Cyan}[>] Downloading {assetName}...{Reset}");
                
                var data = await client.GetByteArrayAsync(downloadUrl);
                string currentExe = Process.GetCurrentProcess().MainModule.FileName;
                string tempExe = currentExe + ".tmp";

                // 3. Записываем новый файл
                await File.WriteAllBytesAsync(tempExe, data);

                // 4. Логика замены (разная для ОС)
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    // На Windows нельзя заменить запущенный EXE, используем скрипт-посредник
                    string cmd = $"/c timeout /t 1 & del \"{currentExe}\" & move \"{tempExe}\" \"{currentExe}\" & echo Update Done!";
                    Process.Start(new ProcessStartInfo("cmd.exe", cmd) { CreateNoWindow = true });
                }
                else
                {
                    // На Linux/macOS просто меняем местами и даем права
                    File.Move(tempExe, currentExe, true);
                    Process.Start("chmod", $"+x {currentExe}").WaitForExit();
                }

                Console.WriteLine($"{Purple}[!] Update downloaded. Please restart vfetch.{Reset}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{White}[-] Update failed: {ex.Message}{Reset}");
            }
        }

        static void ShowFetch()
        {
            Console.Clear();
            Console.WriteLine($"{Cyan}  oooooo     oooo");
            Console.WriteLine($"{Cyan}   `888.     .8'   {Purple}sodrely{White}@{Purple}vfetch{Reset}");
            Console.WriteLine($"{Cyan}    `888.   .8'    {White}--------------------------{Reset}");
            Console.WriteLine($"{Cyan}     `888. .8'     {Cyan}os      {White}➜  {RuntimeInformation.OSDescription}");
            Console.WriteLine($"{Cyan}      `888.8'      {Cyan}version {White}➜  {Version}");
            Console.WriteLine($"{Cyan}       `888'       {Cyan}kernel  {White}➜  {Environment.OSVersion.Version}");
            Console.WriteLine($"{Cyan}        `8'        {Cyan}uptime  {White}➜  {TimeSpan.FromMilliseconds(Environment.TickCount64):h\\h\\ m\\m}");
            Console.WriteLine($"\n                   {Cyan}● {Purple}● {White}●");
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
