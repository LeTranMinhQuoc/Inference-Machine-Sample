using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolishNotation
{
    public enum eTokenType
    {
        Operator,
        Operand,
        Function,
        Bracket,

    }
    public class Token
    {
        public eTokenType Type { get; set; }
        public string Value { get; set; }

        public int Priority
        {
            get
            {
                if (Type == eTokenType.Operator)
                {
                    if (Value == "+" || Value == "-")
                    {
                        return 1;
                    }
                    else if (Value == "*" || Value == "/")
                    {
                        return 2;
                    }
                    else if (Value == "^")
                    {
                        return 3;
                    }
                }
                return -1;
            }
        }

        public Token(eTokenType type, string value)
        {
            Type = type;
            Value = value;
        }

        public Token(eTokenType type, char value)
        {
            Type = type;
            Value = value.ToString();
        }
    }
}
