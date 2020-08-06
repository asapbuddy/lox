using System;
using Cslox;
using NUnit.Framework;
using static Cslox.Expr;

namespace CsLoxTests
{
    public class AstTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void AstPrinterTest()
        {
            Expr expression = new Binary(
                new Unary(
                    new Token(TokenType.Minus, "-", null),
                    new Literal(123)),
                new Token(TokenType.Star, "*", null),
                new Grouping(
                    new Literal(45.67)));
            var result = new AstPrinter().Print(expression);

            Assert.AreEqual(result, "(* (- 123) (group 45,67))");
            Console.WriteLine(result);
        }

        
    };
}