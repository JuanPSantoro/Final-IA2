using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "Objective", menuName = "GOAP/Objective", order = 58)]
public class GoapObjectiveSO : ScriptableObject
{
    public List<PreConditionSO> preconditions;

    public bool Satisfies(WorldState ws)
    {
        return !preconditions.Where(currentPre => !currentPre.ExecutePreCondition(ws)).Any();
    }
}
