using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

public class Day_06 : BaseDay
{
    private readonly string _input;
    private readonly List<int> _fishes;

    public Day_06()
    {
        _input = File.ReadAllText(InputFilePath);
        _fishes = _input.Split(",").Select(x => int.Parse(x)).ToList();
    }

    private long Solve1()
    {
        return CalculateForDays(80);
    }

    private long Solve2()
    {
        return CalculateForDays(256);
    }

    private long CalculateForDays(int days)
    {
        //Move the fish to be countdowns instead. This looks like columns of fish instead
        List<long> countdowns = new List<long> {0, 0, 0, 0, 0, 0, 0, 0, 0};
        foreach (int fish in _fishes)
        {
            //For every existing fish which exists already, setup the countdown array
            countdowns[fish]++;
        }

        //for each day
        for (int i = 0; i < days; i++)
        {
            long numberToCreate = countdowns[0];
            //Remove the first element because we have the required number which have reproduced or are starting their countdown again
            countdowns.RemoveAt(0);
            //Set the number of fish which need to have their count down reset in addition to the number of new fish which have started their countdown (fish which started from 8)
            countdowns[6] += numberToCreate;
            //Because we removed the start of the array earlier, add a new entry onto the end ready for the new fish which are coming
            countdowns.Add(0);
            countdowns[8] = numberToCreate;
        }

        return countdowns.Sum();
    }
    public override ValueTask<string> Solve_1() => new(Solve1().ToString());

    public override ValueTask<string> Solve_2() => new(Solve2().ToString());
}