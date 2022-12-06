namespace CSLisp {
    public class Token {
        public readonly TokenType type;
        public readonly String lexeme;
        public readonly Object? literal;
        public readonly int line;

        public Token(TokenType type, String lexeme, Object? literal, int line) {
            this.type = type;
            this.lexeme =  lexeme;
            if (literal != null) this.literal = literal;
            this.line = line;
        }

        public String toString() {
            return type + " " + lexeme + " " + literal;
        }
    }

}