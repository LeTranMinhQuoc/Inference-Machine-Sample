using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpertSystem.Facts
{
    public class IsFact : Fact
    {
        public IsFact() : base("?", "=", "?") { }
        public IsFact(string name, object value) : base(name, "=", value)
        {
        }
    }
}
