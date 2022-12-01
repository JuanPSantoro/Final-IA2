using System;
using System.Collections;
using UnityEngine;

public class PlayerModel : MonoBehaviour
{
    public Action OnRestEnd;

    public void Rest()
    {
        DoRest();
    }

    private IEnumerator DoRest()
    {
        yield return new WaitForSeconds(3);
        OnRestEnd();
    }

    public void RecoverEnergy()
    {
        
    }
}
