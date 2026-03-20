# 🚀 VibeFetch

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![C Standard](https://img.shields.io/badge/C-99-blue.svg)](https://en.wikipedia.org/wiki/C99)
[![Platform](https://img.shields.io/badge/Platform-Windows%20%7C%20Linux-lightgrey.svg)](#)

Minimalist system fetch tool for power users. Fast, clean, and written in pure C.

### 📊 Project Status & Stats

| Feature | Status | Compatibility |
| :--- | :---: | :--- |
| **CPU Info** | ✅ Ready | Windows, Linux |
| **User/Host Detection** | ✅ Ready | All Systems |
| **ANSI Colors** | 🎨 Active | Modern Terminals |
| **Memory Info** | 🛠 Beta | Linux Only |
| **Aesthetics** | ✨ 100% | High Vibe |

---

### 🛠 Installation

#### Windows (MSYS2 / MinGW)
```bash
gcc main.c -o vfetch.exe -lkernel32 -ladvapi32
./vfetch.exe