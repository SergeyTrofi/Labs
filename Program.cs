using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;
using System.Diagnostics.CodeAnalysis;
using System.Net.Security;
using System.Runtime.InteropServices.JavaScript;
using System.Text;

namespace Labs
{
    class Program
    {
        public class String
        {
            public string input;
            public string rpn;
            public double result;
        }
        public static void Main()
        {
            var String = new String();
            String.input = Console.ReadLine();
            String.rpn = InfixToRPN(String.input);
            Console.WriteLine("Выражение в обратной польской записи (ОПЗ):");
            Console.WriteLine(String.rpn);
            String.result = CalculateRPN(String.rpn);
            Console.WriteLine("Результат: " + String.result);
        }

        public class varInInfix
        {
            public string[] tokens;
            public string rpn;
            public Stack<string> operatorStack;
            public Dictionary<string, int> precedence;
        }
        public static string InfixToRPN(string infix)
        {
            var varInInfix = new varInInfix();
            varInInfix.tokens = infix.Split(' ');
            varInInfix.rpn = "";
            varInInfix.operatorStack = new Stack<string>();

            varInInfix.precedence = new Dictionary<string, int>
            {
                { "+", 1 },
                { "-", 1 },
                { "*", 2 },
                { "/", 2 }
            };

            foreach (string token in varInInfix.tokens)
            {
                if (double.TryParse(token, out double number))
                {
                    varInInfix.rpn += number + " ";
                }
                else if (varInInfix.precedence.ContainsKey(token))
                {
                    while (varInInfix.operatorStack.Count > 0 &&
                           varInInfix.precedence.ContainsKey(varInInfix.operatorStack.Peek()) &&
                           varInInfix.precedence[token] <= varInInfix.precedence[varInInfix.operatorStack.Peek()])
                    {
                        varInInfix.rpn += varInInfix.operatorStack.Pop() + " ";
                    }
                    varInInfix.operatorStack.Push(token);
                }
                else if (token == "(")
                {
                    varInInfix.operatorStack.Push(token);
                }
                else if (token == ")")
                {
                    while (varInInfix.operatorStack.Count > 0 && varInInfix.operatorStack.Peek() != "(")
                    {
                        varInInfix.rpn += varInInfix.operatorStack.Pop() + " ";
                    }
                    if (varInInfix.operatorStack.Count > 0 && varInInfix.operatorStack.Peek() == "(")
                    {
                        varInInfix.operatorStack.Pop();
                    }
                }
            }

            while (varInInfix.operatorStack.Count > 0)
            {
                varInInfix.rpn += varInInfix.operatorStack.Pop() + " ";
            }

            return varInInfix.rpn;
        }

        public class varInCalculateRPN
        {
            public string[] tokens;
            public Stack<double> numberStack;
            public double operand2;
            public double operand1;
        }
        public static double CalculateRPN(string rpn)
        {
            var varInCalculateRPN = new varInCalculateRPN();
            varInCalculateRPN.tokens = rpn.Split(' ');
            varInCalculateRPN.numberStack = new Stack<double>();

            foreach (string token in varInCalculateRPN.tokens)
            {
                if (double.TryParse(token, out double number))
                {
                    varInCalculateRPN.numberStack.Push(number);
                }
                else if (varInCalculateRPN.numberStack.Count >= 2)
                {
                    double operand2 = varInCalculateRPN.numberStack.Pop();
                    double operand1 = varInCalculateRPN.numberStack.Pop();


                    switch (token)
                    {
                        case "+":
                            varInCalculateRPN.numberStack.Push(operand1 + operand2);
                            break;
                        case "-":
                            varInCalculateRPN.numberStack.Push(operand1 - operand2);
                            break;
                        case "*":
                            varInCalculateRPN.numberStack.Push(operand1 * operand2);
                            break;
                        case "/":
                            varInCalculateRPN.numberStack.Push(operand1 / operand2);
                            break;
                    }
                }
            }
            return varInCalculateRPN.numberStack.Count > 0 ? varInCalculateRPN.numberStack.Peek() : 0;
        }
    }
}