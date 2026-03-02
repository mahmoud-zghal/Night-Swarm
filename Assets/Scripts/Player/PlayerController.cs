using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerStats))]
public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private PlayerStats stats;

    [Header("Mobile Touch Joystick")]
    public bool enableMobileTouchJoystick = true;
    public float joystickRadius = 90f;
    public float deadZone = 0.15f;
    public bool drawDebugJoystick = true;

    private Vector2 moveInput;
    private int activeFingerId = -1;
    private Vector2 touchStart;
    private Vector2 touchCurrent;

    private bool IsMobilePlatform =>
        Application.isMobilePlatform || Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        stats = GetComponent<PlayerStats>();
    }

    private void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.IsGameOver) return;

        if (enableMobileTouchJoystick && IsMobilePlatform)
            HandleTouchInput();
        else
            HandleKeyboardInput();
    }

    private void FixedUpdate()
    {
        rb.velocity = moveInput * stats.moveSpeed;
    }

    private void HandleKeyboardInput()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        moveInput = new Vector2(x, y).normalized;
    }

    private void HandleTouchInput()
    {
        if (Input.touchCount == 0)
        {
            activeFingerId = -1;
            moveInput = Vector2.zero;
            return;
        }

        if (activeFingerId == -1)
        {
            // Capture first touch on left side as movement joystick.
            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch t = Input.GetTouch(i);
                if (t.phase == TouchPhase.Began && t.position.x <= Screen.width * 0.6f)
                {
                    activeFingerId = t.fingerId;
                    touchStart = t.position;
                    touchCurrent = t.position;
                    break;
                }
            }
        }

        bool foundActive = false;
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch t = Input.GetTouch(i);
            if (t.fingerId != activeFingerId) continue;

            foundActive = true;
            touchCurrent = t.position;

            if (t.phase == TouchPhase.Ended || t.phase == TouchPhase.Canceled)
            {
                activeFingerId = -1;
                moveInput = Vector2.zero;
            }
            else
            {
                Vector2 delta = (touchCurrent - touchStart) / Mathf.Max(1f, joystickRadius);
                delta = Vector2.ClampMagnitude(delta, 1f);

                if (delta.magnitude < deadZone)
                    moveInput = Vector2.zero;
                else
                    moveInput = delta;
            }
            break;
        }

        if (!foundActive)
        {
            activeFingerId = -1;
            moveInput = Vector2.zero;
        }
    }

    // For external joystick integration if needed.
    public void SetInput(Vector2 input)
    {
        moveInput = input.normalized;
    }

    private void OnGUI()
    {
        if (!drawDebugJoystick || !enableMobileTouchJoystick || !IsMobilePlatform) return;
        if (activeFingerId == -1) return;

        DrawCircle(touchStart, joystickRadius, new Color(1f, 1f, 1f, 0.22f));
        DrawCircle(touchCurrent, joystickRadius * 0.35f, new Color(0.2f, 0.9f, 1f, 0.75f));
    }

    private void DrawCircle(Vector2 center, float radius, Color color)
    {
        Color prev = GUI.color;
        GUI.color = color;
        GUI.DrawTexture(new Rect(center.x - radius, Screen.height - center.y - radius, radius * 2f, radius * 2f), Texture2D.whiteTexture);
        GUI.color = prev;
    }
}
