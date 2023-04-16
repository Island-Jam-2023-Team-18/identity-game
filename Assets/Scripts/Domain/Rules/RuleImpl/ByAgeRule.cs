using System;
using System.Collections;
using System.Collections.Generic;

public class ByAgeRule : IRule
{
    private int minAge;
    private int maxAge;

    public ByAgeRule() {
        Random random = new Random();

        int minAgeRange = 5;
        int maxAgeRange = 40;
        int ageRange = random.Next(minAgeRange, maxAgeRange);

        int minLowAge = 18;
        int maxLowAge = 65 - ageRange;

        minAge = random.Next(minLowAge, maxLowAge);
        maxAge = minAge + ageRange;
    }

    public ValidationResult Validate(Candidate candidate, DateTime currentDate)
    {
        int age = currentDate.Subtract(candidate.dob).Days / 365;
        return age >= minAge && age <= maxAge ? ValidationResult.VALID : ValidationResult.AGE_NOT_MATCH;
    }

    public string Stringify()
    {
        return string.Format("Must be between {0} and {1} years old", minAge, maxAge);
    }
}
