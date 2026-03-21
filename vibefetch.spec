# 1. Отключаем абсолютно все автоматические проверки и обработки
%define debug_package %{nil}
%define __strip /bin/true
%define _build_id_links none

# Фикс для ошибки "External dependency generator is incompatible with v6 packages"
# Мы обнуляем макрос, который запускает сбойные скрипты анализа
%define __spec_install_post %{nil}
%define _use_internal_dependency_generator 0
%global __find_provides %{nil}
%global __find_requires %{nil}

# Параметры пакета
Name:           vibefetch
Version:        1.9.0
Release:        1
Summary:        A stylish system fetch utility written in C#
License:        MIT
Group:          Applications/System

# Файл vfetch должен лежать в ~/rpmbuild/SOURCES/
Source0:        vfetch

%description
VibeFetch v1.9.0. Optimized for CachyOS and built natively.
No dependencies, single-file binary.

%prep
# Авто-чистка от виндовых хвостов (\r) прямо во время сборки
# Мы применяем это ко всем файлам в SOURCES и к самому спеку
sed -i 's/\r$//' %{_sourcedir}/* || :
# Создаем чистую рабочую директорию
rm -rf %{_builddir}/%{name}-%{version}
mkdir -p %{_builddir}/%{name}-%{version}

%build
# Пропускаем, так как бинарник уже скомпилирован
:

%install
# Создаем структуру директорий в образе пакета
rm -rf %{buildroot}
mkdir -p %{buildroot}%{_bindir}

# Копируем бинарник и принудительно ставим права на запуск
cp %{_sourcedir}/vfetch %{buildroot}%{_bindir}/vibefetch
chmod 755 %{buildroot}%{_bindir}/vibefetch

%files
# Просто указываем путь к файлу в системе
%defattr(-,root,root,-)
%{_bindir}/vibefetch

%changelog
* Sat Mar 21 2026 sodrely <sodrely@github.com> - 1.9.0-1
- Absolute bypass of RPM dependency generators
- Added auto-cleanup for CRLF line endings
- Native CachyOS build fix
