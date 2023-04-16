using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class RuleSet
{
    private List<IRule> rules;

    public RuleSet()
    {
        rules = new List<IRule>();
        RuleFactory factory = RuleFactory.GetInstance();

        rules.Add(factory.GetRule(RuleType.BY_AGE));
        rules.Add(factory.GetRule(RuleType.BY_GENDER));
        rules.Add(factory.GetRule(RuleType.BY_ORIGIN));
    }

    public List<String> GetDescriptions()
    {
        return rules.Select(r => r.Stringify()).ToList<string>();
    }

    public ValidationResult Validate(Candidate candidate, DateTime currentDate, int rulesCount)
    {
        if (candidate.expiration.CompareTo(currentDate) < 0)
        {
            return ValidationResult.ID_EXPIRED;
        }

        for (int i = 0; i < rulesCount && i < rules.Count; i++)
        {
            ValidationResult result = rules.ElementAt(i).Validate(candidate, currentDate);
            if (result != ValidationResult.VALID)
            {
                return result;
            }
        }

        return ValidationResult.VALID;
    }
}
