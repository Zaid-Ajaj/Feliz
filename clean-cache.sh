#!/usr/bin/env bash

# remove paket related cache
[ -e paket.lock ] && rm paket.lock
[ -e ./paket-files/paket.restore.cached ] && rm ./paket-files/paket.restore.cached

# remove dotnet related cache
rm -rf ./src/bin
rm -rf ./src/obj
rm -rf ./test/bin
rm -rf ./test/obj