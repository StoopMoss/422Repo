HW_NUM=5
HW_DLL=build/Hw_$(HW_NUM)_lib.dll
TESTS_DLL=build/Hw_$(HW_NUM)_tests.dll


.PHONY: all
all: clean build test


.PHONY: build
build: $(HW_DLL)

.PHONY: test
test: $(TESTS_DLL)
	echo
	echo "Compliling Tests..."	
	- nunit-console -noxml $(TESTS_DLL)

.PHONY: zip
zip: hw$(HW_NUM).zip
hw$(HW_NUM).zip: src/*.cs tests/*.cs
	zip -FSj -9 hw$(HW_NUM).zip src/*.cs tests/*.cs

.PHONY: clean
clean: 
	echo
	echo "Cleaning..."
	- rm -f build/* hw$(HW_NUM).zip


$(HW_DLL):
	echo
	echo "Building library..."
	mkdir -p build
	mcs -target:library -out:$@ -debug src/*.cs

$(TESTS_DLL): test/*.cs $(HW_DLL)
	echo
	echo "Building test library..."
	mcs -r:$(HW_DLL) -pkg:mono-nunit -target:library -out:$@ -debug test/*.cs	
