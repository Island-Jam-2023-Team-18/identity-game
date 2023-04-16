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
}
