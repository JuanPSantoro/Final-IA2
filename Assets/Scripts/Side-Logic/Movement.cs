using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using IA2;

public class Movement : MonoBehaviour
{
    #region VARIABLES
	public Transform body, inventory;

	public event Action<Movement>				OnHitFloor = delegate {};
	public event Action<Movement, Transform>	OnHitWall = delegate {};
	public event Action<Movement, Item>		OnHitItem = delegate {};

    public event Action OnReach = delegate { };
	
	private Vector3 _vel;
	private bool _onFloor;

    [SerializeField]
	private float _speed = 2f;

    [SerializeField]
    private float _gridRadius = 20f;

    Waypoint _gizmoRealTarget;
	IEnumerable<Waypoint> _gizmoPath;

    #endregion

    void Awake()
    {
        _vel = Vector3.zero;
        _onFloor = false;
    }

    #region MOVEMENT & COLLISION
    void FixedUpdate()
    {
        transform.Translate(Time.fixedDeltaTime * _vel * _speed);
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
        var e = other.GetComponent<Movement>();
        if (e != null && e != this)
        {
            Debug.Log(e.name + " hit " + name);
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
			yield return new WaitUntil(() => (FloorPos(destination) - FloorPos(this)).sqrMagnitude < 0.1f);
		}
		
		_vel = Vector3.zero;
        OnReach();
	}

    public Item GetNearestItemOfType(Destination itemDestination)
    {
        return Navigation.instance.GetNearestItem(transform.position + new Vector3(-_gridRadius, 0, -_gridRadius),
            transform.position + new Vector3(_gridRadius, 0, _gridRadius),
            itemDestination,
            x => {
                var position2d = x - transform.position;
                position2d.y = 0;
                return position2d.sqrMagnitude < _gridRadius * _gridRadius;
            });
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, _gridRadius);
    }
}
