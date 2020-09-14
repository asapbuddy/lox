using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;


namespace AstGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Usage: AstGenerator <output_directory>");
                return;
            }

            var outputDir = args[0];
            var baseClass = "Expr";
            var childs = new List<string>
            {
                $"Binary : {baseClass} Left, Token Oper, {baseClass} Right",
                $"Grouping : {baseClass} Expression",
                $"Literal : object value",
                $"Unary : Token Oper, {baseClass} Right"
            };
            DefineAst(outputDir, "Expr", childs.AsReadOnly());
        }

        private static void DefineAst(string outputDir, string baseClass, ReadOnlyCollection<string> childs)
        {
            var path = $"{outputDir}/{baseClass}.cs";
            var lines = new List<string>
            {
                "namespace Cslox{",
                $"abstract class {baseClass}{{",
            };

            var visitor = defineVisitor(baseClass, childs);

            lines.Add(visitor);

            lines.Add($"public abstract T accept<T>(IVisitor<T> visitor);");

            lines.AddRange(
                from child in childs
                select child.Split(":")
                into split
                select (split[0].Trim(), split[1].Trim())
                into childClass
                select DefineType(baseClass, childClass)
            );


            lines.Add("}}");
            File.WriteAllLines(path, lines);
        }

        private static string defineVisitor(string baseClass, ReadOnlyCollection<string> childs)
        {
            var result =
                "public interface IVisitor<T> {";
            foreach (var child in childs)
            {
                var typename = child.Split(":")[0].Trim();
                result += $"T Visit{typename}{baseClass}({typename} {baseClass.ToLower()});";
            }

            result += "}";

            return result;
        }

        private static string DefineType(string baseClass, (string className, string fields) childClass)
        {
            var result =
                $"public class {childClass.className} : {baseClass}" +
                "{" +
                $"\t {childClass.className} ({childClass.fields})" +
                "{";

            var splittedFields = childClass.fields.Split(", ");

            foreach (var splittedField in splittedFields)
            {
                var name = splittedField.Split(" ")[1];
                result += $"this.{name} = {name};";
            }

            result += "}";

            result +=
                $"public override T accept<T>(IVisitor<T> visitor) => visitor.Visit{childClass.className}{baseClass}(this);";

            result += string.Join("\n",
                from field in splittedFields
                select $"public {field};"
            );
            result += "}";


            return result;
        }
    }
}