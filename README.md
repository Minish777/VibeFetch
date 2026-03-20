# ⚡ VibeFetch

![Version](https://img.shields.io/badge/version-1.6.5-blueviolet?style=for-the-badge)
![Platform](https://img.shields.io/badge/platform-Windows%20%7C%20Linux%20%7C%20macOS-lightgrey?style=for-the-badge)
![License](https://img.shields.io/badge/license-MIT-green?style=for-the-badge)

Минималистичный системный fetch-инструмент с поддержкой TrueColor и умной проверкой обновлений.

## ✨ Особенности
* **Dracula Theme**: Идеально подобранные HEX-цвета (Cyan, Purple, Gray).
* **Cross-Platform**: Работает на Windows, Linux (CachyOS/Arch) и macOS.
* **Smart Update**: Проверяет наличие новой версии при запуске. Если обновлений нет — надпись исчезает, не мешая работе.

## 📦 Установка и добавление в PATH

### 🪟 Windows
1. Скачайте `vfetch.exe` из [Releases](https://github.com/Minish777/VibeFetch/releases).
2. Создайте папку, например `C:\bin`, и положите туда файл.
3. Нажмите `Win + R`, введите `sysdm.cpl`, перейдите в **Дополнительно** -> **Переменные среды**.
4. Найдите **Path** в "Переменных пользователя", нажмите "Изменить" и добавьте путь `C:\bin`.
5. Теперь команда `vfetch` работает в любом терминале.

### 🐧 Linux / 🍎 macOS
1. Скачайте бинарный файл.
2. Сделайте его исполняемым: `chmod +x vfetch`.
3. Переместите в PATH:
   ```bash
   sudo mv vfetch /usr/local/bin/
