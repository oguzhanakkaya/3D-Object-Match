using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgePositioner : MonoBehaviour
{
    public List<Transform> edgePoints;
    public List<GameObject> edges;

    private List<float> positions = new List<float>();

    public LevelManager levelManager;

    void Awake()
    {
        SetEdgePoints();
    }
    void SetEdgePoints()  // Set Invisible Collider Positions
    {

        for (int i = 0; i < edgePoints.Count; i++)
        {
            var mousePos = edgePoints[i].transform.position;
            mousePos.z = 14f;
            var pos = Camera.main.ScreenToWorldPoint(mousePos);

            if (i < 2)
            {
                edges[i].transform.position = new Vector3(edges[i].transform.position.x,
                                                        edges[i].transform.position.y,
                                                        pos.z);
                positions.Add(pos.z);
            }
            else
            {
                edges[i].transform.position = new Vector3(pos.x,
                                                        edges[i].transform.position.y,
                                                        edges[i].transform.position.z);
                positions.Add(pos.x);
            }
               
           

        }
        levelManager.SetBorderList(positions);
    }

}
