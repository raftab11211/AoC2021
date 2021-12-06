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

    private int Solve1()
    {
        //For 18 days
        List<int> theGodDamnFish = new List<int>(_fishes);
        for (int i = 0; i < 32; i++)
        {
            int newFishToCreate = 0;
            for (int t = 0; t < theGodDamnFish.Count; t++)
            {
                if (theGodDamnFish[t] == 0)
                {
                    newFishToCreate++;
                    theGodDamnFish[t] = 6;
                }
                else
                {
                    theGodDamnFish[t]--;
                }
            }

            for (int t = 0; t < newFishToCreate; t++)
            {
                theGodDamnFish.Add(8);
            }
        }

        int howmanyFish = theGodDamnFish.Count;
        
        return howmanyFish;
    }

    private long Solve2()
    {
        //Move the fish to be countdowns instead. This looks like columns of fish instead
        List<long> countdowns = new List<long> {0, 0, 0, 0, 0, 0, 0, 0,0};
        foreach (int fish in _fishes)
        {
            //For every existing fish which exists already, setup the countdown array
            countdowns[fish]++;
        }

        //for each day
        for (int i = 0; i < 256; i++)
        {
            //For the number of fish which are on countdown 0 we need to create this many fish.
            //This is also the number of fish which need to be reset to 6
            long numberToCreate = countdowns[0];
            //We now have the number of fish we need to add on, remove the first part of the array
            List<long> shift = new List<long>(countdowns.Skip(1));
            //Add a 0 on the end so that on the next loop we will have a place to put the new fish
            shift.Add(0);
            //For the fish which are having their countdown reset add them to the existing fish who are having their countdown decremented
            //This will be new fish come down from 8
            shift[6] += numberToCreate;
            //Every time a fish reaches 0 we need to create this many fishes
            shift[8] = numberToCreate;
            countdowns = shift;
        }

        return countdowns.Sum();
    }

    public override ValueTask<string> Solve_1() => new(Solve1().ToString());

    public override ValueTask<string> Solve_2() => new(Solve2().ToString());
}