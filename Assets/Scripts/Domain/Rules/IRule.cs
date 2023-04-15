using System.Collections;
using System.Collections.Generic;
using System;

public interface IRule
{
    bool Validate(Candidate candidate, DateTime currentDate);
    string Stringify();
}
