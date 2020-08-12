using System;
using System.Collections.Generic;
using Compiler.Frontend.Ast;
using static Compiler.Core.Parsing.TokenType;
namespace Compiler.Frontend.Grammer
{
    public class Rules
    {
  public static List<List<(object[], Func<AstNode[], AstNode>)>> PassRules =
            new List<List<(object[], Func<AstNode[], AstNode>)>>
            {
   new List<(object[], Func<AstNode[], AstNode>)>
   {
(new object[] {Identifier,DoublePoint,Identifier,},
 (objs) => new NameTypePair () {Name = objs[0],Type = objs[2],}),
(new object[] {Identifier,DoubleDouble,},
 (objs) => new ProcedureNode () {Name = objs[0],}),
},
   new List<(object[], Func<AstNode[], AstNode>)>
   {
(new object[] {typeof(ExprNode),Divide,typeof(ExprNode),},
  (objs) => new ExprNode   {Value = new DivisionNode  {A = objs[0],B = objs[2],}}),
(new object[] {typeof(ExprNode),Multiply,typeof(ExprNode),},
  (objs) => new ExprNode   {Value = new MultiplicationNode  {A = objs[0],B = objs[2],}}),
},
   new List<(object[], Func<AstNode[], AstNode>)>
   {
(new object[] {typeof(ExprNode),Plus,typeof(ExprNode),},
  (objs) => new ExprNode   {Value = new AdditionNode  {A = objs[0],B = objs[2],}}),
(new object[] {typeof(ExprNode),Minus,typeof(ExprNode),},
  (objs) => new ExprNode   {Value = new SubtractionNode  {A = objs[0],B = objs[2],}}),
(new object[] {typeof(NameTypePair),Comma,typeof(NameTypePair),},
 (objs) => new ListNode () {A = objs[0],B = objs[2],}),
},
   new List<(object[], Func<AstNode[], AstNode>)>
   {
(new object[] {typeof(ExprNode),Eq,typeof(ExprNode),},
  (objs) => new StatementNode   {Value = new AssignmentNode  {Name = objs[0],Value = objs[2],}}),
(new object[] {typeof(ExprNode),DoubleEq,typeof(ExprNode),},
  (objs) => new StatementNode   {Value = new DeclNode  {Name = objs[0],Value = objs[2],}}),
(new object[] {typeof(ListNode),Comma,typeof(NameTypePair),},
 (objs) => new ListNode () {A = objs[0],B = objs[2],}),
(new object[] {OpenRoundBracket,typeof(ListNode),CloseRoundBracket,},
 (objs) => new ProcedureArgsNode () {Args = objs[1],}),
(new object[] {OpenRoundBracket,typeof(NameTypePair),CloseRoundBracket,},
 (objs) => new ProcedureArgsNode () {Args = objs[1],}),
},
   new List<(object[], Func<AstNode[], AstNode>)>
   {
(new object[] {typeof(CodeBlockList),typeof(StatementNode),},
 (objs) => new CodeBlockList () {A = objs[0],B = objs[1],}),
(new object[] {typeof(StatementNode),typeof(CodeBlockList),},
 (objs) => new CodeBlockList () {A = objs[0],B = objs[1],}),
(new object[] {typeof(StatementNode),typeof(StatementNode),},
 (objs) => new CodeBlockList () {A = objs[0],B = objs[1],}),
(new object[] {OpenCurlyBracket,typeof(CodeBlockList),CloseCurlyBracket,},
 (objs) => new CodeBlock () {Body = objs[1],}),
(new object[] {OpenCurlyBracket,typeof(StatementNode),CloseCurlyBracket,},
 (objs) => new CodeBlock () {Body = objs[1],}),
(new object[] {OpenCurlyBracket,CloseCurlyBracket,},
 (objs) => new CodeBlock () {}),
(new object[] {typeof(ProcedureNode),typeof(ProcedureArgsNode),typeof(CodeBlock),},
 (objs) => new ProcedureNode () {Name = objs[0],Args = objs[1],Body = objs[2],}),
}
   };
    }
}
