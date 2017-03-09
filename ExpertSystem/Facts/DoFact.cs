using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpertSystem.Facts
{
    public class DoFact : Fact
    {
        public DoFact() : base("?", "=", "?")
        {
        }

        public DoFact(string name, object value) : base(name, "=", value)
        {
        }

        public object DoExecute(List<Fact> factLst)
        {
            var mathExpression = this.Value.ToString();
            foreach (var item in factLst)
            {
                mathExpression = mathExpression.Replace(item.Name, item.Value.ToString());
            }
            var value = new PolishNotation.PolishNotation().Cal(mathExpression);
            return value;
        }
    }
}
