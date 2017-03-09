using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpertSystem.Facts
{
    public abstract class Fact
    {
        public string Name { get; private set; }

        public object Verb { get; private set; }

        public object Value { get; private set; }

        public Fact()
        {
        }

        public Fact(string name, object verb, object value)
        {
            Name = name;
            Verb = verb;
            Value = value;
        }     

        public virtual void SetName(string name)
        {
            this.Name = name;
        }

        public virtual void SetValue(string value)
        {
            this.Value = value;
        }
    }
}
