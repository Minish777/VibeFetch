# 🌌 VibeFetch v1.0

![OS](https://img.shields.io/badge/OS-Windows%20%7C%20Linux%20%7C%20macOS-blue?style=for-the-badge)
![Fast](https://img.shields.io/badge/Speed-Instant-green?style=for-the-badge)
![Config](https://img.shields.io/badge/Config-JSON-yellow?style=for-the-badge)

VibeFetch — это твой личный, полностью настраиваемый инструмент для вывода системной информации. Никаких лишних зависимостей, только скорость и стиль.

### 🧩 Модули и Кастомизация
В `config.json` ты можешь менять как **названия** строк, так и их **значения**:

| JSON Ключ | Описание | Пример |
| :--- | :--- | :--- |
| `custom_name_os` | Имя поля ОС | `"My System"` |
| `custom_value_os` | Сама ОС | `"Super Linux 2.0"` |
| `custom_name_cpu` | Имя поля CPU | `"Nuclear Heart"` |
| `custom_value_cpu` | Модель проца | `"Xeon Platinum 9999"` |

### 🚀 Как запустить везде
1. **Windows:** `gcc main.c -o vfetch.exe -lkernel32 -ladvapi32`
2. **Linux:** `gcc main.c -o vfetch && chmod +x vfetch`
3. **Запуск:** `./vfetch`

---
**Digital Nickname:** `sodrely`
