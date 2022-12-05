
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Advent_Of_Code.RockPaperScissorsTask;

namespace Advent_Of_Code
{
    internal class CraneTask
    {
        public static string WhatIsOnTop1()
        {
            using StreamReader sr = new StreamReader(File.OpenRead(@"..\..\..\2022\12\05\input.txt"));
            string line;
            List<CraneInstruction> instructions = new List<CraneInstruction>();
            List<Stack<char>> cargoState = new List<Stack<char>>();

            //Read input
            int readMode = 0;
            bool runPreBuild = true;
            while ((line = sr.ReadLine()) != null)
            {
                if (String.IsNullOrEmpty(line))
                {
                    //Seperator
                    //Do nothing
                    for (int i = 0; i < cargoState.Count; i++)
                    {
                        Stack<char> revStack = new Stack<char>();
                        char temp;
                        while (cargoState[i].TryPop(out temp))
                        {
                            revStack.Push(temp);
                        }
                        cargoState[i] = revStack;
                    }
                }
                else if (line.StartsWith("move"))
                {
                    //Instructions
                    CraneInstruction instruction = new CraneInstruction();
                    instruction.amount = int.Parse(line.Split(' ')[1]);
                    instruction.from = int.Parse(line.Split(' ')[3]);
                    instruction.to = int.Parse(line.Split(' ')[5]);
                    instructions.Add(instruction);
                }
                else
                {
                    //State
                    int numCols = (line.Length + 1) / 4; //[#]_
                    if (runPreBuild)
                    {
                        for (int i = 0; i < numCols; i++)
                        {
                            cargoState.Add(new Stack<char>());
                        }
                        runPreBuild = false;
                    }
                    for (int i = 0; i < numCols; i++)
                    {
                        string temp = line.Substring(i * 4, 3);
                        if (temp.StartsWith('[') && temp.EndsWith(']'))
                        {
                            //Value
                            cargoState[i].Push(temp.ToCharArray()[1]);
                        }
                        else if (int.TryParse(temp.Trim(), out _))
                        {
                            //Row counters
                            //Discard, as not needed
                        }
                    }
                }

            }

            //Process instructions
            foreach (CraneInstruction instuction in instructions)
            {
                for (int i = 0; i < instuction.amount; i++)
                {
                    cargoState[instuction.to - 1].Push(cargoState[instuction.from - 1].Pop()); //-1 for 0-index offset
                }
            }

            //Get the crate on top
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < cargoState.Count; i++)
            {
                char temp;
                if (cargoState[i].TryPeek(out temp))
                {
                    stringBuilder.Append(temp);
                }
            }

            return stringBuilder.ToString();
        }

        public static string WhatIsOnTop2()
        {
            using StreamReader sr = new StreamReader(File.OpenRead(@"..\..\..\2022\12\05\input.txt"));
            string line;
            List<CraneInstruction> instructions = new List<CraneInstruction>();
            List<Stack<char>> cargoState = new List<Stack<char>>();

            //Read input
            int readMode = 0;
            bool runPreBuild = true;
            while ((line = sr.ReadLine()) != null)
            {
                if (String.IsNullOrEmpty(line))
                {
                    //Seperator
                    //Do nothing
                    for (int i = 0; i < cargoState.Count; i++)
                    {
                        cargoState[i] = ReverseStackChar(cargoState[i]);
                    }
                }
                else if (line.StartsWith("move"))
                {
                    //Instructions
                    CraneInstruction instruction = new CraneInstruction();
                    instruction.amount = int.Parse(line.Split(' ')[1]);
                    instruction.from = int.Parse(line.Split(' ')[3]);
                    instruction.to = int.Parse(line.Split(' ')[5]);
                    instructions.Add(instruction);
                }
                else
                {
                    //State
                    int numCols = (line.Length + 1) / 4; //[#]_
                    if (runPreBuild)
                    {
                        for (int i = 0; i < numCols; i++)
                        {
                            cargoState.Add(new Stack<char>());
                        }
                        runPreBuild = false;
                    }
                    for (int i = 0; i < numCols; i++)
                    {
                        string temp = line.Substring(i * 4, 3);
                        if (temp.StartsWith('[') && temp.EndsWith(']'))
                        {
                            //Value
                            cargoState[i].Push(temp.ToCharArray()[1]);
                        }
                        else if (int.TryParse(temp.Trim(), out _))
                        {
                            //Row counters
                            //Discard, as not needed
                        }
                    }
                }

            }

            //Process instructions (maintaining order, as if removed as group)
            foreach (CraneInstruction instuction in instructions)
            {
                Stack<char> buffer = new Stack<char>();
                for (int i = 0; i < instuction.amount; i++)
                {
                    buffer.Push(cargoState[instuction.from - 1].Pop()); //-1 for 0-index offset
                }
                for (int i = 0; i < instuction.amount; i++)
                {
                    cargoState[instuction.to - 1].Push(buffer.Pop()); //-1 for 0-index offset
                }
            }

            //Get the crate on top
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < cargoState.Count; i++)
            {
                char temp;
                if (cargoState[i].TryPeek(out temp))
                {
                    stringBuilder.Append(temp);
                }
            }

            return stringBuilder.ToString();
        }

        private static Stack<char> ReverseStackChar(Stack<Char> stack)
        {
            Stack<char> revStack = new Stack<char>();
            char temp;
            while (stack.TryPop(out temp))
            {
                revStack.Push(temp);
            }
            return revStack;
        }
    }

    internal class CraneInstruction
    {
        public int amount;
        public int from;
        public int to;
    }
}
