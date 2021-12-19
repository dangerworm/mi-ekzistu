using System;

namespace Assets.Classes
{
    public class Gene
    {
        private readonly Random _randomNumberGenerator;
        private readonly byte[] _bytes;

        public Gene()
        {
            _randomNumberGenerator = new Random();
            _bytes = BitConverter.GetBytes(GetScaledRandomValue());
        }

        public void Mutate()
        {
        }

        private double GetScaledRandomValue()
        {
            var max = (double)float.MaxValue;
            var min = (double)float.MinValue;

            return (_randomNumberGenerator.NextDouble() * (max - min)) + min;
        }

        public override string ToString()
        {
            return BitConverter.ToString(_bytes).Replace("-", "");
        }
    }
}
