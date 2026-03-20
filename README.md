

---

# ⚡ VibeFetch

> **Minimalist & aesthetic system information tool for terminal junkies**

---

## 🧾 Описание

**VibeFetch** — это сверхбыстрая утилита для отображения информации о системе, написанная на **C# (.NET 8)**.

Проект создан для тех, кто ценит:

* чистоту консоли
* минимализм
* высокую производительность

Никакого мусора — только важная информация, оформленная в стильной оболочке в духе **Dracula Theme**.

---

## ✨ Особенности

* 🎨 **Dracula Theme** — тщательно подобранная цветовая палитра (HEX), совместимая с современными терминалами
* 🚀 **High Performance** — работает на **.NET 8**, оптимизирован под x64
* 🛠 **Auto-Installer** — Python-установщик:

  * автоматически редактирует реестр Windows
  * обновляет PATH
  * избавляет от ручной настройки
* 🐧 **Native Linux Support** — нативная работа на **Arch / CachyOS** через self-contained бинарники

---

## 📦 Установка

### 🪟 Windows (рекомендуется)

1. Скачайте последний релиз
2. Поместите:

   * `vfetch.exe`
   * `install.py`
     в одну папку
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

Сборка и установка:

```bash
dotnet publish -c Release -r linux-x64 --self-contained true /p:PublishSingleFile=true
sudo cp ./publish/vfetch /usr/local/bin/
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

### v1.9.0 *(текущая)*

* ⚡ **Smart Refresh**
  Добавлена функция `refresh_env()` — Windows мгновенно обновляет PATH без перезагрузки

* 🧠 **Registry Overdrive**
  Прямое редактирование реестра через Python для максимальной стабильности

---

### v1.8.5

* 🌍 **Cross-OS Logic**
  Исправлены критические ошибки запуска на Linux (WinAPI-конфликты)

* 📊 **UI Polish**
  Добавлена полоса прогресса для отображения RAM

---

### v1.7.0 — v1.8.0

* 🔄 Переход с **C → C#**
* 🌐 Добавлена проверка обновлений через GitHub API

---

### v1.0.0

* 🚀 Первая стабильная версия на **C**

---

## 🤝 Авторы

Создано:

* **sodrely**
* Gemini AI

---
