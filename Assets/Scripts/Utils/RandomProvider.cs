using System.Collections;
using System.Collections.Generic;
using System;

public class RandomProvider : IRandomProvider
{
  private static IRandomProvider instance = null;
  private Random random;

  private RandomProvider()
  {
    random = new Random();
  }

  public static IRandomProvider GetInstance()
  {
    if (instance == null)
    {
      instance = new RandomProvider();
    }

    return instance;
  }

  public int GetNumber(int min, int max)
  {
    return random.Next(min, max);
  }
}
