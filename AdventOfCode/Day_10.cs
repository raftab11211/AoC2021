using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Threading.Tasks;

public class Day_10 : BaseDay
{
    private readonly List<string> _input;
    private readonly string[] openingSymbols;
    private readonly string[] closingSymbols;
    private readonly List<Symbol> _symbols = new List<Symbol>();

    public Day_10()
    {
        _input = File.ReadAllText(InputFilePath).Split("\r\n").ToList();
        _symbols.Add(new Symbol("[", "]", 57));
        _symbols.Add(new Symbol("{", "}", 1197));
        _symbols.Add(new Symbol("(", ")", 3));
        _symbols.Add(new Symbol("<", ">", 25137));

        openingSymbols = _symbols.Select(x => x.OpeningSymbol).ToArray();
        closingSymbols = _symbols.Select(x => x.ClosingSymbol).ToArray();
    }

    private int Solve1()
    {
        List<string> completeLines = new List<string>();
        int score = 0;
        foreach (string line in _input)
        {
            string completeLine = "";
            char[] charsInLine = line.ToCharArray();
            Stack<string> lifoQueue = new Stack<string>();
            for (int i = 0; i < charsInLine.Length; i++)
            {
                string character = charsInLine[i].ToString();

                if (openingSymbols.Contains(character))
                {
                    completeLine += character;
                    lifoQueue.Push(_symbols.FirstOrDefault(x => x.OpeningSymbol == character).ClosingSymbol);
                }
                else if (closingSymbols.Contains(character))
                {
                    bool fullString = false;
                    while (!fullString)
                    {
                        if (lifoQueue.Count == 0)
                        {
                            break;
                        }

                        string expectedCharacter = lifoQueue.Pop();
                        if (character != expectedCharacter)
                        {
                            score += _symbols.FirstOrDefault(x => x.ClosingSymbol == character).Value;
                            completeLine += expectedCharacter;
                            break;
                        }
                        else
                        {
                            completeLine += character;
                            fullString = true;
                        }
                    }
                }

                //End of the line
                if (i == (charsInLine.Length - 1))
                {
                    int initialLifoQueueCount = lifoQueue.Count;
                    //Fill the stack in
                    for (int t = 0; t < initialLifoQueueCount; t++)
                    {
                        completeLine += lifoQueue.Pop();
                    }
                }
            }

            completeLines.Add(completeLine);
        }

        return score;
    }

    private long Solve2()
    {
        List<Line> formattedLines = new List<Line>();
        int score = 0;
        foreach (string rawLine in _input)
        {
            Line line = new Line();
            string completeLine = "";
            char[] charsInLine = rawLine.ToCharArray();
            Stack<string> lifoQueue = new Stack<string>();
            for (int i = 0; i < charsInLine.Length; i++)
            {
                string character = charsInLine[i].ToString();

                if (openingSymbols.Contains(character))
                {
                    completeLine += character;
                    lifoQueue.Push(_symbols.FirstOrDefault(x => x.OpeningSymbol == character).ClosingSymbol);
                }
                else if (closingSymbols.Contains(character))
                {
                    bool fullString = false;
                    while (!fullString)
                    {
                        if (lifoQueue.Count == 0)
                        {
                            break;
                        }

                        string expectedCharacter = lifoQueue.Pop();
                        if (character != expectedCharacter)
                        {
                            score += _symbols.FirstOrDefault(x => x.ClosingSymbol == character).Value;
                            completeLine += expectedCharacter;
                            line.Corrupt = true;
                            break;
                        }
                        else
                        {
                            completeLine += character;
                            fullString = true;
                        }
                    }
                }

                //End of the line
                if (i == (charsInLine.Length - 1))
                {
                    int initialLifoQueueCount = lifoQueue.Count;
                    //Fill the stack in
                    for (int t = 0; t < initialLifoQueueCount; t++)
                    {
                        string characterToFillWith = lifoQueue.Pop();
                        line.Score = line.Score * 5;
                        switch (characterToFillWith)
                        {
                            case ")":
                                line.Score += 1;
                                break;
                            case "]":
                                line.Score += 2;
                                break;
                            case "}":
                                line.Score += 3;
                                break;
                            case ">":
                                line.Score += 4;
                                break;
                        }

                        completeLine += characterToFillWith;
                        
                    }
                }

                line.CompleteLine = completeLine;
            }

            formattedLines.Add(line);                

            
        }

        var sortedLines = formattedLines.Where(x  => !x.Corrupt).OrderBy(x => x.Score).ToList();
        
        return sortedLines[sortedLines.Count /2].Score;
    }

    public class Symbol
    {
        public Symbol(string openingSymbol, string closingSymbol, int value)
        {
            OpeningSymbol = openingSymbol;
            ClosingSymbol = closingSymbol;
            Value = value;
        }

        public string OpeningSymbol { get; set; }
        public string ClosingSymbol { get; set; }

        public int Value { get; set; }
    }

    public class Line
    {
        public bool Corrupt { get; set; }
        public string CompleteLine { get; set; }
        public long Score { get; set; }
    }

    public override ValueTask<string> Solve_1() => new(Solve1().ToString());

    public override ValueTask<string> Solve_2() => new(Solve2().ToString());
}