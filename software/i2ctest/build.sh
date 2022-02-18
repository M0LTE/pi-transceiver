#!/bin/sh -e

dotnet publish -r osx-x64 --sc -o macos -v q --nologo
dotnet publish -r linux-arm --sc -o pi -v q --nologo
