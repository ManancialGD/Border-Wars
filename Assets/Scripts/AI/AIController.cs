using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class AIController : MonoBehaviour
{
    Rigidbody2D rb;
    public Transform target; // The target the AI should follow
    public float directionLength = 2.0f; // Length of the direction lines for visualization
    public float movementSpeed = 5f; // Movement speed of the AI
    public LayerMask wallLayer; // Layer mask for walls
    Vector2[] directions;
    Vector2 nearestObstacle;
    float[] desirability;

    [SerializeField] int bestDirectionIndex;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        directions = CalculateDirections();
        desirability = new float[16];
    }

    void FixedUpdate()
    {
        Vector2[] obstacles = new Vector2[16];
        for (int i = 0; i < 16; i++)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, directions[i], 100, wallLayer);
            if (hit.collider != null)
            {
                obstacles[i] = hit.point;
            }
        }

        float maxDistanceSquared = obstacles[0].sqrMagnitude; // Start with the squared magnitude for comparison

        for (int i = 1; i < obstacles.Length; i++)
        {
            float distanceSquared = obstacles[i].sqrMagnitude;
            if (distanceSquared > maxDistanceSquared)
            {
                maxDistanceSquared = distanceSquared;
                nearestObstacle = obstacles[i];
            }
        }

        // Calculate desirability for each direction
        desirability = CalculateDesirability(directions);

        // Find the direction with the highest adjusted desirability
        bestDirectionIndex = GetBestDirectionIndex(desirability);

        // Move towards the best direction
        Vector2 moveDirection = directions[bestDirectionIndex];
        rb.velocity = moveDirection * movementSpeed;
    }

    Vector2[] CalculateDirections()
    {
        Vector2[] directions = new Vector2[16];
        for (int i = 0; i < 16; i++)
        {
            float angle = i * 22.5f * Mathf.Deg2Rad;
            directions[i] = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        }
        return directions;
    }

    float[] CalculateDesirability(Vector2[] directions)
    {
        Vector2 targetDirection = (target.position - transform.position).normalized;

        float[] desirability = new float[16];

        for (int i = 0; i < 16; i++)
        {
            float targetDot;
            float distance = Vector3.Distance(target.position, transform.position);
            targetDot = Vector2.Dot(directions[i], targetDirection);

            float sideWeight = 1 - Mathf.Abs(targetDot);

            float runWeight = 1- Mathf.Abs(-targetDot - 0.65f);

            // Linear interpolation values
            float runDistance = 30f;
            float roundDistance = 40f;
            float maxDistance = 50f;

            // Calculate interpolation proportion
            float t = Mathf.InverseLerp(maxDistance, runDistance, distance);
            float t2 = Mathf.InverseLerp(maxDistance, roundDistance, distance);
            float t3 = Mathf.InverseLerp(roundDistance, runDistance, distance);

            // Interpolate x and y values
            float x = Mathf.Lerp(1f, 0f, t2);
            float y = Mathf.Lerp(0f, 1f, t);
            float z = Mathf.Lerp(0f, 1f, t3);

            // Adjust desirability value with runWeight
            float desirabilityValue = x * targetDot + y * sideWeight + z * runWeight;

            desirability[i] = desirabilityValue;
        }
        return desirability;
    }

    int GetBestDirectionIndex(float[] desirability)
    {
        int bestIndex = 0;
        float bestDesirability = desirability[0];
        for (int i = 1; i < desirability.Length; i++)
        {
            if (desirability[i] > bestDesirability)
            {
                bestDesirability = desirability[i];
                bestIndex = i;
            }
        }
        return bestIndex;
    }

    void OnDrawGizmos()
    {
        // Ensure we have a target
        if (target == null) return;

        // Ensure directions and desirability are initialized
        if (directions == null || desirability == null) return;

        // Ensure directions and desirability are of correct length
        if (directions.Length != 16 || desirability.Length != 16) return;

        // Draw the desirability lines
        for (int i = 0; i < 16; i++)
        {
            // Scale the length of the line based on desirability
            float scaledLength = directionLength * desirability[i];

            Vector2 directionEnd = (Vector2)transform.position + directions[i] * scaledLength;

            // Draw the line
            if (i == bestDirectionIndex) Gizmos.color = Color.blue;
            else Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, directionEnd);
        }
    }
}
