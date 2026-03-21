import os
import sys
import platform
import shutil
import subprocess

def install():
    current_os = platform.system()
    print(f"[*] Starting universal installation for: {current_os}")

    if current_os == "Windows":
        try:
            import winreg
            exe = "vfetch.exe"
            if not os.path.exists(exe):
                print(f"[-] Error: {exe} not found in current directory.")
                return

            dest = os.path.join(os.environ["LOCALAPPDATA"], "VibeFetch")
            os.makedirs(dest, exist_ok=True)
            shutil.copy2(exe, os.path.join(dest, exe))

            # Add to User PATH
            with winreg.OpenKey(winreg.HKEY_CURRENT_USER, "Environment", 0, winreg.KEY_ALL_ACCESS) as key:
                try:
                    path_val, _ = winreg.QueryValueEx(key, "Path")
                    if dest not in path_val:
                        winreg.SetValueEx(key, "Path", 0, winreg.REG_EXPAND_SZ, f"{path_val};{dest}")
                except FileNotFoundError:
                    winreg.SetValueEx(key, "Path", 0, winreg.REG_EXPAND_SZ, dest)
            print("[+] Done! Please restart your terminal and type 'vfetch'.")
        except Exception as e:
            print(f"[-] Windows install failed: {e}")

    elif current_os == "Linux" or current_os == "Darwin": # Darwin - это macOS
        exe = "vfetch"
        if not os.path.exists(exe):
            print(f"[-] Error: {exe} binary not found.")
            return

        print("[*] Requesting sudo to copy binary to /usr/local/bin...")
        try:
            subprocess.run(["sudo", "cp", exe, "/usr/local/bin/vfetch"], check=True)
            subprocess.run(["sudo", "chmod", "+x", "/usr/local/bin/vfetch"], check=True)
            print("[+] Successfully installed to /usr/local/bin/vfetch")
        except Exception as e:
            print(f"[-] Linux install failed: {e}")

if __name__ == "__main__":
    install()
