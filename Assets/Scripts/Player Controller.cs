using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 4f;
    public float lookSpeed = 4f;
    public Camera camera;
    private InputAction m_move;
    private InputAction m_look;
    private InputAction m_jump;
    private InputAction m_crouch;
    private InputAction m_sprint;
    private InputAction m_attack;
    private Vector2 m_moveVal;
    private Vector2 m_lookVal;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_move = InputSystem.actions.FindAction("Move");
        m_look = InputSystem.actions.FindAction("Look");
        m_jump = InputSystem.actions.FindAction("Jump");
        m_crouch = InputSystem.actions.FindAction("Crouch");
        m_sprint = InputSystem.actions.FindAction("Sprint");
        m_attack = InputSystem.actions.FindAction("Attack");
    }

    // Update is called once per frame
    void Update()
    {
        m_moveVal = m_move.ReadValue<Vector2>();
        m_lookVal = m_look.ReadValue<Vector2>();

        Moving();
        Rotating();

        if (m_jump.WasPressedThisFrame())
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

        if (m_crouch.IsPressed())
        {
            Crouch();
        }
    }

    private void Dash()
    {
    }

    private void Crouch()
    {
    }
    private void Attack()
    {
    }
    private void Jump()
    {
    }
    private void Moving()
    {
        transform.position += transform.rotation * new Vector3(m_moveVal.x, 0, m_moveVal.y) * moveSpeed * Time.deltaTime;
    }
    private void Rotating()
    {
        float rotationYAmount = m_lookVal.x * lookSpeed * Time.deltaTime;
        float rotationXAmount = -1 * m_lookVal.y * lookSpeed * Time.deltaTime;
        transform.localEulerAngles += new Vector3(0, rotationYAmount, 0);
        camera.transform.localEulerAngles += new Vector3(rotationXAmount, 0, 0);
    }
}
