namespace Assignment4
{
    class GeneticAlgorithm : OptimisationAlgorithm
    {
        public GeneticAlgorithm(double[,] z, double[] constraints, int populationSize, int generations, int tournamentSize, double mutationRate, double mutationMagnitude, double crossoverRate)
        {
            this.z = z;
            this.constraints = constraints;
            this.I = populationSize;
            this.N = z.GetLength(0);
            this.generations = generations;
            this.tournamentSize = tournamentSize;
            this.mutationRate = mutationRate;
            this.mutationMagnitude = mutationMagnitude;
            this.crossoverRate = crossoverRate;
            this.fitness = new double[I];
            this.p = new double[I, N];
            fittestIndividual = new double[N];
            this.OptimisationAlgorithmName = "GeneticAlgorithm";
            GenerateInitialPopulation();
            TrainPopulation();
        }

        public override void TrainPopulation()
        {
            Console.WriteLine("Training Population using Genetic Algorithm Optimisation...");
            double prev_best_fitness = EvaluateFitness();
            double best_fitness = double.MaxValue;
            double[] bestFitnessHistory = new double[generations];
            double[] averageFitnessHistory = new double[generations];
            for (generation = 0; generation < generations; generation++)
            {
                double cur_fitness = EvaluateFitness();
                if (cur_fitness < best_fitness)
                {
                    best_fitness = cur_fitness;
                    fittestIndividual = GetBestIndividual();
                }
                bestFitnessHistory[generation] = best_fitness;
                prev_best_fitness = cur_fitness;

                Reproduce();
                double average = fitness.Average();
                averageFitnessHistory[generation] = average;
                if (generation % 10000 == 0) Console.WriteLine($"Generation {generation}: Best Fitness = {best_fitness:N4} | Average Fitness = {average:N4}");
            }
            Console.WriteLine($"Generation {generation}: Best Fitness = {best_fitness:N4} | Average Fitness = {fitness.Average():N4}");
            SaveIndividual(fittestIndividual);
            SaveFitness(bestFitnessHistory, "Best_2");
            SaveFitness(averageFitnessHistory, "Average_2");
        }

        private void Reproduce()
        {
            double[,] newPopulation = new double[I, N];

            for (int i = 0; i < I; i += 2)
            {
                int parent1Index;
                int parent2Index;
                do
                {
                    parent1Index = SelectParent();
                    parent2Index = SelectParent();
                } while (parent1Index == parent2Index);

                double[] parent1 = GetIndividual(parent1Index);
                double[] parent2 = GetIndividual(parent2Index);

                double[] child1, child2;
                Crossover(parent1, parent2, out child1, out child2);

                Mutate(child1);
                Mutate(child2);

                for (int j = 0; j < N; j++)
                {
                    newPopulation[i, j] = child1[j];
                    if (i + 1 < I)
                    {
                        newPopulation[i + 1, j] = child2[j];
                    }
                }
            }
            p = newPopulation;
        }

        private int SelectParent()
        {
            int bestIndex = random.Next(I);
            for (int i = 0; i < tournamentSize; i++)
            {
                int newIndex = random.Next(I);
                if (fitness[newIndex] < fitness[bestIndex])
                {
                    bestIndex = newIndex;
                }
            }

            return bestIndex;
        }

        private void Crossover(double[] parent1, double[] parent2, out double[] child1, out double[] child2)
        {
            child1 = new double[N];
            child2 = new double[N];
            for (int i = 0; i < N; i++)
            {
                if (random.NextDouble() < crossoverRate)
                {
                    child1[i] = parent1[i];
                    child2[i] = parent2[i];
                }
                else
                {
                    child1[i] = parent2[i];
                    child2[i] = parent1[i];
                }
            }
        }
        private void Mutate(double[] individual)
        {
            for (int i = 0; i < individual.Length; i++)
            {
                if (random.NextDouble() < mutationRate)
                {
                    individual[i] += GenerateGaussianNoise(0, mutationMagnitude);
                    if (individual[i] < 0) individual[i] = 0;
                }
            }
        }

        private double GenerateGaussianNoise(double mean, double standardDeviation)
        {
            double u1 = 1.0 - random.NextDouble();
            double u2 = 1.0 - random.NextDouble();
            double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
            return mean + standardDeviation * randStdNormal;
        }
    }
}
