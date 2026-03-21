%define debug_package %{nil}
%define __strip /bin/true
%define _build_id_links none

# Полная блокировка генератора зависимостей
%define _use_internal_dependency_generator 0
%global __find_provides %{nil}
%global __find_requires %{nil}
%define _binary_payload w9.gzdio

Name:           vibefetch
Version:        1.9.0
Release:        1
Summary:        A stylish system fetch utility
License:        MIT

Source0:        vfetch

%description
VibeFetch v1.9.0.

%prep
mkdir -p %{_builddir}/%{name}-%{version}
cp %{_sourcedir}/vfetch %{_builddir}/%{name}-%{version}/

%build
:

%install
mkdir -p %{buildroot}%{_bindir}
install -m 755 %{_builddir}/%{name}-%{version}/vfetch %{buildroot}%{_bindir}/vibefetch

%files
%{_bindir}/vibefetch
