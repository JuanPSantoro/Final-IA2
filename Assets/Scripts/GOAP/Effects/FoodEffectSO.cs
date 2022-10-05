using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Food Effect", menuName = "GOAP/Effects/Food", order = 58)]
public class FoodEffectSO : EffectSO
{
    public int foodAmount;

    public override WorldState ExecuteEffect(WorldState ws)
    {
        ws.food = ExecuteInNumber(ws.food, foodAmount, 0, 9999);
        return ws;
    }
}
