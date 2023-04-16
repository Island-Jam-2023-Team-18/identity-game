using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System;

public class RulesTests
{
    [Test]
    public void CandidateWith120YearsOdShouldBeRejected()
    {
        DateTime currentDate = new DateTime(2023, 4, 16);
        DateTime dob = new DateTime(1903, 4, 16);

        Candidate candidate = new Candidate("John Doe", dob, GenderType.M, OriginType.N, DateTime.Now);

        IRule rule = new ByAgeRule();
        ValidationResult result = rule.Validate(candidate, DateTime.Now);
        Assert.AreEqual(result, ValidationResult.AGE_NOT_MATCH);
    }

    [Test]
    public void GenderRuleShouldGiveAllGenders()
    {
        int males = 0;
        int females = 0;
        int nonBinaries = 0;

        Candidate maleCandidate = new Candidate("John Doe", DateTime.Now, GenderType.M, OriginType.N, DateTime.Now);
        Candidate femaleCandidate = new Candidate("Jane Doe", DateTime.Now, GenderType.F, OriginType.N, DateTime.Now);
        Candidate nbCandidate = new Candidate("Lorem Ipsum", DateTime.Now, GenderType.NB, OriginType.N, DateTime.Now);

        for (int i = 0; i < 1000; i++)
        {
            IRule rule = new ByGenderRule();
            Debug.Log(rule.Stringify());

            if (rule.Validate(maleCandidate, DateTime.Now) == ValidationResult.VALID)
            {
                males++;
            }
            else if (rule.Validate(femaleCandidate, DateTime.Now) == ValidationResult.VALID)
            {
                females++;
            }
            else if (rule.Validate(nbCandidate, DateTime.Now) == ValidationResult.VALID)
            {
                nonBinaries++;
            }
        }

        Assert.Greater(males, 0, "Should have male rules");
        Assert.Greater(females, 0, "Should have female rules");
        Assert.Greater(nonBinaries, 0, "Should have non binary rules");
    }
}
