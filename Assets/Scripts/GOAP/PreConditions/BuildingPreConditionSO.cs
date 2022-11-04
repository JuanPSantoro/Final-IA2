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
        switch (building)
        {
            case Buildings.FARM:
                return ExectuteNumberComparative(ws.farms, amount);
            case Buildings.HOUSE:
                bool builded = amount > 0;
                return ExecuteBoolComparative(ws.houses, builded);
        }
        return false;
    }
}
