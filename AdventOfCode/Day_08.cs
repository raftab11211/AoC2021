using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

public class Day_08 : BaseDay
{
    private readonly List<string> _input;
    private readonly List<Signal> _signals;

    public Day_08()
    {
        _input = File.ReadAllText(InputFilePath).Split("\r\n").ToList();
        _signals = new();
        foreach (string line in _input)
        {
            string inputSignalsRaw = line.Split("|")[0];
            string outputSignalsRaw = line.Split("|")[1];
            Signal signal = new Signal();
            foreach (string inputSignal in inputSignalsRaw.Split(" ", StringSplitOptions.RemoveEmptyEntries))
            {
                SignalItem s = new SignalItem();
                s.SignalText = inputSignal;
                signal.InputSignals.Add(s);
            }

            foreach (string outputSignal in outputSignalsRaw.Split(" ", StringSplitOptions.RemoveEmptyEntries))
            {
                SignalItem s = new SignalItem();
                s.SignalText = outputSignal;
                signal.OutputSignals.Add(s);
            }

            _signals.Add(signal);
        }
    }

    private int Solve1()
    {
        int uniqueOutputVals = 0;
        foreach (Signal signal in _signals)
        {
            foreach (SignalItem outputSignal in signal.OutputSignals)
            {
                if (outputSignal.IsUnique)
                {
                    uniqueOutputVals++;
                }
            }
        }

        return uniqueOutputVals;
    }

    private int Solve2()
    {
        int totalSum = 0;
        foreach (Signal signal in _signals)
        {
            Dictionary<int, char[]> patterns = signal.CalculatePattern();
            string outputValue = "";
            foreach (SignalItem signalItem in signal.OutputSignals)
            {
                foreach (KeyValuePair<int, char[]> pattern in patterns)
                {
                    if (pattern.Value.Length == signalItem.SignalText.Length)
                    {
                        if (signalItem.SignalText.ToCharArray().All(pattern.Value.Contains))
                        {
                            outputValue += pattern.Key.ToString();
                        }
                    }
                }
            }

            totalSum += int.Parse(outputValue);
        }

        return totalSum;
    }

    public class Signal
    {
        private List<SignalItem> _outputSignals = new();
        private List<SignalItem> _inputSignals = new();

        public List<SignalItem> OutputSignals
        {
            get { return _outputSignals; }
            set { _outputSignals = value; }
        }

        public List<SignalItem> InputSignals
        {
            get { return _inputSignals; }
            set { _inputSignals = value; }
        }

        /// <summary>
        /// Shamelessly stolen from here https://i.redd.it/343qtqqpnc481.jpg
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, char[]> CalculatePattern()
        {
            Dictionary<int, char[]> patterns = new Dictionary<int, char[]>();

            string oneInput = _inputSignals.FirstOrDefault(x => x.SignalText.Length == 2).SignalText;
            patterns.Add(1, oneInput.ToCharArray());
            string fourInput = _inputSignals.FirstOrDefault(x => x.SignalText.Length == 4).SignalText;
            patterns.Add(4, fourInput.ToCharArray());
            string sevenInput = _inputSignals.FirstOrDefault(x => x.SignalText.Length == 3).SignalText;
            patterns.Add(7, sevenInput.ToCharArray());
            string eightInput = _inputSignals.FirstOrDefault(x => x.SignalText.Length == 7).SignalText;
            patterns.Add(8, eightInput.ToCharArray());

            //These are the unique lines used to differentiate between the values (they exist between the 5th and 6th segment counts)
            string fourUnique = "";
            foreach (char c in fourInput.ToCharArray())
            {
                if (!oneInput.Contains(c))
                {
                    fourUnique += c;
                }
            }

            IEnumerable<SignalItem> unknownInputSignals = _inputSignals.Where(x => !x.IsUnique);

            //Use a queue because we don't want to loop over an entire collection every time
            Queue<SignalItem> inputs = new Queue<SignalItem>(unknownInputSignals);
            for (int i = 0; i < 6; i++)
            {
                SignalItem signal = inputs.Dequeue();
                if (!signal.IsUnique)
                {
                    if (signal.SignalText.Length == 5)
                    {
                        if (oneInput.ToCharArray().All(signal.SignalText.ToCharArray().Contains))
                        {
                            patterns.Add(3, signal.SignalText.ToCharArray());
                        }
                        else if (fourUnique.ToCharArray().All(signal.SignalText.ToCharArray().Contains))
                        {
                            patterns.Add(5, signal.SignalText.ToCharArray());
                        }
                        else
                        {
                            patterns.Add(2, signal.SignalText.ToCharArray());
                        }
                    }
                    else if (signal.SignalText.Length == 6)
                    {
                        if (fourInput.ToCharArray().All(signal.SignalText.ToCharArray().Contains))
                        {
                            patterns.Add(9, signal.SignalText.ToCharArray());
                        }
                        else if (fourUnique.ToCharArray().All(signal.SignalText.ToCharArray().Contains))
                        {
                            patterns.Add(6, signal.SignalText.ToCharArray());
                        }
                        else
                        {
                            patterns.Add(0, signal.SignalText.ToCharArray());
                        }
                    }
                }
            }

            return patterns;
        }
    }

    public class SignalItem
    {
        public string SignalText { get; set; }

        public bool IsUnique
        {
            get
            {
                if (SignalText.Length == 2 || SignalText.Length == 4 || SignalText.Length == 3 ||
                    SignalText.Length == 7)
                {
                    return true;
                }

                return false;
            }
        }
    }

    public override ValueTask<string> Solve_1() => new(Solve1().ToString());

    public override ValueTask<string> Solve_2() => new(Solve2().ToString());
}