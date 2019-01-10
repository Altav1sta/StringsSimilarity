using System;
using StringsSimilarityAlgorithms;

namespace CommonPurposeConsoleApp
{
    internal class Program
    {
        internal enum AlgorithmType
        {
            Jaro = 1,
            JaroWinkler = 2
        }

        private static void Main(string[] args)
        {
            while (true)
            {
                try
                {
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.Write("Choose the algorithm ({0:D} - for {0}, {1} - for {1:D}): ", AlgorithmType.Jaro, AlgorithmType.JaroWinkler);

                    var input = Console.ReadLine();

                    if (!Enum.TryParse<AlgorithmType>(input?.Trim(), out var algType) || !Enum.IsDefined(typeof(AlgorithmType), algType))
                    {
                        throw new Exception("You entered incorrect option!");
                    }

                    var algorithm = algType == AlgorithmType.Jaro ? (ISimilarityAlgorithm) new JaroAlgorithm() : new JaroWinklerAlgorithm();

                    if (algType == AlgorithmType.JaroWinkler)
                    {
                        CheckIfJaroWinklerParametersAreToBeSet(algorithm as JaroWinklerAlgorithm);
                    }

                    AskForCaseIgnorance(out var ignoreCase);
                    PerformComparison(algorithm, algType, ignoreCase);

                    Console.Write("Press ENTER to restart or any other key to exit the program...");

                    if (Console.ReadKey().Key != ConsoleKey.Enter) break;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Something went wrong!");
                    Console.WriteLine($"Exception text: {e.Message}");
                    Console.WriteLine("Press ENTER for try again or any other key for exit the program");
                    
                    if (Console.ReadKey().Key != ConsoleKey.Enter) break;
                }
            }
        }

        private static void CheckIfJaroWinklerParametersAreToBeSet(JaroWinklerAlgorithm algorithm)
        {
            while (true)
            {
                Console.Write("Do you want to customize values for Jaro-Winkler algorithm parameters? (y/n): ");

                var input = Console.ReadLine()?.Trim() ?? string.Empty;

                if (input.Equals("y", StringComparison.OrdinalIgnoreCase))
                {
                    SetJaroWinklerParameters(algorithm);
                    break;
                }

                if (input.Equals("n", StringComparison.OrdinalIgnoreCase))
                {
                    break;
                }

                Console.WriteLine("You entered incorrect option! Please, try again.");
            }
        }

        private static void SetJaroWinklerParameters(JaroWinklerAlgorithm algorithm)
        {
            while (true)
            {
                Console.Write("Scaling Factor (from 0 to 1, default = 0.1): ");

                var input = Console.ReadLine();

                if (double.TryParse(input?.Trim(), out var sf) && sf >= 0 && sf <= 1)
                {
                    algorithm.ScalingFactor = sf;
                    break;
                }

                Console.WriteLine("You entered incorrect data. Please, try again.");
            }

            while (true)
            {
                Console.Write("Threshold for scaling appliance (from 0 to 1, default = 0.7): ");

                var input = Console.ReadLine();

                if (double.TryParse(input?.Trim(), out var t) && t >= 0 && t <= 1)
                {
                    algorithm.Threshold = t;
                    break;
                }

                Console.WriteLine("You entered incorrect data. Please, try again.");
            }
        }

        private static void PerformComparison(ISimilarityAlgorithm algorithm, AlgorithmType type, bool ignoreCase)
        {
            while (true)
            {
                Console.WriteLine();
                GetStringsForComparison(out var s1, out var s2, ignoreCase);

                Console.WriteLine("{0} similarity = {1}", type, algorithm.GetSimilarity(s1, s2));
                Console.WriteLine("Press ENTER to try another comparison with same settings or any other key to proceed...");

                if (Console.ReadKey().Key != ConsoleKey.Enter) break;
            }
        }

        private static void AskForCaseIgnorance(out bool ignoreCase)
        {
            while (true)
            {
                Console.Write("Apply case ignorance? (y/n): ");

                var input = Console.ReadLine()?.Trim() ?? string.Empty;

                if (input.Equals("y", StringComparison.OrdinalIgnoreCase))
                {
                    ignoreCase = true;
                    break;
                }

                if (input.Equals("n", StringComparison.OrdinalIgnoreCase))
                {
                    ignoreCase = false;
                    break;
                }

                Console.WriteLine("You entered incorrect option! Please, try again.");
            }
        }

        private static void GetStringsForComparison(out string s1, out string s2, bool ignoreCase)
        { 
            Console.Write("First string to compare: ");
            s1 = Console.ReadLine()?.Trim();
            s1 = ignoreCase ? s1?.ToLower() : s1;

            Console.Write("Second string to compare: ");
            s2 = Console.ReadLine()?.Trim();
            s2 = ignoreCase ? s2?.ToLower() : s2;
        }
    }
}
