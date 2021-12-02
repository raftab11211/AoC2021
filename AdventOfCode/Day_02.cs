using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

public class Day_02 : BaseDay
{
    private readonly string _input;
    private readonly List<string> _positions;
    public Day_02()
    {
        _input = File.ReadAllText(InputFilePath);
        _positions = _input.Split("\r\n").ToList();
    }

    private int Solve1()
    {
        int horizontal = 0;
        int depth = 0;
        foreach (string position in _positions)
        {
            string direction = position.Split(" ")[0];
            int count = int.Parse(position.Split(" ")[1]);
            switch (direction)
            {
                case "forward":
                    horizontal += count;
                    break;
                case "up":
                    depth -= count;
                    break;
                case "down":
                    depth += count;
                    break;
                case "backward":
                    horizontal -= count;
                    break;
            }
        }

        return horizontal * depth;
    }

    private int Solve2()
    {
        int horizontal = 0;
        int depth = 0;
        int aim = 0;
        foreach (string position in _positions)
        {
            string direction = position.Split(" ")[0];
            int count = int.Parse(position.Split(" ")[1]);
            switch (direction)
            {
                case "forward":
                    horizontal += count;
                    depth += (aim * count);
                    break;
                case "up":
                    aim -= count;
                    break;
                case "down":
                    aim += count;
                    break;
                case "backward":
                    horizontal -= count;
                    break;
            }
        }

        return horizontal * depth;
    }
    public override ValueTask<string> Solve_1() => new(Solve1().ToString());

    public override ValueTask<string> Solve_2() => new(Solve2().ToString());
}