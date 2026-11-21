using UnityEngine;
using UnityEngine.EventSystems;

public class JoystickController : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [Header("Joystick UI")]
    public RectTransform joystickBackground;
    public RectTransform joystickHandle;
    public float handleLimit = 80f;

    [Header("Player Reference")]
    public PlayerController player;

    private Vector2 inputVector = Vector2.zero;

    void Start()
    {
        if (joystickBackground == null)
        {
            joystickBackground = GetComponent<RectTransform>();
        }

        if (player == null)
        {
            player = FindObjectOfType<PlayerController>();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (joystickBackground == null || joystickHandle == null) return;

        Vector2 localPoint;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                joystickBackground, eventData.position, eventData.pressEventCamera, out localPoint))
        {
            Vector2 radius = joystickBackground.sizeDelta * 0.5f;
            inputVector = new Vector2(localPoint.x / radius.x, localPoint.y / radius.y);

            if (inputVector.magnitude > 1f)
            {
                inputVector = inputVector.normalized;
            }

            // Move the joystick handle knob
            joystickHandle.anchoredPosition = new Vector2(
                inputVector.x * handleLimit,
                inputVector.y * handleLimit);

            if (player != null)
            {
                player.SetJoystickInput(inputVector.x);

                // Allow jumping by flicking the joystick up
                // if (inputVector.y > 0.65f && !upwardJumpTriggered)
                // {
                //     upwardJumpTriggered = true;
                //     player.OnJumpButton();
                // }
                // else if (inputVector.y < 0.3f)
                // {
                //     upwardJumpTriggered = false;
                // }
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        inputVector = Vector2.zero;

        if (joystickHandle != null)
        {
            joystickHandle.anchoredPosition = Vector2.zero;
        }

        if (player != null)
        {
            player.SetJoystickInput(0f);
        }
    }
}
