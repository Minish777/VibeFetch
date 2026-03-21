import os
import sys
import platform
import shutil
import subprocess

def install():
    current_os = platform.system()
    print(f"[*] Starting installation for: {current_os}")

    if current_os == "Windows":
        try:
            import winreg # Импорт внутри условия, чтобы не ломать Linux
            exe = "vfetch.exe"
            if not os.path.exists(exe):
                print(f"[-] Error: {exe} not found.")
                return

            dest = os.path.join(os.environ["LOCALAPPDATA"], "VibeFetch")
            os.makedirs(dest, exist_ok=True)
            shutil.copy2(exe, os.path.join(dest, exe))

            with winreg.OpenKey(winreg.HKEY_CURRENT_USER, "Environment", 0, winreg.KEY_ALL_ACCESS) as key:
                try:
                    path_val, _ = winreg.QueryValueEx(key, "Path")
                    if dest not in path_val:
                        winreg.SetValueEx(key, "Path", 0, winreg.REG_EXPAND_SZ, f"{path_val};{dest}")
                except:
                    winreg.SetValueEx(key, "Path", 0, winreg.REG_EXPAND_SZ, dest)
            print("[+] Installed! Type 'vfetch' in a new terminal.")
        except Exception as e:
            print(f"[-] Windows error: {e}")

    else:
        # Универсально для Linux
        exe = "vfetch"
        if not os.path.exists(exe):
            print("[-] Error: 'vfetch' binary not found.")
            return
        
        print("[*] Copying to /usr/local/bin...")
        try:
            subprocess.run(["sudo", "cp", exe, "/usr/local/bin/vfetch"], check=True)
            subprocess.run(["sudo", "chmod", "+x", "/usr/local/bin/vfetch"], check=True)
            print("[+] Successfully installed to /usr/local/bin!")
        except Exception as e:
            print(f"[-] Linux error: {e}")

if __name__ == "__main__":
    install()
