@echo off
chcp 65001 >nul
echo ========================================================
echo   NEXUS Tactical Warfare - Sincronizar con GitHub
echo ========================================================
echo.
cd /d "%~dp0"
echo Verificando estado...
git status
echo.
echo Enviando cambios al repositorio remoto...
git push -u origin main --force
if %ERRORLEVEL% NEQ 0 (
    echo.
    echo Reintentando con ruta directa de Git...
    "%LOCALAPPDATA%\Programs\Git\cmd\git.exe" push -u origin main --force
)
echo.
echo ========================================================
echo Listo. Si se abrió el navegador, autoriza el inicio de sesión.
echo ========================================================
pause
