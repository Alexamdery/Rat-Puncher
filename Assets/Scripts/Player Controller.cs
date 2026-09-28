using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 4f;
    public float lookSpeed = 4f;
    public float jumpStrength = 4f;
    public float crouchHeight = 0.6f;
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

        Moving();
        Rotating();

        if (m_jump.WasPressedThisFrame() && m_isGrounded)
        {
            Jump();
        }

        if (m_attack.WasPressedThisFrame())
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

        Debug.Log(m_isGrounded);
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
    }
    private void Jump()
    {
        m_rigidBody.linearVelocity = new Vector3(0, jumpStrength, 0);
    }
    private void Moving()
    {
        transform.position += transform.rotation * new Vector3(m_moveVal.x, 0, m_moveVal.y) * moveSpeed *Time.deltaTime;
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
