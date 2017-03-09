using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolishNotation
{
    public class PolishNotation
    {
        public float Cal(string input)
        {
            var tokens = Tokenize(input);
            Queue<Token> postfix = ConvertToPostfix(tokens);
            var resultToken = EvaluatePostfix(postfix);
            return float.Parse(resultToken.Value);
        }

        private List<Token> Tokenize(string input)
        {
            var lstTokens = new List<Token>();
            int counter = 0;
            while (counter < input.Length)
            {
                if (input[counter] != ' ')
                {
                    if (IsOperand(input[counter]))
                    {
                        string tokenValue = input[counter].ToString();

                        while (counter + 1 < input.Length && IsOperand(input[counter + 1]))
                        {
                            tokenValue += input[counter + 1];
                            counter++;
                        }
                        lstTokens.Add(new Token(eTokenType.Operand, tokenValue));
                    }
                    else if (IsChar(input[counter]))
                    {
                        string tokenValue = input[counter].ToString();
                        while (counter + 1 < input.Length && IsChar(input[counter + 1]))
                        {
                            tokenValue += input[counter + 1];
                            counter++;
                        }

                        lstTokens.Add(new Token(eTokenType.Function, tokenValue));
                    }
                    else if (IsOperator(input[counter]))
                    {
                        lstTokens.Add(new Token(eTokenType.Operator, input[counter]));
                    }
                    else if (IsBracket(input[counter]))
                    {
                        lstTokens.Add(new Token(eTokenType.Bracket, input[counter]));
                    }
                }

                counter++;
            }
            return lstTokens;
        }

        private Queue<Token> ConvertToPostfix(List<Token> tokens)
        {
            Stack<Token> stack = new Stack<Token>();
            Queue<Token> queue = new Queue<Token>();
            stack.Push(new Token(eTokenType.Bracket, "("));
            tokens.Add(new Token(eTokenType.Bracket, ")"));
            foreach (var token in tokens)
            {
                if (token.Type == eTokenType.Function)
                {
                    stack.Push(token);
                }
                if (token.Type == eTokenType.Operand)
                {
                    queue.Enqueue(token);
                }
                else if (token.Type == eTokenType.Operator)
                {
                    if (stack.Count == 0)
                    {
                        stack.Push(token);
                    }
                    else
                    {
                        Token lastStack = stack.Pop();
                        if (lastStack.Type == eTokenType.Bracket || token.Priority >= lastStack.Priority)
                        {
                            stack.Push(lastStack);
                            stack.Push(token);
                        }
                        else
                        {
                            do
                            {
                                queue.Enqueue(lastStack);
                                lastStack = stack.Pop();
                            } while (token.Priority < lastStack.Priority);
                            stack.Push(lastStack);
                            stack.Push(token);
                        }
                    }
                }
                else if (token.Type == eTokenType.Bracket)
                {
                    if (token.Value == "(")
                    {
                        stack.Push(token);
                    }
                    else
                    {
                        bool continueCheck = true;
                        do
                        {
                            Token lastStack = stack.Pop();
                            if (lastStack.Type == eTokenType.Bracket && lastStack.Value == "(")
                            {
                                if (stack.Count == 0)
                                {
                                    continueCheck = false;
                                    break;
                                }

                                lastStack = stack.Pop();
                                if (lastStack.Type == eTokenType.Function)
                                {
                                    queue.Enqueue(lastStack);
                                }
                                else
                                {
                                    stack.Push(lastStack);
                                }
                                continueCheck = false;
                                break;
                            }

                            queue.Enqueue(lastStack);
                        } while (continueCheck);
                    }
                }
            }

            return queue;
        }

        private Token EvaluatePostfix(Queue<Token> queue)
        {
            Stack<Token> stack = new Stack<Token>();

            while (queue.Count > 0)
            {
                var lastToken = queue.Dequeue();
                if (lastToken.Type == eTokenType.Operand)
                {
                    stack.Push(lastToken);
                }
                else
                {
                    if (lastToken.Type == eTokenType.Operator)
                    {
                        var token2 = stack.Pop();
                        var token1 = stack.Pop();
                        var newtoken = CalOperator(token1, token2, lastToken);
                        stack.Push(newtoken);
                    }
                    else if (lastToken.Type == eTokenType.Function)
                    {
                        var newtoken = CalFunc(stack, lastToken);
                        stack.Push(newtoken);
                    }
                }
            }

            return stack.Pop();
        }

        private Token CalOperator(Token t1, Token t2, Token op)
        {
            float result = 0F;
            switch (op.Value)
            {
                case "*":
                    result = Convert.ToSingle(t1.Value) * Convert.ToSingle(t2.Value);
                    break;
                case "/":
                    result = Convert.ToSingle(t1.Value) / Convert.ToSingle(t2.Value);
                    break;
                case "+":
                    result = Convert.ToSingle(t1.Value) + Convert.ToSingle(t2.Value);
                    break;
                case "-":
                    result = Convert.ToSingle(t1.Value) - Convert.ToSingle(t2.Value);
                    break;
                case "^":
                    result = (float)Math.Pow(Convert.ToDouble(t1.Value), Convert.ToDouble(t2.Value));
                    break;
                default:
                    break;
            }
            return new Token(eTokenType.Operand, result.ToString());
        }

        private Token CalFunc(Stack<Token> stack, Token fu)
        {
            float result = 0F;
            switch (fu.Value)
            {
                case "Sqrt":
                    var to = stack.Pop();
                    result = (float)Math.Sqrt(Convert.ToSingle(to.Value));
                    break;
                default:
                    break;
            }
            return new Token(eTokenType.Operand, result.ToString());
        }

        private bool IsOperator(char val)
        {
            return val == '*' || val == '/' || val == '+' || val == '-' || val == '^';
        }

        private bool IsOperand(char val)
        {
            return "0123456789".Contains(val);
        }

        private bool IsBracket(char val)
        {
            return val == ')' || val == '(';
        }

        private bool IsChar(char val)
        {
            return (val >= 'a' && val <= 'z') || (val >= 'A' && val <= 'Z');
        }
    }
}
