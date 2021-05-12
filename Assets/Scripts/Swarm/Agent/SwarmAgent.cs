using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmAgent : MonoBehaviour
{
    public AgentMovement movement;
    [Header("Settings")]
    public float shareDist;

    [Header("InGameFields")]
    public int currEndPt = -1;
    public PathData[] paths = new PathData[]
    { 
        new PathData(0),
        new PathData(1),
    };


    public PathData CurrPath => paths[currEndPt];
    public PathData RandomPath => paths[Random.Range(0, paths.Length)];


    private void Awake()
    {
        movement.onCollideWithObj += UpdateEndLocation;

        StartCoroutine(ShareDataCo());
    }

    IEnumerator ShareDataCo()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            for (int i = 0; i < paths.Length; i++)
            {
                paths[i].dist += 1;
            }

            ShareData();
        }
       
    }

    private void ShareData()
    {
        if (currEndPt == -1)
            return;

        foreach (var agent in Physics2D.OverlapCircleAll(movement.transform.position, shareDist))
        {
            agent.GetComponent<SwarmAgent>()?.GetData(this);
        }
    }

    private void UpdateEndLocation(ObstacleObject obstacle)
    {
        paths[obstacle.ID].dist = 0;
        currEndPt = obstacle.ID == 0 ? 1 : 0;

        ShareData();
    }

    public void GetData(SwarmAgent otherAgent)
    {
        var path = otherAgent.RandomPath;

        if (currEndPt == -1)
        {
            currEndPt = path.id;
            paths[currEndPt].dist = path.dist + (int)shareDist;
            movement.SetDirectionTo(otherAgent);
        }

        if (CurrPath.id == path.id && CurrPath.dist > path.dist + (int)shareDist)
        {
            paths[currEndPt].dist = path.dist + (int)shareDist;
            movement.SetDirectionTo(otherAgent); 
        }
    }
}

[System.Serializable]
public struct PathData
{
    public int id;
    public int dist;

    public PathData(int id) : this()
    {
        this.id = id;
        dist = int.MaxValue;
    }
}
