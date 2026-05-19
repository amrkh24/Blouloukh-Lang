using System;
using System.Collections.Generic;

namespace Blolokh.Compiler
{
    public class Parser
    {
        private readonly List<Token> _tokens;
        private int _current = 0;
        private Dictionary<string, int> _variables = new Dictionary<string, int>();

        public Parser(List<Token> tokens) { _tokens = tokens; Console.OutputEncoding = System.Text.Encoding.UTF8; }

        public void Parse() { while (!IsAtEnd()) ParseStatement(); }

        private void ParseStatement()
        {
            if (Match(TokenType.NewLine)) return;
            if (Match(TokenType.ياض)) 
            { 
                Token name = Advance(); 
                Match(TokenType.Assign);
                _variables[name.Lexeme] = Expression(); 
            }
            else if (Match(TokenType.Print)) 
            { 
                Token name = Advance(); 
                if (_variables.ContainsKey(name.Lexeme)) Console.WriteLine(_variables[name.Lexeme]); 
            }
            else Advance();
        }

        private int Expression()
        {
            int expr = Term();
            while (Match(TokenType.Plus) || Match(TokenType.Minus))
            {
                TokenType op = Previous().Type;
                if (op == TokenType.Plus) expr += Term();
                else expr -= Term();
            }
            return expr;
        }

        private int Term()
        {
            int term = Factor();
            while (Match(TokenType.Star) || Match(TokenType.Slash))
            {
                TokenType op = Previous().Type;
                if (op == TokenType.Star) term *= Factor();
                else term /= Factor();
            }
            return term;
        }

        private int Factor()
        {
            if (Match(TokenType.LeftParen)) {
                int expr = Expression();
                Match(TokenType.RightParen);
                return expr;
            }
            if (Match(TokenType.Input)) {
                return int.Parse(Console.ReadLine());
            }
            Token next = Advance();
            if (next.Type == TokenType.Number) return int.Parse(next.Lexeme);
            return _variables.ContainsKey(next.Lexeme) ? _variables[next.Lexeme] : 0;
        }

        private bool Match(TokenType type) { if (Check(type)) { Advance(); return true; } return false; }
        private bool Check(TokenType type) => !IsAtEnd() && Peek().Type == type;
        private Token Advance() { if (!IsAtEnd()) _current++; return Previous(); }
        private bool IsAtEnd() => Peek().Type == TokenType.EOF;
        private Token Peek() => _tokens[_current];
        private Token Previous() => _tokens[_current - 1];
    }
}