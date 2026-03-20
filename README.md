# 🚀 VibeFetch v0.9.0

![Platform](https://img.shields.io/badge/Platform-Windows%20%7C%20Linux%20%7C%20macOS-orange?style=for-the-badge&logo=platformdotsh)
![Language](https://img.shields.io/badge/Language-C-blue?style=for-the-badge&logo=c)
![License](https://img.shields.io/badge/License-MIT-green?style=for-the-badge)
![Status](https://img.shields.io/badge/Status-Beta_0.9.0-red?style=for-the-badge)
![Customization](https://img.shields.io/badge/Custom-JSON_Config-blueviolet?style=for-the-badge)

**VibeFetch** — это сверхбыстрая альтернатива Fastfetch, написанная на чистом C для максимальной производительности на твоем железе (Xeon/GTX).

### 🌍 Поддержка систем
Инструмент универсален и компилируется под любую среду:
- **Windows:** Полная поддержка через Win32 API.
- **Linux:** Работает на Arch, Ubuntu, CachyOS и др.
- **macOS:** Нативная поддержка Darwin.

### ⚙️ Кастомизация через `config.json`
При первом запуске создается файл настроек. Используй префикс `custom_name_`, чтобы переименовать любой модуль:

```json
{
  "custom_name_os": "My OS",
  "custom_name_cpu": "Nuclear Processor",
  "custom_name_gpu": "Graphics Monster"
}
