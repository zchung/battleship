using Battleship.Logic.Factories.Interfaces;
using System;

namespace Battleship.Logic.Factories
{
    public class RandomGeneratorFactory : IRandomGeneratorFactory
    {
        private readonly Random _random;

        public RandomGeneratorFactory()
        {
            _random = new Random();
        }
        public int GetRandomNumber(int min, int max)
        {
            return _random.Next(min, max);
        }
    }
}
