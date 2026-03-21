import os, platform, subprocess, shutil

def clean_old_paths():
    """Чистит мусорные записи vfetch в реестре Windows"""
    if platform.system() != "Windows": return
    import winreg
    try:
        with winreg.OpenKey(winreg.HKEY_CURRENT_USER, "Environment", 0, winreg.KEY_ALL_ACCESS) as key:
            path, _ = winreg.QueryValueEx(key, "Path")
            parts = [p for p in path.split(';') if "vfetch" not in p.lower() and "vibefetch" not in p.lower()]
            winreg.SetValueEx(key, "Path", 0, winreg.REG_EXPAND_SZ, ";".join(parts))
            print("🧹 Старые пути в PATH удалены.")
    except: pass

def build():
    for d in ['bin', 'obj', 'publish']:
        if os.path.exists(d): shutil.rmtree(d)
    
    system = platform.system()
    runtime = "win-x64" if system == "Windows" else "linux-x64"
    
    project = [f for f in os.listdir('.') if f.endswith('.csproj')]
    if not project: return print("❌ Нет .csproj")

    print(f"🔧 Сборка {runtime}...")
    subprocess.run(f'dotnet restore {project[0]}', shell=True)
    cmd = f'dotnet publish {project[0]} -c Release -r {runtime} --self-contained true -o publish'
    
    if subprocess.run(cmd, shell=True).returncode == 0:
        print("✅ Готово!")
        return True
    return False

if __name__ == "__main__":
    print("1. Build Only\n2. Clean PATH & Build & Install")
    choice = input("Выбор: ")
    if choice == "2": clean_old_paths()
    if build() and choice == "2":
        exe = "vfetch.exe" if platform.system() == "Windows" else "vfetch"
        for f in os.listdir("publish"):
            if (platform.system() == "Windows" and f.endswith(".exe")) or (platform.system() != "Windows" and "." not in f):
                shutil.move(os.path.join("publish", f), os.path.join("publish", exe))
                break
        subprocess.run([os.path.abspath(os.path.join("publish", exe)), "install"])
