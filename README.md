# ⚡ VibeFetch (v2.0 Umad)

![Version](https://img.shields.io/badge/version-2.0.0--Umad-bd93f9?style=for-the-badge)
![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?style=for-the-badge&logo=.net&logoColor=white)
![Platform](https://img.shields.io/badge/platform-Windows%20%7C%20Linux%20%7C%20CachyOS-8be9fd?style=for-the-badge)
![License](https://img.shields.io/badge/license-MIT-50fa7b?style=for-the-badge)

**VibeFetch** — это эстетичный, молниеносный и умный системный информатор, написанный на **C# (.NET 8)**. Он создан для терминальных джанки, которые ценят палитру **Dracula Theme** и хотят знать реальное состояние своего железа без лишнего мусора.

Это не просто скриншот, это живые данные вашей системы.

---

## 🐲 Что нового в версии 2.0 (Umad)

* 🧠 **Честный RAM**: Больше никаких фейковых данных. Программа реально опрашивает Windows (через WMI) и Linux (через `/proc/meminfo`), показывая точное использование оперативной памяти.
* 💥 **НОВАЯ ФИЧА — Диски!**: Добавлена строка `disk`, показывающая использование главного системного раздела (в ГБ). Теперь вы знаете, сколько места осталось под игры.
* 🚀 **Linux-Native**: Глубокое определение дистрибутива. Вместо "Linux/macOS" вы увидите гордое "CachyOS" или "Arch Linux".
* 🔄 **Умное Автообновление**: Флаг `--update` переписан для большей надежности на кроссплатформенных системах.

---

## ✨ Ключевые фишки

* 🎨 **Dracula Aesthetics** — идеально подобранные ANSI-цвета, совместимые с TrueColor терминалами.
* 🚀 **High Performance** — компиляция в Single File, мгновенный запуск.
* 🔄 **Self-Update** — встроенная система обновления одной командой прямо из терминала.
* 🛠 **Smart Installer** — универсальный Python-скрипт с поддержкой `PATH` (Windows) и `sudo` (Linux).

---

## 📦 Установка

### 🪟 Windows (Powershell/CMD)
1. Скачайте `vfetch.exe` и `install.py` из [Releases](https://github.com/Minish777/VibeFetch/releases).
2. Запустите установщик:
   ```bash
   python install.py

   Откройте новое окно терминала и просто введите vfetch.

🐧 Linux ( Other / Arch / Ubuntu)

Соберите бинарник и добавьте его в системный путь:
Bash

# Убедитесь, что установлен dotnet-sdk
dotnet publish -c Release -r linux-x64 --self-contained true /p:PublishSingleFile=true
sudo cp ./bin/Release/net8.0/linux-x64/publish/vfetch /usr/local/bin/
sudo chmod +x /usr/local/bin/vfetch

🔄 Обновление

Забудьте про ручное скачивание новых версий. Просто введите команду:
Bash

vfetch --update

Программа автоматически проверит наличие новой версии на GitHub, скачает актуальный бинарник под вашу ОС и заменит старый файл.
🤝 Команда проекта

    sodrely — ведущий разработчик, идейный вдохновитель.

    Gemini AI — архитектура кроссплатформенности, логика автообновления.

⚠️ Статус проекта

Версия v2.0 стабильна для Windows 10/11 и CachyOS. Если вы нашли баг на другой системе — пожалуйста, создайте Issue.
