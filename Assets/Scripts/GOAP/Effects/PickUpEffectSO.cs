using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PickUp Effect", menuName = "GOAP/Effects/PickUp", order = 58)]
public class PickUpEffectSO : EffectSO
{
    public ItemType item;
    public override void ExecuteEffect(WorldState ws)
    {
        ws.tool = ExecuteInString(ws.tool, item.ToString());
    }
}
