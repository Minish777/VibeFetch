import os
import shutil
import ctypes
import winreg

def refresh_env():
    # Сообщаем Windows, что переменные среды изменились
    HWND_BROADCAST = 0xFFFF
    WM_SETTINGCHANGE = 0x001A
    SMTO_ABORTIFHUNG = 0x0002
    result = ctypes.c_long()
    ctypes.windll.user32.SendMessageTimeoutW(
        HWND_BROADCAST, WM_SETTINGCHANGE, 0, u'Environment', 
        SMTO_ABORTIFHUNG, 5000, ctypes.byref(result)
    )

def install_windows():
    exe_name = "vfetch.exe"
    install_dir = r"C:\vfetch"
    
    # 1. Копирование
    if not os.path.exists(install_dir):
        os.makedirs(install_dir)
    
    source = os.path.join(os.getcwd(), exe_name)
    if os.path.exists(source):
        shutil.copy2(source, os.path.join(install_dir, exe_name))
        print(f"[+] Файл скопирован в {install_dir}")
    else:
        print("[-] Ошибка: vfetch.exe не найден в текущей папке!")
        return

    # 2. Запись в PATH
    key = winreg.OpenKey(winreg.HKEY_CURRENT_USER, "Environment", 0, winreg.KEY_ALL_ACCESS)
    try:
        path_val, _ = winreg.QueryValueEx(key, "Path")
        if install_dir.lower() not in path_val.lower():
            winreg.SetValueEx(key, "Path", 0, winreg.REG_EXPAND_SZ, f"{path_val};{install_dir}")
            refresh_env()
            print("[+] Путь добавлен в реестр и обновлен.")
        else:
            print("[!] Путь уже был в системе.")
    finally:
        winreg.CloseKey(key)

if __name__ == "__main__":
    if os.name == "nt":
        install_windows()
        print("\nГотово! Попробуй открыть НОВОЕ окно CMD.")
    else:
        print("Для Linux используй: sudo cp vfetch /usr/local/bin/")
    input("\nНажми Enter для выхода...")