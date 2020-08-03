using System;
using Cslox;
using NUnit.Framework;
using static Cslox.Expr;

namespace CsLoxTests
{
    public class Tests
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

        [Test]
        public void RpnPrinterTest()
        {
            var expression = new Binary
            (
                new Grouping(
                    new Binary(
                        new Literal(1),
                        new Token(TokenType.Plus, "+", null),
                        new Literal(2))
                ),
                new Token(TokenType.Star, "*", null),
                new Grouping(
                    new Binary(
                        new Literal(4),
                        new Token(TokenType.Minus, "-", null),
                        new Literal(3)
                    )
                )
            );
            var result = new RpnPrinter().Print(expression);

            Assert.AreEqual(result, "1 2 + 4 3 - *");
            Console.WriteLine(expression);
        }
    };
}