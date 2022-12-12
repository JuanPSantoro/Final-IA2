using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class Navigation : MonoBehaviour
{
    public static Navigation instance;
    private List<Waypoint> _waypoints = new List<Waypoint>();
    private List<Item> _allItems = new List<Item>();
    private SpatialGrid _grid => GetComponent<SpatialGrid>();
	void Start ()
    {
        if (instance != null)
            Destroy(gameObject);

        instance = this;

		foreach(Transform xf in transform)
        {
			var wp = xf.GetComponent<Waypoint>();
			if(wp != null)
				_waypoints.Add(wp);
		}

        _allItems = FindObjectsOfType<Item>().ToList();
	}

	public bool Reachable(Vector3 from, Vector3 to)
    {
		var srcWp = NearestTo(from);
		var dstWp = NearestTo(to);

		Waypoint wp = srcWp;

		if(srcWp != dstWp) {
			var path = AStarNormal<Waypoint>.Run(
				  srcWp
				, dstWp
				, (wa, wb) => Vector3.Distance(wa.transform.position, wb.transform.position)
				, w => w == dstWp
				, w =>
					w.adyacent.Select(a => new AStarNormal<Waypoint>.Arc(a, Vector3.Distance(a.transform.position, w.transform.position)))
			);
			if(path == null)
				return false;

			wp = path.Last();
		}
		Debug.Log("Reachable from " + wp.name);

		var delta = (to - wp.transform.position);
		var distance = delta.magnitude;

		return !Physics.Raycast(wp.transform.position, delta/distance, distance, LayerMask.GetMask(new []{"Blocking"}));
	}

	public IEnumerable<Waypoint> All() {
		return _waypoints;
	}

	public Waypoint Random() {
		return _waypoints[UnityEngine.Random.Range(0, _waypoints.Count)];
	}

	public Waypoint NearestTo(Vector3 pos) {
		return All()
			.OrderBy(wp => {
				var d = wp.transform.position - pos;
				d.y = 0;
				return d.sqrMagnitude;
			})
			.First();
	}

    private IEnumerable<Item> GetItemsOfType(Destination destination)
    {
        return _allItems.Where(item => item.destination == destination).ToList();
    }

    public Item GetNearestItem(Vector3 from, Destination destination)
    {
        var queredList = _grid.Query(transform.position + new Vector3(-20, 0, -20),
            transform.position + new Vector3(20, 0, 20),
            x =>
            {
                var position2d = x - transform.position;
                position2d.y = 0;
                return position2d.sqrMagnitude < 20 * 20;
            }).Where(x =>
            {
                var itemType = GetComponent<Item>();
                return itemType != null && itemType.destination == destination;
            });
        if (queredList.Any())
        {
            Debug.Log("FOUND USING GRID");
            return queredList.OrderBy(x => Vector3.SqrMagnitude(x.transform.position - from)).FirstOrDefault().GetComponent<Item>();
        }

        var filteredItems = GetItemsOfType(destination);
        var item = filteredItems.OrderBy(x => Vector3.SqrMagnitude(x.transform.position - from)).FirstOrDefault();
        return item;
    }

    public void RemoveItem(Item item)
    {
        if (_allItems.Contains(item))
            _allItems.Remove(item);
    }

    public void AddItem(Item item)
    {
        if (!_allItems.Contains(item))
            _allItems.Add(item);
    }
}
