using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Building Precondition", menuName = "GOAP/Preconditions/Building", order = 58)]

public class BuildingPreConditionSO : PreConditionSO
{
    public Buildings building;
    public int amount;

    public override bool ExecutePreCondition(WorldState ws)
    {
        int builded = 0;
        switch (building)
        {
            case Buildings.FARM:
                builded = ws.farms;
                break;
            case Buildings.HOUSE:
                builded = ws.houses;
                break;
        }
        return ExectuteNumberComparative(builded, amount);
    }
}
