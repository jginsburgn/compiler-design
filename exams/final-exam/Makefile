MCS=mcs

all: source/CILGenerator.cs source/Driver.cs source/Node.cs source/Parser.cs source/Scanner.cs source/SpecificNodes.cs source/SyntaxError.cs source/Token.cs source/TokenCategory.cs
	$(MCS) -out:trillian.exe source/*.cs

test:
	mono trillian.exe sample-inputs/test.trillian
	ilasm output.il
	mono output.exe

clean:
	rm trillian.exe
	rm output.il
	rm output.exe
