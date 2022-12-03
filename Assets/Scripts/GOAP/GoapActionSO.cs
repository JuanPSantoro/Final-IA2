using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GOAP Action", menuName = "GOAP/Action", order = 58)]
public class GoapActionSO : ScriptableObject
{
    public string actionName;
    public int cost;
    public List<PreConditionSO> preconditions;
    public List<EffectSO> effects;
    public ActionEntity actionEntity;
    public Destination destination;
}
