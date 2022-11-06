using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    [SerializeField]
    private float _energy;
    [SerializeField]
    private int _wood;
    [SerializeField]
    private int _food;
    [SerializeField]
    private string _tool;
    [SerializeField]
    private bool _houseBuilded;
    [SerializeField]
    private int _farms;

    private WorldState _worldState;

    void Start()
    {
        _worldState = new WorldState()
        {
            energy = _energy,
            wood = _wood,
            food = _food,
            tool = _tool,
            houses = _houseBuilded,
            farms = _farms
        };
    }
    
    public WorldState GetCurrentWorldState()
    {
        return _worldState.Clone();
    }

    public void SaveWorldState(WorldState ws)
    {
        _worldState = ws.Clone();

        _energy = ws.energy;
        _food = ws.food;
        _wood = ws.wood;
        _tool = ws.tool;
        _houseBuilded = ws.houses;
        _farms = ws.farms;
    }
}
