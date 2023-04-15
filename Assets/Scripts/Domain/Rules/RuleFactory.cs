using System.Collections;
using System.Collections.Generic;
using System;

public class RuleFactory
{
    private static RuleFactory instance = null;

    private RuleFactory() {}

    public static RuleFactory GetInstance()
    {
        if (instance == null)
        {
            instance = new RuleFactory();
        }

        return instance;
    }

    public IRule GetRule(RuleType type)
    {
        switch (type)
        {
            case RuleType.BY_AGE:
                return new ByAgeRule();
            case RuleType.BY_GENDER:
                return new ByGenderRule();
            case RuleType.BY_ORIGIN:
                return new ByOriginRule();
            default:
                throw new NotSupportedException();
        }
    }
}
