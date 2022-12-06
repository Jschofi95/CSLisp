namespace CSLisp
{
    public class Scanner
    {
        private readonly String source;
        private readonly List<Token> tokens = new List<Token>();
        private int current = 0;
        private int start = 0;
        private int line = 1;

        static readonly Dictionary<string, TokenType> keywords = new Dictionary<string, TokenType>
        {
            {"define", TokenType.DEFINE},
            {"set", TokenType.SET},
            {"cons", TokenType.CONS},
            {"cond", TokenType.COND},
            {"car", TokenType.CAR},
            {"cdr", TokenType.CDR},
            {"number?", TokenType.IS_NUMBER},
            {"symbol?", TokenType.IS_SYMBOL},
            {"list?", TokenType.IS_LIST},
            {"nil?", TokenType.IS_NIL},
            {"eq?", TokenType.IS_EQUAL}
        };

        public Scanner(String source)
        {
            this.source = source;
        }

        public List<Token> scanTokens()
        {
            while (!isAtEnd())
            {
                start = current;
                scanToken();
            }

            tokens.Add(new Token(TokenType.EOF, "", null, line));
            return tokens;
        }

        public void scanToken()
        {
            char c = advance();
            switch (c)
            {
                case '(':
                    addToken(TokenType.LEFT_PAREN);
                    break;
                case ')':
                    addToken(TokenType.RIGHT_PAREN);
                    break;
                case '=':
                    addToken(TokenType.EQUAL);
                    break;
                case '.':
                    addToken(TokenType.DOT);
                    break;
                case '-':
                    addToken(TokenType.MINUS);
                    break;
                case '+':
                    addToken(TokenType.PLUS);
                    break;
                case '*':
                    addToken(TokenType.STAR);
                    break;
                case '/':
                    addToken(TokenType.SLASH);
                    break;
                case '<':
                    addToken(match('=') ? TokenType.LESS_EQUAL : TokenType.LESS);
                    break;
                case '>':
                    addToken(match('=') ? TokenType.GREATER_EQUAL : TokenType.GREATER);
                    break;
                case '\n':
                    line++;
                    break;
                case '"':
                    scanString();
                    break;
                // Ignore meaningless characters like whitespace
                case ' ':
                case '\r':
                case '\t':
                    break;
                default:
                    if (isDigit(c)) { addNumber(); }
                    else if (isAlpha(c)) { addIdentifier(); }
                    else
                    {
                        Lisp.error(line, "Unexpected character.");
                    }
                    break;
            }
        }

        private Boolean isAtEnd() { return current >= source.Length; }

        private char advance() { return source[current++]; }

        private void addToken(TokenType type) { addToken(type, null); }

        private void addToken(TokenType type, Object? literal)
        {
            String text = source.Substring(start, current - start);
            tokens.Add(new Token(type, text, literal, line));
        }

        private Boolean match(char expected)
        {
            if (isAtEnd() || source[current] != expected) return false;

            current++;
            return true;
        }

        private void addNumber()
        {
            while (isDigit(peek())) advance();

            // Look for a fractional part
            if (peek() == '.' && isDigit(peekNext()))
            {
                // Consume the "."
                advance();

                while (isDigit(peek())) advance();
            }

            addToken(TokenType.NUMBER, Double.Parse(source.Substring(start, current - start)));
        }

        private void addIdentifier()
        {
            while (isAlphaNumeric(peek())) advance();

            String text = source.Substring(start, current - start);
            if (keywords.ContainsKey(text))
            {
                addToken(keywords[text]);
            }
            else
            {
                addToken(TokenType.IDENTIFIER);
            }
        }

        private void scanString()
        {
            while (peek() != '"' && !isAtEnd())
            {
                if (peek() == '\n') line++;
                advance();
            }

            if (isAtEnd())
            {
                Lisp.error(line, "Un-terminated string.");
                return;
            }

            // Trim the surrounding quotes
            String value = source.Substring(start + 1, current - start - 1);
            addToken(TokenType.STRING, value);

            advance();
        }


        // Like advance() but doesnt consume the character, called a lookahead
        private char peek()
        {
            if (isAtEnd()) return '\0';

            return source[current];
        }

        private char peekNext()
        {
            if (current + 1 > source.Length) return '\0';
            return source[current + 1];
        }

        private Boolean isDigit(char c)
        {
            return c >= '0' && c <= '9';
        }

        private Boolean isAlphaNumeric(char c)
        {
            return isAlpha(c) || isDigit(c);
        }

        private Boolean isAlpha(char c)
        {
            return (c >= 'a' && c <= 'z') ||
                    (c >= 'A' && c <= 'Z') ||
                    c == '_';
        }
    }
}