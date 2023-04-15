using System.Collections;
using System.Collections.Generic;
using System;

public class Candidate
{
    public string name { get; private set; }
    public DateTime dob { get; private set; }
    public GenderType gender { get; private set; }
    public DateTime expiration { get; private set; }

    public Candidate(string name, DateTime dob, GenderType gender, DateTime expiration)
    {
        this.name = name;
        this.dob = dob;
        this.gender = gender;
        this.expiration = expiration;
    }
}
