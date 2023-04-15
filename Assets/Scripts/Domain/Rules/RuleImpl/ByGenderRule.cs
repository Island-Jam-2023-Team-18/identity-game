using System;
using System.Collections;
using System.Collections.Generic;

public class ByGenderRule : IRule
{
    private GenderType gender;

    public ByGenderRule()
    {
        int iGender = new Random().Next((int)GenderType.M, (int)GenderType.NB);
        GenderType gender = (GenderType)iGender;
    }
    public bool Validate(Candidate candidate, DateTime currentDate)
    {
        return candidate.gender == gender;
    }

    public string Stringify()
    {
        return "Gender: " + gender;
    }
}
