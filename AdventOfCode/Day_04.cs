using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

public class Day_04 : BaseDay
{
    private readonly string _input;
    private readonly List<string> _boardInput;
    private readonly List<Board> _boards = new();
    private readonly List<int> _bingoNumbers = new();

    public Day_04()
    {
        _input = File.ReadAllText(InputFilePath);
        _boardInput = _input.Split("\r\n").ToList();
        foreach (string bingoNumber in _boardInput[0].Split(","))
        {
            _bingoNumbers.Add(int.Parse(bingoNumber));
        }
        //Remove the bingo number lines and the extra space after
        _boardInput.RemoveRange(0,2);

        //For every board
        for (int i = 0; i < _boardInput.Count; i+=6)
        {
            Board b = new Board();
            Line col1 = new Line();
            col1.IsHorizontal = false;
            Line col2 = new Line();
            col2.IsHorizontal = false;
            Line col3 = new Line();
            col3.IsHorizontal = false;
            Line col4 = new Line();
            col4.IsHorizontal = false;
            Line col5 = new Line();
            col5.IsHorizontal = false;
            
            //For every row
            for (int t = 0; t < 5; t++)
            {
                Line l = new Line();
                l.IsHorizontal = true;
                string[] row = _boardInput[i + t].Split(" ", StringSplitOptions.RemoveEmptyEntries);
                
                //Since we're iterating over every row we can also populate the columns
                col1.Numbers.Add(new Number(int.Parse(row[0]),t,0));
                col2.Numbers.Add(new Number(int.Parse(row[1]),t,1));
                col3.Numbers.Add(new Number(int.Parse(row[2]),t,2));
                col4.Numbers.Add(new Number(int.Parse(row[3]),t,3));
                col5.Numbers.Add(new Number(int.Parse(row[4]),t,4));

                //For every column in the row
                for (int j = 0; j < 5; j++)
                {
                    Number number = new Number(int.Parse(row[j]));
                    number.Row = t;
                    number.Column = j;
                    l.Numbers.Add(number);
                }
                b.Lines.Add(l);
            }
            
            b.Lines.Add(col1);
            b.Lines.Add(col2);
            b.Lines.Add(col3);
            b.Lines.Add(col4);
            b.Lines.Add(col5);
            
            _boards.Add(b);
        }
    }

    private int Solve1()
    {
        foreach (int bingoNumber in _bingoNumbers)
        {
            foreach (Board board in _boards)
            {
                board.CheckNumber(bingoNumber);
                if (board.AnyBingo())
                {
                    return board.CalculcateAllUnchecked() * bingoNumber;
                }
            }
        }
        return 0;
    }

    private int Solve2()
    {
        List<Board> boardsLeftToSolve = new List<Board>();
        
        foreach (Board b in _boards)
        {
            boardsLeftToSolve.Add(b);
        }
        
        foreach (int bingoNumber in _bingoNumbers)
        {
            foreach (Board board in _boards)
            {
                //Set a number as checked
                board.CheckNumber(bingoNumber);
                
                //This scans all boards and is bad - it should only scan the boards which are still awaiting to be solved
                if (board.AnyBingo())
                {
                    //We only want to remove boards until we get to 1
                    if (boardsLeftToSolve.Count > 1)
                    {
                        boardsLeftToSolve.Remove(board);
                    }
                }
            }
            if (boardsLeftToSolve.Count == 1)
            {
                if (boardsLeftToSolve[0].AnyBingo())
                {
                    return boardsLeftToSolve[0].CalculcateAllUnchecked() * bingoNumber;
                }
            }
        }

        return 0;
    }

    public override ValueTask<string> Solve_1() => new(Solve1().ToString());

    public override ValueTask<string> Solve_2() => new(Solve2().ToString());
}

public class Board
{
    private List<Line> _lines = new();
    
    public List<Line> Lines
    {
        get { return _lines; }
        set { _lines = value; }
    }

    public int CalculcateAllUnchecked()
    {
        int uncheckedCalc = 0;
        foreach(Line line in _lines)
        {
            foreach (Number number in line.Numbers)
            {
                if (!number.Checked)
                {
                    uncheckedCalc += number.NumberValue;

                    //Where items exist in the same line at the same row and column index, 0 them out, this stops us from counting them twice
                    foreach (Line lineConflict in _lines)
                    {
                        foreach (Number numberConflict in lineConflict.Numbers)
                        {
                            if (numberConflict.Column == number.Column && numberConflict.Row == number.Row)
                            {
                                numberConflict.NumberValue = 0;
                            }
                        }
                    }
                }
            }
        }
        
        return uncheckedCalc;
    }

    public void CheckNumber(int number)
    {
        foreach (Line line in Lines)
        {
            foreach (Number numberCheck in line.Numbers)
            {
                if (numberCheck.NumberValue.Equals(number))
                {
                    numberCheck.Checked = true;
                }
            }
        }
    }

    public bool AnyBingo()
    {
        foreach (Line line in Lines)
        {
            if (line.IsAllChecked())
            {
                return true;
            }
        }
        return false;
    }
}

public class Line
{
    private List<Number> _numbers = new();
    public bool IsHorizontal { get; set; } 
    public List<Number> Numbers
    {
        get { return _numbers; }
        set { _numbers = value; }
    }

    public bool IsAllChecked()
    {
        foreach (Number number in Numbers)
        {
            if (!number.Checked)
            {
                return false;
            }
        }

        return true;
    }
}

public class Number
{
    public Number(int number)
    {
        Checked = false;
        NumberValue = number;
    }

    public Number(int number, int row, int column)
    {
        Checked = false;
        NumberValue = number;
        Row = row;
        Column = column;
    }
    public int NumberValue { get; set; }
    public bool Checked { get; set; }
    public int Row { get; set; }
    public int Column { get; set; }
}