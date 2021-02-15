;--------------------------------

; The name of the installer
Name "IBIT"

; The file to write
OutFile "IBIT_Versao01_Instalador_Windows.exe"

; Build Unicode installer
Unicode True

; The default installation directory
InstallDir "C:\Udesc\IBIT_1.0"

;--------------------------------



; Pages

Page components
Page directory
Page instfiles

UninstPage uninstConfirm
UninstPage instfiles

;--------------------------------


; The stuff to install
Section "I BLUE IT (required)"

  SectionIn RO

  ; Set output path to the installation directory.
  SetOutPath $INSTDIR

  ; Put file there
  File /r *.*

  ; Write the uninstall keys for Windows
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\IBIT" "DisplayName" "IBIT"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\IBIT" "UninstallString" '"$INSTDIR\Desinstalar.exe"'
  WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\IBIT" "NoModify" 1
  WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\IBIT" "NoRepair" 1
  WriteUninstaller "$INSTDIR\Desinstalar.exe"

SectionEnd

; Optional section (can be disabled by the user)
Section "Atalhos no Menu Iniciar e Desktop"

  CreateDirectory "$SMPROGRAMS\IBIT"
  CreateShortcut "$SMPROGRAMS\IBIT\Desinstalar.lnk" "$INSTDIR\Desinstalar.exe" "" "$INSTDIR\Desinstalar.exe" 0
  CreateShortcut "$SMPROGRAMS\IBIT\IBlueIt.lnk" "$INSTDIR\IBlueIt.exe" "" "$INSTDIR\IBlueIt.exe" 0
  CreateShortcut "$DESKTOP\IBlueIt.lnk" "$INSTDIR\IBlueIt.exe" "" "$INSTDIR\IBlueIt.exe" 0
  Delete $INSTDIR\scriptInstalador_NSIS.nsi

SectionEnd

;--------------------------------

; Uninstaller

Section "Uninstall"

  ; Remover registro
  DeleteRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\IBIT"
  DeleteRegKey HKLM SOFTWARE\NSIS_IBIT

  ; Remover desinstalador e todos os arquivos
  Delete $INSTDIR\Desinstalar.exe
  Delete $INSTDIR\*.*

  ; Remover atalhos relacionados
  Delete "$SMPROGRAMS\IBIT\*.*"
  Delete "$DESKTOP\IBlueIt.lnk"

  ; Remover diretorios
  RMDir "$SMPROGRAMS\IBIT"
  RMDir /r "$INSTDIR"

SectionEnd
