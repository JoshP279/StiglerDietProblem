namespace Assignment4
{
    class DifferentialEvolution : OptimisationAlgorithm
    {

        private double F;
        public DifferentialEvolution(double[,] z, double[] constraints, int populationSize, int generations, double scalingFactor, double crossoverRate)
        {
            this.z = z;
            this.constraints = constraints;
            this.I = populationSize;
            this.N = z.GetLength(0);
            this.generations = generations;
            this.F = scalingFactor;
            this.crossoverRate = crossoverRate;
            this.fitness = new double[I];
            this.p = new double[I, N];
            this.OptimisationAlgorithmName = "DifferentialEvolution";

            GenerateInitialPopulation();
            TrainPopulation();
        }

        public override void TrainPopulation()
        {
            Console.WriteLine("Training Population using Differential Evolution Optimisation...");
            double prev_best_fitness = EvaluateFitness();
            double best_fitness = prev_best_fitness;
            double[] bestFitnessHistory = new double[generations];
            double[] averageFitnessHistory = new double[generations];

            for (generation = 0; generation < generations; generation++)
            {
                for (int i = 0; i < I; i++)
                {
                    double[] xr1, xr2, xr3;
                    do
                    {
                        xr1 = SelectIndividual();
                        xr2 = SelectIndividual();
                        xr3 = SelectIndividual();
                    } while (xr1 == xr2 || xr1 == xr3 || xr2 == xr3);

                    double[] v = Mutate(xr1, xr2, xr3);

                    double[] u = CreateTrial(v, i);

                    double trialFitness = EvaluateIndividualFitness(u);

                    if (trialFitness < fitness[i])
                    {
                        for (int j = 0; j < N; j++)
                        {
                            p[i, j] = u[j];
                        }
                        fitness[i] = trialFitness;
                    }
                }

                double current_best_fitness = fitness.Min();
                double average = fitness.Average();

                if (current_best_fitness < best_fitness)
                {
                    best_fitness = current_best_fitness;
                    fittestIndividual = GetBestIndividual();
                }
                bestFitnessHistory[generation] = best_fitness;
                averageFitnessHistory[generation] = average;

                if (generation % 10000 == 0) Console.WriteLine($"Generation {generation}: Best Fitness = {fitness.Min():N4} | Average Fitness = {average:N4}");
            }
            Console.WriteLine($"Generation {generation}: Best Fitness = {fitness.Min():N4} | Average Fitness = {fitness.Average():N4}");
            SaveIndividual(fittestIndividual);
            SaveFitness(bestFitnessHistory, "Best_3");
            SaveFitness(averageFitnessHistory, "Average_3");
        }


        private double[] CreateTrial(double[] v, int i)
        {
            double[] u = new double[N];
            int r = random.Next(N);
            int j = r;
            bool stopCrossover = false;

            do
            {
                u[j] = v[j];
                j = (j + 1) % N;

                if (random.NextDouble() >= crossoverRate)
                {
                    stopCrossover = true;
                }
            }
            while (!stopCrossover && j != r);

            while (j != r)
            {
                u[j] = p[i, j];
                j = (j + 1) % N;
            }
            return u;
        }


        private double[] Mutate(double[] xr1, double[] xr2, double[] xr3)
        {
            double[] v1 = new double[N];
            for (int j = 0; j < N; j++)
            {
                v1[j] = xr1[j] + F * (xr2[j] - xr3[j]);
                if (v1[j] < 0) v1[j] = 0;
            }
            return v1;
        }

        private double[] SelectIndividual()
        {
            int index = random.Next(I);
            double[] individual = new double[N];

            for (int i = 0; i < N; i++)
            {
                individual[i] = p[index, i];
            }
            return individual;
        }
    }
}
