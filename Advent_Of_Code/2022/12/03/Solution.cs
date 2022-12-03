using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Advent_Of_Code.RockPaperScissorsTask;

namespace Advent_Of_Code
{
    internal static class RucksackTask
    {
        public static int Search1()
        {
            using StreamReader sr = new StreamReader(File.OpenRead(@"..\..\..\2022\12\03\input.txt"));
            string line;
            int prioritySum = 0;
            
            //Read input
            while ((line = sr.ReadLine()) != null)
            {
                char[] compartment1 = line.Substring(0, line.Length / 2).ToCharArray();
                char[] compartment2 = line.Substring(line.Length / 2).ToCharArray();
                char common = compartment1.First(c => compartment2.Contains(c));

                prioritySum += CharToPriority(common);
            }

            return prioritySum;
        }

        public static int Search2()
        {
            using StreamReader sr = new StreamReader(File.OpenRead(@"..\..\..\2022\12\03\input.txt"));
            string line;
            List<string> lines = new List<string>(3);
            int prioritySum = 0;

            //Read input
            while ((line = sr.ReadLine()) != null)
            {
                lines.Add(line);
                if (lines.Count == 3)
                {
                    char common = lines[0].First(c => lines[1].Contains(c) && lines[2].Contains(c));
                    prioritySum += CharToPriority(common);
                    lines.Clear();
                }
            }

            return prioritySum;
        }

        private static int CharToPriority(char c)
        {
            //Remap to a=1, z=26 and A=27, Z=52, then add to sum total
            int i = (int)c;
            //a=97 z=122
            if (i >= 97 && i <= 122) return (i - 96);
            //A=65, Z=90
            else if (i >= 65 && i <= 90) return (i - 64 + 26);
            else return 0;
        }
    }
}
