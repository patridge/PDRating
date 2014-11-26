MDTOOL ?= /Applications/Xamarin\ Studio.app/Contents/MacOS/mdtool

.PHONY: all clean

all: PDRating.dll PDRating-Classic.dll

PDRating-Classic.dll:
	$(MDTOOL) build -c:Release ./src/PDRating/PDRating-Classic.csproj
	mkdir -p ./samples/lib/classic
	mv ./src/PDRating/bin/classic/Release/* ./samples/lib/classic

PDRating.dll:
	$(MDTOOL) build -c:Release ./src/PDRating/PDRating.csproj
	mkdir -p ./samples/lib/unified
	mv ./src/PDRating/bin/unified/Release/* ./samples/lib/unified

clean:
	rm -rf ./src/PDRating/bin/ ./src/PDRating/obj/
	rm -rf ./samples/lib
