using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PreConditionSO : ScriptableObject
{
    public abstract bool ExecutePreCondition(WorldState ws);
    public Comparatives comparative;

    protected bool ExectuteNumberComparative(float obtained, float desired)
    {
        switch (comparative)
        {
            case Comparatives.GREATER:
                return obtained > desired;
            case Comparatives.LESS:
                return obtained < desired;
            case Comparatives.EQUAL:
                return obtained == desired;
            case Comparatives.DIFFERENT:
                return obtained != desired;
            case Comparatives.GREATER_OR_EQUAL:
                return obtained >= desired;
            case Comparatives.LESS_OR_EQUAL:
                return obtained <= desired;
        }
        Debug.LogError("UNEXPECTED COMPARATIVE BETWEEN FLOATS");
        return false;
    }

    protected bool ExectuteNumberComparative(int obtained, int desired)
    {
        switch (comparative)
        {
            case Comparatives.GREATER:
                return obtained > desired;
            case Comparatives.LESS:
                return obtained < desired;
            case Comparatives.EQUAL:
                return obtained == desired;
            case Comparatives.DIFFERENT:
                return obtained != desired;
            case Comparatives.GREATER_OR_EQUAL:
                return obtained >= desired;
            case Comparatives.LESS_OR_EQUAL:
                return obtained <= desired;
        }
        Debug.LogError("UNEXPECTED COMPARATIVE BETWEEN INTS");
        return false;
    }

    protected bool ExecuteBoolComparative(bool obtained, bool desired)
    {
        switch (comparative)
        {
            case Comparatives.TRUE:
                return obtained == desired;
            case Comparatives.FALSE:
                return obtained != desired;
        }
        Debug.LogError("UNEXPECTED COMPARATIVE BETWEEN BOOLS");
        return false;
    }

    protected bool ExecuteStringComparative(string obtained, string desired)
    {
        switch (comparative)
        {
            case Comparatives.EQUAL:
                return obtained == desired;
            case Comparatives.DIFFERENT:
                return obtained != desired;
        }
        Debug.LogError("UNEXPECTED COMPARATIVE BETWEEN STRINGS");
        return false;
    }
}
