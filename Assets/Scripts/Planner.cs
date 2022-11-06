using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class Planner : MonoBehaviour 
{
    public List<GoapActionSO> actions;
    private GoapObjectiveSO _goal;
    private GoapHeuristicSO _heuristic;

    public GoapObjectiveSO Objective { set { _goal = value; } }
    public GoapHeuristicSO Heuristic { set { _heuristic = value; } }

    public World world;

    private void Awake()
    {
        if (world == null)
            world = FindObjectOfType<World>();
    }

    public void StartPlan()
    {
        if (_goal != null && _heuristic != null)
            StartCoroutine(Plan());
    }

    private IEnumerator Plan() {
		yield return new WaitForSeconds(0.2f);
		
        GoapState initial = new GoapState();
        initial.worldState = world.GetCurrentWorldState();

        Func<GoapState, float> h = (curr) =>
        {
            return _heuristic.ProcessHeuristic(curr.worldState);
        };

        Func<GoapState, bool> objective = (curr) =>
        {
            return _goal.Satisfies(curr.worldState) /*|| (curr.generator != null && curr.generator.actionName == "Sleep")*/;
        };

		var plan = Goap.Execute(initial, null, objective, h, actions);

		if (plan == null)
			Debug.Log("Couldn't plan");
		else {
            var currentWorldState = world.GetCurrentWorldState();

            foreach (var currentAction in plan)
            {
                foreach (var currentEffect in currentAction.effects)
                {
                    currentWorldState = currentEffect.ExecuteEffect(currentWorldState);
                    world.SaveWorldState(currentWorldState);
                }
            }
            
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
