using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PickUp Effect", menuName = "GOAP/Effects/PickUp", order = 58)]
public class PickUpEffectSO : EffectSO
{
    public Items item;
    public override WorldState ExecuteEffect(WorldState ws)
    {
        ws.tool = ExecuteInString(ws.tool, item.ToString());
        return ws;
    }
}
