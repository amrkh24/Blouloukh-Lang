using System;
using System.Collections.Generic;

namespace Blolokh.Compiler
{
    public class Lexer
    {
        private readonly string _source;
        private readonly List<Token> _tokens = new List<Token>();
        private int _start = 0, _current = 0, _line = 1;
        public Lexer(string source) { _source = source; }

        public List<Token> ScanTokens()
        {
            while (!IsAtEnd()) { _start = _current; ScanToken(); }
            _tokens.Add(new Token(TokenType.EOF, "", _line));
            return _tokens;
        }

        private void ScanToken()
        {
            char c = Advance();
            switch (c)
            {
                case '=': AddToken(TokenType.Assign); break;
                case '+': AddToken(TokenType.Plus); break;
                case '-': AddToken(TokenType.Minus); break;
                case '*': AddToken(TokenType.Star); break;
                case '/': AddToken(TokenType.Slash); break;
                case '(': AddToken(TokenType.LeftParen); break;
                case ')': AddToken(TokenType.RightParen); break;
                case '\n': AddToken(TokenType.NewLine); _line++; break;
                case ' ': case '\r': case '\t': break;
                default:
                    if (char.IsLetter(c)) Identifier();
                    else if (char.IsDigit(c)) Number();
                    break;
            }
        }

        private void Identifier()
        {
            while (char.IsLetterOrDigit(Peek())) Advance();
            string text = _source.Substring(_start, _current - _start);
            if (text == "ياض") AddToken(TokenType.ياض);
            else if (text == "هات") AddToken(TokenType.Print);
            else if (text == "هاتلي") AddToken(TokenType.Input);
            else if (text == "حاسب") AddToken(TokenType.If);
            else if (text == "وعلى") AddToken(TokenType.While);
            else if (text == "انزل") AddToken(TokenType.StartBlock);
            else if (text == "خلص") AddToken(TokenType.EndBlock);
            else AddToken(TokenType.Identifier);
        }
        private void Number() { while (char.IsDigit(Peek())) Advance(); AddToken(TokenType.Number); }
        private bool IsAtEnd() => _current >= _source.Length;
        private char Advance() => _source[_current++];
        private char Peek() => IsAtEnd() ? '\0' : _source[_current];
        private void AddToken(TokenType type) => _tokens.Add(new Token(type, _source.Substring(_start, _current - _start), _line));
    }
}