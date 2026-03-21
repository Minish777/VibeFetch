%define debug_package %{nil}
%define __strip /bin/true
%define _build_id_links none
%undefine _missing_build_ids_terminate_build
%define _use_internal_dependency_generator 0
AutoReqProv: no

Name:           vibefetch
Version:        1.9.0
Release:        1%{?dist}
Summary:        A stylish system fetch utility written in C#
License:        MIT

Source0:        vfetch

%description
VibeFetch v1.9.0. Built on CachyOS.

%prep
rm -rf %{_builddir}/%{name}-%{version}
mkdir -p %{_builddir}/%{name}-%{version}
cp %{_sourcedir}/vfetch %{_builddir}/%{name}-%{version}/

%build
:

%install
rm -rf %{buildroot}
mkdir -p %{buildroot}%{_bindir}
install -p -m 755 %{_builddir}/%{name}-%{version}/vfetch %{buildroot}%{_bindir}/vibefetch

%files
%defattr(-,root,root,-)
%{_bindir}/vibefetch

%changelog
* Sat Mar 21 2026 sodrely <sodrely@github.com> - 1.9.0-1
- Native CachyOS build
- Fixed binary corruption by disabling rpm-strip
