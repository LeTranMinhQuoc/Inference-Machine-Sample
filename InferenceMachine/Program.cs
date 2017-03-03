using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InferenceMachine
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Engine e = new Engine();
            e.Init();

            string sample = "if a=30 and b=19 and c=14 then S=?";
            Console.WriteLine("Q: " + sample);
            Console.WriteLine("A: " + e.GetResult(sample));

            while (true)
            {
                Console.WriteLine("Q: ");
                var q = Console.ReadLine();
                var result = e.GetResult(q);
                Console.WriteLine("A: " + result);
            }
            // ruleParser.Parse();
        }
    }
}
