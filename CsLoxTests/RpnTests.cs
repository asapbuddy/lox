using System;
using Cslox;
using NUnit.Framework;
using static Cslox.Expr;

namespace CsLoxTests
{
    public class RpnTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void PrinterTest()
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

        [Test]
        public void ConsiderTheSum()
        {
            var sumSixTwo = new Binary(new Literal(6), new Token(TokenType.Plus, "+", null), new Literal(2));
            var productSumSixTwo = new Binary(new Literal(3), new Token(TokenType.Star, "*", null), sumSixTwo);
            var diffProduct = new Binary(productSumSixTwo, new Token(TokenType.Minus, "-", null), new Literal(4));
            var leftGroup = new Grouping(diffProduct);
            var sumThreeSeven = new Binary(new Literal(3), new Token(TokenType.Plus, "+", null), new Literal(7));
            var final = new Binary(leftGroup, new Token(TokenType.Slash, "/", null), sumThreeSeven);

            var result = new RpnPrinter().Print(final);

            Assert.AreEqual(result, "3 6 2 + * 4 - 3 7 + /");
            Console.WriteLine(result);
        }
    }
}