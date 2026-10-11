using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 10f;
    
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        // Only do something if a finger is touching the screen
        if (Input.touchCount > 0)
        {
            // Get the first finger that touched the screen
            Touch touch = Input.GetTouch(0);

            // Turn the screen pixels into world coordinates so the player knows where to go
            Vector3 targetPosition = mainCamera.ScreenToWorldPoint(touch.position);
            
            // Keep the z-axis exactly the same so the 2D sprite doesn't vanish behind the camera
            targetPosition.z = transform.position.z;

            // Move step-by-step towards the target so it doesn't just teleport instantly
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
    }
}
