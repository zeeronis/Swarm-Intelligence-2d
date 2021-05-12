using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmController : MonoBehaviour
{
    [SerializeField] private SwarmAgent afentPrefab;
    [Space]
    [SerializeField] private Vector2 mapSize;
    [SerializeField] private int agentsCount;

    private List<SwarmAgent> agents = new List<SwarmAgent>();


    private void Awake()
    {
        for (int i = 0; i < agentsCount; i++)
        {
            var agent = Instantiate(
                afentPrefab, 
                new Vector3(Random.Range(-mapSize.x, mapSize.x), Random.Range(-mapSize.y, mapSize.y), 0), 
                Quaternion.identity);
            
            agents.Add(agent);
        }

        Time.timeScale = 5;
    }
}
