using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public PlayerAttributes attributes;
    public GameObject playerObject;
    public GameObject cameraObject;
    private PlayerState _currentState;
    private Dictionary<PlayerStateIndex, PlayerState> _playerStates;
    private InputAction _move;
    private InputAction _look;
    private InputAction _jump;
    private InputAction _crouch;
    private InputAction _dash;
    private InputAction _attack;
    private Queue<Inputs> _inputQueue = new Queue<Inputs>();
    private Vector2 _moveVal;
    private Vector2 _lookVal;

    void Start()
    {
        _move = InputSystem.actions.FindAction("Move");
        _look = InputSystem.actions.FindAction("Look");
        _jump = InputSystem.actions.FindAction("Jump");
        _crouch = InputSystem.actions.FindAction("Crouch");
        _dash = InputSystem.actions.FindAction("Sprint");
        _attack = InputSystem.actions.FindAction("Attack");
        _playerStates = new Dictionary<PlayerStateIndex, PlayerState> {
            { PlayerStateIndex.Grounded, new PlayerStateGrounded(attributes, this) },
            { PlayerStateIndex.Air, new PlayerStateAir(attributes, this) },
            { PlayerStateIndex.Punching, new PlayerStatePunching(attributes, this) }
        };
        _currentState = _playerStates[PlayerStateIndex.Grounded];
    }

    void Update()
    {
        _moveVal = _move.ReadValue<Vector2>();
        _lookVal = _look.ReadValue<Vector2>();

        if (_jump.WasPressedThisFrame())
        {
            _inputQueue.Enqueue(Inputs.Jump);
        }

        if (_attack.WasPressedThisFrame())
        {
            _inputQueue.Enqueue(Inputs.Lunge);
        }

        if (_attack.WasReleasedThisFrame())
        {
            _inputQueue.Enqueue(Inputs.Attack);
        }

        if (_dash.WasPressedThisFrame())
        {
            _inputQueue.Enqueue(Inputs.Dash);
        }

        if (_crouch.WasPressedThisFrame())
        {
            _inputQueue.Enqueue(Inputs.Crouch);
        }

        if (_crouch.WasReleasedThisFrame())
        {
            _inputQueue.Enqueue(Inputs.Uncrouch);
        }
    }
    private void FixedUpdate()
    {
        _currentState.Moving(_moveVal);
        _currentState.Looking(_lookVal);
        if (_inputQueue.Count > 0)
        {
            _currentState.HandleInput(_inputQueue.Dequeue());
        }
    }
    public void SetState(PlayerStateIndex stateIndex)
    {
        _currentState = _playerStates[stateIndex];
    }
}
