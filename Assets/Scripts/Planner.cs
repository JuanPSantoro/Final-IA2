using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class Planner : MonoBehaviour 
{
	private readonly List<Tuple<Vector3, Vector3>> _debugRayList = new List<Tuple<Vector3, Vector3>>();

    public List<GoapActionSO> actions;

	private void Start ()
    {
		StartCoroutine(Plan());
	}
	
    private IEnumerator Plan() {
		yield return new WaitForSeconds(0.2f);

		var observedState = new WorldState();
		
		/*var nav = Navigation.instance; //Consigo los items
		var floorItems = nav.AllItems();
		var inventory = nav.AllInventories();
		var everything = nav.AllItems().Union(nav.AllInventories());// .Union() une 2 colecciones sin agregar duplicados(eso incluye duplicados en la misma coleccion)
        */
        GoapState initial = new GoapState();
        initial.worldState = new WorldState();
        observedState.energy = 100;

        initial.worldState = observedState;
        Debug.Log(observedState);

        Func<GoapState, float> heuristc = (curr) =>
        {
            int count = 0;

            if (curr.worldState.wood < 20)
                count++;
            /*if (curr.worldState.energy < 10)
                count++;*/
            return count;
        };

        Func<GoapState, bool> objective = (curr) =>
        {
            return curr.worldState.wood >= 40;
        };

		var plan = Goap.Execute(initial, null, objective, heuristc, actions);

		if (plan == null)
			Debug.Log("Couldn't plan");
		else {

            var planList = plan.ToList();
            for (int i = 0; i < planList.Count; i++)
            {
                Debug.Log(planList[i].actionName);
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

    void OnDrawGizmos()
    {
		Gizmos.color = Color.cyan;
		foreach(var t in _debugRayList)
        {
			Gizmos.DrawRay(t.Item1, (t.Item2-t.Item1).normalized);
			Gizmos.DrawCube(t.Item2+Vector3.up, Vector3.one*0.2f);
		}
	}
}
