using UnityEngine;

public class Item : MonoBehaviour
{
    public Destination destination;
	private Waypoint _wp;

	private void Start ()
    {
		_wp = Navigation.instance.NearestTo(transform.position);
		_wp.nearbyItems.Add(this);
	}

	private void OnDestroy()
    {
		if (_wp != null && _wp.nearbyItems != null)
			_wp.nearbyItems.Remove(this);
	}
}
