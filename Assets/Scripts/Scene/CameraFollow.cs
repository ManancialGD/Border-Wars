using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class CameraFollow : MonoBehaviour
{
    private Transform target;
    Rigidbody2D rb;
    private Vector3 velocity = Vector3.zero;

    [SerializeField] private bool cameraShaking;
    [SerializeField] private float cameraShakeCooldown = 1;
    [SerializeField] private float cameraShakeResistence = 2;

    [Range(0, 1)]
    public float smoothTime = 0.3f;
    public Vector3 positionOffset;

    [Header("Limitation")]
    [SerializeField] private bool hasLimits;
    [SerializeField, ShowIf(nameof(hasLimits))] private Vector2 xLimit; // Vector2(Xmin, Xmax)
    [SerializeField, ShowIf(nameof(hasLimits))] private Vector2 yLimit; // Vector2(Ymin, Ymax)

    [SerializeField, ShowIf(nameof(hasLimits))] Color limitsDebugColour = Color.blue;

    Camera cam;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (target == null) return;

        FollowTarget(target);
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    /// <summary>
    /// This will follow the target with a smooth motion and keep the camera within specified limits.
    /// It calculates the target position and applies smoothing to the camera movement. If limits are set,
    /// it clamps the target position within these limits before applying the smooth motion.
    /// </summary>
    private void FollowTarget(Transform target)
    {
        Vector3 targetPosition = target.position + positionOffset;
        if (hasLimits)
        {
            // Clamp the target position within the specified limits
            float clampedX = Mathf.Clamp(targetPosition.x, xLimit.x, xLimit.y);
            float clampedY = Mathf.Clamp(targetPosition.y, yLimit.x, yLimit.y);
            float clampedZ = transform.position.z; // Maintain current z-position

            Vector3 clampedPosition = new Vector3(clampedX, clampedY, clampedZ);

            // Smoothly move the camera towards the clamped position
            transform.position = Vector3.SmoothDamp(transform.position, clampedPosition, ref velocity, smoothTime);
        }
        else
        {
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        }
    }

    /// <summary>
    /// This initiates a camera shake effect by setting relevant properties and starting coroutines to handle
    /// the shake duration and interval.
    /// </summary>
    public void CameraShake(float shakeTime, float shakeInterval, float shakeResistence, Vector2 velocityIntensity)
    {
        if (!cameraShaking) cameraShaking = true;
        cameraShakeCooldown = shakeTime;
        cameraShakeResistence = shakeResistence;

        StartCoroutine(SetCameraShakeProperties(0f, shakeInterval, velocityIntensity));
        StartCoroutine(FinishCameraShake(shakeTime));
        return;
    }

    /// <summary>
    /// This coroutine manages the properties of the camera shake effect, alternating the direction
    /// and intensity of the shake based on the given interval.
    /// </summary>
    private IEnumerator SetCameraShakeProperties(float waitTime, float shakeInterval, Vector2 velocityIntensity)
    {
        yield return new WaitForSeconds(waitTime);

        Debug.Log("Camera Shake started");

        int num = 1;

        Vector2 currentDisplacement = new Vector2(0f, 0f);

        while (cameraShaking)
        {
            if (cameraShakeCooldown <= 0f)
            {
                cameraShaking = false;
                cameraShakeCooldown = 0f;
                break;
            }

            currentDisplacement += num * velocityIntensity;
            rb.velocity += num * velocityIntensity;
            num = -num;
            yield return new WaitForSeconds(shakeInterval);
        }

        rb.velocity -= currentDisplacement;

        Debug.Log("Camera Shake finished");
    }

    /// <summary>
    /// This coroutine ends the camera shake effect after a specified duration.
    /// </summary>
    private IEnumerator FinishCameraShake(float cooldown)
    {
        yield return new WaitForSeconds(cooldown);
        cameraShakeCooldown = 0f;
        cameraShaking = false;
    }

    public void SetHasLimits(bool b)
    {
        hasLimits = b;
    }

    /// <summary>
    /// This will draw a box with the limits on the scene view for debugging purposes.
    /// It calculates the center and size of the box based on the camera's orthographic size and aspect ratio.
    /// </summary>
    private void OnDrawGizmos()
    {
        if (!hasLimits) return;
        if (cam == null)
        {
            cam = GetComponent<Camera>();
            if (cam == null) return;
        }

        Gizmos.color = limitsDebugColour;

        // Calculate the center and size of the rectangle, adjusted for camera size
        float camHeight = cam.orthographicSize * 2;
        float camWidth = camHeight * cam.aspect;

        Vector3 center = new Vector3((xLimit.x + xLimit.y) / 2, (yLimit.x + yLimit.y) / 2, transform.position.z);
        Vector3 size = new Vector3(xLimit.y - xLimit.x + camWidth, yLimit.y - yLimit.x + camHeight, 1);

        // Draw the wireframe cube
        Gizmos.DrawWireCube(center, size);
    }
}