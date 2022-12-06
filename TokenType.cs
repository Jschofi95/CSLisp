namespace CSLisp
{
    public enum TokenType
    {
        // Operators
        PLUS, MINUS, STAR, SLASH, EQUAL, LESS, GREATER,
        LESS_EQUAL, GREATER_EQUAL,

        // Single-character tokens.
        LEFT_PAREN, RIGHT_PAREN, DOT,

        // Literals
        STRING, NUMBER, IDENTIFIER,

        // Keywords
        DEFINE, SET, CONS, COND, CAR, CDR, IS_NUMBER,
        IS_SYMBOL, IS_LIST, IS_NIL, IS_EQUAL,

        EOF
    }
}

