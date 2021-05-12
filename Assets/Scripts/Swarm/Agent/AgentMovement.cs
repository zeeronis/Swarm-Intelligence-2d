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
    private float speed;
    private Vector3 direction;

    public event OnCollideWithObj onCollideWithObj;


    private void Awake()
    {
        speed = Random.Range(speedRange.x, speedRange.y);
        direction = Random.insideUnitCircle.normalized;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Wall"))
        {
            direction = -direction;
        }
        else //if (collision.collider.CompareTag("Obj"))
        {
            onCollideWithObj?.Invoke(collision.gameObject.GetComponent<ObstacleObject>());
        }
    }

    public void Move()
    {
        transform.Translate(direction * speed * Time.deltaTime);
        direction = Quaternion.Euler(0, 0, Random.Range(changeAngleRange.x, changeAngleRange.y)) * direction;
    }

    public void SetDirectionTo(SwarmAgent otherAgent)
    {
        direction = (otherAgent.transform.position - transform.position).normalized;

        Debug.DrawLine(transform.position, otherAgent.transform.position, Color.white, 0.05f);
    }
}
