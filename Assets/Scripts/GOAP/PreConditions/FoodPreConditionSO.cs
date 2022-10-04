using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Food Precondition", menuName = "GOAP/Preconditions/Food", order = 58)]
public class FoodPreConditionSO : PreConditionSO
{
    public int foodAmount;
    public override bool ExecutePreCondition(WorldState ws)
    {
        return ExectuteNumberComparative(ws.food, foodAmount);
    }
}
