using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : Item
{
    public override void ExecuteAction()
    {
        Navigation.instance.RemoveItem(this);
        Destroy(gameObject);
    }
}
