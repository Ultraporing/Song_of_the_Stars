using System;

public class RandomGenerator
{
    private static Random Rand = null;
    private static int Seed = 0;

    public static void SetSeed(int seed)
    {
        if (seed != Seed || Rand == null)
        {
            Seed = seed;
            Rand = new Random(Seed);
        }
            
    }

    public static int GenerateInt(int min, int max)
    {
        return Rand.Next(min, max);
    }

    public static double GenerateDouble(double min, double max)
    {
        return Rand.NextDouble() * (max - min) + min;
    }
}
