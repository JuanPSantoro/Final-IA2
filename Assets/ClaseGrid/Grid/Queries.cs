using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Queries : MonoBehaviour
{
    public float radius = 20f;
    public SpatialGrid targetGrid;
    public IEnumerable<GridEntity> selected = new List<GridEntity>();

    public IEnumerable<GridEntity> Query()
    {
        //creo una "caja" con las dimensiones deseadas, y luego filtro segun distancia para formar el círculo
        return targetGrid.Query(
            transform.position + new Vector3(-radius, 0, -radius),
            transform.position + new Vector3(radius, 0, radius),
            x => {
                var position2d = x - transform.position;
                position2d.y = 0;
                return position2d.sqrMagnitude < radius * radius;
            });
    }

    void OnDrawGizmos()
    {
        if (targetGrid == null)
            return;

        Gizmos.color = Color.cyan;
        Gizmos.matrix *= Matrix4x4.Scale(Vector3.forward + Vector3.right);
        Gizmos.DrawWireSphere(transform.position, radius);

        if (Application.isPlaying)
        {
            selected = Query();
            var temp = FindObjectsOfType<GridEntity>().Where(x=>!selected.Contains(x));
            foreach (var item in temp)
            {
                item.onGrid = false;
            }
            foreach (var item in selected)
            {
                item.onGrid = true;
            }

        }
    }
}
