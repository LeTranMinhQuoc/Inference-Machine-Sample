using ExpertSystem.Facts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpertSystem.Rules
{
    public class Rule
    {
        public IList<Fact> Antecedents { get; private set; }
        public Fact Consequents { get; private set; }

        public Rule(IList<Fact> antecedents, Fact consequents)
        {
            this.Antecedents = antecedents;
            this.Consequents = consequents;
        }        
    }
}
