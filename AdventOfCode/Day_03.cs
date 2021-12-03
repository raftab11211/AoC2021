using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

public class Day_03 : BaseDay
{
    private readonly string _input;
    private readonly List<string> _bits;

    public Day_03()
    {
        _input = File.ReadAllText(InputFilePath);
        _bits = _input.Split("\r\n").ToList();
    }

    private int Solve1()
    {
        Tuple<string, string> commonValues = GetCommonListsAsBinary(_bits);
        return Convert.ToInt32(commonValues.Item1, 2) * Convert.ToInt32(commonValues.Item2, 2);
    }

    private int Solve2()
    {
        List<string> oxygenNumbers = _bits;
        List<string> co2Numbers = _bits;

        int oxyIndex = 0;
        while (oxygenNumbers.Count != 1)
        {
            int oneSum = 0;
            int zeroSum = 0;
            foreach (string oxyNumber in oxygenNumbers)
            {
                if (oxyNumber[oxyIndex].ToString() == "1")
                {
                    oneSum++;
                }
                else
                {
                    zeroSum++;
                }
            }

            if (oneSum >= zeroSum)
            {
                oxygenNumbers = oxygenNumbers.Where(x => x[oxyIndex].ToString().Equals("1")).ToList();
            }
            else
            {
                oxygenNumbers = oxygenNumbers.Where(x => x[oxyIndex].ToString().Equals("0")).ToList();
            }

            oxyIndex++;
        }
        
        int co2Index = 0;
        while (co2Numbers.Count != 1)
        {
            int oneSum = 0;
            int zeroSum = 0;
            foreach (string co2Number in co2Numbers)
            {
                if (co2Number[co2Index].ToString() == "1")
                {
                    oneSum++;
                }
                else
                {
                    zeroSum++;
                }
            }

            if (oneSum >= zeroSum)
            {
                co2Numbers = co2Numbers.Where(x => x[co2Index].ToString().Equals("0")).ToList();
            }
            else
            {
                co2Numbers = co2Numbers.Where(x => x[co2Index].ToString().Equals("1")).ToList();
            }

            co2Index++;
        }
        
        int o2Gen = Convert.ToInt32(oxygenNumbers.FirstOrDefault(), 2);
        int co2Scrubber = Convert.ToInt32(co2Numbers.FirstOrDefault(), 2);
        
        return o2Gen * co2Scrubber;
    }
    
    private Tuple<string, string> GetCommonListsAsBinary(List<string> bits)
    {
        int col1 = 0, col2 = 0, col3 = 0, col4 = 0, col5 = 0, col6 = 0, col7 = 0, col8 = 0, col9 = 0, col10 = 0, col11 = 0, col12 = 0;
        for (int i = 0; i < bits.Count; i++)
        {
            col1 += int.Parse(bits[i].ToCharArray()[0].ToString());
            col2 += int.Parse(bits[i].ToCharArray()[1].ToString());
            col3 += int.Parse(bits[i].ToCharArray()[2].ToString());
            col4 += int.Parse(bits[i].ToCharArray()[3].ToString());
            col5 += int.Parse(bits[i].ToCharArray()[4].ToString());
            col6 += int.Parse(bits[i].ToCharArray()[5].ToString());
            col7 += int.Parse(bits[i].ToCharArray()[6].ToString());
            col8 += int.Parse(bits[i].ToCharArray()[7].ToString());
            col9 += int.Parse(bits[i].ToCharArray()[8].ToString());
            col10 += int.Parse(bits[i].ToCharArray()[9].ToString());
            col11 += int.Parse(bits[i].ToCharArray()[10].ToString());
            col12 += int.Parse(bits[i].ToCharArray()[11].ToString());
        }
        
        
        string mostCommon = string.Empty;
        mostCommon += col1 >= ((bits.Count-1) / 2) ? "1" : "0";
        mostCommon += col2 >= ((bits.Count-1) / 2) ? "1" : "0";
        mostCommon += col3 >= ((bits.Count-1) / 2) ? "1" : "0";
        mostCommon += col4 >= ((bits.Count-1) / 2) ? "1" : "0";
        mostCommon += col5 >= ((bits.Count-1) / 2) ? "1" : "0";
        mostCommon += col6 >= (bits.Count / 2) ? "1" : "0";
        mostCommon += col7 >= (bits.Count / 2) ? "1" : "0";
        mostCommon += col8 >= (bits.Count / 2) ? "1" : "0";
        mostCommon += col9 >= (bits.Count / 2) ? "1" : "0";
        mostCommon += col10 >= (bits.Count / 2) ? "1" : "0";
        mostCommon += col11 >= (bits.Count / 2) ? "1" : "0";
        mostCommon += col12 >= (bits.Count / 2) ? "1" : "0";

        string leastCommon = string.Empty;
        leastCommon += col1 >= (bits.Count / 2) ? "0" : "1";
        leastCommon += col2 >= (bits.Count / 2) ? "0" : "1";
        leastCommon += col3 >= (bits.Count / 2) ? "0" : "1";
        leastCommon += col4 >= (bits.Count / 2) ? "0" : "1";
        leastCommon += col5 >= (bits.Count / 2) ? "0" : "1";
        leastCommon += col6 >= (bits.Count / 2) ? "0" : "1";
        leastCommon += col7 >= (bits.Count / 2) ? "0" : "1";
        leastCommon += col8 >= (bits.Count / 2) ? "0" : "1";
        leastCommon += col9 >= (bits.Count / 2) ? "0" : "1";
        leastCommon += col10 >= (bits.Count / 2) ? "0" : "1";
        leastCommon += col11 >= (bits.Count / 2) ? "0" : "1";
        leastCommon += col12 >= (bits.Count / 2) ? "0" : "1";

        return new Tuple<string, string>(mostCommon, leastCommon);
    }

    public override ValueTask<string> Solve_1() => new(Solve1().ToString());

    public override ValueTask<string> Solve_2() => new(Solve2().ToString());
}