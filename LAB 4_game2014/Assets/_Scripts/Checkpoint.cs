using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [Header("Visuals")]
    public Sprite activatedSprite;

    private bool isActivated = false;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isActivated) return;

        if (collision.CompareTag("Player"))
        {
            PlayerRespawn respawn = collision.GetComponent<PlayerRespawn>();
            if (respawn != null)
            {
                isActivated = true;
                respawn.SetCheckpoint(transform.position + Vector3.up * 0.5f);

                if (spriteRenderer != null && activatedSprite != null)
                {
                    spriteRenderer.sprite = activatedSprite;
                }
            }
        }
    }
}
