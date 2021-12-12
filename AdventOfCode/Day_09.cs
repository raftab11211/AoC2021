using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Threading.Tasks;

public class Day_09 : BaseDay
{
    private readonly List<string> _input;
    private List<Point> _points = new List<Point>();

    public Day_09()
    {
        _input = File.ReadAllText(InputFilePath).Split("\r\n").ToList();

        //Create grid
        for (int i = 0; i < _input.Count; i++)
        {
            char[] charsForLine = _input[i].ToCharArray();
            for (int t = 0; t < charsForLine.Length; t++)
            {
                //Check to see if the loop has created this point already
                Point p = _points.FirstOrDefault(x => x.X == i && x.Y == t);
                if (p == null)
                {
                    p = new Point();
                    p.X = i;
                    p.Y = t;
                }

                p.PointValue = int.Parse(charsForLine[t].ToString());

                //We can go up
                if (i > 0)
                {
                    Point up = _points.FirstOrDefault(x => x.X == (i - 1) && x.Y == t);
                    if (up == null)
                    {
                        up = new Point();
                        up.X = i - 1;
                        up.Y = t;
                        _points.Add(up);
                    }
                    else
                    {
                        up.Down = p;
                    }

                    p.Up = up;
                }

                //We can go down
                if (i != (_input.Count - 1))
                {
                    Point down = _points.FirstOrDefault(x => x.X == (i + 1) && x.Y == t);
                    if (down == null)
                    {
                        down = new Point();
                        down.X = i + 1;
                        down.Y = t;
                        _points.Add(down);
                    }
                    else
                    {
                        down.Up = p;
                    }

                    p.Down = down;
                }

                //We can go left
                if (t > 0)
                {
                    Point left = _points.FirstOrDefault(x => x.X == i && x.Y == (t - 1));
                    if (left == null)
                    {
                        left = new Point();
                        left.X = i;
                        left.Y = t - 1;

                        _points.Add(left);
                    }
                    else
                    {
                        left.Right = p;
                    }

                    p.Left = left;
                }

                //We can go right
                if (t != (charsForLine.Length - 1))
                {
                    Point right = _points.FirstOrDefault(x => x.X == i && x.Y == (t + 1));
                    if (right == null)
                    {
                        right = new Point();
                        right.X = i;
                        right.Y = t + 1;
                        _points.Add(right);
                    }
                    else
                    {
                        right.Left = p;
                    }

                    p.Right = right;
                }

                _points.Add(p);
            }
        }
    }

    private int Solve1()
    {
        int totalOfLowPoints = 0;
        List<Point> lowestPoints = GetLowestPoints();
        totalOfLowPoints += GetLowestPoints().Sum(x => x.PointValue);
        totalOfLowPoints += lowestPoints.Count;

        return totalOfLowPoints;
    }

    private int Solve2()
    {
        List<Point> lowestPoints = GetLowestPoints();
        List<Basin> basins = GetBasins(lowestPoints);

        int threeLargestBasins = 0;
        var orderedBasins = basins.OrderByDescending(x => x.Points.Count).ToList();
        threeLargestBasins = orderedBasins[0].Points.Count
                             * orderedBasins[1].Points.Count
                             * orderedBasins[2].Points.Count;


        return threeLargestBasins;
    }

    public List<Point> GetLowestPoints()
    {
        List<Point> lowestPoints = new List<Point>();

        foreach (Point p in _points)
        {
            if (p.Up != null && p.Up.PointValue <= p.PointValue)
            {
                continue;
            }

            if (p.Down != null && p.Down.PointValue <= p.PointValue)
            {
                continue;
            }

            if (p.Left != null && p.Left.PointValue <= p.PointValue)
            {
                continue;
            }

            if (p.Right != null && p.Right.PointValue <= p.PointValue)
            {
                continue;
            }

            if (!lowestPoints.Contains(p))
            {
                lowestPoints.Add(p);
            }
        }

        return lowestPoints;
    }

    public List<Basin> GetBasins(List<Point> lowestPoints)
    {
        List<Basin> basins = new List<Basin>();
        foreach (Point lowestPoint in lowestPoints)
        {
            Basin b = new Basin();
            List<Point> pointsInBasin = new List<Point>();
            pointsInBasin.Add(lowestPoint);
            pointsInBasin.AddRange(GetNextLowestPoint(lowestPoint, new List<Point>()));
            b.Points = pointsInBasin;
            basins.Add(b);
        }

        return basins;
    }

    /// <summary>
    /// Recursively look through each point to get the basin
    /// </summary>
    /// <param name="lowestPoints"></param>
    /// <returns></returns>
    public List<Point> GetNextLowestPoint(Point point, List<Point> pointsInBasin)
    {
        point.Visited = true;
        if (point.Down != null && !point.Down.Visited && point.Down.PointValue < 9)
        {
            pointsInBasin.Add(point.Down);
            GetNextLowestPoint(point.Down, pointsInBasin);
        }

        if (point.Up != null && !point.Up.Visited && point.Up.PointValue < 9)
        {
            pointsInBasin.Add(point.Up);
            GetNextLowestPoint(point.Up, pointsInBasin);
        }

        if (point.Right != null && !point.Right.Visited && point.Right.PointValue < 9)
        {
            pointsInBasin.Add(point.Right);
            GetNextLowestPoint(point.Right, pointsInBasin);
        }

        if (point.Left != null && !point.Left.Visited && point.Left.PointValue < 9)
        {
            pointsInBasin.Add(point.Left);
            GetNextLowestPoint(point.Left, pointsInBasin);
        }

        return pointsInBasin;
    }

    public override ValueTask<string> Solve_1() => new(Solve1().ToString());

    public override ValueTask<string> Solve_2() => new(Solve2().ToString());

    public class Point
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int PointValue { get; set; }
        public Point Up { get; set; }
        public Point Down { get; set; }
        public Point Right { get; set; }
        public Point Left { get; set; }
        public bool Visited { get; set; }
    }

    public class Basin
    {
        private List<Point> _points = new();
        public int CountForBasin { get; set; }

        public List<Point> Points
        {
            get { return _points; }
            set { _points = value; }
        }
    }
}