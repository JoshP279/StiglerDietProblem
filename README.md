# Optimisation Algorithms for Solving Stigler's Diet Problem
## Overview

This project explores the application of three optimisation algorithms — Genetic Algorithm (GA), Differential Evolution (DE), and Particle Swarm Optimization (PSO)—to solve Stigler's Diet Problem. The goal is to determine the most cost-effective combination of 77 food items to meet specific nutritional requirements for an average adult male, based on real-world constraints.
## Problem Statement

Stigler's Diet Problem is a classic optimization challenge that seeks to identify the lowest-cost diet satisfying all essential nutritional needs. The problem is defined by a set of foods, each with specific costs and nutritional values, and a requirement to meet minimum annual nutrient values. By minimizing the cost function, each algorithm attempts to optimize the diet while satisfying all constraints.
## Approach

Three evolutionary algorithms were implemented to address this optimization problem, each with unique strategies for balancing exploration (broadly searching the solution space) and exploitation (focusing on refining promising solutions):

1. **Genetic Algorithm (GA)** - Utilizes mechanisms like tournament selection, uniform crossover, and Gaussian mutation to evolve a population of solutions across generations.

2. **Differential Evolution (DE)** - Generates new solutions by creating mutant vectors through differential scaling and crossover, iteratively replacing less fit individuals in the population.

3. **Particle Swarm Optimization (PSO)** - Adapts particle velocities based on dynamic inertia, cognitive, and social components, guiding particles toward optimal solutions in the search space.

## Key Components

* Fitness Function: The fitness function calculates total diet cost and applies a penalty for solutions that do not meet nutritional constraints. This guides each algorithm to minimize cost while fulfilling dietary requirements.
* Parameter Configurations: Each algorithm was tested with different configurations focused on exploration, exploitation, and a balance of both, enabling an analysis of how each parameter set affects performance.

## Observations

* Algorithm Performance: Each algorithm effectively identified low-cost solutions, with the PSO achieving the best results, followed closely by the GA and DE. Variability in exploration and exploitation strategies significantly impacted solution quality, with balanced configurations generally yielding the best outcomes.
* Nutritional Constraints: All algorithms were able to prioritize foods with high nutritional value and low cost, producing viable diet plans that met the problem's requirements.

## Conclusion

This project demonstrates the effectiveness of computational optimization algorithms for solving complex real-world problems, highlighting the strengths of each approach in balancing exploration and exploitation within the solution space.
