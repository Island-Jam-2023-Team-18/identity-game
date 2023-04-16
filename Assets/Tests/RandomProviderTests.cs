using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class RandomProviderTests
{
    [Test]
    public void RandomProviderTestsShouldGiveNumbersBetween1And100()
    {
        IRandomProvider randomProvider = RandomProvider.GetInstance();
        Assert.NotNull(randomProvider);

        for (int i = 0; i < 1000; i++)
        {
            int num = randomProvider.GetNumber(1, 100);
            Assert.GreaterOrEqual(num, 1);
            Assert.LessOrEqual(num, 100);
        }
    }
}
