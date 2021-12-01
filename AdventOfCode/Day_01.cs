using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

public class Day_01 : BaseDay
{
    private readonly string _input;
    private readonly int[] _numbers;
    public Day_01()
    {
        _input = File.ReadAllText(InputFilePath);
        _numbers = _input.Split("\r\n").Select(t => int.Parse(t)).ToArray();
    }

    private int Solve1()
    {
        int increasedCount = 0;
        for (int i = 0; i < _numbers.Length;i++)
        {
            if (i > 0)
            {
                if (_numbers[i] > _numbers[i - 1])
                {
                    increasedCount++;
                }
            }
        }
        return increasedCount;
    }

    private int Solve2()
    {
        int increasedCount = 0;
        int previousSum = 0;
        for (int i = 0; i < _numbers.Length; i++)
        {
            if (i == 0)
            {
                continue;
            }
            int totalSum = 0;
            totalSum += _numbers[i];
            if (_numbers.Length >= i + 2)
            {
                totalSum += _numbers[i + 1];    
            }

            if (_numbers.Length >= i + 3)
            {
                totalSum += _numbers[i + 2];                
            }
            
            if (totalSum > previousSum)
            {
                increasedCount++;
            }

            previousSum = totalSum;
        }

        return increasedCount;
    }
    public override ValueTask<string> Solve_1() => new(Solve1().ToString());

    public override ValueTask<string> Solve_2() => new(Solve2().ToString());
}
