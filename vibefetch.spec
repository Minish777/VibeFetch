%define debug_package %{nil}
%define __strip /bin/true
%define _build_id_links none

# Запрещаем любые проверки зависимостей на корню
%define _use_internal_dependency_generator 0
AutoReqProv: no
%global __find_provides %{nil}
%global __find_requires %{nil}

Name:           vibefetch
Version:        1.9.0
Release:        1
Summary:        A stylish system fetch utility
License:        MIT

Source0:        vfetch

%description
VibeFetch v1.9.0.

%prep
# Просто создаем пустую папку, чтобы rpmbuild не ругался
mkdir -p %{_builddir}/%{name}-%{version}

%build
:

%install
mkdir -p %{buildroot}%{_bindir}
# Копируем напрямую из SOURCES, минуя все промежуточные этапы
cp %{_sourcedir}/vfetch %{buildroot}%{_bindir}/vibefetch
chmod 755 %{buildroot}%{_bindir}/vibefetch

%files
# Указываем файл напрямую
/usr/bin/vibefetch
