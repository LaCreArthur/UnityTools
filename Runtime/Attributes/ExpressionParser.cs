using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace AS.Toolbox.Attributes
{
    public static class ExpressionParser
    {
        public delegate bool EvaluateFunction(string content);

        public static bool EvaluateExpression(string expression, EvaluateFunction evalFunction)
        {
            var cleanExpr = Regex.Replace(expression, @"\s+", "");// Remove white spaces
            cleanExpr = Regex.Replace(cleanExpr, @"&&", "&");// Replace double &&
            cleanExpr = Regex.Replace(cleanExpr, @"\|\|", "|");// Replace double ||

            var tokens = new List<Token>();
            var reader = new StringReader(cleanExpr);
            Token t = null;
            do
            {
                t = new Token(reader);
                tokens.Add(t);
            } while (t.type != Token.TokenType.ExprEnd);

            List<Token> polishNotation = Token.TransformToPolishNotation(tokens);

            var enumerator = polishNotation.GetEnumerator();
            enumerator.MoveNext();
            var root = MakeExpression(ref enumerator, evalFunction);

            return root.Evaluate();
        }

        public class Token
        {
            static readonly Dictionary<char, KeyValuePair<TokenType, string>> TypesDict =
                new Dictionary<char, KeyValuePair<TokenType, string>>
                {
                    { '(', new KeyValuePair<TokenType, string>(TokenType.OpenParen, "(") },
                    { ')', new KeyValuePair<TokenType, string>(TokenType.CloseParen, ")") },
                    { '!', new KeyValuePair<TokenType, string>(TokenType.UnaryOp, "NOT") },
                    { '&', new KeyValuePair<TokenType, string>(TokenType.BinaryOp, "AND") },
                    { '|', new KeyValuePair<TokenType, string>(TokenType.BinaryOp, "OR") },
                };

            public enum TokenType { OpenParen, CloseParen, UnaryOp, BinaryOp, Literal, ExprEnd }

            public readonly TokenType type;
            public readonly string value;

            public Token(StringReader s)
            {
                var c = s.Read();
                if (c == -1)
                {
                    type = TokenType.ExprEnd;
                    value = "";
                    return;
                }

                var ch = (char)c;

                //Special case: solve bug where !COND_FALSE_1 && COND_FALSE_2 would return True
                var embeddedNot = ch == '!' && s.Peek() != '(';

                if (TypesDict.ContainsKey(ch) && !embeddedNot)
                {
                    type = TypesDict[ch].Key;
                    value = TypesDict[ch].Value;
                }
                else
                {
                    var sb = new StringBuilder();
                    sb.Append(ch);
                    while (s.Peek() != -1 && !TypesDict.ContainsKey((char)s.Peek()))
                        sb.Append((char)s.Read());
                    type = TokenType.Literal;
                    value = sb.ToString();
                }
            }

            public static List<Token> TransformToPolishNotation(List<Token> infixTokenList)
            {
                var outputQueue = new Queue<Token>();
                var stack = new Stack<Token>();

                var index = 0;
                while (infixTokenList.Count > index)
                {
                    var t = infixTokenList[index];

                    switch (t.type)
                    {
                        case TokenType.Literal:
                            outputQueue.Enqueue(t);
                            break;
                        case TokenType.BinaryOp:
                        case TokenType.UnaryOp:
                        case TokenType.OpenParen:
                            stack.Push(t);
                            break;
                        case TokenType.CloseParen:
                            while (stack.Peek().type != TokenType.OpenParen)
                                outputQueue.Enqueue(stack.Pop());
                            stack.Pop();
                            if (stack.Count > 0 && stack.Peek().type == TokenType.UnaryOp)
                                outputQueue.Enqueue(stack.Pop());
                            break;
                        case TokenType.ExprEnd:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    index++;
                }

                while (stack.Count > 0)
                    outputQueue.Enqueue(stack.Pop());

                var list = new List<Token>(outputQueue);
                list.Reverse();
                return list;
            }
        }

        abstract class Expression
        {
            public abstract bool Evaluate();
        }

        class ExpressionLeaf : Expression
        {
            readonly string _content;
            readonly EvaluateFunction _evalFunction;

            public ExpressionLeaf(EvaluateFunction evalFunction, string content)
            {
                _evalFunction = evalFunction;
                _content = content;
            }

            public override bool Evaluate()
            {
                //embedded not, see special case in Token declaration
                if (_content.StartsWith("!"))
                    return !_evalFunction(_content[1..]);

                return _evalFunction(_content);
            }
        }

        class ExpressionAnd : Expression
        {
            readonly Expression _left;
            readonly Expression _right;

            public ExpressionAnd(Expression left, Expression right)
            {
                _left = left;
                _right = right;
            }

            public override bool Evaluate() => _left.Evaluate() && _right.Evaluate();
        }

        class ExpressionOr : Expression
        {
            readonly Expression _left;
            readonly Expression _right;

            public ExpressionOr(Expression left, Expression right)
            {
                _left = left;
                _right = right;
            }

            public override bool Evaluate() => _left.Evaluate() || _right.Evaluate();
        }

        class ExpressionNot : Expression
        {
            readonly Expression _expr;

            public ExpressionNot(Expression expr)
            {
                _expr = expr;
            }

            public override bool Evaluate() => !_expr.Evaluate();
        }

        static Expression MakeExpression(ref List<Token>.Enumerator polishNotationTokensEnumerator, EvaluateFunction evalFunction)
        {
            if (polishNotationTokensEnumerator.Current.type == Token.TokenType.Literal)
            {
                Expression lit = new ExpressionLeaf(evalFunction, polishNotationTokensEnumerator.Current.value);
                polishNotationTokensEnumerator.MoveNext();
                return lit;
            }

            switch (polishNotationTokensEnumerator.Current.value)
            {
                case "NOT":
                {
                    polishNotationTokensEnumerator.MoveNext();
                    var operand = MakeExpression(ref polishNotationTokensEnumerator, evalFunction);
                    return new ExpressionNot(operand);
                }
                case "AND":
                {
                    polishNotationTokensEnumerator.MoveNext();
                    var left = MakeExpression(ref polishNotationTokensEnumerator, evalFunction);
                    var right = MakeExpression(ref polishNotationTokensEnumerator, evalFunction);
                    return new ExpressionAnd(left, right);
                }
                case "OR":
                {
                    polishNotationTokensEnumerator.MoveNext();
                    var left = MakeExpression(ref polishNotationTokensEnumerator, evalFunction);
                    var right = MakeExpression(ref polishNotationTokensEnumerator, evalFunction);
                    return new ExpressionOr(left, right);
                }
                default:
                    return null;
            }
        }
    }
}
