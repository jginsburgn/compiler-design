#Jonathan Ginsburg. October 4, 2017.
#For use in macOS, to use in other UNIX-like OS's replace the gcc flag -ll with -lfl.

all: fb1-2.l
	flex fb1-2.l;
	gcc lex.yy.c -ll -o english_to_american;

test:
	cat test_input | ./english_to_american;

clean:
	rm english_to_american;
	rm lex.yy.c;
