using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent_Of_Code
{
    internal static class RockPaperScissorsTask
    {
        public static int Strategy1()
        {
            using StreamReader sr = new StreamReader(File.OpenRead(@"..\..\..\2022\12\02\input.txt"));
            string line;
            int score = 0;
            Move mmv, tmv;

            //Read input
            while ((line = sr.ReadLine()) != null)
            {
                tmv = DecodeTheirMove[line[0]];
                mmv = DecodeMyMove[line[2]];

                //Score from move
                score += ((int)mmv);

                //Determine outcome, and score bonus
                if ((mmv == Move.Rock && tmv == Move.Scisors) ||
                    (mmv == Move.Paper && tmv == Move.Rock) || 
                    (mmv == Move.Scisors && tmv == Move.Paper))
                {
                    score += ((int)Outcome.Win);
                }
                else if ((mmv == Move.Rock && tmv == Move.Paper) ||
                         (mmv == Move.Paper && tmv == Move.Scisors) ||
                         (mmv == Move.Scisors && tmv == Move.Rock))
                {
                    score += ((int)Outcome.Lose);
                }
                else
                {
                    score += ((int)Outcome.Draw);
                }
            }

            return score;
        }

        public static int Strategy2()
        {
            using StreamReader sr = new StreamReader(File.OpenRead(@"..\..\..\2022\12\02\input.txt"));
            string line;
            int score = 0;
            Move mmv, tmv;
            Outcome dsrOut;

            //Read input
            while ((line = sr.ReadLine()) != null)
            {
                tmv = DecodeTheirMove[line[0]];
                dsrOut = DecodeOutcome[line[2]];

                //Score from outcome
                score += ((int)dsrOut);

                //Determine score of move
                if ((tmv == Move.Rock && dsrOut == Outcome.Draw) ||
                    (tmv == Move.Paper && dsrOut == Outcome.Lose) ||
                    (tmv == Move.Scisors && dsrOut == Outcome.Win))
                {
                    score += ((int)Move.Rock);
                }
                else if ((tmv == Move.Rock && dsrOut == Outcome.Win) ||
                         (tmv == Move.Paper && dsrOut == Outcome.Draw) ||
                         (tmv == Move.Scisors && dsrOut == Outcome.Lose))
                {
                    score += ((int)Move.Paper);
                }
                else
                {
                    score += ((int)Move.Scisors);
                }
            }

            return score;
        }

        internal enum Move : uint 
        {
            Rock = 1,
            Paper = 2,
            Scisors = 3
        }
        internal enum Outcome : uint
        {
            Lose = 0,
            Draw = 3,
            Win = 6
        }

        internal static Dictionary<char, Move> DecodeTheirMove = new Dictionary<char, Move>()
        {
            {'A', Move.Rock},
            {'B', Move.Paper},
            {'C', Move.Scisors}
        };
        internal static Dictionary<char, Move> DecodeMyMove = new Dictionary<char, Move>()
        {
            {'X', Move.Rock},
            {'Y', Move.Paper},
            {'Z', Move.Scisors}
        };
        internal static Dictionary<char, Outcome> DecodeOutcome = new Dictionary<char, Outcome>()
        {
            {'X', Outcome.Lose},
            {'Y', Outcome.Draw},
            {'Z', Outcome.Win}
        };

    }
}
