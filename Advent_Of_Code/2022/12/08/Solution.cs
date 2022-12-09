
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Advent_Of_Code.RockPaperScissorsTask;

namespace Advent_Of_Code
{
    internal class TreeTopTask
    {
        private static int[,] LoadTreeHeights()
        {
            using StreamReader sr = new StreamReader(File.OpenRead(@"..\..\..\2022\12\08\input.txt"));
            List<string> lineBuffer = new List<string>();
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                lineBuffer.Add(line);
            }
            if (lineBuffer.Count == 0) return null;

            int height = lineBuffer.Count;
            int width = lineBuffer[0].Length;

            int[,] treeHeights = new int[width,height];
            for (int row = 0; row < height; row++)
            {
                for (int column = 0; column < width; column++)
                {
                    treeHeights[column,row] = int.Parse(lineBuffer[row][column].ToString());
                }
            } 
            return treeHeights;
        }

        public static int CountVisibleTrees()
        {
            int[,] treeGrid = LoadTreeHeights();
            int rowCount = treeGrid.GetLength(0);
            int columnCount = treeGrid.GetLength(1);

            //A tree is visible if it can bee seen from the edge (ie. all trees between it and the edge are smaller)
            List<Coordinate> seenFromTop = new List<Coordinate>();
            List<Coordinate> seenFromBottom = new List<Coordinate>();
            List<Coordinate> seenFromLeft = new List<Coordinate>();
            List<Coordinate> seenFromRight = new List<Coordinate>();
            //Top looking Down
            for (int col = 0; col < columnCount; col++)
            {
                int localMaxHeight = -1;
                for (int row = 0; row < rowCount; row++)
                {
                    int dummy = treeGrid[col, row];
                    if (treeGrid[col, row] > localMaxHeight)
                    {
                        seenFromTop.Add(new Coordinate(col, row));
                        localMaxHeight= treeGrid[col, row];
                    }
                }
            }
            //Bottom looking Up
            for (int col = 0; col < columnCount; col++)
            {
                int localMaxHeight = -1;
                for (int row = (rowCount - 1); row >= 0; row--)
                {
                    int dummy = treeGrid[col, row];
                    if (treeGrid[col, row] > localMaxHeight)
                    {
                        seenFromBottom.Add(new Coordinate(col, row));
                        localMaxHeight = treeGrid[col, row];
                    }
                }
            }
            //Left looking Right
            for (int row = 0; row < rowCount; row++)
            {
                int localMaxHeight = -1;
                for (int col = 0; col < columnCount; col++)
                {
                    int dummy = treeGrid[col, row];
                    if (treeGrid[col, row] > localMaxHeight)
                    {
                        seenFromLeft.Add(new Coordinate(col, row));
                        localMaxHeight = treeGrid[col, row];
                    }
                }
            }
            //Right looking Left
            for (int row = 0; row < rowCount; row++)
            {
                int localMaxHeight = -1;
                for (int col = (columnCount - 1); col >= 0; col--)
                {
                    int dummy = treeGrid[col, row];
                    if (treeGrid[col, row] > localMaxHeight)
                    {
                        seenFromRight.Add(new Coordinate(col, row));
                        localMaxHeight = treeGrid[col, row];
                    }
                }
            }

            seenFromTop.AddRange(seenFromBottom);
            seenFromTop.AddRange(seenFromLeft);
            seenFromTop.AddRange(seenFromRight);
            int visibleTreeCount = seenFromTop.Distinct().Count();
            return visibleTreeCount;
        }

        public static int HighestScenicScore()
        {
            int[,] treeGrid = LoadTreeHeights();
            int rowCount = treeGrid.GetLength(0);
            int columnCount = treeGrid.GetLength(1);

            int maxScenicScore = 0;
            for (int column = 1; column < columnCount; column++)
            {
                for (int row = 1; row < rowCount; row++) 
                { 
                    int treeHeight = treeGrid[column, row];
                    int counter;
                    bool sameHeightOrTaller;
                    //Look left & count
                    counter = 1;
                    sameHeightOrTaller = false;
                    while ((column - counter >= 0) && (sameHeightOrTaller == false))
                    {
                        if (treeGrid[column - counter, row] >= treeHeight)
                        {
                            sameHeightOrTaller = true;
                        }
                        counter++;
                    }
                    int treesLeft = counter - 1;

                    //Look right & count
                    counter = 1;
                    sameHeightOrTaller = false;
                    while ((column + counter < columnCount) && (sameHeightOrTaller == false))
                    {
                        if (treeGrid[column + counter, row] >= treeHeight)
                        {
                            sameHeightOrTaller = true;
                        }
                        counter++;
                    }
                    int treesRight = counter - 1;

                    //Look up & count
                    counter = 1;
                    sameHeightOrTaller = false;
                    while ((row - counter >= 0) && (sameHeightOrTaller == false))
                    {
                        if (treeGrid[column, row - counter] >= treeHeight)
                        {
                            sameHeightOrTaller = true;
                        }
                        counter++;
                    }
                    int treesUp = counter - 1;

                    //Look down & count
                    counter = 1;
                    sameHeightOrTaller = false;
                    while ((row + counter < rowCount) && (sameHeightOrTaller == false))
                    {
                        if (treeGrid[column, row + counter] >= treeHeight)
                        {
                            sameHeightOrTaller = true;
                        }
                        counter++;
                    }
                    int treesDown = counter - 1;

                    //Calculate scenic score & compare
                    int scenicScore = treesLeft * treesRight * treesUp * treesDown;
                    if (scenicScore > maxScenicScore)
                    {
                        maxScenicScore = scenicScore;
                    }
                }
            }
            return maxScenicScore;
        }

        private class Coordinate
        {
            public int column;
            public int row;
            public int x { get { return column; } set { column = value; } }
            public int y { get { return row; } set { row = value; } }

            public Coordinate(int x, int y)
            {
                this.column = x;
                this.row = y;
            }
            public Coordinate()
            {
                this.column = 0;
                this.row = 0;
            }

            public override bool Equals(Object obj)
            {
                if ((obj == null) || !this.GetType().Equals(obj.GetType()))
                {
                    return false;
                }
                else
                {
                    Coordinate coord = (Coordinate)obj;
                    return (column == coord.column) && (row == coord.row);
                }
            }
            public override int GetHashCode()
            {
                return (x)+(y * (Int32.MaxValue/2));
            }
        }

    }
}
