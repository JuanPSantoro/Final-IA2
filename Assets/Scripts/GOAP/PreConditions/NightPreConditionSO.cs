using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Night Precondition", menuName = "GOAP/Preconditions/Night", order = 58)]
public class NightPreConditionSO : PreConditionSO
{
    public bool isNight;
    public override bool ExecutePreCondition(WorldState ws)
    {
        return ExecuteBoolComparative(ws.night, isNight);
    }
}
