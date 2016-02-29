namespace _03.CalculateArithmeticExpression
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    public static class CalculateArithmeticExpression
    {
        private static readonly string[] Operators = { "+", "-", "*", "/", "%" };

        public static void Main()
        {
            Console.WriteLine("Insert your expression:");
            var expression = Console.ReadLine();
            EvaluateExpression(expression);
        }

        private static void EvaluateExpression(string expression)
        {
            expression = 
                Regex.Replace(expression, @"((?<=[\-\+\/\*\%\(\)\= ])|(?<=^))\-?\d+\.?(?:\d+)?|[\(\)]", " $0 ").Trim();

            var postfixExpression = ToReversedPolishNotation(expression);

            try
            {
                var result = ReversePolishNotationParser(postfixExpression);
                Console.WriteLine(result);
            }
            catch (Exception)
            {
                Console.WriteLine("error");
            }
        }

        private static string[] ToReversedPolishNotation(string expression)
        {
            var splittedExp = expression.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            var workStack = new Stack<string>();
            var outputQueue = new Queue<string>();
            foreach (var token in splittedExp)
            {
                if (IsOperator(token))
                {
                    if (workStack.Count == 0)
                    {
                        workStack.Push(token);
                    }
                    else
                    {
                        if (GetOperatorPrecedence(token) <= GetOperatorPrecedence(workStack.Peek()))
                        {
                            while (workStack.Count > 0 &&
                                   GetOperatorPrecedence(token) <= GetOperatorPrecedence(workStack.Peek()))
                            {
                                outputQueue.Enqueue(workStack.Pop());
                            }
                        }

                        workStack.Push(token);
                    }
                }
                else if (token == "(" || token == ")")
                {
                    if (token == "(")
                    {
                        workStack.Push("(");
                    }
                    else
                    {
                        while (workStack.Peek() != "(")
                        {
                            outputQueue.Enqueue(workStack.Pop());
                        }

                        workStack.Pop();
                    }
                }
                else
                {
                    outputQueue.Enqueue(token);
                }
            }

            while (workStack.Count > 0)
            {
                outputQueue.Enqueue(workStack.Pop());
            }

            return outputQueue.ToArray();
        }

        private static double ReversePolishNotationParser(string[] postfixExpression)
        {
            var outputStack = new Stack<double>();
            foreach (var token in postfixExpression)
            {
                if (IsOperator(token))
                {
                    var rightOperand = outputStack.Pop();
                    var leftOPerand = outputStack.Pop();
                    var @operator = token;

                    var result = DoOperation(leftOPerand, rightOperand, @operator);
                    outputStack.Push(result);
                }
                else
                {
                    outputStack.Push(double.Parse(token));
                }
            }

            if (outputStack.Count > 1)
            {
                throw new InvalidOperationException("Expression is wrong!");
            }

            return outputStack.Pop();
        }

        private static double DoOperation(
            double leftOPerand,
            double rightOperand,
            string @operator)
        {
            switch (@operator)
            {
                case "+":
                    return leftOPerand + rightOperand;
                case "-":
                    return leftOPerand - rightOperand;
                case "*":
                    return leftOPerand * rightOperand;
                case "/":
                    return leftOPerand / rightOperand;
                case "%":
                    return leftOPerand % rightOperand;
                default:
                    throw new NotImplementedException("Operation is not implemented yet");
            }
        }

        private static int GetOperatorPrecedence(string @operator)
        {
            switch (@operator)
            {
                case "+":
                case "-":
                    return 0;
                case "*":
                case "/":
                case "%":
                    return 1;
                default:
                    return -1;
            }
        }

        private static bool IsOperator(string @operator)
        {
            return Array.IndexOf(Operators, @operator) >= 0;
        }
    }
}