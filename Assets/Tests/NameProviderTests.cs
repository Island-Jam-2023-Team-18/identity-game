using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class NameProviderTests
{
    class MockRandomProvider : IRandomProvider
    {
        private readonly int mockValue;

        public MockRandomProvider(int mockValue)
        {
            this.mockValue = mockValue;
        }

        public int GetNumber(int min, int max)
        {
            return mockValue;
        }
    }

    // A Test behaves as an ordinary method
    [Test]
    public void NameProviderBuilderShouldGiveANonNullObject()
    {
        NameProvider.NameProviderBuilder builder = new NameProvider.NameProviderBuilder();
        NameProvider nameProvider = builder.Build();
        Assert.NotNull(nameProvider);
    }

    [Test]
    public void NameProviderShouldGiveFirstAvailableNameAndFirstAvailableSurname()
    {
        IRandomProvider mockRandomProvider = new MockRandomProvider(0);
        NameProvider.NameProviderBuilder builder = new NameProvider.NameProviderBuilder();
        NameProvider nameProvider = builder
            .SetRandomProvider(mockRandomProvider)
            .Build();

        string name = nameProvider.GetFullName();
        Assert.AreEqual(name, "Emma Smith");
    }
}
