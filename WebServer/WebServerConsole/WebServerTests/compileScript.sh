#!/bin/bash

mcs -r:$(HW_DLL) -pkg:mono-nunit -target:library -out:$@ -debug test/*.cs
