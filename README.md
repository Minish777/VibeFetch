# ⚡ VibeFetch

![Version](https://img.shields.io/badge/version-1.5.2-blueviolet?style=for-the-badge)
![Platform](https://img.shields.io/badge/platform-Windows%20%7C%20Linux-lightgrey?style=for-the-badge)
![License](https://img.shields.io/badge/license-MIT-green?style=for-the-badge)

Минималистичный и быстрый системный fetch-инструмент с идеальной палитрой. Создан для тех, кто ценит эстетику консоли и мгновенный отклик.

## ✨ Особенности
* **Dracula Palette**: Сбалансированные цвета (Cyan, Purple, Gray), которые не режут глаза в любом терминале.
* **Visual RAM Bar**: Графический индикатор загрузки памяти для твоего конфига.
* **Ultra Lightweight**: Один `.exe` файл, который не требует установки гигабайтов мусора.
* **Update Notifier**: Автоматическая проверка новых релизов на GitHub при каждом запуске.

## 📦 Установка и запуск
1. Скачайте `vfetch.exe` из раздела **Releases**.
2. Поместите файл в любую папку (например, `C:\bin`).
3. Добавьте путь к этой папке в системную переменную **PATH**.
4. Напишите `vfetch` в CMD или PowerShell.

## 🛠 Сборка из исходников
Для самостоятельного билда (требуется .NET 8.0 SDK):
```bash
dotnet publish -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true /p:PublishTrimmed=true
```
Данное ПО распространяется под лицензией MIT. Вы можете свободно изменять, копировать и использовать код в любых целях.

Developed with effort by sodrely & Gemini 🤝

## 📝 Обновления

### v1.5.3 (Current)
* **TrueColor Evolution**: Полный переход на 24-битную HEX-палитру для идеальной цветопередачи в стиле Dracula.
* **VT-Mode Fix**: Исправлена инициализация виртуального терминала, теперь цвета работают корректно даже в старой CMD.
* **UI Polish**: Финальная калибровка отступов и выравнивания элементов интерфейса.

### v1.5.2
* **Formatting**: Настройка позиционирования текста относительно ASCII-логотипа.
* **Bugfix**: Устранены ошибки компиляции статических функций в C# коде.

### v1.5.0
* **C# Migration**: Проект полностью переписан на C# для лучшей совместимости с Windows.
* **New Aesthetic**: Добавлен новый логотип "V" и визуальный индикатор RAM.

### v1.2.0
* **Go Experiment**: Попытка реализации на языке Go (версия признана нестабильной из-за проблем с импортами).

### v1.0.0 (Legacy)
* **The Beginning**: Изначальная версия на языке **C**. Максимальный минимализм и прямое взаимодействие с WinAPI.
