using System.Collections;
using System.Collections.Generic;
using System;

public class CandidateFactory
{
    private static CandidateFactory instance = null;

    private CandidateFactory() { }

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
        Random random = new Random();

        int age = random.Next(18, 65);
        DateTime dob = currentDate.AddYears(-age);

        int iGender = random.Next((int)GenderType.M, (int)GenderType.NB);
        GenderType gender = (GenderType)iGender;

        int iOrigin = random.Next((int)OriginType.N, (int)OriginType.W);
        OriginType origin = (OriginType)iOrigin;

        bool expired = random.Next(100) < 20;
        DateTime expiration;
        if (expired)
        {
            int expiredDays = random.Next(1, 365);
            expiration = currentDate.AddDays(-expiredDays);
        }
        else
        {
            int expireDays = random.Next(0, 365 * 5);
            expiration = currentDate.AddDays(expireDays);
        }

        return new Candidate("John Doe", dob, gender, origin, expiration);
    }
}
