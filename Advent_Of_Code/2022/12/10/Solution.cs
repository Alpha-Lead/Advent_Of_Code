
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Advent_Of_Code
{
    internal class CathodeCPUTask
    {
        public static int SumSignalStrength()
        {
            using StreamReader sr = new StreamReader(File.OpenRead(@"..\..\..\2022\12\10\input.txt"));
            string line;
            List<string> instructions = new List<string>();
            while ((line = sr.ReadLine()) != null)
            {
                instructions.Add(line);
            }

            int registerX = 1; //Starting value
            List<int> registerX_history= new List<int>();
            foreach (string instruction in instructions) {
                registerX_history.Add(registerX);
                if (instruction.ToLower().StartsWith("addx"))
                {
                    // addx V takes two cycles to complete.
                    // After two cycles, the X register is increased by the value V. (V can be negative.)
                    registerX_history.Add(registerX);
                    registerX = registerX + int.Parse(instruction.Substring("addx ".Length));
                }
                else if (instruction.ToLower().StartsWith("noop"))
                {
                    // noop takes one cycle to complete. It has no other effect.
                }
            }

            int sumSignalStrength = 0;
            sumSignalStrength += 20 * registerX_history[20 - 1];
            sumSignalStrength += 60 * registerX_history[60 - 1];
            sumSignalStrength += 100 * registerX_history[100 - 1];
            sumSignalStrength += 140 * registerX_history[140 - 1];
            sumSignalStrength += 180 * registerX_history[180 - 1];
            sumSignalStrength += 220 * registerX_history[220 - 1];
            return sumSignalStrength;
        }

        public static string DrawCRTOutput()
        {
            using StreamReader sr = new StreamReader(File.OpenRead(@"..\..\..\2022\12\10\input.txt"));
            string line;
            List<string> instructions = new List<string>();
            while ((line = sr.ReadLine()) != null)
            {
                instructions.Add(line);
            }

            int screenWidth = 40, screenHeight = 6;
            char[,] screen = new char[screenWidth, screenHeight];

            int instructionNumber = 0;
            int cycleNumber = 0;
            int registerX = 1; 
            int registerGhost = 1;
            int state = 0;
            do
            {
                //Start of cycle
                switch (state)
                {
                    case 0: //Read next instruction
                        if (instructionNumber < instructions.Count)
                        {
                            if (instructions[instructionNumber].ToLower().StartsWith("addx"))
                            {
                                // "addx V" takes two cycles to complete.
                                // After two cycles, the X register is increased by the value V. (V can be negative.)
                                registerGhost = registerX + int.Parse(instructions[instructionNumber].Substring("addx ".Length));
                                state += 2;
                            }
                            else if (instructions[instructionNumber].ToLower().StartsWith("noop"))
                            {
                                // "noop" takes one cycle to complete. It has no other effect.
                                registerGhost = registerX;
                                state += 1;
                            }
                            instructionNumber++;
                        }
                        break;
                }

                //During cycle
                //Calculate CRT output
                int targetPixelColumn = cycleNumber % screenWidth;
                int targetPixelRow = ((int)(cycleNumber/screenWidth)) % screenHeight;
                if((targetPixelColumn >= registerX - 1) && (targetPixelColumn <= registerX + 1))
                {
                    screen[targetPixelColumn, targetPixelRow] = '#'; //ON
                }
                else
                {
                    screen[targetPixelColumn, targetPixelRow] = '.'; //OFF
                }

                //End of Cycle
                switch (state)
                {
                    case 1: //Finishing an instruction
                        registerX = registerGhost;
                        break;
                }

                //Move on
                cycleNumber++;
                state--;
            } while (state >= 0);

            //Render screen as string
            StringBuilder stringBuilder= new StringBuilder();
            for(int i = 0; i < (screenHeight*screenWidth); i ++)
            {
                int targetPixelColumn = i % screenWidth;
                int targetPixelRow = ((int)(i / screenWidth)) % screenHeight;
                stringBuilder.Append(screen[targetPixelColumn, targetPixelRow]);
                if (((i + 1) % screenWidth) == 0) stringBuilder.Append('\n');
            }
            return stringBuilder.ToString();
        }
    }
}
