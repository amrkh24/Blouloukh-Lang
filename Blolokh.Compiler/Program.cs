using System;

namespace Blolokh.Compiler
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("أدخل رقم العملية لتجربة هاتلي:");
            string code = "ياض س = (هاتلي + 5) * 2\nهات س";
            Lexer lexer = new Lexer(code);
            Parser parser = new Parser(lexer.ScanTokens());
            parser.Parse();
            Console.ReadKey();
        }
    }
}