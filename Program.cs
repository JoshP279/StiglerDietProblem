using Assignment4;

namespace Assignment4
{
    public class Program
    {
        private static bool display = false;

        public static void Main(string[] args)
        {
            double[,] z;
            string[] items;

            LoadData("TrainingData.csv", out z, out items);

            double[] requiredNutrition;
            string[] nutrientTypes;
            LoadConstraints("Constraints.csv", out requiredNutrition, out nutrientTypes);

            OptimisationAlgorithm optimisationAlgorithm = LoadAlgorithm(3, z, requiredNutrition);

            double[,] z_v;
            LoadValidationData("ValidationData.csv", out z_v);
            ValidateSolution(z_v, z, requiredNutrition, items, nutrientTypes, optimisationAlgorithm);
        }

        private static OptimisationAlgorithm LoadAlgorithm(int choice, double[,] z, double[] requiredNutrition)
        {
            switch (choice)
            {
                case 1:
                    return new GeneticAlgorithm(
                            z,
                            requiredNutrition,
                            populationSize: 250,
                            generations: 200000,
                            tournamentSize: 25,
                            mutationRate: 0.05,
                            mutationMagnitude: 0.005,
                            crossoverRate: 0.3);
                case 2:
                    return new DifferentialEvolution(
                            z,
                            requiredNutrition,
                            populationSize: 500,
                            generations: 200000,
                            scalingFactor: 1.75,
                            crossoverRate: 0.5);
                case 3:
                    return new ParticleSwarm(
                            z,
                            requiredNutrition,
                            populationSize: 100,
                            generations: 200000,
                            inertia: 1,
                            cognitive_component: 1.3,
                            social_component: 1.5);
                default:
                    return null;
            }
        }

        private static void ValidateSolution(double[,] z_v, double[,] z, double[] constraints, string[] items, string[] nutrientTypes, OptimisationAlgorithm optimisationAlgorithm)
        {
            double totalCost = 0;
            double[] nutrients = optimisationAlgorithm.GetFittestIndividual();

            double[] predictions = new double[z_v.GetLength(0)];
            Console.WriteLine("{0,-20} {1,-20} {2,-20}", "Item", "Predicted Weight", "Cost");

            for (int i = 0; i < z_v.GetLength(0); i++)
            {
                double prediction = optimisationAlgorithm.Predict(nutrients, i);

                predictions[i] = prediction;
                double denominator = denominator = z[i, 0];
                double itemCost = prediction / denominator;
                totalCost += itemCost;

                Console.WriteLine("{0,-20} {1,-20:N4} {2,-20:N4}", items[i], prediction, itemCost);
            }

            Console.WriteLine($"Total Cost: {totalCost:N4}");
            Console.WriteLine();

            for (int i = 0; i < nutrientTypes.Length; i++)
            {
                double totalNutrients = 0;
                for (int j = 0; j < nutrients.Length; j++)
                {
                    double denominator = denominator = z[j, 0];
                    double multiplier = multiplier = z[j, i + 1];

                    totalNutrients += predictions[j] / z[j, 0] * z[j, i + 1];
                }
                Console.WriteLine("--------------------------------------------------");
                if (totalNutrients < constraints[i])
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"{nutrientTypes[i],-20} [FAILED]");
                    Console.WriteLine($"Actual:   {totalNutrients,10:N4}");
                    Console.WriteLine($"Required: {constraints[i],10:N4}");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"{nutrientTypes[i],-20} [PASSED]");
                    Console.WriteLine($"Actual:   {totalNutrients,10:N4}");
                    Console.WriteLine($"Required: {constraints[i],10:N4}");
                }

                Console.ResetColor();
            }
            Console.WriteLine("--------------------------------------------------");
        }


        private static void LoadData(string fileName, out double[,] z, out string[] items)
        {
            Console.WriteLine("Loading Training Data...");

            string[] lines = File.ReadAllLines(fileName);
            int numLines = lines.Length - 1;
            int numInputs = lines[0].Split(",").Length - 1;
            items = new string[numLines];
            z = new double[numLines, numInputs];

            for (int i = 0; i < numLines; i++)
            {
                string[] val = lines[i + 1].Split(",");

                items[i] = val[0];

                for (int j = 1; j < numInputs + 1; j++)
                {
                    double value = Convert.ToDouble(val[j]);
                    z[i, j - 1] = value;
                }
            }

            if (display)
            {
                for (int i = 0; i < numLines; i++)
                {
                    for (int j = 0; j < numInputs; j++)
                    {
                        Console.Write("{0,-14:F4}", z[i, j]);
                    }
                    Console.WriteLine();
                }
            }
            Console.WriteLine();
        }

        private static void LoadConstraints(string fileName, out double[] requiredNutrition, out string[] nutrientTypes)
        {
            Console.WriteLine("Loading Constraints...");
            string[] lines = File.ReadAllLines(fileName);
            int numItems = lines.Length - 1;

            nutrientTypes = new string[numItems];
            requiredNutrition = new double[numItems];

            if (display) Console.WriteLine("{0,-17} {1,-14}", "Item", "Required Amounts");
            for (int i = 1; i < lines.Length; i++)
            {
                string[] val = lines[i].Split(",");
                nutrientTypes[i - 1] = val[0];
                double value = Convert.ToDouble(val[1]);
                int index = i - 1;

                requiredNutrition[i - 1] = value;

                if (display) Console.WriteLine("{0,-17} {1,-14:F4}", nutrientTypes[i - 1], requiredNutrition[i - 1]);

            }
            Console.WriteLine();
        }

        private static void LoadValidationData(string fileName, out double[,] z)
        {
            Console.WriteLine("Loading Validation Data...");
            string[] lines = File.ReadAllLines(fileName);
            int numLines = lines.Length;
            int numInputs = lines[0].Split(",").Length - 3;

            z = new double[numLines, numInputs];

            for (int i = 0; i < numLines; i++)
            {
                string[] val = lines[i].Split(",");
                for (int j = 0; j < numInputs; j++)
                {
                    double value = Convert.ToDouble(val[j + 2]);
                    z[i, j] = value;

                    if (display) Console.Write("{0,-14:F4}", z[i, j]);
                }
                if (display) Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}
