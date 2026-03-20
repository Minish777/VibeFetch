# 🚀 VibeFetch v1.2.0

![OS Detection](https://img.shields.io/badge/OS_Detect-Smart-success?style=for-the-badge)
![Shell Detection](https://img.shields.io/badge/Shell-Auto-blue?style=for-the-badge)

**VibeFetch** теперь официально "умнее". Программа не просто пишет "Windows", а определяет билд твоей системы и реально используемую оболочку (Shell).

### ✨ Что нового:
- **Умный конфиг:** Если мы добавим новые фишки в v1.3.0, твой `config.json` обновится сам, не затирая твои кастомные имена.
- **Точность:** Больше никаких "10/11". Только конкретика.

### 🛠 Сборка
```bash
gcc main.c -o vfetch.exe -lkernel32 -ladvapi32 -luser32 -lntdll
