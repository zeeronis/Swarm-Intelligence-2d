using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentController : MonoBehaviour
{
    public AgentMovement movement;
   
    [Header("Settings")]
    [SerializeField] private float shareDist;

    private int currEndPt;
    private SharedData[] paths;

    public SharedData RandomPath => paths[Random.Range(0, paths.Length)];

    private void Awake()
    {
        movement.OnCollideWithObj += OnReachEndPt;

        currEndPt = -1;
        paths = new SharedData[]
        {
            new SharedData(0),
            new SharedData(1),
        };
    }

    private void OnReachEndPt(ObstacleObject obstacle)
    {
        paths[obstacle.ID].dist = 0;
        currEndPt = obstacle.ID == 0 ? 1 : 0;

        //ShareData();
    }

    public void IncreacePathsLength()
    {
        for (int i = 0; i < paths.Length; i++)
        {
            if (paths[i].dist < int.MaxValue)
                paths[i].dist++;
        }
    }

    private static RaycastHit2D[] wallRaycast = new RaycastHit2D[1];
    public void ShareData(ref Collider2D[] colliders)
    {
        if (currEndPt == -1)
            return;

        var collidersCount = Physics2D.OverlapCircle(
            movement.transform.position, 
            shareDist, SwarmController.AgentFilter, colliders);
       
        for (int i = 0; i < collidersCount; i++)
        {
            if (SwarmController.CheckWalls)
            {
                /*int count = Physics2D.LinecastNonAlloc(
                    movement.transform.position, 
                    colliders[i].transform.position, 
                    wallRaycast, SwarmController.WallFilter.layerMask);*/

                var ss = Physics2D.Linecast(movement.transform.position, colliders[i].transform.position, SwarmController.WallFilter.layerMask);
                if (ss.collider == null)
                {
                    colliders[i].GetComponent<AgentController>().RecieveData(this);
                }
            }
            else
            {
                colliders[i].GetComponent<AgentController>().RecieveData(this);
            }
        }
    }

    public void RecieveData(AgentController otherAgent)
    {
        SharedData data = otherAgent.RandomPath;
        int sharedPathDist = data.dist + (int)shareDist;

        if (currEndPt == -1)
            currEndPt = data.pathID;

        if (paths[currEndPt].pathID == data.pathID && paths[currEndPt].dist > sharedPathDist)
        {
            paths[currEndPt].dist = sharedPathDist;
            movement.SetDirectionTo(otherAgent); 
        }
    }
}
