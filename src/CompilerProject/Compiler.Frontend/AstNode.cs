using System;
using System.Collections.Generic;
using Compiler.Core.Parsing;

namespace Compiler.Frontend
{
    public class AstNode
    {
        public virtual AstNode Drain()
        {
            return this;
        }

        public Token Raw { get; set; }
        public Token ReportToken { get; set; }

        public List<string> Attributes { get; set; } = new List<string>();

        public bool Parsed { get; set; }

        public int ClosestExpected { get; set; }
        public string Expected { get; set; }
        public Token Found { get; set; }

        #region DebugPrintOut

        private class Node
        {
            public string Name { get; set; }
            public List<Node> Children { get; } = new List<Node>();
        }

        // Constants for drawing lines and spaces
        private const string _cross = " ├─";
        private const string _corner = " └─";
        private const string _vertical = " │ ";
        private const string _space = "   ";

        private void PrintNode(Node node, string indent)
        {
            Console.WriteLine(node.Name);

            // Loop through the children recursively, passing in the
            // indent, and the isLast parameter
            var numberOfChildren = node.Children.Count;
            for (var i = 0; i < numberOfChildren; i++)
            {
                var child = node.Children[i];
                var isLast = (i == (numberOfChildren - 1));
                PrintChildNode(child, indent, isLast);
            }
        }

        private void PrintChildNode(Node node, string indent, bool isLast)
        {
            // Print the provided pipes/spaces indent
            Console.Write(indent);

            // Depending if this node is a last child, print the
            // corner or cross, and calculate the indent that will
            // be passed to its children
            if (isLast)
            {
                Console.Write(_corner);
                indent += _space;
            }
            else
            {
                Console.Write(_cross);
                indent += _vertical;
            }

            PrintNode(node, indent);
        }

        private List<Node> CreateNodeList()
        {
            var re = new List<Node>();
            IterateAst(re, this);

            return re;
        }

        private string Foreground(int c)
        {
            return $"\x1b[38;5;{c}m";
        }

        private string Background(int c)
        {
            return $"\x1b[48;5;{c}m";
        }

        private string Reset()
        {
            return $"\x1b[0m";
        }

        private void IterateAst(List<Node> nodes, object active)
        {
            if(active == null) return;
            var node = new Node();
            node.Name = Foreground(3) + active.GetType().Name + Reset();

            foreach (var property in active.GetType().GetProperties())
            {
                if (property.PropertyType == typeof(List<AstNode>))
                {
                    var lst = (List<AstNode>) property.GetValue(active);
                    if (lst == null)
                    {
                    }
                    else
                    {
                        var n = new Node();
                        n.Name = Foreground(5) + property.Name + Reset();
                        foreach (var x in lst)
                        {
                            IterateAst(n.Children, x);
                        }

                        node.Children.Add(n);
                    }
                }
                else if (property.PropertyType == typeof(List<object>))
                {
                    var lst = (List<object>) property.GetValue(active);
                    if (lst == null)
                    {
                    }
                    else
                    {
                        var n = new Node();
                        n.Name = Foreground(5) + property.Name + Reset();
                        foreach (var x in lst)
                        {
                            if (x is Token t)
                            {
                                node.Children.Add(new Node()
                                {
                                    Name =
                                        $"{Foreground(6)}{property.Name}{Foreground(251)} = {Foreground(1)}'{t.Raw}'{Reset()}"
                                });
                            }
                            else
                            {
                                IterateAst(n.Children, x);
                                node.Children.Add(n);
                            }
                        }
                    }
                }
                else if (property.PropertyType == typeof(List<string>))
                {
                    var lst = (List<string>) property.GetValue(active);
                    if (lst == null)
                    {
                    }
                    else
                    {
                        var n = new Node();
                        n.Name = Foreground(5) + property.Name + Reset();
                        foreach (var x in lst)
                        {
                            if (x is string)
                            {
                                node.Children.Add(new Node()
                                {
                                    Name =
                                        $"{Foreground(6)}{property.Name}{Foreground(251)} = {Foreground(1)}'{x}'{Reset()}"
                                });
                            }
                            else
                            {
                                IterateAst(n.Children, x);
                                node.Children.Add(n);
                            }
                        }
                    }
                }
                else if (property.PropertyType == typeof(AstNode))
                {
                    var x = (AstNode) property.GetValue(active);
                    if (x != null)
                    {
                        var n = new Node();
                        /*if (x.Raw != null)
                        {
                            n.Name =
                                $"{Foreground(6)}{property.Name}{Foreground(251)} = {Foreground(1)}'{x.Raw.Raw}'{Reset()}";
                        }
                        else*/
                        {
                            n.Name = Foreground(5) + property.Name + Reset();
                        }


                        IterateAst(n.Children, x);

                        node.Children.Add(n);
                    }
                }
                else if (property.PropertyType == typeof(Token))
                {
                    var x = (Token) property.GetValue(active);
                    if (x != null)
                    {
                        var n = new Node();
                        {
                            n.Name =
                                $"{Foreground(6)}{property.Name}{Foreground(251)} = {Foreground(1)}'{x.Raw}'{Reset()}";
                        }


                        //IterateAst(n.Children, x);

                        node.Children.Add(n);
                    }
                }
                else
                {
                    var val = property.GetValue(active);

                    if (val != null)
                        node.Children.Add(new Node()
                        {
                            Name =
                                $"{Foreground(6)}{property.Name}{Foreground(251)} = {Foreground(1)}'{val}'{Reset()}"
                        });
                }
            }


            nodes.Add(node);
        }


        public void DebugPrint()
        {
            List<Node> topLevelNodes = CreateNodeList();


            foreach (var node in topLevelNodes)
            {
                PrintNode(node, indent: "");
            }
        }

        #endregion
    }
}