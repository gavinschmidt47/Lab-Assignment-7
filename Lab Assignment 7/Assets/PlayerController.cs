using NumericsVector2 = System.Numerics.Vector2;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;
using Unity.VisualScripting;

public class PlayerController : MonoBehaviour
{
    //Input variables
    public InputActionAsset playerControls;
    private InputAction movement;
    private InputAction look;
    private UnityEngine.Vector2 move;
    private UnityEngine.Vector2 lookDir;
    private UnityEngine.Vector3 moveDirection;

    //Player Components
    private CharacterController controller;

    //Enemy Components
    public NavMeshAgent agent;

    //Player variables
    public float speed = 5.0f;
    public float lookSpeed = 2.0f;
    
    void OnEnable()
    {
        movement = playerControls.FindActionMap("Player").FindAction("Move");
        look = playerControls.FindActionMap("Player").FindAction("Look");
        
        movement.Enable();
        look.Enable();
    }
    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (controller == null)
        {
            Debug.LogError("CharacterController component not found on the player.");
        }
        
        //Disable the cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        move = movement.ReadValue<Vector2>().normalized;
        lookDir = look.ReadValue<Vector2>();
        // Rotate the player
        transform.Rotate(new Vector3(0, lookDir.x, 0) * lookSpeed);

        // Convert move direction to be relative to the player's facing direction
        moveDirection = transform.right * move.x + transform.forward * move.y;
    }

    void FixedUpdate()
    {
        // Move the player
        controller.SimpleMove(moveDirection * speed);

        // Move the enemy
        if (agent != null)
        {
            agent.SetDestination(transform.position + new UnityEngine.Vector3(moveDirection.x, 0, moveDirection.z) * 5);
        }
    }
}
