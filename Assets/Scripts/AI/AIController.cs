using System.Collections;
using UnityEngine;

public class AIController : MonoBehaviour
{
    [SerializeField] Animator anim;
    EnemyHealth hp;
    [SerializeField] Transform weaponRotator;
    Rigidbody2D rb;
    public Transform target; // The target the AI should follow
    public float distanceToTarget;
    public float directionLength = 2.0f; // Length of the direction lines for visualization
    public float movementSpeed = 55f; // Movement speed of the AI
    [SerializeField] private float persueMovementSpeed;
    [SerializeField] private float attackMovementSpeed;
    [SerializeField] private float runMovementSpeed;
    public LayerMask wallLayer; // Layer mask for walls
    public LayerMask playerLayer;
    Vector2[] directions;
    float[] desirability;
    float[] undesirability;
    [SerializeField] Transform weaponPos;

    public bool IsAttacking { get; private set; }

    public enum AiState { idle, persuing, attacking, running }

    private AiState currentState;
    [SerializeField] int bestDirectionIndex;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        hp = GetComponent<EnemyHealth>();

        directions = CalculateDirections();
        desirability = new float[16];
        currentState = AiState.idle;
        IsAttacking = false;
    }

    void Update()
    {
        if (IsAttacking)
        {
            Collider2D hitcollider = Physics2D.OverlapCircle(weaponPos.position, 13f, playerLayer);

            if (hitcollider != null)
            {
                CharacterHP playerHP = hitcollider.GetComponent<CharacterHP>();
                if (playerHP != null)
                {
                    playerHP.Damage(1, transform.position, 100);
                }
            }
        }
    }
    void FixedUpdate()
    {
        if (hp.IsStunned)
        {
            currentState = AiState.persuing;
            return;
        }
        if (!(currentState == AiState.idle)) distanceToTarget = Vector3.Distance(transform.position, target.position);
        if (currentState == AiState.idle)
        {
            Collider2D hitCollides = Physics2D.OverlapCircle(transform.position, 85f, playerLayer);

            if (hitCollides != null)
            {
                CharacterMovement characterMovement = hitCollides.GetComponent<CharacterMovement>();
                if (characterMovement != null)
                {
                    target = hitCollides.transform;
                    currentState = AiState.persuing;
                }
            }
            return;
        }
        else if (currentState == AiState.persuing)
        {
            anim.SetBool("Attack", false);
            movementSpeed = persueMovementSpeed;
            if (distanceToTarget < 55f && distanceToTarget > 45f)
            {
                StartCoroutine(AttackInRandonTime());
            }
            else if (distanceToTarget < 45) StartCoroutine(AttackInRandonTime());
            else if (distanceToTarget > 65f) movementSpeed = runMovementSpeed;
            else
            {
                StopAllCoroutines();
            }

        }
        else if (currentState == AiState.attacking)
        {
            movementSpeed = attackMovementSpeed;
            if (distanceToTarget < 22)
            {
                anim.SetBool("Attack", true);
                currentState = AiState.running;
            }
            if (IsAttacking)
            {
                anim.SetBool("Attack", false);
                currentState = AiState.running;
            }
        }
        else if (currentState == AiState.running)
        {
            movementSpeed = runMovementSpeed;
            anim.SetBool("Attack", false);
            if (distanceToTarget > 65f)
            {
                currentState = AiState.persuing;
            }
        }

        Vector3 direction = target.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        weaponRotator.rotation = Quaternion.Euler(0, 0, angle);

        // Calculate desirability for each direction
        desirability = CalculateDesirability(directions);

        // Find the direction with the highest adjusted desirability
        bestDirectionIndex = GetBestDirectionIndex(desirability);

        // Move towards the best direction
        Vector2 moveDirection = directions[bestDirectionIndex];

        rb.velocity = moveDirection * movementSpeed;
    }
    IEnumerator AttackInRandonTime()
    {
        yield return new WaitForSeconds(Random.Range(1, 4));
        currentState = AiState.attacking;
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

    public void SetIsAttacking(bool b)
    {
        IsAttacking = b;
    }

    float[] CalculateDesirability(Vector2[] directions)
    {
        Vector2 targetDirection = (target.position - transform.position).normalized;

        float[] desirability = new float[16];
        float minDesirability = float.MaxValue;
        float maxDesirability = float.MinValue;

        for (int i = 0; i < 16; i++)
        {
            float dotWithTargetDirection = Vector2.Dot(directions[i], targetDirection);

            float sideAvoidance = 1 - Mathf.Abs(dotWithTargetDirection);

            float sideRunAvoidance = 1 - Mathf.Abs(-dotWithTargetDirection - 0.65f);

            float semiRoundAvoidance = 1 - Mathf.Abs(dotWithTargetDirection - 0.65f);

            float alignmentWithCurrentVelocity = Vector2.Dot(directions[i], rb.velocity.normalized);

            // Maximum distances for interpolation
            float maxRunDistance = 30f;
            float maxSideRunDistance = 40f;
            float maxRoundDistance = 50f;
            float maxSemiRoundDistance = 60f;
            float maxOverallDistance = 70f;

            // Calculate interpolation proportion for distances
            float sideAvoidanceProportion = Mathf.InverseLerp(maxSemiRoundDistance, maxRoundDistance, distanceToTarget);
            float sideAvoidanceProportion2 = Mathf.InverseLerp(maxRoundDistance, maxRunDistance, distanceToTarget);

            float targetProximityProportion = Mathf.InverseLerp(maxOverallDistance, maxSemiRoundDistance, distanceToTarget);

            float sideRunAvoidanceProportion = Mathf.InverseLerp(maxRoundDistance, maxSideRunDistance, distanceToTarget);
            float sideRunAvoidanceProportion2 = Mathf.InverseLerp(maxSideRunDistance, maxRunDistance, distanceToTarget);

            float semiRoundAvoidanceProportion = Mathf.InverseLerp(maxOverallDistance, maxSemiRoundDistance, distanceToTarget);
            float semiRoundAvoidanceProportion2 = Mathf.InverseLerp(maxSemiRoundDistance, maxRoundDistance, distanceToTarget);

            // Weights for each component
            float targetWeight = Mathf.Lerp(1f, 0f, targetProximityProportion);

            float sideWeight = Mathf.Lerp(0f, 1f, sideAvoidanceProportion);
            float sideWeight2 = Mathf.Lerp(1f, 0f, sideAvoidanceProportion2);

            float sideRunWeight = Mathf.Lerp(0f, 1f, sideRunAvoidanceProportion);
            float sideRunWeight2 = Mathf.Lerp(0f, 1f, sideRunAvoidanceProportion2);

            float semiRoundWeight = Mathf.Lerp(0f, 1f, semiRoundAvoidanceProportion);
            float semiRoundWeight2 = Mathf.Lerp(1f, 0f, semiRoundAvoidanceProportion2);

            float bestDirectionAlignmentWeight = .3f;

            // Adjust desirability value with the weights
            float desirabilityValue = 0;

            if (currentState == AiState.persuing)
            {
                desirabilityValue = targetWeight * dotWithTargetDirection;
                desirabilityValue += sideWeight2 * (sideWeight * sideAvoidance);

                desirabilityValue += sideRunWeight2 * (sideRunWeight * sideRunAvoidance);

                desirabilityValue += semiRoundWeight2 * (semiRoundWeight * semiRoundAvoidance);

                desirabilityValue += bestDirectionAlignmentWeight * alignmentWithCurrentVelocity;
            }
            else if (currentState == AiState.attacking)
            {
                desirabilityValue = dotWithTargetDirection * 0.6f;
                desirabilityValue += (1 - Mathf.Abs(dotWithTargetDirection - 0.65f)) * 0.4f;
            }
            else if (currentState == AiState.running)
            {
                desirabilityValue = -dotWithTargetDirection;
            }

            desirability[i] = desirabilityValue;

            // Update min and max desirability values
            if (desirabilityValue < minDesirability) minDesirability = desirabilityValue;
            if (desirabilityValue > maxDesirability) maxDesirability = desirabilityValue;
        }

        // Calculate obstacle avoidance
        undesirability = CalculateUndesirability(directions);

        // Combine desirability and undesirability
        for (int i = 0; i < 16; i++)
        {
            desirability[i] = desirability[i] / 2 - undesirability[i];
        }

        // Normalize desirability values
        for (int i = 0; i < 16; i++)
        {
            desirability[i] = (desirability[i] - minDesirability) / (maxDesirability - minDesirability);
        }

        return desirability;
    }

    float[] CalculateUndesirability(Vector2[] directions)
    {
        float[] undesirability = new float[16];

        for (int i = 0; i < 16; i++)
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, directions[i], 25, wallLayer);

            foreach (var hit in hits)
            {
                if (hit.collider != null)
                {
                    for (int j = 0; j < 16; j++)
                    {
                        float distance = Vector3.Distance(transform.position, hit.point);
                        Vector2 obstacleDirection = (hit.point - (Vector2)transform.position).normalized;
                        float dotWithObstacle = Vector2.Dot(directions[j], obstacleDirection);

                        // Shape that reduces desirability if direction is aligned with an obstacle
                        // float avoidanceShape = 1 - Mathf.Abs(dotWithObstacle - 0.65f);
                        float avoidanceShape = dotWithObstacle;

                        // The closer the obstacle, the higher the undesirability
                        float distanceFactor = Mathf.Lerp(1f, 0f, distance / 40f);

                        undesirability[j] += distanceFactor * avoidanceShape;
                    }
                }
            }
        }

        return undesirability;
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
