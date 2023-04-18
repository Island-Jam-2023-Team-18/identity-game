using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System;
using System.Text.RegularExpressions;

public class RulesTests
{
  [Test]
  public void CandidateWith120YearsOdShouldBeRejected()
  {
    DateTime currentDate = new DateTime(2023, 4, 16);
    DateTime dob = new DateTime(1903, 4, 16);

    Candidate candidate = new Candidate("John Doe", dob, GenderType.M, OriginType.NORTH, DateTime.Now);

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

    Candidate maleCandidate = new Candidate("John Doe", DateTime.Now, GenderType.M, OriginType.NORTH, DateTime.Now);
    Candidate femaleCandidate = new Candidate("Jane Doe", DateTime.Now, GenderType.F, OriginType.NORTH, DateTime.Now);
    Candidate nbCandidate = new Candidate("Lorem Ipsum", DateTime.Now, GenderType.NB, OriginType.NORTH, DateTime.Now);

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

  [Test]
  public void RuleSetShouldHaveAllDescriptions()
  {
    RuleSet rules = new ();

    List<string> descriptions = rules.GetDescriptions();
    foreach (string d in descriptions)
    {
      Debug.Log(d);
    }

    Regex ageExp = new (@"^Must be between \d+ and \d+ years old$");
    Regex genderExp = new (@"^Gender: (M|F|NB)$");
    Regex originExp = new(@"^Origin: (NORTH|SOUTH|EAST|WEST)$");

    Assert.AreEqual(descriptions.Count, 3);
    Assert.IsTrue(ageExp.IsMatch(descriptions[0]), "Should match age expression");
    Assert.IsTrue(genderExp.IsMatch(descriptions[1]), "Should match gender expression");
    Assert.IsTrue(originExp.IsMatch(descriptions[2]), "Should match origin expression");
  }

  [Test]
  public void RulesetShouldValidateCandidateAge()
  {
    RuleSet rules = new ();

    string ageDescription = rules.GetDescriptions()[0];
    Debug.Log(ageDescription);
    Regex ageExp = new(@"^Must be between (\d+) and (\d+) years old$");
    int minAge = Int32.Parse(ageExp.Matches(ageDescription)[0].Groups[1].Value);
    Debug.Log("Min age: " + minAge);

    DateTime currentDate = DateTime.Now;

    Candidate candidate = new Candidate("John Doe", currentDate.AddYears(-minAge), GenderType.M, OriginType.NORTH, currentDate.AddYears(1));

    ValidationResult result = rules.Validate(candidate, currentDate, 1);

    Assert.AreEqual(result, ValidationResult.VALID, "Candidate age should be valid");
  }

  [Test]
  public void RulesetShouldRejectCandidateAge()
  {
    RuleSet rules = new();

    string ageDescription = rules.GetDescriptions()[0];
    Debug.Log(ageDescription);
    Regex ageExp = new(@"^Must be between (\d+) and (\d+) years old$");
    int minAge = Int32.Parse(ageExp.Matches(ageDescription)[0].Groups[1].Value);
    Debug.Log("Min age: " + minAge);

    DateTime currentDate = DateTime.Now;

    Candidate candidate = new Candidate("John Doe", currentDate.AddYears(-minAge + 1), GenderType.M, OriginType.NORTH, currentDate.AddYears(1));

    ValidationResult result = rules.Validate(candidate, currentDate, 1);

    Assert.AreEqual(result, ValidationResult.AGE_NOT_MATCH, "Candidate age should be rejected by age");
  }

  [Test]
  public void RulesetShouldValidateCandidateGender()
  {
    RuleSet rules = new();

    string ageDescription = rules.GetDescriptions()[0];
    Debug.Log(ageDescription);
    Regex ageExp = new(@"^Must be between (\d+) and (\d+) years old$");
    int minAge = Int32.Parse(ageExp.Matches(ageDescription)[0].Groups[1].Value);
    Debug.Log("Min age: " + minAge);

    string genderDescription = rules.GetDescriptions()[1];
    Debug.Log(genderDescription);
    Regex genderExp = new(@"^Gender: (M|F|NB)$", RegexOptions.Compiled);
    GenderType gender = (GenderType)Enum.Parse(typeof(GenderType), genderExp.Matches(genderDescription)[0].Groups[1].Value);
    Debug.Log("Candidate gender: " + gender);

    DateTime currentDate = DateTime.Now;

    Candidate candidate = new Candidate("John Doe", currentDate.AddYears(-minAge), gender, OriginType.NORTH, currentDate.AddYears(1));

    ValidationResult result = rules.Validate(candidate, currentDate, 2);

    Assert.AreEqual(result, ValidationResult.VALID, "Candidate gender should be valid");
  }

  [Test]
  public void RulesetShouldRejectCandidateGender()
  {
    RuleSet rules = new();

    string ageDescription = rules.GetDescriptions()[0];
    Debug.Log(ageDescription);
    Regex ageExp = new(@"^Must be between (\d+) and (\d+) years old$");
    int minAge = Int32.Parse(ageExp.Matches(ageDescription)[0].Groups[1].Value);
    Debug.Log("Min age: " + minAge);

    string genderDescription = rules.GetDescriptions()[1];
    Debug.Log(genderDescription);
    Regex genderExp = new(@"^Gender: (M|F|NB)$", RegexOptions.Compiled);
    GenderType gender = (GenderType)Enum.Parse(typeof(GenderType), genderExp.Matches(genderDescription)[0].Groups[1].Value);
    Debug.Log("Candidate gender: " + gender);

    if (gender == GenderType.M)
    {
      gender = GenderType.F;
    }
    else
    {
      gender = GenderType.M;
    }

    DateTime currentDate = DateTime.Now;

    Candidate candidate = new Candidate("John Doe", currentDate.AddYears(-minAge), gender, OriginType.NORTH, currentDate.AddYears(1));

    ValidationResult result = rules.Validate(candidate, currentDate, 2);

    Assert.AreEqual(result, ValidationResult.GENDER_NOT_MATCH, "Candidate gender should be rejected");
  }

  [Test]
  public void RulesetShouldValidateCandidateOrigin()
  {
    RuleSet rules = new();

    string ageDescription = rules.GetDescriptions()[0];
    Debug.Log(ageDescription);
    Regex ageExp = new(@"^Must be between (\d+) and (\d+) years old$");
    int minAge = Int32.Parse(ageExp.Matches(ageDescription)[0].Groups[1].Value);
    Debug.Log("Min age: " + minAge);

    string genderDescription = rules.GetDescriptions()[1];
    Debug.Log(genderDescription);
    Regex genderExp = new(@"^Gender: (M|F|NB)$", RegexOptions.Compiled);
    GenderType gender = (GenderType)Enum.Parse(typeof(GenderType), genderExp.Matches(genderDescription)[0].Groups[1].Value);
    Debug.Log("Candidate gender: " + gender);

    string originDescription = rules.GetDescriptions()[2];
    Debug.Log(originDescription);
    Regex originExp = new(@"^Origin: (NORTH|SOUTH|EAST|WEST)$", RegexOptions.Compiled);
    OriginType origin = (OriginType)Enum.Parse(typeof(OriginType), originExp.Matches(originDescription)[0].Groups[1].Value);
    Debug.Log("Candidate origin: " + origin);

    DateTime currentDate = DateTime.Now;

    Candidate candidate = new Candidate("John Doe", currentDate.AddYears(-minAge), gender, origin, currentDate.AddYears(1));

    ValidationResult result = rules.Validate(candidate, currentDate, 3);

    Assert.AreEqual(result, ValidationResult.VALID, "Candidate origin should be valid");
  }

  [Test]
  public void RulesetShouldRejectCandidateOrigin()
  {
    RuleSet rules = new();

    string ageDescription = rules.GetDescriptions()[0];
    Debug.Log(ageDescription);
    Regex ageExp = new(@"^Must be between (\d+) and (\d+) years old$");
    int minAge = Int32.Parse(ageExp.Matches(ageDescription)[0].Groups[1].Value);
    Debug.Log("Min age: " + minAge);

    string genderDescription = rules.GetDescriptions()[1];
    Debug.Log(genderDescription);
    Regex genderExp = new(@"^Gender: (M|F|NB)$", RegexOptions.Compiled);
    GenderType gender = (GenderType)Enum.Parse(typeof(GenderType), genderExp.Matches(genderDescription)[0].Groups[1].Value);
    Debug.Log("Candidate gender: " + gender);

    string originDescription = rules.GetDescriptions()[2];
    Debug.Log(originDescription);
    Regex originExp = new(@"^Origin: (NORTH|SOUTH|EAST|WEST)$", RegexOptions.Compiled);
    OriginType origin = (OriginType)Enum.Parse(typeof(OriginType), originExp.Matches(originDescription)[0].Groups[1].Value);
    Debug.Log("Candidate origin: " + origin);

    if (origin == OriginType.NORTH)
    {
      origin = OriginType.SOUTH;
    }
    else
    {
      origin = OriginType.NORTH;
    }

    DateTime currentDate = DateTime.Now;

    Candidate candidate = new Candidate("John Doe", currentDate.AddYears(-minAge), gender, origin, currentDate.AddYears(1));

    ValidationResult result = rules.Validate(candidate, currentDate, 3);

    Assert.AreEqual(result, ValidationResult.ORIGIN_NOT_MATCH, "Candidate origin should be valid");
  }

  [Test]
  public void RuleSetShouldRejectCandidateWithExpiredID()
  {
    RuleSet rules = new ();

    DateTime currentDate = DateTime.Now;

    Candidate candidate = new Candidate("John Doe", currentDate, GenderType.M, OriginType.NORTH, currentDate.AddDays(-1));

    ValidationResult result = rules.Validate(candidate, currentDate, 1);

    Assert.AreEqual(ValidationResult.ID_EXPIRED, result, "Candidate ID should be rejected by expiration");
  }
}
