XCOPY ..\AllJoynDeviceLib\bin\Release\AllJoynClientLib.dll package\lib\uap10.0\ /Y
XCOPY dotMorten.AllJoyn.AllJoynClientLib.nuspec package\ /Y
nuget pack package\dotMorten.AllJoyn.AllJoynClientLib.nuspec
RMDIR package /S /Q