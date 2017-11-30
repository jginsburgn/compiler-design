/*
Author:
	Jonathan Ginsburg A01021617
*/

using System;
using System.Collections.Generic;

namespace Trillian {

	class Parser {

		IEnumerator<Token> tokenStream;

		public Parser(IEnumerator<Token> tokenStream) {
			this.tokenStream = tokenStream;
			this.tokenStream.MoveNext();
		}

		public TokenCategory CurrentToken {
			get { return tokenStream.Current.Category; }
		}

		public Token Expect(TokenCategory category) {
			if (Environment.GetEnvironmentVariable("VERBOSE") == "true") {
				Console.WriteLine("Expecting: " + tokenStream.Current.Lexeme);
				Console.WriteLine(System.Environment.StackTrace);
			}
			if (CurrentToken == category) {
				if (Environment.GetEnvironmentVariable("verbose") == "true") {
					Console.WriteLine("Consuming: " + tokenStream.Current.Lexeme);
				}
				Token current = tokenStream.Current;
				tokenStream.MoveNext();
				return current;
			}
			else {
				if (Environment.GetEnvironmentVariable("verbose") == "true") {
					Console.WriteLine("Not consuming: " + tokenStream.Current.Lexeme);
				}
				throw new SyntaxError(category, tokenStream.Current);
			}
		}

		// Grammar entry point
		public Node Program() {
			Node retVal = Max();
			Expect(TokenCategory.EOF);
			return retVal;
		}

		public Node Max() {
			Node retVal = Simple();
			if (CurrentToken == TokenCategory.BANG) {
				NMax nMax = new NMax();
				nMax.Add(retVal);
				retVal = nMax;
			}
			while (CurrentToken == TokenCategory.BANG) {
				Expect(TokenCategory.BANG);
				retVal.Add(Simple());
			}
			return retVal;
		}

		public Node Simple() {
			switch (CurrentToken) {
				case TokenCategory.LITERAL: {
					NLiteral nLiteral = new NLiteral();
					nLiteral.AnchorToken = Expect(TokenCategory.LITERAL);
					return nLiteral;
				}
				case TokenCategory.ASTERISK: {
					NDuplicate nDuplicate = new NDuplicate();
					nDuplicate.AnchorToken = Expect(TokenCategory.ASTERISK);
					nDuplicate.Add(Simple());
					return nDuplicate;
				}
				case TokenCategory.SQUARE_BRACKET_LEFT: {
					NSummation nSummation = new NSummation();
					nSummation.AnchorToken = Expect(TokenCategory.SQUARE_BRACKET_LEFT);
					MaxList(nSummation);
					Expect(TokenCategory.SQUARE_BRACKET_RIGHT);
					if (nSummation.children.Count == 1) {
						return nSummation[0];
					}
					return nSummation;
				}
				default: {
					Console.WriteLine(CurrentToken);
					throw new Exception("This code should never be reached.");
				}
			}
		}

		public void MaxList(Node parent) {
			parent.Add(Max());
			while (CurrentToken == TokenCategory.COMMA) {
				Expect(TokenCategory.COMMA);
				parent.Add(Max());
			}
		}
	}
}
