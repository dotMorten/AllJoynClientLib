XCOPY ..\Release\DeviceProviders\DeviceProviders.winmd DeviceProviders\lib\uap10.0\ /Y
XCOPY ..\Release\DeviceProviders\DeviceProviders.dll DeviceProviders\runtimes\win10-x86\native\ /Y
XCOPY ..\x64\Release\DeviceProviders\DeviceProviders.dll DeviceProviders\runtimes\win10-x64\native\ /Y
XCOPY ..\ARM\Release\DeviceProviders\DeviceProviders.dll DeviceProviders\runtimes\win10-arm\native\ /Y
XCOPY dotMorten.AllJoyn.DeviceProviders.nuspec DeviceProviders\ /Y
XCOPY dotMorten.AllJoyn.DeviceProviders.targets DeviceProviders\build\native\ /Y
nuget pack DeviceProviders\dotMorten.AllJoyn.DeviceProviders.nuspec
RMDIR DeviceProviders /S /Q