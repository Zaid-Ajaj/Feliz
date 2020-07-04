@echo off
cls

dotnet tool restore
dotnet fake build %*
