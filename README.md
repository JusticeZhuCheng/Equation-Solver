# Equation-Solver
It's a simple equation solver C# library which provides some common functions for processing string inputs.</br>

# Logics
Firstly split the equation into two parts by '='</br>
Translate each part into the formation of ax+b (a, b can be zero)</br>
Finally get a1X + b1 = a2X + b2</br>
X = (b2 - b1) / (a1 - a2)</br>

# How to use
1.Add the .cs file into your project</br>
2.Add "using EquationSolver" at the beginning of your codes along with other "using"s</br>
Then you can call the functions in your code</br>

# Functions & Classes

## Classes
The Result class is created to store double a, b of ax+b</br>
The Program class is the class where all the functions are stored</br>

## EquationSolver.Program.CheckInput(string equ) > return bool
Check if the input equation is in the right format with one equal sign and at least one variable</br>

## EquationSolver.Program.ReorganizeEqu(string equ) > return string
Reorganize the equation</br>
Remove all the space, change all the letters to X and some other format issues</br>
You can see them in detail by the comments in codes</br>

## EquationSolver.Program.DeleteBracket(string str) > return string
Delete the brackets if the string begins with "(" and ends with ")"</br>

## EquationSolver.Program.IsNum(string str) > return bool
Check if the string is a number</br>

## EquationSolver.Program.IsRightString(string str) > return bool
Check if the string is in the format of ax+b</br>

## EquationSolver.Program.GetStandardString(string str) > return string
Method to make the string like "ax" only or "b+ax" into the format of "ax+b"</br>

## EquationSolver.Program.GetResultFromString(string str) > return Result
Get a,b from ax+b and return a,b in the format of class Result</br>

## EquationSolver.Program.Translate(string str) > return Result
Translate and calculate the input string into the format of ax+b, return a,b in the format of class Result</br>

## EquationSolver.Program.Calculate(string str1, string str2) > return string
Return the answer by inputing both sides of the equal sign</br>
You may get "No solution", "Any solution" or the exact solution arccording to your input</br>

## EquationSolver.Program.RunSolver(string equ) > return string
Input the whole equation and get the answer in the type of string</br>
You may get "No solution", "Any solution" or the exact solution arccording to your input</br>
