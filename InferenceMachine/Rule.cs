using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InferenceMachine
{
    class Rule
    {
        private string[] ruleLines;
        public Rule()
        {
            ruleLines = System.IO.File.ReadAllLines("MathRules.txt");
        }

        public List<Tuple<string[], string, string>> LoadRules()
        {
            var ruleTuples = new List<Tuple<string[], string, string>>();
            foreach (var rule in ruleLines)
            {
                var ruleExpression = rule.Split(new string[] { "IF", "THEN", "DO" }, StringSplitOptions.RemoveEmptyEntries);

                var ifExpress = ruleExpression[0].Split(new string[] { "AND" }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(a => a.Trim()).ToArray();
                var thenExpress = ruleExpression[1].Trim();
                var doExpress = ruleExpression[2].Trim();
                ruleTuples.Add(new Tuple<string[], string, string>(ifExpress, thenExpress, doExpress));
            }
            return ruleTuples;
        }

        public void SaveNewRule(List<string> ifExpress, string thenExpress, string formula)
        {
            string rule = "IF ";
            rule += string.Join(" AND ", ifExpress.ToArray());
            rule += " THEN " + thenExpress + " DO " + formula;
            System.IO.File.AppendAllText("MathRules.txt", Environment.NewLine + rule);
        }
    }
}
