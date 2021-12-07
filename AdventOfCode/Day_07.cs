using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

public class Day_07 : BaseDay
{
    private readonly string _input;
    private readonly List<int> _crabs;

    public Day_07()
    {
        _input = File.ReadAllText(InputFilePath);
        _crabs = _input.Split(",").Select(x => int.Parse(x)).ToList();
    }

    private int Solve1()
    {
        int highestNum = _crabs.OrderByDescending(x => x).FirstOrDefault();
        Dictionary<int, int> fuelRecords = new Dictionary<int, int>();
        for (int fuel = 1; fuel < highestNum; fuel++)
        {
            int fuelTaken = 0;
            foreach (int crab in _crabs)
            {
                if (crab < fuel)
                {
                    fuelTaken += fuel - crab;
                }
                else
                {
                    fuelTaken += crab - fuel;
                }
            }

            fuelRecords.Add(fuel, fuelTaken);
        }

        return fuelRecords.OrderBy(x => x.Value).FirstOrDefault().Value;
    }

    private int Solve2()
    {
        int highestNum = _crabs.OrderByDescending(x => x).FirstOrDefault();
        Dictionary<int, int> extraFuel = new Dictionary<int, int>();
        for (int i = 0; i < highestNum; i++)
        {
            extraFuel.Add(i, extraFuel.LastOrDefault().Value + i);
        }

        Dictionary<int, int> fuelRecords = new Dictionary<int, int>();
        for (int fuel = 1; fuel < highestNum; fuel++)
        {
            int fuelTaken = 0;
            foreach (int crab in _crabs)
            {
                int fuelForStep;
                if (crab < fuel)
                {
                    fuelForStep = fuel - crab;
                }
                else
                {
                    fuelForStep = crab - fuel;
                }

                int extraFuelRequired = ExtraFuelRequired(fuelForStep);

                int totalFuelUsed = fuelForStep + extraFuelRequired;
                fuelTaken += totalFuelUsed;
            }

            fuelRecords.Add(fuel, fuelTaken);
        }

        int fuelUsed = fuelRecords.OrderBy(x => x.Value).FirstOrDefault().Value;
        return fuelUsed;
    }

    /// <summary>
    /// Gauss method
    /// </summary>
    /// <param name="fuelForStep"></param>
    /// <returns></returns>
    private int ExtraFuelRequired(int fuelForStep)
    {
        return (fuelForStep * (fuelForStep + 1)) / 2;
    }
    public override ValueTask<string> Solve_1() => new(Solve1().ToString());

    public override ValueTask<string> Solve_2() => new(Solve2().ToString());
}