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
            Console.Write("Number of votes: ");
            int numVotes = 0;
            do
            {
                string line = Console.ReadLine();
                if (int.TryParse(line, out numVotes))
                {
                    break;
                }
            } while (true);
            Console.Write("Color: ");
            Color target = new Color();
            string[] colorValues = null;
            do
            {
                string line = Console.ReadLine();
                colorValues = line.Split();
                if (colorValues.Length == 3
                    && int.TryParse(colorValues[0], out target.Red)
                    && int.TryParse(colorValues[1], out target.Green)
                    && int.TryParse(colorValues[2], out target.Blue)
                    && InRange(target.Red)
                    && InRange(target.Green)
                    && InRange(target.Blue)
                    )
                {
                    break;
                }
            } while (true);
            Console.WriteLine("Target color: " + target.Red + " " + target.Green + " " + target.Blue);

            NamedColor[] colorSeq = new NamedColor[numVotes];
            Color total = new Color();
            for (int i = 0; i < colorSeq.Length; i++)
            {
                double bestDiff = 0.0;
                for (int k = 0; k < colors.Length; k++)
                {
                    total.Gray(0);
                    for (int j = 0; j < i; j++)
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
            total.Gray(0);
            Console.WriteLine("Sequence:");
            int count = 0;
            for (int i = 0; i < colorSeq.Length; i++)
            {
                if (colorSeq[i] == final)
                {
                    Console.WriteLine("final");
                    break;
                }
                total.Add(colorSeq[i]);
                count++;
                Console.WriteLine(colorSeq[i].Name); // + "     --> " + total.Red / (i+1) + " " + total.Green / (i + 1) + " " + total.Blue / (i + 1));
            }
            total.Divide(count);
            Console.WriteLine("Resulting color: " + total.Red + " " + total.Green + " " + total.Blue);
            Console.ReadLine();
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
