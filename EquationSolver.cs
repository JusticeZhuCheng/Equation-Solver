using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace EquationSolver
{

    /*  
     *  Created by ChengZhu UTS 13000359
     *  
     *  Firstly split the equation into two parts by '='
     *  Translate each part into the formation of ax+b
     *  a, b can be zero
     *  Finally get a1X + b1 = a2X + b2
     *  X = ((b2 - b1) / (a1 - a2))
     *  
    */
        // create a new class to store the a,b
    public class Result
    {
        double a;
        double b;
        //constructors
        public Result()
        {
        }
        public Result(double a, double b)
        {
            this.a = a;
            this.b = b;
        }
        //setter and getter
        public double GetA()
        {
            return a;
        }
        public double GetB()
        {
            return b;
        }
        public void SetA(double a)
        {
            this.a = a;
        }
        public void SetB(double b)
        {
            this.b = b;
        }
    }
    public class Program
    {

        //method to check if the string is a number
        public static bool IsNum(string str)
        {
            bool isNum = false;
            // if the string is number, return true
            if (double.TryParse(str, out double num))
            {
                isNum = true;
            }
            return isNum;
        }

        //method to check if the string is in the format of ax+b
        public static bool IsRightString(string str)
        {
            //split the string into a char array
            char[] c = str.ToCharArray();
            // count the number of +,- operators
            int i = 0;
            int variableNum = 0;
            for (int j = 0; j < c.Length; j++)
            {
                if ('+' == c[j] || '-' == c[j])
                {
                    i++;
                }
                if (c[j] == 'X')
                {
                    variableNum += 1;
                }
            }
            //if there's one or none operator and it's not * or / and there is one X, return true
            if (variableNum == 1 && i <= 1 && !str.Contains('*') && !str.Contains('/'))
            {
                if (!str.Contains('('))
                {
                    return true;
                    // (ax+b) is also ok
                }
                else if (str.Contains('(') && str.IndexOf('(') == 0)
                {
                    return true;
                }
            }
            return false;
        }

        //method to sort the string into the format of ax+b
        public static string GetStandardString(string str)
        {
            char[] chars = str.ToCharArray();
            int i = 0;
            StringBuilder s1 = new StringBuilder(str);
            // if there is operator, remerber the position by i;
            if (str.Contains('+') || str.Contains('-'))
            {
                for (int j = 0; j < chars.Length; j++)
                {
                    if ('+' == chars[j] || '-' == chars[j])
                    {
                        i = j;
                        break;
                    }
                }
                //if x is before the operator ,it's the right format
                if (str.IndexOf('X') < i)
                {
                    return str;
                }
                //else change the position, make it ax+b
                else
                {
                    string s2 = str.Substring(0, i);
                    string s3 = str.Substring(i, str.Length - i);
                    StringBuilder ss = new StringBuilder(s3);
                    ss.Append(s2);
                    return ss.ToString();
                }
            }
            // if thers is no operator, append +0 to the string to make it ax+b
            else
            {
                return s1.Append("+0").ToString();
            }
        }

        //method to delete the bracket
        public static string DeleteBracket(string str)
        {
            if (str.StartsWith("(") && str.EndsWith(")"))
            {
                str = str.Substring(1, str.Length - 2);
            }
            return str;
        }

        //method to get the number of brackets
        // num is the number of bracket; index is the position; chars is char array;
        public static int GetBracketNum(int num, int index, char[] chars)
        {
            if ('(' == chars[index])
            {
                num++;
            }
            else if (')' == chars[index])
            {
                num--;
            }
            return num;
        }

        //method to get the value of a,b from ax+b, return Result
        public static Result GetResultFromString(string str)
        {
            Result result = new Result();
            // if the string is empty, a=0,b=0
            if (str == "")
            {
                result.SetA(0.0);
                result.SetB(0.0);
            }
            // if the string is in the right format, delete the bracket first if there is any
            // use substring to get a,b and return Result  
            else if (IsRightString(str))
            {
                str = DeleteBracket(str);
                string standardString = GetStandardString(str);
                // X is equal to 1X
                if (standardString.StartsWith("X"))
                {
                    result.SetA(1.0);
                }
                else
                {
                    result.SetA(Convert.ToDouble(standardString.Substring(0, standardString.IndexOf('X'))));
                }
                // check the format, if it's not valid, print the error
                try
                {
                    result.SetB(Convert.ToDouble(standardString.Substring(standardString.IndexOf('X') + 1, standardString.Length - standardString.IndexOf('X') - 1)));
                }
                catch (Exception)
                {
                    Console.WriteLine("Invalid input format");
                    System.Environment.Exit(10);
                }
            }
            // if the string is number or number in brackets, a=0, b=string
            else if (IsNum(str) || IsNum(DeleteBracket(str)))
            {
                result.SetA(0.0);
                result.SetB(Convert.ToDouble(DeleteBracket(str)));
            }
            // else the string needs to be translated
            else
            {
                result = Translate(str);
            }
            return result;
        }

        //method to split the string by +,-,*,/ and operate the calculation between 'ax+b's
        public static Result Translate(string str)
        {
            //if the string is totally inside the brackets, delete the brackets
            str = DeleteBracket(str);
            char[] chars = str.ToCharArray();
            // firtsly split by + 
            // keep those brackets in the middle of the string, deal with them later
            for (int i = 0, bracketNum = 0; i < chars.Length; i++)
            {
                bracketNum = GetBracketNum(bracketNum, i, chars);
                if (bracketNum == 0 && ('+' == chars[i]))
                {
                    //use substring to split the string into two parts 
                    string str1 = str.Substring(0, i);
                    string str2 = str.Substring(i + 1, str.Length - i - 1);
                    // get a,b from both parts
                    // the getResultFromString method will do translate again if a,b is not available
                    Result result1 = GetResultFromString(str1);
                    Result result2 = GetResultFromString(str2);
                    // get a=a1+a2, b=b1+b2
                    Result result = new Result();
                    result.SetA(result1.GetA() + result2.GetA());
                    result.SetB(result1.GetB() + result2.GetB());
                    return result;
                }
            }
            //split by - and keep those brackets in the middle of the string
            for (int i = chars.Length - 1, bracketNum = 0; i >= 0; i--)
            {
                bracketNum = GetBracketNum(bracketNum, i, chars);
                if (bracketNum == 0 && ('-' == chars[i]))
                {
                    string str1 = str.Substring(0, i);
                    string str2 = str.Substring(i + 1, str.Length - i - 1);
                    Result result1 = GetResultFromString(str1);
                    Result result2 = GetResultFromString(str2);
                    Result result = new Result();
                    // get a=a1-a2, b=b1-b2
                    result.SetA(result1.GetA() - result2.GetA());
                    result.SetB(result1.GetB() - result2.GetB());
                    return result;
                }
            }
            //split by *,/ and keep those brackets in the middle of the string
            for (int i = 0, bracketNum = 0; i < chars.Length; i++)
            {
                bracketNum = GetBracketNum(bracketNum, i, chars);
                if (bracketNum == 0 && ('*' == chars[i] || '/' == chars[i]))
                {
                    string str1 = str.Substring(0, i);
                    string opt = str.Substring(i, 1);
                    string str2 = str.Substring(i + 1, str.Length - i - 1);
                    Result result1 = GetResultFromString(str1);
                    Result result2 = GetResultFromString(str2);
                    Result result = new Result();
                    // since it's linear equation, a1=0 or a2=0
                    // a=a1*b2 or a=a2*b1, b=b1*b2
                    if (opt == "*")
                    {
                        if (result1.GetA() != 0)
                        {
                            result.SetA(result1.GetA() * result2.GetB());
                            result.SetB(result1.GetB() * result2.GetB());
                        }
                        if (result2.GetA() != 0)
                        {
                            result.SetA(result1.GetB() * result2.GetA());
                            result.SetB(result1.GetB() * result2.GetB());
                        }
                        if (result1.GetA() == 0 && result2.GetA() == 0)
                        {
                            result.SetA(0.0);
                            result.SetB(result1.GetB() * result2.GetB());
                        }
                    }
                    //since it's linear equation, a2=0
                    //a =a1/b2,b=b1/b2
                    else if (opt == "/")
                    {
                        double setA;
                        double setB;
                        // check the divide by zero error;
                        try
                        {
                            if (result2.GetB() == 0)
                            {
                                throw new Exception("Divide by zero error!");
                            }
                            setA = result1.GetA() / result2.GetB();
                            setB = result1.GetB() / result2.GetB();
                            result.SetA(setA);
                            result.SetB(setB);
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Divide by zero error!");
                            System.Environment.Exit(11);
                        }
                    }
                    return result;
                }
            }
            return null;
        }

        //method to get answer of X;
        public static string Calculate(string str1, string str2)
        {
            string answer = "";
            //two strings from both sides of '='
            Result result1 = GetResultFromString(str1);
            Result result2 = GetResultFromString(str2);
            double a1 = result1.GetA();
            double b1 = result1.GetB();
            double a2 = result2.GetA();
            double b2 = result2.GetB();
            //print different answers according to different conditons of a,b
            if (a1 == a2 && b1 != b2)
            {
                answer = "No Solution";
            }
            else if (a1 == a2 && b1 == b2)
            {
                answer = "Any Solution";
            }
            else if (a1 != a2)
            {
                answer = Convert.ToString((b2 - b1) / (a1 - a2));
            }
            return answer;
        }

        //method to reorganize the equation
        public static string ReorganizeEqu(string equ)
        {
            string newEqu = "";
            for (int i = 0; i < equ.Length; i++)
            {
                // remove all the space
                if (equ[i] != ' ')
                {
                    // change all the letters to X
                    if (Char.IsLetter(equ[i]))
                    {
                        newEqu += "X";
                    }
                    // ignore the multiple concurrent operators
                    else if (i != equ.Length - 1 && equ[i] == equ[i + 1] && (equ[i] == '+' || equ[i] == '-' || equ[i] == '*' || equ[i] == '/'))
                    { }
                    // add '*' between bracket and number or between bracket and letter
                    else if (equ[i] == '(')
                    {
                        if (i > 0 && (int.TryParse(Convert.ToString(equ[i - 1]), out int num) || Char.IsLetter(equ[i - 1])))
                        {
                            newEqu += "*(";
                        }
                        else
                        {
                            newEqu += "(";
                        }
                    }
                    else if (equ[i] == ')')
                    {
                        if (i < equ.Length - 1 && (int.TryParse(Convert.ToString(equ[i + 1]), out int num) || Char.IsLetter(equ[i + 1])))
                        {
                            newEqu += ")*";
                        }
                        else
                        {
                            newEqu += ")";
                        }
                    }
                    // if there is operator in the end ,ignore it
                    else if (i == equ.Length - 1 && (equ[i] == '+' || equ[i] == '-' || equ[i] == '*' || equ[i] == '/'))
                    { }
                    else
                    {
                        newEqu += Convert.ToString(equ[i]);
                    }
                }
            }
            return newEqu;
        }
        
        //method to check the input format
        public static bool CheckInput(string equ)
        {
            int variableNum = 0;
            int equalNum = 0;
            bool isRightInput = false;
            for (int i = 0; i < equ.Length; i++)
            {
                if (Char.IsLetter(equ[i]))
                {
                    variableNum += 1;
                }
                if (equ[i] == '=')
                {
                    equalNum += 1;
                }
            }
            if (equalNum == 1 && variableNum >= 1)
            {
                isRightInput = true;
            }
            // no variable and one equal
            else if (variableNum == 0 && equalNum == 1)
            {
                Console.WriteLine("Invalid Input: No Variable");
            }
            // more than one variable and no equal
            else if (equalNum == 0 && variableNum >= 1)
            {
                Console.WriteLine("Invalid Input: No = Sign");
            }
            // no variable and no equal
            else if (equalNum == 0 && variableNum == 0)
            {
                Console.WriteLine("Invalid Input: No Variable and = Sign");
            }
            // more than one equal
            else if (equalNum > 1)
            {
                Console.WriteLine("Invalid Input: Wrong Format");
            }
            return isRightInput;
        }
        
        //method to get the answer by inputing the whole equation
        public static string RunSolver(string equ)
        {
            string answer = "";
            //check the input
            if (CheckInput(equ))
            {
                string equL = equ.Substring(0, equ.IndexOf('='));
                string equR = equ.Substring(equ.IndexOf('=') + 1, (equ.Length - equ.IndexOf('=') - 1));
                // sort both parts
                equL = ReorganizeEqu(equL);
                equR = ReorganizeEqu(equR);
                // calculate the answer
                answer = Calculate(equL, equR);
            }
            return answer;
        }
    }
}
