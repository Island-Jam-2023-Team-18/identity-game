using System.Collections;
using System.Collections.Generic;
using System;

public class CandidateFactory
{
    private static CandidateFactory instance = null;

    private readonly NameProvider nameProvider;
    private readonly IRandomProvider randomProvider;

    private CandidateFactory()
    {
        randomProvider = RandomProvider.GetInstance();
        NameProvider.NameProviderBuilder builder = new NameProvider.NameProviderBuilder();
        nameProvider = builder
            .SetRandomProvider(randomProvider)
            .Build();
    }

    public static CandidateFactory GetInstance()
    {
        if(instance == null)
        {
            instance = new CandidateFactory();
        }

        return instance;
    }

    public Candidate GetCandidate(DateTime currentDate)
    {
        string name = nameProvider.GetFullName();

        int age = randomProvider.GetNumber(18, 65);
        DateTime dob = currentDate.AddYears(-age);

        int iGender = randomProvider.GetNumber((int)GenderType.M, (int)GenderType.NB + 1);
        GenderType gender = (GenderType)iGender;

        int iOrigin = randomProvider.GetNumber((int)OriginType.N, (int)OriginType.W);
        OriginType origin = (OriginType)iOrigin;

        bool expired = randomProvider.GetNumber(0, 100) < 20;
        DateTime expiration;
        if (expired)
        {
            int expiredDays = randomProvider.GetNumber(1, 365);
            expiration = currentDate.AddDays(-expiredDays);
        }
        else
        {
            int expireDays =randomProvider.GetNumber(0, 365 * 5);
            expiration = currentDate.AddDays(expireDays);
        }

        return new Candidate(name, dob, gender, origin, expiration);
    }
}
