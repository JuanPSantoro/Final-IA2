using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CityManager : MonoBehaviour
{
    [SerializeField]
    private List<Item> _houses;
    [SerializeField]
    private List<Item> _farms;

    public bool IsHouseBuilded()
    {
        return _houses.Any(house => house.destination == Destination.HOUSE);
    }

    public int FarmsAmount()
    {
        return _farms.Count(farm => farm.destination == Destination.FARM);
    }
}
