# 🌌 VibeFetch

![Version](https://img.shields.io/badge/version-0.7.0-blueviolet)
![Platform](https://img.shields.io/badge/platform-Windows-blue)

A high-performance, customizable system fetch tool inspired by Fastfetch but built for speed in pure C.

### 📋 Features Matrix

| Feature | Support | Description |
| :--- | :--- | :--- |
| **Custom ASCII** | ✅ Full | Load any `.txt` logo via config |
| **Module Toggle** | ✅ Ready | Enable/Disable CPU, GPU, Disk |
| **Hardware** | ✅ Deep | Native Win32 API calls for Xeon/NVIDIA |
| **JSON Export** | 📅 v1.0 | Coming soon |

### ⚙️ Configuration (`config.vibe`)
You can customize the output without recompiling:
```bash
import: logo.txt  # Path to your ASCII art
cpu=true          # Show/Hide CPU
gpu=false         # Show/Hide GPU
