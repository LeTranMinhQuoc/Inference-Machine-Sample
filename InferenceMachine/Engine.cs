using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
namespace InferenceMachine
{
    class Engine
    {
        public Rule Rule { get; private set; }
        public PolishNotation.PolishNotation PolishNotation { get; private set; }
        List<Tuple<string[], string, string>> logicRules;
        List<Tuple<string, int>> havingExp;
        List<string> questExp;
        Regex regexQuest = new Regex(@"(.?)=\?");

        public void Init()
        {
            Rule = new Rule();
            PolishNotation = new PolishNotation.PolishNotation();
            logicRules = Rule.LoadRules();
        }

        public string GetResult(string input)
        {
            ParseQuestion(input);
            var hashRules = new HashSet<Tuple<string[], string, string>>(logicRules);
            var variables = havingExp.Select(e => e.Item1).ToList();

            //testing only run one case
            foreach (var q in questExp)
            {
                List<string> solution = new List<string>();
                var hasSolution = FindSolution(variables, q, hashRules, ref solution);

                if (hasSolution)
                {
                    var finalSol = ParseSolution(solution);
                    var result = Calculate(finalSol, havingExp);
                    if (solution.Count > 1)
                    {
                        var havingVar = havingExp.Select(e => e.Item1).ToList();
                        Rule.SaveNewRule(havingVar, q, finalSol);                        
                    }
                    return string.Format("{0}={1}", q, result);
                }
            }
            return "Can't find solution!";
        }

        public string ParseSolution(List<string> solution)
        {
            string exp = "";
            for (int i = solution.Count - 1; i >= 0; i--)
            {
                var sol = solution[i];
                if (i == solution.Count - 1)
                {
                    exp = solution[i];
                }
                else
                {
                    var sols = solution[i].Split('=');
                    exp = exp.Replace(sols[0], "(" + sols[1] + ")");
                }
            }
            return exp;
        }

        public float Calculate(string formula, List<Tuple<string, int>> havingEx)
        {
            foreach (var item in havingEx)
            {
                formula = formula.Replace(item.Item1, item.Item2.ToString());
            }
            return PolishNotation.Cal(formula.Split('=')[1]);
        }

        private bool FindSolution(List<string> variables, string quest, HashSet<Tuple<string[], string, string>> rules, ref List<string> solution)
        {
            foreach (var ru in rules)
            {
                var checkVariables = ru.Item1.Intersect(variables, StringComparer.Ordinal);
                if (checkVariables.Count() >= ru.Item1.Count())
                {
                    solution.Add(ru.Item3);

                    if (ru.Item2 == quest)
                    {
                        return true;
                    }
                    else
                    {
                        variables.Add(ru.Item2);
                        rules.Remove(ru);
                        return FindSolution(variables, quest, rules, ref solution);
                    }
                }
            }

            return false;
        }

        private void ParseQuestion(string input)
        {
            havingExp = new List<Tuple<string, int>>();
            questExp = new List<string>();
            var inputExpression = input.Split(new string[] { "if", "then" }, StringSplitOptions.RemoveEmptyEntries);

            var ifExpress = inputExpression[0].Split(new string[] { "and" }, StringSplitOptions.RemoveEmptyEntries).Select(a => a.Trim());
            foreach (var itemIf in ifExpress)
            {
                var variables = itemIf.Split('=');
                havingExp.Add(new Tuple<string, int>(variables[0], Convert.ToInt32(variables[1])));
            }

            var quest = regexQuest.Matches(inputExpression[1]);
            foreach (Match q in quest)
            {
                questExp.Add(q.Value.Split('=')[0]);
            }
        }
    }
}
