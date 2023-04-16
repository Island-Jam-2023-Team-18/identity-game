using System;
using System.Collections;
using System.Collections.Generic;

public class ByGenderRule : IRule
{
    private GenderType gender;

    public ByGenderRule()
    {
        int iGender = new Random().Next((int)GenderType.M, (int)GenderType.NB + 1);
        gender = (GenderType)iGender;
    }
    public ValidationResult Validate(Candidate candidate, DateTime currentDate)
    {
        return candidate.gender == gender ? ValidationResult.VALID : ValidationResult.GENDER_NOT_MATCH;
    }

    public string Stringify()
    {
        return "Gender: " + gender;
    }
}
