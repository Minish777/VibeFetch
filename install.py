import os
import platform
import shutil
import subprocess
import sys

def install():
    current_os = platform.system()
    print(f"[*] Installing for: {current_os}")

    if current_os == "Windows":
        exe = "vfetch.exe"

        if not os.path.exists(exe):
            print(f"[-] {exe} not found")
            return

        dest = os.path.join(os.environ["LOCALAPPDATA"], "VibeFetch")
        os.makedirs(dest, exist_ok=True)

        shutil.copy2(exe, os.path.join(dest, exe))

        print("[+] Installed to:", dest)
        print("[!] Add this folder to PATH manually if needed")
        print("[+] Done. Restart terminal and run: vfetch")

    else:
        exe = "vfetch"

        if not os.path.exists(exe):
            print("[-] vfetch binary not found")
            return

        try:
            subprocess.run(["sudo", "cp", exe, "/usr/local/bin/vfetch"], check=True)
            subprocess.run(["sudo", "chmod", "+x", "/usr/local/bin/vfetch"], check=True)
            print("[+] Installed to /usr/local/bin")
        except Exception as e:
            print("[-] Install failed:", e)

if __name__ == "__main__":
    install()
