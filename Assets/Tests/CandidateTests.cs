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

  [Test]
  public void CreatedCandidateShouldGenerateNonBinaryOnes()
  {
    int nbs = 0;
    CandidateFactory factory = CandidateFactory.GetInstance();

    for (int i = 0; i < 1000; i++)
    {
      Candidate candidate = factory.GetCandidate(DateTime.Now);

      if (candidate.gender == GenderType.NB)
      {
        nbs++;
      }

      Debug.Log("Test candidate gender: " + candidate.gender);
    }

    Assert.Greater(nbs, 0, "Should have some NB candidates");
  }

  [Test]
  public void CreatedCandidateShouldHaveAllOrigins()
  {
    int north = 0;
    int south = 0;
    int east = 0;
    int west = 0;

    CandidateFactory factory = CandidateFactory.GetInstance();

    for (int i = 0; i < 1000; i++)
    {
      Candidate candidate = factory.GetCandidate(DateTime.Now);

      switch (candidate.origin)
      {
        case OriginType.NORTH:
          north++;
          break;
        case OriginType.SOUTH:
          south++;
          break;
        case OriginType.EAST:
          east++;
          break;
        case OriginType.WEST:
          west++;
          break;
      }

      Debug.Log("Candidate from " + candidate.origin);
    }

    Assert.Greater(north, 0, "Should have candidates from north");
    Assert.Greater(south, 0, "Should have candidates from south");
    Assert.Greater(east, 0, "Should have candidates from east");
    Assert.Greater(west, 0, "Should have candidates from west");
  }
}
