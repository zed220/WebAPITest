using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPIBooks.App_Code {
    public static class Calculator_RPN {
        public static RPNResult Calculate(string expression) {
            if(string.IsNullOrWhiteSpace(expression))
                return RPNResult.CreateError();
            string[] values = expression.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if(values.Length % 2 == 0)
                return RPNResult.CreateError();
            Stack<double> operands = new Stack<double>();
            foreach(var value in values) {
                if(TryGetOperand(value, out var operand)) {
                    operands.Push(operand);
                    continue;
                }
                if(TryGetOperator(value, out var processMethod)) {
                    if(operands.Count < 2)
                        return RPNResult.CreateError();
                    operands.Push(processMethod(operands.Pop(), operands.Pop()));
                    continue;
                }
                return RPNResult.CreateError();
            }
            if(operands.Count != 1)
                return RPNResult.CreateError();
            return RPNResult.Create(operands.Pop());
        }

        static bool TryGetOperand(string value, out double number) {
            return double.TryParse(value, out number);
        }
        static bool TryGetOperator(string value, out Func<double, double, double> processMethod) {
            processMethod = null;
            if(value.Length != 1)
                return false;
            switch(value.Single()) {
                case '+':
                    processMethod = (v2, v1) => v1 + v2;
                    return true;
                case '-':
                    processMethod = (v2, v1) => v1 - v2;
                    return true;
                case '*':
                    processMethod = (v2, v1) => v1 * v2;
                    return true;
                case '/':
                    processMethod = (v2, v1) => v1 / v2;
                    return true;
                case '^':
                    processMethod = (v2, v1) => Math.Pow(v1, v2);
                    return true;
                default:
                    return false;
            }
        }
    }
}