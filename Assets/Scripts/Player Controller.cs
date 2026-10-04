using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 4f;
    public float lookSpeed = 4f;
    public float jumpStrength = 4f;
    public float crouchHeight = 0.6f;
    public float attackDist = 10f;
    public float lungeForce = 10f;
    public float airControl = 0.5f;
    public float airControlThreshold = 10f;
    public float groundFriction = 1.0f;
    public GameObject attackObject;
    public Camera camera;
    private CameraController m_cameraController;
    private InputAction m_move;
    private InputAction m_look;
    private InputAction m_jump;
    private InputAction m_crouch;
    private InputAction m_sprint;
    private InputAction m_attack;
    private Vector2 m_moveVal;
    private Vector2 m_lookVal;
    private Rigidbody m_rigidBody;
    private CapsuleCollider m_collider;
    private bool m_isGrounded;
    private bool m_isCrouching = false;
    private bool m_isAttacking = false;
    private float m_colliderCrouchDist;
    private float m_cameraCrouchDist;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_move = InputSystem.actions.FindAction("Move");
        m_look = InputSystem.actions.FindAction("Look");
        m_jump = InputSystem.actions.FindAction("Jump");
        m_crouch = InputSystem.actions.FindAction("Crouch");
        m_sprint = InputSystem.actions.FindAction("Sprint");
        m_attack = InputSystem.actions.FindAction("Attack");
        m_rigidBody = GetComponent<Rigidbody>();
        m_collider = GetComponent<CapsuleCollider>();
        m_cameraController = camera.GetComponent<CameraController>();

        float centerHeight = m_collider.height / 2;
        m_colliderCrouchDist = (1 - crouchHeight) * centerHeight;
        m_cameraCrouchDist = (1 - crouchHeight) * (centerHeight + camera.transform.localPosition.y);
    }

    // Update is called once per frame
    void Update()
    {
        m_moveVal = m_move.ReadValue<Vector2>();
        m_lookVal = m_look.ReadValue<Vector2>();

        Rotating();

        if (m_jump.WasPressedThisFrame() && m_isGrounded)
        {
            Jump();
        }

        if (m_attack.WasPressedThisFrame() && !m_isAttacking)
        {
            Attack();
        }

        if (m_sprint.WasPressedThisFrame())
        {
            Dash();
        }

        if (m_crouch.WasPressedThisFrame() || m_crouch.WasReleasedThisFrame())
        {
            ToggleCrouch();
        }
    }

    private void FixedUpdate()
    {
        Moving();
    }

    private void OnCollisionEnter(Collision collision)
    {
        m_isGrounded = Physics.Raycast(transform.position, -Vector3.up, m_collider.height / 2 + 0.1f);
    }

    private void OnCollisionExit(Collision collision)
    {
        m_isGrounded = Physics.Raycast(transform.position, -Vector3.up, m_collider.height / 2 + 0.1f);
    }

    private void Dash()
    {
    }

    private void ToggleCrouch()
    {
        if (m_isCrouching)
        {
            m_cameraController.MoveCameraY(m_cameraCrouchDist);
            m_collider.center = new Vector3(0, 0, 0);
            m_collider.height /= crouchHeight;
        }
        else
        {
            m_cameraController.MoveCameraY(-m_cameraCrouchDist);
            m_collider.center -= new Vector3(0, m_colliderCrouchDist, 0);
            m_collider.height *= crouchHeight;
        }
        m_isCrouching = !m_isCrouching;
    }
    private void Attack()
    {
        Debug.Log("Attacking...");
        m_isAttacking = true;
        Lunge();
        AttackController attackController =
            Instantiate(attackObject, camera.transform.position + camera.transform.forward * attackDist,
                        camera.transform.rotation, camera.transform)
                .GetComponent<AttackController>();
        attackController.doneEvent.AddListener(OnAttackDone);
    }

    private void Lunge()
    {
        m_rigidBody.linearVelocity = (camera.transform.forward * lungeForce + camera.transform.up * lungeForce / 2);
    }
    private void OnAttackDone()
    {
        m_isAttacking = false;
        Debug.Log("Done Attacking!");
    }
    private void Jump()
    {
        m_rigidBody.linearVelocity =
            new Vector3(m_rigidBody.linearVelocity.x, jumpStrength, m_rigidBody.linearVelocity.z);
    }
    private void Moving()
    {
        Vector3 xzVelocity = Vector3.Scale(m_rigidBody.linearVelocity, new Vector3(1, 0, 1));
        Vector3 input = new Vector3(m_moveVal.x, 0, m_moveVal.y);
        Debug.Log(xzVelocity);
        if (m_isGrounded)
        {
            m_rigidBody.AddForce(transform.rotation * input *
                                     moveSpeed *Time.deltaTime,
                                 ForceMode.VelocityChange);

            m_rigidBody.AddForce(-xzVelocity * groundFriction);
        }
        else if (Mathf.Abs(Vector3.SignedAngle(xzVelocity, transform.rotation * input, Vector3.up)) >= 90)
        {
            // Only allow input if it's in the opposite direction (outside of a 180 deg range)
            m_rigidBody.AddForce(transform.rotation * input * moveSpeed *
                                     airControl * Time.deltaTime,
                                 ForceMode.VelocityChange);
        }
    }
    private void Rotating()
    {
        // TODO: DO NOT let player exceed top and bottom
        // Keep the euler angle between 90 and 270 degrees
        // Use Camera Controller?
        float rotationYAmount = m_lookVal.x * lookSpeed * Time.deltaTime;
        float rotationXAmount = -1 * m_lookVal.y * lookSpeed * Time.deltaTime;
        transform.localEulerAngles += new Vector3(0, rotationYAmount, 0);
        camera.transform.localEulerAngles += new Vector3(rotationXAmount, 0, 0);
    }
}
