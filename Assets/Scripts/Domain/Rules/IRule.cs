using System.Collections;
using System.Collections.Generic;
using System;

public interface IRule
{
    ValidationResult Validate(Candidate candidate, DateTime currentDate);
    string Stringify();
}
