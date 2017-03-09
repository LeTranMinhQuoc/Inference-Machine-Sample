using ExpertSystem.Facts;
using ExpertSystem.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpertSystem.Parser
{
    public class Parse
    {
        private Stack<Token> tokensStack = new Stack<Token>();
        private Stack<Token> parseTokenStack = new Stack<Token>();
        private byte[] textFile;
        private int counter;
        public Parse(string filePath)
        {
            textFile = System.IO.File.ReadAllBytes(filePath);
            counter = 0;
        }

        public List<Rule> Run()
        {
            var rules = new List<Rule>();
            while (counter != textFile.Length)
            {
                var token = NextToken();
                tokensStack.Push(token);
            }
            
            while (tokensStack.Count > 0)
            {
                var popStack = tokensStack.Pop();
                parseTokenStack.Push(popStack);

                if (popStack.Type == TokenType.IF)
                {                 
                    var r = ParseRule();
                    rules.Add(r);
                }
            }

            return rules;
        }

        private Rule ParseRule()
        {
            List<Fact> antecedents = new List<Fact>();
            Fact consequents;
            while (parseTokenStack.Count > 0)
            {
                var lastStack = parseTokenStack.Pop();
                if (lastStack.Type == TokenType.IF || lastStack.Type == TokenType.AND)
                {
                    var fa = ParseFact<IsFact>();
                    antecedents.Add(fa);
                }
                if (lastStack.Type == TokenType.THEN)
                {
                    consequents = ParseFact<DoFact>();
                    return new Rule(antecedents, consequents);
                }
            }
            return null;
        }

        private T ParseFact<T>() where T : Fact, new()
        {
            var lastStack = parseTokenStack.Pop();
            var f = new T();
            var express = lastStack.Value.Split('=');
            f.SetName(express[0]);
            f.SetValue(express[1]);
            return f;
        }

        private Token NextToken()
        {
            var word = NextWord();
            switch (word)
            {
                case "IF":
                    return new Token(word, TokenType.IF);
                case "THEN":
                    return new Token(word, TokenType.THEN);
                case "END":
                    return new Token(word, TokenType.END);
                case "AND":
                    return new Token(word, TokenType.AND);
                default:
                    {
                        var lastToken = tokensStack.Pop();
                        if (lastToken.Type == TokenType.CLAUSE)
                        {
                            lastToken.Value += word;
                            tokensStack.Push(lastToken);
                        }
                        else
                        {
                            tokensStack.Push(lastToken);
                            tokensStack.Push(new Token(word, TokenType.CLAUSE));
                        }

                        return NextToken(); ;
                    }
            }
        }

        private string NextWord()
        {
            string value = "";
            char ch;
            for (ch = NextChar(); ch != ' ' && ch != 10; ch = NextChar())
            {
                value += ch;
            }

            if (ch == 10)
            {
                return NextWord();
            }

            return value;
        }

        private char NextChar()
        {
            if (counter == textFile.Length)
            {
                return ' ';
            }

            int ch = textFile[counter];
            counter++;

            if (ch == 13)
            {
                return NextChar();
            }
            return (char)ch;
        }
    }
}
