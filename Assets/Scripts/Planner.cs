using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class Planner : MonoBehaviour 
{
    public List<GoapActionSO> actions;
    public GoapObjectiveSO goal;
    public GoapHeuristicSO heuristic;

	private void Start ()
    {
		StartCoroutine(Plan());
	}
	
    private IEnumerator Plan() {
		yield return new WaitForSeconds(0.2f);

		var observedState = new WorldState();
		
        GoapState initial = new GoapState();
        initial.worldState = new WorldState();
        observedState.energy = 100;

        initial.worldState = observedState;

        Func<GoapState, float> h = (curr) =>
        {
            return heuristic.ProcessHeuristic(curr.worldState);
        };

        Func<GoapState, bool> objective = (curr) =>
        {
            return goal.Satisfies(curr.worldState);
        };

		var plan = Goap.Execute(initial, null, objective, h, actions);

		if (plan == null)
			Debug.Log("Couldn't plan");
		else {
			/*GetComponent<Guy>().ExecutePlan(
				plan
				.Select(a => 
                {
                    Item i2 = everything.FirstOrDefault(i => i.target == a.target);
                    if (actDict.ContainsKey(a.actionName) && i2 != null)
                    {
                        return Tuple.Create(actDict[a.actionName], i2);
                    }
                    else
                    {
                        return null;
                    }
				}).Where(a => a != null)
				.ToList()
			);*/
		}
	}
}
