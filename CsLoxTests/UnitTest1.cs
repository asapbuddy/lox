using System;
using Cslox;
using NUnit.Framework;

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
            Expr expression = new Expr.Binary(
                new Expr.Unary(
                    new Token(TokenType.Minus, "-", null),
                    new Expr.Literal(123)),
                new Token(TokenType.Star, "*", null),
                new Expr.Grouping(
                    new Expr.Literal(45.67)));
            var result = new AstPrinter().Print(expression);

            Assert.AreEqual(result, "(* (- 123) (group 45,67))");
            Console.WriteLine(result);
        }
    }
}