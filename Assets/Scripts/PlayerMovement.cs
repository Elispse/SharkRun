using System.Collections;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Keyboard input = null;
    private Vector2 moveVector = Vector2.zero;
    private Rigidbody2D rb = null;
    [SerializeField] float moveSpeed = 10f;

    private bool movementEnabled = true;
    private bool letGo = false;
    private Vector2 oldMovement;

    private void Awake()
    {  
        input = new Keyboard();
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        input.Enable();
        input.Player.Movement.performed += OnMovementPerformed;
        input.Player.Movement.canceled += OnMovementCanceled;
    }

    private void OnDisable()
    {
        input.Disable();
        input.Player.Movement.performed -= OnMovementPerformed;
        input.Player.Movement.canceled -= OnMovementCanceled;
    }

    private void FixedUpdate()
    {
        rb.velocity = moveSpeed * moveVector;
        if (movementEnabled) gameObject.GetComponent<SpriteRenderer>().flipX = moveVector.x < 0 ? true : false;
    }

    private void OnMovementPerformed(InputAction.CallbackContext value)
    {
        if (!movementEnabled) 
        {
            letGo = false;
            oldMovement = value.ReadValue<Vector2>();
            return;
        };
        moveVector = value.ReadValue<Vector2>();   
    }

    private void OnMovementCanceled(InputAction.CallbackContext value)
    {
        if (!movementEnabled) 
        {
            letGo = true;
            return;
        };
        moveVector = Vector2.zero;
    }

    IEnumerator ObjectHitCoro()
    {
        if (moveVector != Vector2.zero)
        {
            letGo = false;
            oldMovement = moveVector;
        } else
        {
            letGo = true;
        }

        movementEnabled = false;
        moveVector = new Vector2(-.5f, 0);

        yield return new WaitForSeconds(1);

        if (letGo) moveVector = Vector2.zero;
        else moveVector = oldMovement;

        movementEnabled = true;
    }

    IEnumerator ShowPlayerHit()
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        Color norm = renderer.color;
        Color hitColor = norm;
        hitColor.a = .3f;

        for (int i = 0; i < 4; i++)
        {
            renderer.color = renderer.color == norm ? hitColor : norm;
            yield return new WaitForSeconds(.2f);
        }


    }

    public void OnObstacleHit()
    {
        if (!movementEnabled) return;

        StartCoroutine(ObjectHitCoro());
        StartCoroutine(ShowPlayerHit());
    }

}