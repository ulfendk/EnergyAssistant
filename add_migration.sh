#!/bin/bash
if [ $# -eq 0 ]
  then
    echo "Missing migration name"
    exit 1
fi
basedir=`dirname "$0"`
pushd "$basedir/src/EnergyAssistantLib"
dotnet ef migrations add "$1"
popd
