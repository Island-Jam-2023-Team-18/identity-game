using System.Collections;
using System.Collections.Generic;
using System;

public interface IRule
{
    public bool Validate(Candidate candidate, DateTime currentDate);
    public string Stringify();
}
