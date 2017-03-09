using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolishNotation
{
    public class Program
    {
        public static void Main(string[] args)
        {   
            PolishNotation polish = new PolishNotation();
            string sample = "Sqrt(9+(8+2)*2^2)";
            Console.WriteLine("input: "+ sample);
            Console.WriteLine("output: ");
            Console.WriteLine(polish.Cal(sample));

            while (true)
            {
                Console.WriteLine("input: ");
                var infix = Console.ReadLine();
                var result = polish.Cal(infix);
                Console.WriteLine("output: ");
                Console.WriteLine(result);
            }
            //Console.ReadKey();
        }
    }
}
