using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Compiler.Core.Parsing;

namespace Compiler.Frontend
{
    class Program
    {
        static void Main(string[] args)
        {
            //here we will load the grammer file and generate our code

            var grammerFile = File.ReadAllLines("./Grammer/cp.grammer");

            var sb = new StringBuilder();

            sb.AppendLine("using System;\nusing System.Collections.Generic;\nusing Compiler.Frontend.Ast;\nusing static Compiler.Core.Parsing.TokenType;");
            sb.AppendLine("namespace Compiler.Frontend.Grammer\n{\n    public class Rules\n    {");
            
            sb.AppendLine(
                "  public static List<List<(object[], Func<AstNode[], AstNode>)>> PassRules =\n            new List<List<(object[], Func<AstNode[], AstNode>)>>\n            {");

            var enumNames = Enum.GetNames(typeof(TokenType));

            var passed = false;

            foreach (var s in grammerFile)
            {
                if (s.Trim().StartsWith("//")) continue;
                if (string.IsNullOrEmpty(s.Trim())) continue;

                if (s.Trim().StartsWith("PASS"))
                {
                    if (!passed)
                    {
                        passed = true;
                    }
                    else
                    {
                        sb.AppendLine("},");
                    }


                    sb.AppendLine("   new List<(object[], Func<AstNode[], AstNode>)>\n   {");
                }
                else
                {
                    var segs = Regex.Split(s, "\\=\\>");
                    var parts = segs.Last().Split(',');

                    sb.Append("(new object[] {");

                    var ags = new Dictionary<string, string>();

                    for (var i = 0; i < parts.Length; i++)
                    {
                        var part = parts[i];
                        if (part.Trim() == "") continue;

                        part = part.Trim();

                        if (part.Contains("["))
                        {
                            var prop = part.Split(']')[0].TrimStart('[');
                            var name = part.Split(']').Last();

                            ags.Add(prop, "objs[" + i + "]");
                            if (enumNames.Contains(name))
                            {
                                sb.Append(name + ",");
                            }
                            else
                            {
                                sb.Append("typeof(" + name + "),");
                            }
                        }
                        else
                        {
                            if (enumNames.Contains(part))
                            {
                                sb.Append(part + ",");
                            }
                            else
                            {
                                sb.Append("typeof(" + part + "),");
                            }
                        }
                    }

                    sb.AppendLine("},");

                    if (segs.Length == 2)
                    {
                        sb.Append(" (objs) => new " + segs[0] + "() {");

                        foreach (var (key, value) in ags)
                        {
                            sb.Append($"{key} = {value},");
                        }

                        sb.AppendLine("}),");
                    }

                    if (segs.Length == 3)
                    {
                        sb.Append("  (objs) => new " + segs[0]);

                        var subName = segs[1].Split(']')[1];
                        var subProp = segs[1].Split(']')[0].Trim().TrimStart('[');

                        sb.Append("  {" + subProp + " = new " + subName + " {");

                        foreach (var (key, value) in ags)
                        {
                            sb.Append($"{key} = {value},");
                        }

                        sb.AppendLine("}}),");
                    }
                }
            }

            sb.AppendLine("}");

            sb.AppendLine("   };");
            sb.AppendLine("    }\n}");
            File.WriteAllText("./Grammer/cp_gen.cs", sb.ToString());
        }
    }
}