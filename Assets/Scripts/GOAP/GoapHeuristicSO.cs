using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "Heuristic", menuName = "GOAP/Heuristic", order = 58)]
public class GoapHeuristicSO : ScriptableObject
{
    public List<PreConditionSO> conditionsThatIncrementsHeuristic;

    public float ProcessHeuristic(WorldState ws)
    {
        var amount = conditionsThatIncrementsHeuristic.Where(currentPre => currentPre.ExecutePreCondition(ws)).Count();
        return amount;
    }
}
