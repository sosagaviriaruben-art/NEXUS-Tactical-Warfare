@echo off
title NEXUS: Tactical Warfare (PENDIENTE TACTICAL SHOOTER) - 3D C#
echo =========================================================================
echo    INICIANDO NEXUS: TACTICAL WARFARE [PENDIENTE TACTICAL SHOOTER]
echo    Shooter Tactico 3D en C# - Motor Grafico Acelerado por Hardware
echo =========================================================================
echo.
cd /d "%~dp0\NexusTacticalWarfare.Game"
dotnet run -c Release --no-restore
if %ERRORLEVEL% NEQ 0 (
    echo.
    echo Ejecutando binario directo...
    bin\Debug\net10.0\NexusTacticalWarfare.exe
)
pause
