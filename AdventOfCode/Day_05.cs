using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

public class Day_05 : BaseDay
{
    private readonly string _input;
    private readonly List<string> _rawCoords;
    private readonly List<Line> _lines = new List<Line>();

    public Day_05()
    {
        _input = File.ReadAllText(InputFilePath);
        _rawCoords = _input.Split("\r\n").ToList();
        foreach (string coord in _rawCoords)
        {
            string[] bothCoords = coord.Split(("->"));
            string firstX = bothCoords[0].Split(",")[0];
            string firstY = bothCoords[0].Split(",")[1];
            string secondX = bothCoords[1].Split(",")[0];
            string secondY = bothCoords[1].Split(",")[1];
            Line line = new Line();
            Coordinate firstCoord = new Coordinate(int.Parse(firstX), int.Parse(firstY));
            Coordinate secondCoord = new Coordinate(int.Parse(secondX), int.Parse(secondY));
            line.FirstCoordinate = firstCoord;
            line.SecondCoordinate = secondCoord;
            _lines.Add(line);
        }
    }

    private int Solve1()
    {
        var noDiagonalLines = _lines.Where(x => !x.IsDiagonal()).ToList();

        List<Coordinate> allCoords = new List<Coordinate>();
        foreach (Line line in noDiagonalLines)
        {
            allCoords.AddRange(line.FullRangeNoDiag());
        }

        int overlapCount = allCoords.GroupBy(x => new {x.X, x.Y})
            .Where(g => g.Count() > 1)
            .Select(y => new {Element = y.Key, Counter = y.Count()})
            .ToList().Count;

        return overlapCount;
    }

    private int Solve2()
    {
        List<Coordinate> allLines = new List<Coordinate>();
        foreach (Line line in _lines)
        {
            allLines.AddRange(line.FullRangeNoDiag());
        }
        foreach (Line line in _lines)
        {
            allLines.AddRange(line.FullRangeDiag());
        }
        
        int overlapCount = allLines.GroupBy(x => new {x.X, x.Y})
            .Where(g => g.Count() > 1)
            .Select(y => new {Element = y.Key, Counter = y.Count()})
            .ToList().Count;

        return overlapCount;
    }

    public override ValueTask<string> Solve_1() => new(Solve1().ToString());

    public override ValueTask<string> Solve_2() => new(Solve2().ToString());

    public class Line
    {
        public Coordinate FirstCoordinate { get; set; }
        public Coordinate SecondCoordinate { get; set; }

        public bool IsDiagonal()
        {
            if (FirstCoordinate.X != SecondCoordinate.X && FirstCoordinate.Y != SecondCoordinate.Y)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Horrendous
        /// </summary>
        /// <returns></returns>
        public List<Coordinate> FullRangeDiag()
        {
            List<Coordinate> fullRange = new List<Coordinate>();
            if (!IsDiagonal())
            {
                return fullRange;
            }
            fullRange.Add(FirstCoordinate);
            
            bool intersected = false;
            while (!intersected)
            {
                Coordinate coordToAdd = new Coordinate();
                Coordinate coordToCompare;
                if (fullRange.Count > 0)
                {
                    coordToCompare = fullRange.LastOrDefault();
                }
                else
                {
                    coordToCompare = FirstCoordinate;
                }

                if (coordToCompare.X > SecondCoordinate.X)
                {
                    coordToAdd.X = coordToCompare.X - 1;
                }
                else
                {
                    coordToAdd.X = coordToCompare.X + 1;
                }

                if (coordToCompare.Y > SecondCoordinate.Y)
                {
                    coordToAdd.Y = coordToCompare.Y - 1;
                }
                else
                {
                    coordToAdd.Y = coordToCompare.Y + 1;
                }

                fullRange.Add(coordToAdd);


                if (coordToAdd.X == SecondCoordinate.X && coordToAdd.Y == SecondCoordinate.Y)
                {
                    intersected = true;
                }
            }


            return fullRange;
        }

        /// <summary>
        /// There must be a better way to do this, this is grim, calculate all items within the range
        /// </summary>
        /// <returns>A rubbish Coorindate array</returns>
        public List<Coordinate> FullRangeNoDiag()
        {
            List<Coordinate> fullRange = new List<Coordinate>();
            if (IsDiagonal())
            {
                return fullRange;
            }
            
            int xDiff = Math.Abs(FirstCoordinate.X - SecondCoordinate.X);
            int yDiff = Math.Abs(FirstCoordinate.Y - SecondCoordinate.Y);
            for (int i = 0; i < xDiff; i++)
            {
                Coordinate c;
                if (FirstCoordinate.X > SecondCoordinate.X)
                {
                    c = new Coordinate(FirstCoordinate.X - i, FirstCoordinate.Y);
                }
                else
                {
                    c = new Coordinate(FirstCoordinate.X + i, FirstCoordinate.Y);
                }

                fullRange.Add(c);
            }

            for (int i = 0; i < yDiff; i++)
            {
                Coordinate c;
                if (FirstCoordinate.Y > SecondCoordinate.Y)
                {
                    c = new Coordinate(FirstCoordinate.X, FirstCoordinate.Y - i);
                }
                else
                {
                    c = new Coordinate(FirstCoordinate.X, FirstCoordinate.Y + i);
                }

                fullRange.Add(c);
            }

            fullRange.Add(SecondCoordinate);
            return fullRange;
        }
    }

    public class Coordinate
    {
        public Coordinate()
        {
        }

        public Coordinate(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; set; }
        public int Y { get; set; }
    }
}