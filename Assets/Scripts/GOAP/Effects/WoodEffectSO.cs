using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wood Effect", menuName = "GOAP/Effects/Wood", order = 58)]
public class WoodEffectSO : EffectSO
{
    public int woodAmount;
    public override void ExecuteEffect(WorldState ws)
    {
        ws.wood = ExecuteInNumber(ws.wood, woodAmount, 0, 9999);
    }
}
