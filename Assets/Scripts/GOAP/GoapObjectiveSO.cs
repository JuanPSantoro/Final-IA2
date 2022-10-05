using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Objective", menuName = "GOAP/Objective", order = 58)]
public class GoapObjectiveSO : ScriptableObject
{
    public List<PreConditionSO> preconditions;

    public bool Satisfies(WorldState ws)
    {
        foreach (PreConditionSO currentPre in preconditions)
        {
            if (!currentPre.ExecutePreCondition(ws))
                return false;
        }
        return true;
    }
}
