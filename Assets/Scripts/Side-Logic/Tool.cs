using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool : MonoBehaviour
{
    public Items tool;
    private Transform _toolsContainer;

    public void SetTool(Transform container)
    {
        _toolsContainer = container;
    }

    public void GrabTool(Transform newParent, Vector3 position)
    {
        transform.parent = newParent;
        transform.localPosition = position;
    }

    public void DropTool()
    {
        transform.parent = _toolsContainer;
        transform.localPosition = Vector3.zero;
    }
}
