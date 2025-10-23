using UnityEngine;
using Pathfinding; 
public class EnemyMovement : MonoBehaviour
{
    public AIPath aIPath;

    Vector2 direction;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        faceVelocity();
    }

    void faceVelocity()
    {
        direction = aIPath.desiredVelocity;

        transform.right = direction;
    }
}
