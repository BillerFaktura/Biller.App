using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biller.Data.Performance
{
    public class Stopwatch : System.Diagnostics.Stopwatch
    {
        public Stopwatch(string description)
            : base()
        {
            _description = description;
        }

        private string _description;

        public void PrintResultToConsole()
        {
            Console.WriteLine("Duration for " + _description + ": " + ElapsedMilliseconds.ToString() + "ms");
        }

        public void PrintResultToConsole(int items)
        {
            Console.WriteLine("Duration for " + _description + " with " + items.ToString() + " items: " + ElapsedMilliseconds.ToString() + "ms");
        }

        public string Result()
        {
            return "Duration for " + _description + ": " + ElapsedMilliseconds.ToString() + "ms";
        }

        public string Result(int items)
        {
            return "Duration for " + _description + " with " + items.ToString()+" items: " + ElapsedMilliseconds.ToString() + "ms";
        }
    }
}
