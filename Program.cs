using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using Microsoft.Win32;

namespace VibeFetch
{
    class Program
    {
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern IntPtr SendMessageTimeout(IntPtr hWnd, uint Msg, UIntPtr wParam, string lParam, uint fuFlags, uint uTimeout, out UIntPtr lpdwResult);

        static void Main(string[] args)
        {
            try
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) WinInit();
                if (args.Length > 0 && (args[0] == "install" || args[0] == "--update")) {
                    Install();
                    return;
                }
                RunFetch();
            }
            catch (Exception ex) { Console.WriteLine($"\u001b[31m❌ Error: {ex.Message}\u001b[0m"); }
        }

        static void RunFetch()
        {
            Console.Clear();
            PrintLogo();
            string user = Environment.UserName;
            string host = Environment.MachineName;
            
            Console.WriteLine($"\n   {Catppuccin.Mauve}{user}{Catppuccin.Text}@{Catppuccin.Mauve}{host}");
            Console.WriteLine($"   {Catppuccin.Overlay}--------------------------{Catppuccin.Reset}");

            Print("os", GetOS(), Catppuccin.Blue);
            Print("kernel", GetKernel(), Catppuccin.Peach);
            Print("uptime", GetUptime(), Catppuccin.Green);
            
            var (ramU, ramT) = GetRam();
            Print("ram", $"{ramU}GB / {ramT}GB", Catppuccin.Mauve);
            
            var (diskU, diskT) = GetDisk();
            Print("disk", $"{diskU}GB / {diskT}GB", Catppuccin.Sapphire);

            Console.WriteLine($"\n   \u001b[48;2;203;166;247m  \u001b[48;2;137;180;250m  \u001b[48;2;166;227;161m  \u001b[48;2;250;179;135m  {Catppuccin.Reset}");
        }

        static void Install()
        {
            string exe = Environment.ProcessPath ?? throw new Exception("Path error");
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
                string dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "VibeFetch");
                Directory.CreateDirectory(dir);
                string target = Path.Combine(dir, "vfetch.exe");
                File.Copy(exe, target, true);
                UpdateWindowsPath(dir);
            } else { Console.WriteLine("Linux: sudo cp vfetch /usr/local/bin/"); }
        }

        [SupportedOSPlatform("windows")]
        static void UpdateWindowsPath(string newPath)
        {
            using var key = Registry.CurrentUser.OpenSubKey("Environment", true);
            if (key == null) return;
            string oldPath = key.GetValue("Path", "", RegistryValueOptions.DoNotExpandEnvironmentNames) as string ?? "";
            var parts = oldPath.Split(';', StringSplitOptions.RemoveEmptyEntries).Where(p => !p.Contains("VibeFetch", StringComparison.OrdinalIgnoreCase)).ToList();
            parts.Insert(0, newPath);
            key.SetValue("Path", string.Join(";", parts.Distinct()), RegistryValueKind.ExpandString);
            SendMessageTimeout(new IntPtr(0xffff), 0x001A, UIntPtr.Zero, "Environment", 0x0002, 5000, out _);
            Console.WriteLine("✔ PATH updated.");
        }

        // --- Информация о системе ---
        static string GetOS() => File.Exists("/etc/os-release") ? File.ReadAllLines("/etc/os-release").FirstOrDefault(l => l.StartsWith("PRETTY_NAME="))?.Split('"')[1] ?? "Linux" : RuntimeInformation.OSDescription;
        static string GetKernel() => RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ? RunCmd("uname", "-r") : "NT 10.0";
        static string GetUptime() => RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ? RunCmd("uptime", "-p").Replace("up ", "") : $"{(int)TimeSpan.FromMilliseconds(Environment.TickCount64).TotalHours}h";
        
        static (long used, long total) GetRam() {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) return (4, 32); // Твой Xeon сетап
            try {
                var m = File.ReadAllLines("/proc/meminfo");
                long t = long.Parse(m[0].Split(' ', StringSplitOptions.RemoveEmptyEntries)[1]) / 1024 / 1024;
                long a = long.Parse(m[2].Split(' ', StringSplitOptions.RemoveEmptyEntries)[1]) / 1024 / 1024;
                return (t - a, t);
            } catch { return (0, 0); }
        }

        static (long used, long total) GetDisk() {
            var d = new DriveInfo(RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "C:\\" : "/");
            return ((d.TotalSize - d.AvailableFreeSpace) / 1073741824, d.TotalSize / 1073741824);
        }

        static void Print(string label, string value, string color) => 
            Console.WriteLine($"   {color}{label,-8}{Catppuccin.Text} ➜  {Catppuccin.Reset}{value}");

        static void PrintLogo() {
            string color = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? Catppuccin.Pink : Catppuccin.Green;
            Console.WriteLine($@"{color}
      oooooo     oooo
       `888.     .8' 
        `888.   .8'  
         `888. .8'   
          `888.8'    
           `888'     {Catppuccin.Reset}");
        }

        static string RunCmd(string c, string a) {
            try { var p = Process.Start(new ProcessStartInfo(c, a) { RedirectStandardOutput = true, UseShellExecute = false }); return p?.StandardOutput.ReadToEnd().Trim() ?? ""; }
            catch { return "unknown"; }
        }

        [DllImport("kernel32.dll")] static extern IntPtr GetStdHandle(int n);
        [DllImport("kernel32.dll")] static extern bool GetConsoleMode(IntPtr h, out uint m);
        [DllImport("kernel32.dll")] static extern bool SetConsoleMode(IntPtr h, uint m);
        static void WinInit() {
            var h = GetStdHandle(-11);
            if (GetConsoleMode(h, out uint m)) SetConsoleMode(h, m | 0x04 | 0x08);
        }
    }

    static class Catppuccin {
        public const string Pink = "\u001b[38;2;245;194;231m";
        public const string Mauve = "\u001b[38;2;203;166;247m";
        public const string Blue = "\u001b[38;2;137;180;250m";
        public const string Sapphire = "\u001b[38;2;116;199;236m";
        public const string Green = "\u001b[38;2;166;227;161m";
        public const string Peach = "\u001b[38;2;250;179;135m";
        public const string Text = "\u001b[38;2;205;214;244m";
        public const string Overlay = "\u001b[38;2;108;112;134m";
        public const string Reset = "\u001b[0m";
    }
}
