
using System;
using System.IO;
using System.Threading.Tasks;

public class Day_26 : BaseDay
{
    private readonly string _input;

    public Day_26()
    {
        _input = File.ReadAllText(InputFilePath);
    }

    public override ValueTask<string> Solve_1() => new(_input.Length.ToString());

    public override ValueTask<string> Solve_2() => new(_input.Length.ToString());
}
