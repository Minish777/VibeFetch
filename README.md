![Version](https://img.shields.io/badge/version-1.9.0-blue)
![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=.net&logoColor=white)
![Platform](https://img.shields.io/badge/platform-Windows%20%7C%20Linux%20%7C%20macOS-lightgrey)
![License](https://img.shields.io/badge/license-MIT-green)
![Status](https://img.shields.io/badge/status-stable-brightgreen)

---

# ⚡ VibeFetch

> **Minimalist & aesthetic system information tool for terminal users**

---

## 🧾 Описание

**VibeFetch** — это сверхбыстрая утилита для отображения информации о системе, написанная на **C# (.NET 8)**.

Проект создан для тех, кто ценит:

* чистоту консоли
* минимализм
* высокую производительность

Никакого лишнего вывода — только полезная информация в эстетичной оболочке в стиле **Dracula Theme**.

---

## ✨ Особенности

* 🎨 **Dracula Theme** — тщательно подобранная цветовая палитра (HEX), совместимая с современными терминалами
* 🚀 **High Performance** — написан на **.NET 8**, оптимизирован под x64
* 🛠 **Auto-Installer (Windows)** — Python-скрипт:

  * автоматически добавляет программу в PATH
  * упрощает установку
  * избавляет от ручной настройки

---

## 📦 Установка

### 🪟 Windows (рекомендуется)

1. Скачайте последнюю версию из **Releases**
2. Поместите в одну папку:

   * `vfetch.exe`
   * `install.py`
3. Запустите:

```bash
python install.py
```

4. Перезапустите терминал
5. Используйте:

```bash
vfetch
```

---

### 🐧 Linux / 🍎 macOS

Если вы собираете из исходников:

```bash
dotnet publish -c Release -r linux-x64 --self-contained true /p:PublishSingleFile=true
```

После сборки бинарник будет находиться здесь:

```
bin/Release/net8.0/linux-x64/publish/
```

Установка:

```bash
sudo cp ./bin/Release/net8.0/linux-x64/publish/vfetch /usr/local/bin/
sudo chmod +x /usr/local/bin/vfetch
```

---

## 🛠 Сборка из исходников

Требуется:

* **.NET 8 SDK**

### Windows

```bash
dotnet publish -r win-x64 -c Release --self-contained true /p:PublishSingleFile=true /p:PublishTrimmed=true
```

### Linux

```bash
dotnet publish -r linux-x64 -c Release --self-contained true /p:PublishSingleFile=true
```

---

## 📜 История обновлений

### v1.9.0 (текущая)

* ⚡ **Smart Refresh**
  Добавлена функция `refresh_env()` — Windows мгновенно обновляет PATH без перезагрузки

* 🧠 **Registry Overdrive**
  Прямое редактирование реестра через Python для более надежной установки

---

### v1.8.5

* 🌍 **Cross-OS Logic**
  Исправлены критические ошибки запуска на Linux (связанные с WinAPI)

* 📊 **UI Polish**
  Добавлена полоса прогресса для отображения оперативной памяти

---

### v1.7.0 — v1.8.0

* 🔄 Переход с **C → C#**
* 🌐 Добавлена проверка обновлений через GitHub API

---

### v1.0.0

* 🚀 Первая стабильная версия на языке **C**

---

## 🤝 Авторы

Создано:

* **sodrely**
* Gemini AI

---

## ⚠️ Примечание

Linux и macOS поддержка пока не полностью протестирована.
Если вы обнаружите ошибки или нестабильную работу — создайте issue или сообщите о проблеме. Это поможет улучшить проект.

---
