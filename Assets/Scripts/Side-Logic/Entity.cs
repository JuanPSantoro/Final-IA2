using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using IA2;

public class Entity : MonoBehaviour
{
    #region VARIABLES
	public Transform body, inventory;

	public event Action<Entity>				OnHitFloor = delegate {};
	public event Action<Entity, Transform>	OnHitWall = delegate {};
	public event Action<Entity, Item>		OnHitItem = delegate {};
	public event Action<Entity, Waypoint, bool>	OnReachDestination = delegate {};
    public event Action OnReach = delegate { };

	public List<Item> initialItems;
	
	List<Item> _items;
	Vector3 _vel;
	bool _onFloor;

	public float speed = 2f;

	Waypoint _gizmoRealTarget;
	IEnumerable<Waypoint> _gizmoPath;

    #region GETTERS & SETTERS
    public IEnumerable<Item> items { get { return _items; } }
    #endregion

    #endregion

    void Awake()
    {
        _items = new List<Item>();
        _vel = Vector3.zero;
        _onFloor = false;
    }

    void Start()
    {
        foreach (var it in initialItems)
            AddItem(Instantiate(it));
    }

    #region MOVEMENT & COLLISION
    void FixedUpdate()
    {
        transform.Translate(Time.fixedDeltaTime * _vel * speed);
    }

    public void Jump()
    {
        if (_onFloor)
        {
            _onFloor = false;
            GetComponent<Rigidbody>().AddForce(Vector3.up * 3f, ForceMode.Impulse);
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.collider.tag == "Floor")
        {
            _onFloor = true;
            OnHitFloor(this);
        }
        else if (col.collider.tag == "Wall")
            OnHitWall(this, col.collider.transform);
        else
        {
            var item = col.collider.GetComponentInParent<Item>();
            if (item && item.transform.parent != inventory)
                OnHitItem(this, item);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        var e = other.GetComponent<Entity>();
        if (e != null && e != this)
        {
            Debug.Log(e.name + " hit " + name);
        }
    }
    #endregion

    #region ITEM MANAGEMENT
    public void AddItem(Item item) {
		_items.Add(item);
		item.OnInventoryAdd();
		item.transform.parent = inventory;
		RefreshItemPositions();
	}

	public Item Removeitem(Item item) {
		_items.Remove(item);
		item.OnInventoryRemove();
		item.transform.parent = null;
		RefreshItemPositions();
		return item;
	}

	public IEnumerable<Item> RemoveAllitems() {
		var ret = _items;
		foreach(var item in items) {
			item.OnInventoryRemove();
		}
		_items = new List<Item>();
		RefreshItemPositions();
		return ret;
	}

    void RefreshItemPositions()
    {
        const float Dist = 1.25f;
        for (int i = 0; i < _items.Count; i++)
        {
            var phi = (i + 0.5f) * Mathf.PI / (_items.Count);
            _items[i].transform.localPosition = new Vector3(-Mathf.Cos(phi) * Dist, Mathf.Sin(phi) * Dist, 0f);
        }
    }
    #endregion

    Vector3 FloorPos(MonoBehaviour b) {
		return FloorPos(b.transform.position);
	}
	Vector3 FloorPos(Vector3 v) {
		return new Vector3(v.x, 0f, v.z);
	}

	Coroutine _navCR;
	public void GoTo(Vector3 destination) {
		_navCR = StartCoroutine(Navigate(destination));
	}

	public void Stop() {
		if(_navCR != null) StopCoroutine(_navCR);
		_vel = Vector3.zero;
	}

	protected virtual IEnumerator Navigate(Vector3 destination)
    {
		var srcWp = Navigation.instance.NearestTo(transform.position);
		var dstWp = Navigation.instance.NearestTo(destination);
		
		_gizmoRealTarget = dstWp;
		Waypoint reachedDst = srcWp;

		if(srcWp != dstWp)
        {
			var path = _gizmoPath = AStarNormal<Waypoint>.Run(
				  srcWp
				, dstWp
				, (wa, wb) => Vector3.Distance(wa.transform.position, wb.transform.position)
				, w => w == dstWp
				, w =>
					w.adyacent
					.Select(a => new AStarNormal<Waypoint>.Arc(a, Vector3.Distance(a.transform.position, w.transform.position)))
			);
			if (path != null) {
                var floorPosList = path.Select(w => w.transform.position).ToList();
				foreach(var next in floorPosList) {
                    while((next - transform.position).sqrMagnitude >= 0.05f) {
						_vel = (next - transform.position).normalized;
						yield return null;
					}
				}
			}
			reachedDst = path.Last();
		}

		if(reachedDst == dstWp) {
			_vel = (FloorPos(destination) - FloorPos(this)).normalized;
			yield return new WaitUntil(() => (FloorPos(destination) - FloorPos(this)).sqrMagnitude < 0.05f);
		}
		
		_vel = Vector3.zero;
        OnReachDestination(this, reachedDst, reachedDst == dstWp);
        OnReach();
	}
}
