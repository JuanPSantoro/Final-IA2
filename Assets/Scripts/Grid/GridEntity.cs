using System;
using UnityEngine;

public class GridEntity : MonoBehaviour
{
	public event Action<GridEntity> OnUpdate = delegate {};
    public event Action<GridEntity> OnDestroyElement = delegate { };

    public bool onGrid;

    private void Update()
    {
        OnUpdate.Invoke(this);
    }

    private void OnDestroy()
    {
        OnDestroyElement.Invoke(this);
    }
}
