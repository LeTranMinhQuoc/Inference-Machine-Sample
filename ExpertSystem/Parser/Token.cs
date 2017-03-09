using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpertSystem.Parser
{
    public class Token
    {
        public Token(string value, TokenType type)
        {
            this.Value = value;
            this.Type = type;
        }

        public string Value { get; set; }

        public TokenType Type { get; private set; }
    }
}
