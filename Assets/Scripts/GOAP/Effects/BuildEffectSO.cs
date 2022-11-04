using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Build Effect", menuName = "GOAP/Effects/Build", order = 58)]

public class BuildEffectSO : EffectSO
{
    public Buildings building;
    public override WorldState ExecuteEffect(WorldState ws)
    {
        switch(building)
        {
            case Buildings.HOUSE:
                ws.houses = ExecuteInBool(ws.houses, true); ;
                break;
            case Buildings.FARM:
                ws.farms = ExecuteInNumber(ws.farms, 1, 0); ;
                break;
        }
        return ws;
    }
}
