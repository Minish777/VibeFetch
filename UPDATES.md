# 📝 VibeFetch Update Log

### [v0.9.0] - 2026-03-20
**Добавлено:**
- **JSON Config:** Полный переход на `config.json`. Создается автоматически при первом запуске.
- **Custom Labels:** Реализована логика `custom_name_`. Если в JSON прописано `"custom_name_os": "My Distro"`, то вместо "OS" выведется "My Distro".
- **Cross-Platform:** Добавлена корректная обработка для **Linux**, **macOS** и **Windows** в одном файле.
- **Shields Overload:** README теперь выглядит как серьезный Open Source проект.

**Исправлено:**
- Ошибки линковки `sys/utsname.h` на Windows (теперь через условия `#ifdef`).
- Исправлен `Segmentation fault` при отсутствии файла конфигурации.
