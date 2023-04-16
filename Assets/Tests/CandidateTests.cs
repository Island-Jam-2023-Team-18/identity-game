using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System;

public class CandidateTests
{
    [Test]
    public void CreatedCandidateShouldHaveAnAgeBetween18And65()
    {
        CandidateFactory factory = CandidateFactory.GetInstance();
        DateTime currentDate = new(2023, 4, 16);

        for (int i = 0; i < 1000; i++)
        {
            Candidate candidate = factory.GetCandidate(currentDate);
            Assert.LessOrEqual(candidate.dob.Year, 2005);
            Assert.GreaterOrEqual(candidate.dob.Year, 1958);

            Debug.Log("Test candidate: " + candidate.name);
        }
    }
}
