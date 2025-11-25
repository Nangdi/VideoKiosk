@echo off
color 1F
cd /d %~dp0
title NTP Setting

:menu
cls
echo 1. [Server] - NTP 서버 설정(NTP Server 기능 ON, w32time 서비스 자동화, 방화벽 인바운드 설정[UDP 123포트 연결 허용])
echo 2. [Client] - NTP 서버 설정 확인
echo 3. [Client] - NTP 서버 주소 변경(ex 192.168.0.1,0x9, 방화벽 아웃운드 설정[UDP 123포트 연결 허용])
echo 4. [Client] - 시간 허용치 동기화 주기 확인
echo 5. [Client] - 서비스 재시작(설정 초기화)



set /p choice=Enter your choice (1-5):

if "%choice%"=="1" goto NTP_Setting
if "%choice%"=="2" goto NTP_Check
if "%choice%"=="3" goto NTP_Server_Change
if "%choice%"=="4" goto Recheck_Settings
if "%choice%"=="5" goto Restart_Service

goto menu

:NTP_Setting
::-------------- Windows Time 서비스를 시작합니다. --------------------
net start w32time

::-------------- NTP Server 기능 ON --------------------
reg add "HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\W32Time\TimeProviders\NtpServer" /v "Enabled" /t REG_DWORD /d 1 /f

::-------------- w32time 서비스 자동화 --------------------
reg add "HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\W32Time\Config" /v "AnnounceFlags" /t REG_DWORD /d 5 /f

::-------------- 방화벽 인바운드 설정 (UDP 123포트 연결 허용) --------------------
netsh advfirewall firewall add rule name="MyNTPServer" protocol=UDP dir=in localport=123 action=allow enable=yes

::-------------- Windows Time 서비스를 멈춥니다. --------------------
net stop w32time

::-------------- Windows Time 서비스를 시작합니다. --------------------
net start w32time

pause
goto menu

:NTP_Check
::-------------- NTP서버 설정 확인 --------------------
w32tm /dumpreg /subkey:parameters
pause
goto menu

:NTP_Server_Change
::-------------- Windows Time 서비스를 시작합니다. --------------------
net start w32time

::-------------- NTP 서버 IP주소를 입력 하시오. --------------------
set /p server=NTP 서버 IP주소를 입력하시오. (ex 192.168.0.1) :

::-------------- NTPClient가 바라볼(접근하여 요청할) NTPServer의 IP를 설정한다. -------------------
w32tm /config /syncfromflags:manual /manualpeerlist:%server%,0x9 /reliable:yes /update

::-------------- 동기화 주기 변경 (10분 설정) -------------------
reg add "HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\W32Time\TimeProviders\NtpClient" /v "SpecialPollInterval" /t REG_DWORD /d 600 /f

::-------------- 시간 허용치 변경 (10분 설정) -------------------
reg add "HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\W32Time\Config" /v "MaxPosPhaseCorrection" /t REG_DWORD /d 600 /f

reg add "HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\W32Time\Config" /v "MaxNegPhaseCorrection" /t REG_DWORD /d 600 /f

netsh advfirewall firewall add rule name="MyNTPServer" protocol=UDP dir=out localport=123 action=allow enable=yes

::-------------- Windows Time 서비스가 자동으로 시작되도록 설정 --------------------
sc config w32time start= auto

::-------------- 재부팅 후 자동으로 시작되지 않는 문제가 해결되도록 설정 --------------------
sc triggerinfo w32time start/networkon stop/networkoff

::-------------- NTP서버로 부터 시간 동기화를 시작합니다. --------------------
w32tm /resync

pause
goto menu

:Recheck_Settings
::-------------- 시간동기화 설정 확인 -------------------
w32tm /query /configuration
pause
goto menu

:Restart_Service
::-------------- Windows Time 서비스를 멈춥니다. --------------------
net stop w32time

::-------------- W32Time Windows 시간 서비스의 등록을 취소하고 레지스트리에서 모든 구성 정보를 제거합니다. --------------------
w32tm /unregister

::-------------- W32Time 서비스로 실행할 Windows 시간 서비스를 등록하고 기본 구성 정보를 레지스트리에 추가합니다. --------------------
w32tm /register

::-------------- Windows Time 서비스를 시작합니다. --------------------
net start w32time

pause
goto menu
