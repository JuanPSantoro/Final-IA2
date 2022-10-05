using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item Precondition", menuName = "GOAP/Preconditions/Item", order = 58)]

public class ItemPreConditionSO : PreConditionSO
{
    public Items item;
    public override bool ExecutePreCondition(WorldState ws)
    {
        return ExecuteStringComparative(ws.tool, item.ToString());
    }
}
