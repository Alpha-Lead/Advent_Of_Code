using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Advent_Of_Code
{
    internal static class CalorieTask
    {
        public static int? TheMostCarried()
        {
            using StreamReader sr = new StreamReader(File.OpenRead(@"..\..\..\2022\12\01\input.txt"));
            List<int> amounts = new List<int>();
            string lineStr;
            int linInt = 0;
            int current = 0;

            while ((lineStr = sr.ReadLine()) != null)
            {
                if ((lineStr.Length > 0) && (int.TryParse(lineStr, out linInt)))
                {
                    current += linInt;
                }
                else
                {
                    amounts.Add(current);
                    current = 0;
                }
            }
            return amounts.Max();
        }

        public static int? TopThreeMostCarriedTotal()
        {
            StreamReader sr = new StreamReader(File.OpenRead(@"..\..\..\2022\12\01\input.txt"));
            List<int> amounts = new List<int>();
            string lineStr;
            int linInt = 0;
            int current = 0;

            while ((lineStr = sr.ReadLine()) != null)
            {
                if ((lineStr.Length > 0) && (int.TryParse(lineStr, out linInt)))
                {
                    current += linInt;
                }
                else
                {
                    amounts.Add(current);
                    current = 0;
                }
            }
            //Sort smallest to largest
            amounts.Sort();
            //Sort largest to smallest by reversing
            amounts.Reverse();
            //Get total of 3-largest amounts
            return amounts.GetRange(0, 3).Sum();
        }
    }
}
