using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EffectSO : ScriptableObject
{
    public EffectType effectType;
    public abstract WorldState ExecuteEffect(WorldState ws);

    protected float ExecuteInNumber(float variableToChange, float value, float minAmount = -1, float maxAmount = -1)
    {
        switch (effectType)
        {
            case EffectType.INCREMENT:
                variableToChange += value;
                if (maxAmount != -1)
                    variableToChange = Mathf.Min(variableToChange, maxAmount);
                break;
            case EffectType.DECREMENT:
                variableToChange -= value;
                if (minAmount != -1)
                    variableToChange = Mathf.Max(variableToChange, minAmount);
                break;
            case EffectType.SET:
                variableToChange = value;
                break;
        }
        return variableToChange;
        Debug.LogError("UNEXPECTED EFFECT FOR FLOATS");
    }

    protected int ExecuteInNumber(int variableToChange, int value, int minAmount = -1, int maxAmount = -1)
    {
        switch (effectType)
        {
            case EffectType.INCREMENT:
                variableToChange += value;
                if (maxAmount != -1)
                    variableToChange = Mathf.Min(variableToChange, maxAmount);
                break;
            case EffectType.DECREMENT:
                variableToChange -= value;
                if (minAmount != -1)
                    variableToChange = Mathf.Max(variableToChange, minAmount);
                break;
            case EffectType.SET:
                variableToChange = value;
                break;
        }
        return variableToChange;
        Debug.LogError("UNEXPECTED EFFECT FOR INTS");
    }

    protected bool ExecuteInBool(bool variableToChange, bool value)
    {
        if (effectType != EffectType.SET)
        {
            Debug.LogError("UNEXPECTED EFFECT FOR BOOLS");
        }
        else
        {
            variableToChange = value;
        }
        return variableToChange;
    }

    protected string ExecuteInString(string variableToChange, string value)
    {
        if (effectType != EffectType.SET)
        {
            Debug.LogError("UNEXPECTED EFFECT FOR FLOATS");
        }
        else
        {
            variableToChange = value;
        }
        return variableToChange;
    }
}
