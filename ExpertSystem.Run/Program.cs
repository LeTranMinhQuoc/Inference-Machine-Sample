using ExpertSystem.Facts;
using ExpertSystem.Rules;
using ExpertSystem.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpertSystem.Run
{
    class Program
    {
        static void Main(string[] args)
        {
            Parse parser = new Parse("rule.txt");
            var rules = parser.Run();

            Engine engine = new Engine();

            foreach (var rule in rules)
            {
                engine.AddRule(rule);
            }          
                
            engine.AddFact(new IsFact("a", "6"));
            engine.AddFact(new IsFact("b", "9"));
            engine.AddFact(new IsFact("c", "9"));
            var a = engine.AskValueFact("S");
            


            //Fact f1antecedent = new IsFact("x", "Human");
            //Fact f1consequent = new IsFact("x", "Mortal");

            //engine.AddRule(new Rule(new List<Fact> { f1antecedent }, f1consequent));

            //engine.AddFact(new IsFact("Ahmed", "Human"));
            //engine.AddFact(new IsFact("Salwa", "Human"));
            //engine.AddFact(new IsFact("Anas", "Human"));
            //engine.AddFact(new IsFact("ANOS", "Software"));
            //engine.AddFact(new IsFact("Julie", "Cat"));
            //engine.AddFact(new IsFact("Tomboss", "Cat"));
            //engine.AddFact(new IsFact("Julie", "Mortal"));
            //engine.AddFact(new IsFact("Tomboss", "Mortal"));
            //var a = engine.AskIsFact(new IsFact("Ahmed", "Mortal"));

        }
    }
}
