/*
 * VibeFetch v1.1.0
 * Licensed under MIT (c) 2026 sodrely & Gemini AI
 * GitHub: https://github.com/sodrely/VibeFetch
 */
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <windows.h>
#include <psapi.h>

#define VERSION "0.6.0"

// Цвета
#define C_CYAN    "\x1b[36m"
#define C_GREEN   "\x1b[32m"
#define C_MAGENTA "\x1b[35m"
#define C_YELLOW  "\x1b[33m"
#define C_RESET   "\x1b[0m"
#define C_DIM     "\x1b[2m"

void get_gpu(char* out, size_t len) {
    HKEY hKey;
    if (RegOpenKeyExA(HKEY_LOCAL_MACHINE, "SYSTEM\\CurrentControlSet\\Control\\Class\\{4d36e968-e325-11ce-bfc1-08002be10318}\\0000", 0, KEY_READ, &hKey) == ERROR_SUCCESS) {
        DWORD sz = (DWORD)len;
        RegQueryValueExA(hKey, "DriverDesc", NULL, NULL, (LPBYTE)out, &sz);
        RegCloseKey(hKey);
    } else { strcpy(out, "Integrated Graphics"); }
}

void print_info(const char* key, const char* val) {
    printf("%s%-12s%s %s\n", C_GREEN, key, C_RESET, val);
}

int main() {
    // Включаем поддержку ANSI в консоли Windows
    HANDLE hOut = GetStdHandle(STD_OUTPUT_HANDLE);
    DWORD dwMode = 0;
    GetConsoleMode(hOut, &dwMode);
    SetConsoleMode(hOut, dwMode | 0x0004);

    char cpu[128] = "Unknown", gpu[128] = "Unknown", user[64], host[64];
    DWORD us = 64, hs = 64;
    GetUserNameA(user, &us); GetComputerNameA(host, &hs);

    // CPU Info
    HKEY hKey;
    if (RegOpenKeyExA(HKEY_LOCAL_MACHINE, "HARDWARE\\DESCRIPTION\\System\\CentralProcessor\\0", 0, KEY_READ, &hKey) == ERROR_SUCCESS) {
        DWORD sz = 128;
        RegQueryValueExA(hKey, "ProcessorNameString", NULL, NULL, (LPBYTE)cpu, &sz);
        RegCloseKey(hKey);
    }
    get_gpu(gpu, 128);

    // RAM Info
    MEMORYSTATUSEX mem; mem.dwLength = sizeof(mem);
    GlobalMemoryStatusEx(&mem);
    int total_ram = mem.ullTotalPhys / (1024 * 1024);
    int used_ram = total_ram - (mem.ullAvailPhys / (1024 * 1024));

    // Uptime
    DWORD uptime_ms = GetTickCount();
    int h = uptime_ms / 3600000;
    int m = (uptime_ms % 3600000) / 60000;

    // Screen Resolution
    int screen_w = GetSystemMetrics(SM_CXSCREEN);
    int screen_h = GetSystemMetrics(SM_CYSCREEN);

    // Disk Info
    ULARGE_INTEGER free_bytes, total_bytes;
    GetDiskFreeSpaceExA("C:\\", &free_bytes, &total_bytes, NULL);
    double total_gb = total_bytes.QuadPart / (1024.0 * 1024 * 1024);
    double free_gb = free_bytes.QuadPart / (1024.0 * 1024 * 1024);

    // --- LOGO TOP ---
    printf("\n%s       .---.       \n", C_CYAN);
    printf("      /     \\      \n");
    printf("      | (O) |      %s%s%s@%s%s%s\n", C_MAGENTA, user, C_RESET, C_MAGENTA, host, C_RESET);
    printf("      \\     /      %s--------------------%s\n", C_DIM, C_RESET);
    printf("       '---'       \n%s", C_RESET);

    // --- MODULES ---
    print_info("OS", "Windows 10/11 x86_64");
    print_info("Kernel", "NT 10.0.19045");
    printf("%s%-12s%s %d hours, %d mins\n", C_GREEN, "Uptime", C_RESET, h, m);
    print_info("Shell", "PowerShell / MSYS2");
    printf("%s%-12s%s %dx%d\n", C_GREEN, "Resolution", C_RESET, screen_w, screen_h);
    print_info("CPU", cpu);
    print_info("GPU", gpu);
    printf("%s%-12s%s %dMiB / %dMiB\n", C_GREEN, "Memory", C_RESET, used_ram, total_ram);
    printf("%s%-12s%s %.1fGiB / %.1fGiB\n", C_GREEN, "Disk (C:)", C_RESET, (total_gb - free_gb), total_gb);
    
    printf("\n%s   ", C_RESET);
    for(int i=40; i<48; i++) printf("\x1b[%dm   ", i); // Цветовая палитра
    printf("%s\n\n", C_RESET);

    return 0;
}
