using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Color_Mixer
{
    class Color
    {
        public int Red, Green, Blue;

        internal void Add(Color other)
        {
            Red += other.Red;
            Green += other.Green;
            Blue += other.Blue;
        }

        internal void Multiply(int num)
        {
            Red *= num;
            Green *= num;
            Blue *= num;
        }

        internal void Divide(int num)
        {
            Red /= num;
            Green /= num;
            Blue /= num;
        }
        internal void Gray(int gray)
        {
            Red = Green = Blue = gray;
        }
    }

    class NamedColor : Color
    {
        public readonly string Name;

        public NamedColor(string name, int red, int green, int blue)
        {
            Name = name;
            Red = red;
            Green = green;
            Blue = blue;
        }
    }

    class Program
    {
        static NamedColor[] colors =
        {
            new NamedColor("black", 0, 0, 0),
            new NamedColor("red", 255, 0, 0),
            new NamedColor("green", 0, 255, 0),
            new NamedColor("blue", 0, 0, 255),
            new NamedColor("yellow", 255, 255, 0),
            new NamedColor("cyan", 0, 255, 255),
            new NamedColor("magenta", 255, 0, 255),
            new NamedColor("white", 255, 255, 255),
        };
        static NamedColor final = new NamedColor("final", -1, -1, -1);

        static void Main(string[] args)
        {
            do
            {
                int numVotes = ReadNonNegativeInt("Votes needed (0 to quit)");
                if (numVotes == 0) break;

                int votesSoFar = ReadNonNegativeInt("Votes so far");

                Color target = ReadColor("Color (red green blue)");

                Color current;
                if (votesSoFar > 0)
                {
                    current = ReadColor("Current color (red green blue)");
                    current.Multiply(votesSoFar);
                }
                else
                    current = new Color();

                NamedColor[] colorSeq = new NamedColor[numVotes];
                Color total = new Color();
                for (int i = votesSoFar; i < colorSeq.Length; i++)
                {
                    double bestDiff = 0.0;
                    for (int k = 0; k < colors.Length; k++)
                    {
                        total.Gray(0);
                        total.Add(current);
                        for (int j = votesSoFar; j < i; j++)
                        {
                            total.Add(colorSeq[j]);
                        }
                        total.Add(colors[k]);
                        total.Divide(i + 1);
                        double diff = Diff(total, target);
                        if (colorSeq[i] == null || diff < bestDiff)
                        {
                            colorSeq[i] = colors[k];
                            bestDiff = diff;
                        }
                        if (diff < 10 && i < colors.Length - 1)
                        {
                            colorSeq[i + 1] = final;
                        }
                    }
                }

                Dictionary<string, int> counts = new Dictionary<string, int>();
                total.Gray(0);
                total.Add(current);
                Console.WriteLine();
                Console.WriteLine("Sequence:");
                int count = votesSoFar;
                bool hasFinal = false;
                for (int i = votesSoFar; i < colorSeq.Length; i++)
                {
                    if (colorSeq[i] == final)
                    {
                        Console.WriteLine("  final");
                        hasFinal = true;
                        break;
                    }
                    total.Add(colorSeq[i]);
                    Inc(colorSeq[i].Name, counts);
                    count++;
                    Console.WriteLine("  " + colorSeq[i].Name); // + "     --> " + total.Red / (i+1) + " " + total.Green / (i + 1) + " " + total.Blue / (i + 1));
                }
                total.Divide(count);
                Console.WriteLine("Resulting color: " + total.Red + " " + total.Green + " " + total.Blue);
                Console.WriteLine("Target color   : " + target.Red + " " + target.Green + " " + target.Blue);
                Console.WriteLine("Difference     : " + (target.Red - total.Red) + " " + (target.Green - total.Green) + " " + (target.Blue - total.Blue));
                Console.WriteLine("Difference     : " + Diff(total, target));
                Console.WriteLine("Summary:");
                foreach (string key in counts.Keys)
                    Console.WriteLine("  " + counts[key] + "x " + key);
                if (hasFinal)
                    Console.WriteLine("  final");
                Console.WriteLine();
            } while (true);
        }

        private static void Inc(string name, Dictionary<string, int> counts)
        {
            if (counts.ContainsKey(name))
                counts[name]++;
            else
                counts[name] = 1;
        }

        private static void Prompt(string prompt)
        {
            if (prompt != null && prompt.Trim().Length > 0)
                Console.Write(prompt + ": ");
        }

        private static Color ReadColor(string prompt)
        {
            Prompt(prompt);
            return GetColor();
        }

        private static Color GetColor()
        {
            Color color = new Color();
            string[] colorValues = null;
            do
            {
                string line = Console.ReadLine();
                colorValues = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (colorValues.Length == 3
                    && int.TryParse(colorValues[0], out color.Red)
                    && int.TryParse(colorValues[1], out color.Green)
                    && int.TryParse(colorValues[2], out color.Blue)
                    && InRange(color.Red)
                    && InRange(color.Green)
                    && InRange(color.Blue)
                    )
                {
                    break;
                }
            } while (true);
            return color;
        }

        private static int ReadNonNegativeInt(string prompt)
        {
            Prompt(prompt);
            return GetNonNegativeInt();
        }

        private static int GetNonNegativeInt()
        {
            int num = 0;
            do
            {
                string line = Console.ReadLine();
                if (int.TryParse(line, out num) && num >= 0)
                {
                    break;
                }
            } while (true);
            return num;
        }

        private static double Diff(Color c1, Color c2)
        {
            return Math.Sqrt(Sq(c1.Red - c2.Red) + Sq(c1.Green - c2.Green) + Sq(c1.Blue - c2.Blue));
        }

        private static int Sq(int num)
        {
            return num * num;
        }

        private static bool InRange(int c)
        {
            return c >= 0 && c <= 255;
        }
    }
}
