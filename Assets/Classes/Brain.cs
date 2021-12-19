using System;

namespace Assets.Classes
{
    public class Brain
    {
        public Gene[] Genome;
        public int InnerNeurons;

        private readonly Random _randomNumberGenerator;

        public Brain(int genomeLength, int innerNeurons)
        {
            _randomNumberGenerator = new Random();

            GenerateGenome(genomeLength);
            InnerNeurons = innerNeurons;
        }

        public Brain(Brain parent1Brain, Brain parent2Brain)
            : this(parent1Brain.Genome.Length, parent1Brain.InnerNeurons)
        {
            BreedGenome(parent1Brain.Genome, parent2Brain.Genome);
        }

        private void GenerateGenome(int genomeLength)
        {
            Genome = new Gene[genomeLength];

            for (var i = 0; i < genomeLength; i++)
            {
                Genome[i] = new Gene();
            }
        }

        private void BreedGenome(Gene[] genome1, Gene[] genome2)
        {
            for (var i = 0; i < Genome.Length; i++)
            {
                var next = _randomNumberGenerator.Next(0, 1);
                Genome[i] = _randomNumberGenerator.Next(0, 1) == 0
                    ? genome1[i]
                    : genome2[i];
            }
        }
    }
}