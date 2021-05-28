using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmController : MonoBehaviour
{
    public static ContactFilter2D AgentFilter;
    public static ContactFilter2D WallFilter;
    public static bool CheckWalls;

    [Header("Prefabs")]
    [SerializeField] private AgentController afentPrefab;

    [Header("Map settings")]
    [SerializeField] private Vector2 mapSize;

    [Header("Agent settings")]
    [SerializeField] private bool checkWalls;
    [SerializeField] private int agentsCount;
    [SerializeField] private int overlapCollidersCount;

    [Header("Game settings")]
    [SerializeField] [Range(0, 10)] private float timescale;


    private Collider2D[] overlapColliders;
    private List<AgentController> agents = new List<AgentController>();


    private void Awake()
    {
        CheckWalls = checkWalls;

        overlapColliders = new Collider2D[overlapCollidersCount];
        for (int i = 0; i < agentsCount; i++)
        {
            var agent = Instantiate(
                afentPrefab, 
                new Vector3(Random.Range(-mapSize.x, mapSize.x), Random.Range(-mapSize.y, mapSize.y), 0), 
                Quaternion.identity);
            
            agents.Add(agent);
        }

        AgentFilter = new ContactFilter2D()
        {
            useLayerMask = true,
            layerMask = 1 << LayerMask.NameToLayer("Agent"),
            minDepth = -0.01f,
            maxDepth = 0.01f,
        };
        WallFilter = new ContactFilter2D()
        {
            useLayerMask = true,
            layerMask = 1 << LayerMask.NameToLayer("Wall"),
            minDepth = -0.01f,
            maxDepth = 0.01f,
        };

        StartCoroutine(ShareAgentsData());
    }

    private void Update()
    {
        Time.timeScale = timescale;
        for (int i = 0; i < agents.Count; i++)
        {
            agents[i].movement.Move();
        }
    }

    private IEnumerator ShareAgentsData()
    {
        yield return null;

        int index = 0;
        while (true)
        {
            yield return new WaitForFixedUpdate();
            for (int i = 0; i < agentsCount / 50; i++)
            {
                index = (index + 1) % agentsCount;
                agents[index].ShareData(ref overlapColliders);
                agents[index].IncreacePathsLength();
            }

            //  yield return new WaitForSeconds(1f);
            /* foreach (var agent in agents)
             {
                 agent.ShareData(ref overlapColliders);
                 agent.IncreacePathsLength();
             }*/
        }
    }
}
