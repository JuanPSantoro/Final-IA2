using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wood Precondition", menuName = "GOAP/Preconditions/Wood", order = 58)]
public class WoodPreConditionSO : PreConditionSO
{
    public int woodAmount;
    public override bool ExecutePreCondition(WorldState ws)
    {
        return ExectuteNumberComparative(ws.wood, woodAmount);
    }
}
