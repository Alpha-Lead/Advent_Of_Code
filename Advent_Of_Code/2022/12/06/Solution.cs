
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Advent_Of_Code.RockPaperScissorsTask;

namespace Advent_Of_Code
{
    internal class CommunicatorTask
    {
        public static int FindStartOfPacket()
        {
            using StreamReader sr = new StreamReader(File.OpenRead(@"..\..\..\2022\12\06\input.txt"));

            //Start of packet indicator: 4 different characters

            //Read input
            int read;
            int position = 0;
            char[] buffer = new char[4];
            while ((read = sr.Read()) >= 0)
            {
                buffer[position%4] = (char)read;
                position++;
                if (position >= 4)
                {
                    if ((buffer[0] != buffer[1]) && (buffer[0] != buffer[2]) && (buffer[0] != buffer[3]) &&
                        (buffer[1] != buffer[2]) && (buffer[1] != buffer[3]) && (buffer[2] != buffer[3]))
                    {
                        return position;
                    }
                }
            }
            return position;
        }

        public static int FindStartOfMessage()
        {
            using StreamReader sr = new StreamReader(File.OpenRead(@"..\..\..\2022\12\06\input.txt"));

            //Start of message indicator: 14 different characters
            int disctinctCharacterLengthTrigger = 14;

            //Read input
            int read;
            int position = 0;
            char[] buffer = new char[disctinctCharacterLengthTrigger];
            while ((read = sr.Read()) >= 0)
            {
                buffer[position % disctinctCharacterLengthTrigger] = (char)read;
                position++;
                if (position >= disctinctCharacterLengthTrigger)
                {
                    if (buffer.ToList().Distinct().Count() == disctinctCharacterLengthTrigger)
                    {
                        return position;
                    }
                }
            }
            return position;
        }


    }
}
