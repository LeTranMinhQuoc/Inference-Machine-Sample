using ExpertSystem.Facts;
using ExpertSystem.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpertSystem
{
    public class Engine
    {
        private List<Rule> rules = new List<Rule>();
        private List<Fact> facts = new List<Fact>();

        public void AddFact(Fact fact)
        {
            this.facts.Add(fact);
        }

        public void AddRule(Rule rule)
        {
            this.rules.Add(rule);
        }

        public bool AskIsFact(Fact askingFact)
        {
            var rulesSet = new HashSet<Rule>(this.rules);
            var forwardChaning = ForwardChaining(this.facts, rulesSet);

            var result = forwardChaning.AsQueryable().Any(fa => fa.Name == askingFact.Name && fa.Value == askingFact.Value);
            return result;
        }

        public object AskValueFact(string askName)
        {
            var rulesSet = new HashSet<Rule>(this.rules);
            var forwardChaning = ForwardChaining(this.facts, rulesSet);

            var result = forwardChaning.AsQueryable().FirstOrDefault(fa => fa.Name == askName);
            if (result != null)
            {
                return result.Value;
            }
            return null;
        }

        public List<Fact> ForwardChaining(List<Fact> factLst, HashSet<Rule> rulesSet)
        {
            foreach (var ru in rulesSet)
            {
                var matching_rule = ForwardMatch(factLst, ru);
                if (matching_rule.Any())
                {
                    factLst.AddRange(matching_rule);
                    rulesSet.Remove(ru);
                    return ForwardChaining(factLst, rulesSet);
                }
            }

            return factLst;
        }

        public List<Fact> ForwardMatch(List<Fact> factLst, Rule ru)
        {
            var factList = new List<Fact>();

            var matchRule = factLst.Select(r => r.Name).Intersect(ru.Antecedents.Select(f => f.Name));
            if (matchRule.Count() == ru.Antecedents.Count)
            {
                var matchFact = new List<Fact>();
                foreach (var item in ru.Antecedents)
                {
                    var fact = factLst.First(f => f.Name == item.Name);
                    matchFact.Add(new IsFact(item.Value.ToString(), fact.Value));
                }

                var consiquentFact = ru.Consequents as DoFact;
                var value = consiquentFact.DoExecute(matchFact);
                factList.Add(new IsFact(ru.Consequents.Name, value));
            }
            return factList;

            //AND Antecedents
            //var matchRule = factLst.Select(r => r.Value).Intersect(ru.Antecedents.Select(f => f.Value)).ToList();
            //if (matchRule.Count() == ru.Antecedents.Count)
            //{
            //    var matchFact = factLst.Where(f => matchRule.Contains(f.Value)).ToList();
            //    foreach (var item in matchFact)
            //    {
            //        factList.Add(new IsFact(item.Name, ru.Consequents.Value));
            //    }                
            //}
            //return factList;
        }
    }
}
