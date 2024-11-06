namespace Assignment4
{
    abstract class OptimisationAlgorithm
    {
        protected double[,] z;
        protected double[,] p;

        protected double[] constraints;
        protected double[] fitness;
        protected double[] fittestIndividual;

        protected double mutationRate;
        protected double crossoverRate;
        protected double mutationMagnitude;

        protected int I;
        protected int N;
        protected int generations;
        protected int tournamentSize;
        protected int generation;

        protected string OptimisationAlgorithmName;

        protected Random random = new Random();

        abstract public void TrainPopulation();

        protected void GenerateInitialPopulation()
        {
            for (int i = 0; i < I; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    p[i, j] = random.NextDouble();
                }
            }
        }

        public double Predict(double[] bestIndividual, int i)
        {
            return bestIndividual[i] * z[i, 0];
        }
        protected double[] GetBestIndividual()
        {
            return GetIndividual(Array.IndexOf(fitness, fitness.Min()));
        }
        public double[] GetFittestIndividual()
        {
            return fittestIndividual;
        }

        protected double[] GetIndividual(int index)
        {
            double[] individual = new double[N];
            for (int j = 0; j < N; j++)
            {
                individual[j] = p[index, j];
            }
            return individual;
        }
        protected double EvaluateFitness()
        {
            for (int i = 0; i < I; i++)
            {
                fitness[i] = EvaluateIndividualFitness(GetIndividual(i));
            }
            return fitness.Min();
        }
        protected double EvaluateIndividualFitness(double[] individual)
        {
            double totalCost = 0;
            double[] totalNutrients = new double[constraints.GetLength(0)];
            for (int i = 0; i < individual.Length; i++)
            {
                for (int j = 0; j < totalNutrients.Length; j++)
                {
                    totalNutrients[j] += individual[i] * z[i, j + 1];
                }
                totalCost += individual[i];
            }

            double fitness = totalCost;
            for (int i = 0; i < totalNutrients.Length; i++)
            {
                if (totalNutrients[i] < constraints[i])
                {
                    fitness += 100 * (constraints[i] - totalNutrients[i]);
                }
            }
            return fitness;
        }

        public void SaveIndividual(double[] fittestIndividual)
        {
            string fileName = $"{OptimisationAlgorithmName}_Fittest.csv";

            using (StreamWriter writer = new StreamWriter(fileName))
            {
                for (int i = 0; i < fittestIndividual.Length; i++)
                {
                    writer.WriteLine($"{fittestIndividual[i] * z[i, 0]}");
                }
            }
            Console.WriteLine($"Fittest individual saved to {fileName}");
        }

        public void SaveFitness(double[] fitness, string type)
        {
            string fileName = $"{OptimisationAlgorithmName}_{type}.csv";

            using (StreamWriter writer = new StreamWriter(fileName))
            {
                for (int i = 0; i < fitness.Length; i++)
                {
                    writer.WriteLine($"{fitness[i]}");
                }
            }
            Console.WriteLine($"{type} fitness history saved to {fileName}");
        }
    }
}