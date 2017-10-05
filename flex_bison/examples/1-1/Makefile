#Jonathan Ginsburg. October 4, 2017.
#For use in macOS, to use in other UNIX-like OS's replace the gcc flag -ll with -lfl.

all: fb1-1.l
	flex fb1-1.l;
	gcc lex.yy.c -ll -o wc_imitation;

test:
	cat fb1-1.l | ./wc_imitation;

clean:
	rm wc_imitation;
	rm lex.yy.c;
