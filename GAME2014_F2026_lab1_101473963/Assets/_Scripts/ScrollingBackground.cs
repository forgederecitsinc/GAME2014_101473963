using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    // How fast the background moves down
    public float scrollSpeed = 3f;
    
    // The Y position where the sprite is completely off the bottom of the screen
    public float bottomEdge = -10f;
    
    // The Y position to teleport the sprite to (exactly above the other sprite)
    public float resetPosition = 10f;

    void Update()
    {
        // Move the sprite straight down every frame
        transform.Translate(Vector3.down * scrollSpeed * Time.deltaTime);

        // Check if the sprite has moved completely past the bottom edge
        if (transform.position.y <= bottomEdge)
        {
            // Snap the sprite back up to the top to create the endless loop
            transform.position = new Vector3(transform.position.x, resetPosition, transform.position.z);
        }
    }
}
