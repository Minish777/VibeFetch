import os
import sys
import shutil
import subprocess

def install():
    is_windows = os.name == "nt"
    exe_name = "vfetch.exe" if is_windows else "vfetch"
    
    # Определяем путь к бинарнику (ищем в текущей папке)
    source_path = os.path.join(os.getcwd(), exe_name)

    if not os.path.exists(source_path):
        print(f"[-] Error: {exe_name} not found in current folder!")
        return

    if is_windows:
        import winreg  # Импортируем только внутри блока Windows
        import ctypes
        
        install_dir = r"C:\vfetch"
        if not os.path.exists(install_dir):
            os.makedirs(install_dir)
        
        shutil.copy2(source_path, os.path.join(install_dir, exe_name))

        # Запись в PATH
        key = winreg.OpenKey(winreg.HKEY_CURRENT_USER, "Environment", 0, winreg.KEY_ALL_ACCESS)
        try:
            path_val, _ = winreg.QueryValueEx(key, "Path")
            if install_dir.lower() not in path_val.lower():
                winreg.SetValueEx(key, "Path", 0, winreg.REG_EXPAND_SZ, f"{path_val};{install_dir}")
                print(f"[+] Added {install_dir} to PATH.")
        finally:
            winreg.CloseKey(key)
        print("[SUCCESS] Restart CMD and type 'vfetch'")

    else:
        # Логика для Linux (CachyOS)
        target = "/usr/local/bin/vfetch"
        print(f"[*] Installing to {target}...")
        try:
            # Используем sudo для копирования в системную папку
            subprocess.run(["sudo", "cp", source_path, target], check=True)
            subprocess.run(["sudo", "chmod", "+x", target], check=True)
            print("[SUCCESS] Global command 'vfetch' is now available!")
        except Exception as e:
            print(f"[-] Failed to install: {e}")

if __name__ == "__main__":
    install()
