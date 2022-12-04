
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent_Of_Code
{
    internal class CampCleanupTask
    {
        public static int Overlap1()
        {
            using StreamReader sr = new StreamReader(File.OpenRead(@"..\..\..\2022\12\04\input.txt"));
            int overlapCounter = 0;
            string line;

            //Read input
            while ((line = sr.ReadLine()) != null)
            {
                string range1 = line[..line.IndexOf(',')];
                string range2 = line[(line.IndexOf(',')+1)..];
                int range1start = int.Parse(range1.Split('-')[0]);
                int range1end = int.Parse(range1.Split('-')[1]);
                int range2start = int.Parse(range2.Split('-')[0]);
                int range2end = int.Parse(range2.Split('-')[1]);
                
                if ((range1start <= range2start && range1end >= range2end) ||
                    (range2start <= range1start && range2end >= range1end))
                {
                    overlapCounter++;
                }
            }

            return overlapCounter;
        }

        public static int Overlap2()
        {
            using StreamReader sr = new StreamReader(File.OpenRead(@"..\..\..\2022\12\04\input.txt"));
            int overlapCounter = 0;
            string line;

            //Read input
            while ((line = sr.ReadLine()) != null)
            {
                string range1 = line[..line.IndexOf(',')];
                string range2 = line[(line.IndexOf(',') + 1)..];
                int range1start = int.Parse(range1.Split('-')[0]);
                int range1end = int.Parse(range1.Split('-')[1]);
                int range2start = int.Parse(range2.Split('-')[0]);
                int range2end = int.Parse(range2.Split('-')[1]);

                if ((range2start >= range1start && range2start <= range1end) ||
                    (range1start >= range2start && range1start <= range2end) ||
                    (range2end >= range1start && range2end <= range1end) ||
                    (range1end >= range2start && range1end <= range2end))
                {
                    overlapCounter++;
                }
            }

            return overlapCounter;
        }
    }
}
