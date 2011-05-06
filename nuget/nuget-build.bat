echo off
cls

:: Build the project
cd ..\
call releasebuild.bat

:: Remove the existing files
cd nuget
if exist blackbox.1.0.1.nupkg del blackbox.1.0.1.nupkg
if exist blackbox.1.0.1\lib rmdir /S /Q blackbox.1.0.1\lib
mkdir blackbox.1.0.1\lib
mkdir blackbox.1.0.1\lib\net40

:: Copy files
copy blackbox.nuspec blackbox.1.0.1\
copy ..\src\BlackBox\bin\Release\BlackBox.dll blackbox.1.0.1\lib\net40
copy ..\src\BlackBox\bin\Release\BlackBox.XML blackbox.1.0.1\lib\net40
copy ..\src\BlackBox\bin\Release\BlackBox.pdb blackbox.1.0.1\lib\net40
copy ..\LICENSE blackbox.1.0.1
copy ..\LICENSE.LESSER blackbox.1.0.1
copy ..\README blackbox.1.0.1

:: Build the NuGet package
nuget pack blackbox.1.0.1\blackbox.nuspec
