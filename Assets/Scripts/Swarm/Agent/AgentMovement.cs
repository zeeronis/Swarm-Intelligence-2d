using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void OnCollideWithObj(ObstacleObject obj);

public class AgentMovement : MonoBehaviour
{
    public Transform transform;
    
    [Header("Settings")]
    public Vector2 speedRange;
    public Vector2 changeAngleRange;
    
    [Header("Fields")]
    public float speed;
    public Vector3 direction;

    public event OnCollideWithObj onCollideWithObj;


    private void Awake()
    {
        speed = Random.Range(speedRange.x, speedRange.y);
        direction = Random.insideUnitCircle.normalized;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        direction = -direction;

        if (collision.collider.CompareTag("Obj"))
        {
            onCollideWithObj?.Invoke(collision.gameObject.GetComponent<ObstacleObject>());
        }
    }

    private void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
        direction = Quaternion.Euler(0, 0, Random.Range(changeAngleRange.x, changeAngleRange.y)) * direction;
    }

    internal void SetDirectionTo(SwarmAgent otherAgent)
    {
        direction = (otherAgent.transform.position - transform.position).normalized;

        Debug.DrawLine(transform.position, otherAgent.transform.position, Color.white, 0.05f);
    }
}
