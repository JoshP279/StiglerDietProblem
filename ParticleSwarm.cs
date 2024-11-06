namespace Assignment4
{
    class ParticleSwarm : OptimisationAlgorithm
    {
        private double[,] v;
        private double[,] pBest;

        private double[] pBestFitness;
        private double[] gBest;

        private double gBestFitness;
        private double w;
        private double w_max;
        private double w_min;

        private double c1;
        private double c1_max;
        private double c1_min;
        private double c2;
        private double c2_max;
        private double c2_min;

        public ParticleSwarm(double[,] z, double[] constraints, int populationSize, int generations, double inertia, double cognitive_component, double social_component)
        {
            this.z = z;
            this.constraints = constraints;
            this.I = populationSize;
            this.N = z.GetLength(0);
            this.generations = generations;
            this.fitness = new double[I];
            this.p = new double[I, N];
            this.v = new double[I, N];
            this.pBest = new double[I, N];
            this.pBestFitness = new double[I];
            this.gBest = new double[N];
            this.gBestFitness = double.MaxValue;
            this.w = inertia;
            this.w_max = inertia;
            this.w_min = 0.4;
            this.c1 = cognitive_component;
            this.c1_max = cognitive_component;
            this.c1_min = 0.5;
            this.c2 = social_component;
            this.c2_max = social_component;
            this.c2_min = 0.5;
            this.OptimisationAlgorithmName = "ParticleSwarm";
            GenerateInitialPopulation();
            SetInitialBestFitness();
            TrainPopulation();
        }

        public override void TrainPopulation()
        {
            Console.WriteLine("Training Population using Particle Swarm Optimisation...");
            double[] bestFitnessHistory = new double[generations];
            double[] averageFitnessHistory = new double[generations];

            for (int generation = 0; generation < generations; generation++)
            {
                w = w_max - (((w_max - w_min) / generations) * generation);
                c1 = c1_max - (((c1_max - c1_min) / generations) * generation);
                c2 = c2_max - (((c2_max - c2_min) / generations) * generation);

                EvaluateFitness();

                for (int i = 0; i < I; i++)
                {
                    if (fitness[i] < pBestFitness[i])
                    {
                        pBestFitness[i] = fitness[i];
                        fittestIndividual = GetBestIndividual();
                        TransferParticle(p, pBest, i);
                    }

                    if (pBestFitness[i] < gBestFitness)
                    {
                        gBestFitness = pBestFitness[i];
                        TransferParticle(pBest, gBest, i);
                    }
                }

                for (int i = 0; i < I; i++)
                {
                    for (int j = 0; j < N; j++)
                    {
                        double r1 = random.NextDouble();
                        double r2 = random.NextDouble();

                        v[i, j] = w * v[i, j] +
                            c1 * r1 * (pBest[i, j] - p[i, j]) +
                            c2 * r2 * (gBest[j] - p[i, j]);

                        p[i, j] += v[i, j];

                        if (p[i, j] < 0) p[i, j] = 0;
                    }
                }

                double average = fitness.Average();
                averageFitnessHistory[generation] = average;
                bestFitnessHistory[generation] = gBestFitness;
                if (generation % 10000 == 0)
                {
                    Console.WriteLine($"Generation {generation}: Best Fitness = {gBestFitness:N4} | Average Fitness = {average:N4}");
                }


            }
            Console.WriteLine($"Final Best Fitness: {gBestFitness:N4}");
            SaveIndividual(fittestIndividual);
            SaveFitness(bestFitnessHistory, "Best_1");
            SaveFitness(averageFitnessHistory, "Average_1");
        }

        private void SetInitialBestFitness()
        {
            EvaluateFitness();

            for (int i = 0; i < I; i++)
            {
                pBestFitness[i] = fitness[i];
                TransferParticle(p, pBest, i);

                if (fitness[i] < gBestFitness)
                {
                    gBestFitness = fitness[i];
                    TransferParticle(pBest, gBest, i);
                }
            }
        }

        private void TransferParticle(double[,] source, double[,] destination, int index)
        {
            for (int j = 0; j < N; j++)
            {
                destination[index, j] = source[index, j];
            }
        }
        private void TransferParticle(double[,] source, double[] destination, int index)
        {
            for (int j = 0; j < N; j++)
            {
                destination[j] = source[index, j];
            }
        }
    }
}