# 🌌 VibeFetch (powered by Gemini)

![Version](https://img.shields.io/badge/Version-1.1.0-blueviolet?style=for-the-badge)
![Dev](https://img.shields.io/badge/Dev-sodrely%20%26%20Gemini-orange?style=for-the-badge)
![Platform](https://img.shields.io/badge/Platform-Windows%20%7C%20Linux%20%7C%20macOS-white?style=for-the-badge)

**VibeFetch** (или *VibeCode*) — это современный, эстетичный системный фетч, разработанный в коллаборации с нейросетью Gemini. Ориентирован на кастомизацию и скорость.

### 🌟 Ключевые фишки
- **Auto-Update Checker:** Проверяет новые релизы на GitHub прямо при запуске.
- **Gemini Engine Inside:** Оптимизированный код и чистый дизайн.
- **Deep JSON Config:** Настраивай каждое слово и каждое значение.
- **Mega Modules:** Выводит всё — от IP до уровня заряда батареи.

### ⚙️ Модули в `config.json`
Вы можете переименовать любой параметр:
- `custom_name_net`: Свой заголовок для IP.
- `custom_value_cpu`: Возможность скрыть свой Xeon и написать "NASA Supercomputer".

### 🛠 Сборка
```bash
gcc main.c -o vfetch.exe -lkernel32 -ladvapi32 -luser32
