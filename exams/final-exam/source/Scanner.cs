/*
Author:
    Jonathan Ginsburg A01021617
*/

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Trillian {

    class Scanner {

        readonly string input;

        static readonly Regex regex = new Regex(
            @"
                (?<Literal>     -?[0-9]+(\.[0-9]+)?       )
              | (?<Asterisk>        [*]       )
              | (?<Bang>    [!]       )
              | (?<SquareBracketLeft>    [[]       )
              | (?<SquareBracketRight>   [\]]       )
              | (?<Comma>      [,]       )
              | (?<NewLine>     \n       )
              | (?<WhiteSpace> \s        )     # Must go anywhere after Newline.
              | (?<Unknown>      .         )     # Must be last: match any other character.
            ",
            RegexOptions.IgnorePatternWhitespace
                | RegexOptions.Compiled
                | RegexOptions.Multiline
            );

        public Scanner(string input) {
            this.input = input;
        }

        public IEnumerable<Token> Start() {

            var row = 1;
            var columnStart = 0;

            Func<Match, TokenCategory, Token> newTok = (m, tc) =>
                new Token(m.Value, tc, row, m.Index - columnStart + 1);

            foreach (Match m in regex.Matches(input)) {
                if (m.Groups["NewLine"].Success) {
                    // Found a new line.
                    row++;
                    columnStart = m.Index + m.Length;
                } 
                else if (m.Groups["WhiteSpace"].Success) {
                    // Skip white space and comments.
                } 
                else if (m.Groups["Literal"].Success) {
                    yield return newTok(m, TokenCategory.LITERAL);
                }
                else if (m.Groups["Asterisk"].Success) {
                    yield return newTok(m, TokenCategory.ASTERISK);
                }
                else if (m.Groups["Bang"].Success) {
                    yield return newTok(m, TokenCategory.BANG);
                }
                else if (m.Groups["SquareBracketLeft"].Success) {
                    yield return newTok(m, TokenCategory.SQUARE_BRACKET_LEFT);
                }
                else if (m.Groups["SquareBracketRight"].Success) {
                    yield return newTok(m, TokenCategory.SQUARE_BRACKET_RIGHT);
                }
                else if (m.Groups["Comma"].Success) {
                    yield return newTok(m, TokenCategory.COMMA);
                }
                else if (m.Groups["Unknown"].Success) {
                    // Found an illegal character.
                    yield return newTok(m, TokenCategory.UNKNOWN);
                }
                else {
                    // Supposedly unreachable code.
                    throw new Exception("This code should never be reached.");
                }
            }

            yield return new Token(null,
                                   TokenCategory.EOF,
                                   row,
                                   input.Length - columnStart + 1);
        }
    }
}
