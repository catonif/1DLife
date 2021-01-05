@echo off
rem 1DLife builder
cd /d %~dp0
dotnet build
rd /s /q obj