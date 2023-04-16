using System;
using System.Collections;
using System.Collections.Generic;

public class ByOriginRule : IRule
{
    private OriginType origin;

    public ByOriginRule()
    {
        int iOrigin = new Random().Next((int)OriginType.N, (int)OriginType.W);
        origin = (OriginType)iOrigin;
    }
    public string Stringify()
    {
        return "Origin: " + origin;
    }

    public ValidationResult Validate(Candidate candidate, DateTime currentDate)
    {
        return candidate.origin == origin ? ValidationResult.VALID : ValidationResult.ORIGIN_NOT_MATCH;
    }
}
