import os, platform, subprocess, shutil

def clean_system():
    """Полная очистка хвостов в Windows и Linux"""
    print("🧹 Начинаю глубокую очистку...")
    if platform.system() == "Windows":
        import winreg
        try:
            with winreg.OpenKey(winreg.HKEY_CURRENT_USER, "Environment", 0, winreg.KEY_ALL_ACCESS) as key:
                path, _ = winreg.QueryValueEx(key, "Path")
                # Удаляем все вхождения vfetch и VibeFetch
                new_path = ";".join([p for p in path.split(';') if "vfetch" not in p.lower() and "vibefetch" not in p.lower()])
                winreg.SetValueEx(key, "Path", 0, winreg.REG_EXPAND_SZ, new_path)
                print("✅ Windows PATH очищен.")
        except: pass
    else:
        # В Linux удаляем из /usr/local/bin
        if os.path.exists("/usr/local/bin/vfetch"):
            print("⚠ Требуется sudo для удаления старой версии из /usr/local/bin")
            os.system("sudo rm /usr/local/bin/vfetch")
    
    for d in ['bin', 'obj', 'publish']:
        if os.path.exists(d): shutil.rmtree(d)

def build():
    sys_name = platform.system()
    runtime = "win-x64" if sys_name == "Windows" else "linux-x64"
    print(f"🚀 Сборка под {runtime}...")
    
    cmd = f"dotnet publish -c Release -r {runtime} --self-contained true -o publish /p:PublishSingleFile=true"
    if subprocess.run(cmd, shell=True).returncode == 0:
        exe = "vfetch.exe" if sys_name == "Windows" else "vfetch"
        # Находим и переименовываем бинарник
        for f in os.listdir("publish"):
            if f.endswith(".exe") or ("." not in f and f != "vfetch"):
                shutil.move(os.path.join("publish", f), os.path.join("publish", exe))
                break
        return True
    return False

def install():
    sys_name = platform.system()
    src = os.path.join("publish", "vfetch.exe" if sys_name == "Windows" else "vfetch")
    
    if sys_name == "Windows":
        dest_dir = os.path.join(os.environ['LOCALAPPDATA'], "VibeFetch")
        os.makedirs(dest_dir, exist_ok=True)
        dest_file = os.path.join(dest_dir, "vfetch.exe")
        shutil.copy2(src, dest_file)
        
        # Добавляем в PATH
        import winreg
        with winreg.OpenKey(winreg.HKEY_CURRENT_USER, "Environment", 0, winreg.KEY_ALL_ACCESS) as key:
            path, _ = winreg.QueryValueEx(key, "Path")
            if dest_dir not in path:
                winreg.SetValueEx(key, "Path", 0, winreg.REG_EXPAND_SZ, f"{dest_dir};{path}")
        print(f"✅ Установлено в {dest_dir}. ПЕРЕЗАПУСТИ ТЕРМИНАЛ!")
    else:
        print(f"🚀 Установка в Linux...")
        os.system(f"sudo cp {src} /usr/local/bin/vfetch")
        os.system("sudo chmod +x /usr/local/bin/vfetch")
        print("✅ Установлено в /usr/local/bin/vfetch")

if __name__ == "__main__":
    print("--- VibeFetch Ultimate Builder ---")
    print("1. Просто собрать (Build)")
    print("2. Полная переустановка (Clean + Build + Install)")
    
    choice = input("Выбор: ")
    
    if choice == "2":
        clean_system()
        if build():
            install()
    else:
        if build():
            print("✅ Сборка готова в папке ./publish")
