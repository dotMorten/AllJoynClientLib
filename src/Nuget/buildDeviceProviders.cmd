XCOPY ..\DeviceProviders\bin\Win32\Release\DeviceProviders\DeviceProviders.winmd DeviceProviders\lib\uap10.0\ /Y
XCOPY ..\DeviceProviders\bin\Win32\Release\DeviceProviders\DeviceProviders.dll DeviceProviders\runtimes\win10-x86\native\ /Y
XCOPY ..\DeviceProviders\bin\x64\Release\DeviceProviders\DeviceProviders.dll DeviceProviders\runtimes\win10-x64\native\ /Y
XCOPY ..\DeviceProviders\bin\ARM\Release\DeviceProviders\DeviceProviders.dll DeviceProviders\runtimes\win10-arm\native\ /Y
XCOPY dotMorten.AllJoyn.DeviceProviders.nuspec DeviceProviders\ /Y
nuget pack DeviceProviders\dotMorten.AllJoyn.DeviceProviders.nuspec
RMDIR DeviceProviders /S /Q