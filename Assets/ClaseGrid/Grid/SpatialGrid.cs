using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class SpatialGrid : MonoBehaviour
{
    #region Variables
    //punto de inicio de la grilla en X
    public float x;
    //punto de inicio de la grilla en Z
    public float z;
    //ancho de las celdas
    public float cellWidth;
    //alto de las celdas
    public float cellHeight;
    //cantidad de columnas (el "ancho" de la grilla)
    public int width;
    //cantidad de filas (el "alto" de la grilla)
    public int height;

    private Dictionary<GridEntity, Tuple<int, int>> lastPositions;
    private HashSet<GridEntity>[,] buckets;

    readonly public Tuple<int, int> Outside = Tuple.Create(-1, -1);
    readonly public GridEntity[] Empty = new GridEntity[0];
 
    #endregion

    #region FUNCIONES
    private void Awake()
    {
        lastPositions = new Dictionary<GridEntity, Tuple<int, int>>();
        buckets = new HashSet<GridEntity>[width, height];

        for (int i = 0; i < width; i++)
            for (int j = 0; j < height; j++)
                buckets[i, j] = new HashSet<GridEntity>();
        var ents = FindObjectsOfType<GridEntity>().Where(ent => ent.onGrid = true);

        foreach (var e in ents)
        {
            e.OnUpdate += CheckGridPosition;
            CheckGridPosition(e);
            e.OnDestroyElement += DestroyGridElement;
        }
    }

    public void CheckGridPosition(GridEntity entity)
    {
        var lastPos = lastPositions.ContainsKey(entity) ? lastPositions[entity] : Outside;
        var currentPos = GetPositionInGrid(entity.gameObject.transform.position);

        if (lastPos.Equals(currentPos))
            return;

        if (IsInsideGrid(lastPos))
            buckets[lastPos.Item1, lastPos.Item2].Remove(entity);

        if (IsInsideGrid(currentPos))
        {
            buckets[currentPos.Item1, currentPos.Item2].Add(entity);
            lastPositions[entity] = currentPos;
        }
        else
            lastPositions.Remove(entity);
    }

    public IEnumerable<GridEntity> Query(Vector3 fromPosition, Vector3 toPosition, Func<Vector3, bool> filterByPosition)
    {
        var from = new Vector3(Mathf.Min(fromPosition.x, toPosition.x), 0, Mathf.Min(fromPosition.z, toPosition.z));
        var to = new Vector3(Mathf.Max(fromPosition.x, toPosition.x), 0, Mathf.Max(fromPosition.z, toPosition.z));

        var fromCoord = GetPositionInGrid(from);
        var toCoord = GetPositionInGrid(to);

        fromCoord = Tuple.Create(Utility.ClampInt(fromCoord.Item1, 0, width), Utility.ClampInt(fromCoord.Item2, 0, height));
        toCoord = Tuple.Create(Utility.ClampInt(toCoord.Item1, 0, width), Utility.ClampInt(toCoord.Item2, 0, height));

        if (!IsInsideGrid(fromCoord) && !IsInsideGrid(toCoord))
            return Empty;
        
        var cols = Generate(fromCoord.Item1, x => x + 1)
            .TakeWhile(x => x < width && x <= toCoord.Item1);

        var rows = Generate(fromCoord.Item2, y => y + 1)
            .TakeWhile(y => y < height && y <= toCoord.Item2);

        var cells = cols.SelectMany(
            col => rows.Select(
                row => Tuple.Create(col, row)
            )
        );

        return cells
            .SelectMany(cell => buckets[cell.Item1, cell.Item2])
            .Where(gridEnt =>
                from.x <= gridEnt.transform.position.x && gridEnt.transform.position.x <= to.x &&
                from.z <= gridEnt.transform.position.z && gridEnt.transform.position.z <= to.z
            ).Where(selEntity => filterByPosition(selEntity.transform.position));
    }

    public Tuple<int, int> GetPositionInGrid(Vector3 pos)
    {
        return Tuple.Create(Mathf.FloorToInt((pos.x - x) / cellWidth),
                            Mathf.FloorToInt((pos.z - z) / cellHeight));
    }

    public bool IsInsideGrid(Tuple<int, int> gridPosition)
    {
        return 0 <= gridPosition.Item1 && gridPosition.Item1 < width &&
            0 <= gridPosition.Item2 && gridPosition.Item2 < height;
    }

    void OnDestroy()
    {
        var ents = RecursiveWalker(transform).Select(x => x.GetComponent<GridEntity>()).Where(x => x != null);
        foreach (var e in ents)
            e.OnUpdate -= CheckGridPosition;
    }

    void DestroyGridElement(GridEntity ent)
    {
        ent.OnUpdate -= CheckGridPosition;
        var lastPos = lastPositions.ContainsKey(ent) ? lastPositions[ent] : Outside;
        buckets[lastPos.Item1, lastPos.Item2].Remove(ent);
        lastPositions.Remove(ent);
        
    }

    #region GENERATORS
    private static IEnumerable<Transform> RecursiveWalker(Transform parent)
    {
        foreach (Transform child in parent)
        {
            foreach (Transform grandchild in RecursiveWalker(child))
                yield return grandchild;
            yield return child;
        }
    }

    IEnumerable<T> Generate<T>(T seed, Func<T, T> mutate)
    {
        T accum = seed;
        while (true)
        {
            yield return accum;
            accum = mutate(accum);
        }
    }
    #endregion

    #endregion

    #region GRAPHIC REPRESENTATION
    public bool AreGizmosShutDown;
    public bool activatedGrid;
    public bool showLogs = true;
    private void OnDrawGizmos()
    {
        var rows = Generate(z, curr => curr + cellHeight)
                .Select(row => Tuple.Create(new Vector3(x, 0, row),
                                            new Vector3(x + cellWidth * width, 0, row)));

          var cols = Generate(x, curr => curr + cellWidth)
                   .Select(col => Tuple.Create(new Vector3(col, 0, z), new Vector3(col, 0, z + cellHeight * height)));

        var allLines = rows.Take(width + 1).Concat(cols.Take(height + 1));

        foreach (var elem in allLines)
        {
            Gizmos.DrawLine(elem.Item1, elem.Item2);
        }
    }
    #endregion
}
