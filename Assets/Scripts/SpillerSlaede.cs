using UnityEngine;
using UnityEngine.InputSystem;

// Slæden står stille på skærmen og hopper mellem tre faste spor.
// W/S eller pil op/ned skifter spor.
[RequireComponent(typeof(Rigidbody2D))]
public class SpillerSlaede : MonoBehaviour
{
    public float[] sporHoejder = new float[] { 2.4f, 0f, -2.4f };
    public float skifteFart = 12f;
    public int startSpor = 1;

    private Rigidbody2D body;
    private int spor;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        spor = startSpor;
    }

    private void Update()
    {
        Keyboard keyboard = Keyboard.current;
        if (keyboard == null) return;

        if (keyboard.wKey.wasPressedThisFrame || keyboard.upArrowKey.wasPressedThisFrame)
        {
            spor = spor - 1;
        }

        if (keyboard.sKey.wasPressedThisFrame || keyboard.downArrowKey.wasPressedThisFrame)
        {
            spor = spor + 1;
        }

        spor = Mathf.Clamp(spor, 0, sporHoejder.Length - 1);
    }

    private void FixedUpdate()
    {
        float maalHoejde = sporHoejder[spor];
        float afstand = maalHoejde - body.position.y;
        float maksSkridt = skifteFart * Time.fixedDeltaTime;

        if (Mathf.Abs(afstand) <= maksSkridt)
        {
            // Tæt nok på: land præcis på sporet.
            body.linearVelocity = new Vector2(0f, afstand / Time.fixedDeltaTime);
        }
        else
        {
            body.linearVelocity = new Vector2(0f, Mathf.Sign(afstand) * skifteFart);
        }
    }
}
