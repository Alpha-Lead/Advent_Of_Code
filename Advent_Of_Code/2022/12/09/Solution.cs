
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
    internal class RopeBridgeTask
    {
        public static int CountTailPositions(bool printLocations = false)
        {
            List<Instruction> instructions = new List<Instruction>();
            using StreamReader sr = new StreamReader(File.OpenRead(@"..\..\..\2022\12\09\input.txt"));
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                instructions.Add(new Instruction(line));
            }

            Point start = new Point(0, 0);
            Point head = new Point(0, 0);
            Point tail = new Point(0, 0);

            List<Point> headHistory = new List<Point>() { new Point(0,0) };
            List<Point> tailHistory = new List<Point>() { new Point(0,0) };
            foreach (Instruction instruction in instructions)
            {
                if (printLocations) Console.WriteLine(instruction.ToString());
                for(int i = 0; i < instruction.steps; i++)
                {
                    //Move head
                    switch(instruction.direction)
                    {
                        case Direction.Up:
                            head.Y++;
                            break;
                        case Direction.Right:
                            head.X++;
                            break;
                        case Direction.Down:
                            head.Y--;
                            break;
                        case Direction.Left:
                            head.X--;
                            break;
                    }
                    headHistory.Add(new Point(head.X, head.Y));
                    if (printLocations) Console.Write($"H: ({head.X}, {head.Y})    ");

                    //Check if tail needs to move
                    if ((Math.Abs(head.Y - tail.Y) > 1) || (Math.Abs(head.X - tail.X) > 1))
                    {
                        //This accounts for both diagonal moving, and same column/row moving (not using >= || <=)
                        //Tail will always move diagonal if there is a change in both
                        if (head.Y > tail.Y) tail.Y++;
                        if (head.Y < tail.Y) tail.Y--;
                        if (head.X > tail.X) tail.X++;
                        if (head.X < tail.X) tail.X--;
                        //Log new position in history
                        tailHistory.Add(new Point(tail.X, tail.Y));
                        if (printLocations) Console.Write($"T: ({tail.X}, {tail.Y})*");
                    }
                    else
                    {
                        if (printLocations) Console.Write($"T: ({tail.X}, {tail.Y})");
                    }
                    if (printLocations) Console.Write('\n');
                }
            }

            int positionsTailTouched = tailHistory.Distinct().Count();
            return positionsTailTouched;
        }

        public static int CountTailPositionsLongRope(bool printLocations = false)
        {
            List<Instruction> instructions = new List<Instruction>();
            using StreamReader sr = new StreamReader(File.OpenRead(@"..\..\..\2022\12\09\input.txt"));
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                instructions.Add(new Instruction(line));
            }

            Point startPoint = new Point(0, 0);
            int numberOfRopeSegments = 10;
            LinkedList<RopeSegment> ropeList = new LinkedList<RopeSegment>();
            for (int i = 0; i < numberOfRopeSegments; i++)
            {
                ropeList.AddLast(new RopeSegment(startPoint));
            }

            foreach (Instruction instruction in instructions)
            {
                for (int i = 0; i < instruction.steps; i++)
                {
                    LinkedList<RopeSegment>.Enumerator enumerator = ropeList.GetEnumerator();
                    enumerator.MoveNext();
                    enumerator.Current.DoInstruction(instruction);
                    Point lastNodeLocation = new Point(enumerator.Current.point.X, enumerator.Current.point.Y);
                    while(enumerator.MoveNext())
                    {
                        enumerator.Current.Update(lastNodeLocation);
                        lastNodeLocation = new Point(enumerator.Current.point.X, enumerator.Current.point.Y);
                    }
                }
            }

            List<Point> tailHistory = ropeList.Last.Value.path;
            int positionsTailTouched = tailHistory.Distinct().Count();
            return positionsTailTouched;
        }

        private enum Direction : uint
        {
            None = 0,
            Up = 1,
            Right = 2,
            Down = 3,
            Left = 4
        }

        private static Dictionary<char, Direction> Char2Direction = new Dictionary<char, Direction>()
        {
            //Uppercase
            {'U', Direction.Up},
            {'R', Direction.Right},
            {'D', Direction.Down},
            {'L', Direction.Left},
            //Lowercase
            {'u', Direction.Up},
            {'r', Direction.Right},
            {'d', Direction.Down},
            {'l', Direction.Left}
        };

        private class Instruction
        {
            public Direction direction;
            public int steps;

            public Instruction(Direction d, int s)
            {
                this.direction = d;
                this.steps = s;
            }
            public Instruction(string instruction)
            {
                if (Regex.IsMatch(instruction, @"^(U|R|D|L) ([0-9]+)$", RegexOptions.IgnoreCase))
                {
                    Match match = Regex.Match(instruction, @"^(U|R|D|L) ([0-9]+)$", RegexOptions.IgnoreCase);
                    direction = Char2Direction[match.Groups[1].Value[0]];
                    steps = int.Parse(match.Groups[2].Value);
                }
                else
                {
                    throw new Exception("Invalid input format for Instruction class constructor");
                }
            }
            public Instruction()
            {
                this.direction = Direction.None;
                this.steps = 0;
            }

            public override bool Equals(Object obj)
            {
                if ((obj == null) || !this.GetType().Equals(obj.GetType()))
                {
                    return false;
                }
                else
                {
                    Instruction instr = (Instruction)obj;
                    return (direction == instr.direction) && (steps == instr.steps);
                }
            }
            public override int GetHashCode()
            {
                return (int)((steps)+((uint)direction * (Int32.MaxValue/4)));
            }
            public override string ToString()
            {
                string dirChr = "X";
                if (this.direction == Direction.Up) dirChr = "U";
                if (this.direction == Direction.Right) dirChr = "R";
                if (this.direction == Direction.Down) dirChr = "D";
                if (this.direction == Direction.Left) dirChr = "L";
                return $"{dirChr} {this.steps}";
            }
        }

        private class RopeSegment
        {
            public Point point;
            public List<Point> path;

            public RopeSegment(Point pt)
            {
                this.point = new Point(pt.X, pt.Y);
                this.path = new List<Point>() { point };
            }
            public RopeSegment()
            {
                this.point = new Point(0, 0);
                this.path = new List<Point>() { point };
            }

            public void DoInstruction(Instruction instruction)
            {
                switch (instruction.direction)
                {
                    case Direction.Up:
                        this.point.Y++;
                        break;
                    case Direction.Right:
                        this.point.X++;
                        break;
                    case Direction.Down:
                        this.point.Y--;
                        break;
                    case Direction.Left:
                        this.point.X--;
                        break;
                }
                path.Add(new Point(this.point.X, this.point.Y));
            }

            public void Update(Point pt)
            {
                //Check if this node needs to move
                if ((Math.Abs(pt.Y - this.point.Y) > 1) || (Math.Abs(pt.X - this.point.X) > 1))
                {
                    //This accounts for both diagonal moving, and same column/row moving (not using >= || <=)
                    //Tail will always move diagonal if there is a change in both
                    if (pt.Y > this.point.Y) this.point.Y++;
                    if (pt.Y < this.point.Y) this.point.Y--;
                    if (pt.X > this.point.X) this.point.X++;
                    if (pt.X < this.point.X) this.point.X--;
                    //Log new position in history
                    path.Add(new Point(this.point.X, this.point.Y));
                }
            }
        }
    }
}
