using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Heuristic", menuName = "GOAP/Heuristic", order = 58)]
public class GoapHeuristicSO : ScriptableObject
{
    public List<PreConditionSO> conditionsThatIncrementsHeuristic;

    public float ProcessHeuristic(WorldState ws)
    {
        int count = 0;

        foreach (PreConditionSO currentPre in conditionsThatIncrementsHeuristic)
        {
            if (currentPre.ExecutePreCondition(ws))
                count++;
        }
        return count;
    }
}
