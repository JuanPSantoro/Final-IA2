using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEditor;

public class Waypoint : MonoBehaviour
{
	public List<Waypoint> adyacent = new List<Waypoint>();
	public HashSet<Item> nearbyItems = new HashSet<Item>();

	void Start ()
    {
		foreach(var wp in adyacent) {
			if(wp != null && wp.adyacent != null) {
				if(!wp.adyacent.Contains(this))
					wp.adyacent.Add(this);
			}
		}
		adyacent = adyacent.Where(x=>x!=null).Distinct().ToList();
	}
	
	void Update ()
    {
		nearbyItems.RemoveWhere(it => !it.isActiveAndEnabled);
	}
	
	void OnDrawGizmos()
    {
		Gizmos.color = Color.white;
		Gizmos.DrawWireSphere(transform.position, 0.3f);
		Gizmos.color = Color.blue;
		foreach(var wp in adyacent)
        {
            Gizmos.DrawLine(transform.position, wp.transform.position);
		}
	}

	[ExecuteInEditMode]
	public void CreateAndLink()
    {
#if UNITY_EDITOR
		var newWaypoint = Instantiate(this, transform.position, Quaternion.identity);
		newWaypoint.name = "Waypoint";
		newWaypoint.transform.parent = transform.parent;
		newWaypoint.adyacent = new List<Waypoint>();
		newWaypoint.LinkWaypoint(this);
		Selection.SetActiveObjectWithContext(newWaypoint, newWaypoint);
#endif
	}

	[ExecuteInEditMode]
	public void LinkWaypoint(Waypoint newWaypoint)
	{
		if (!adyacent.Contains(newWaypoint))
			adyacent.Add(newWaypoint);
	}
}
