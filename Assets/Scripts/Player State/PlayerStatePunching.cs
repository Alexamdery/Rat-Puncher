using System.Threading;
using UnityEngine;

public class PlayerStatePunching : PlayerState
{
    private float _timer = 0;
    private Punchable _punchable;
    public PlayerStatePunching(PlayerAttributes attributes, PlayerManager playerController)
        : base(attributes, playerController)
    {
    }
    public override void OnStateEnter()
    {
        rigidBody.isKinematic = true;
        _punchable = playerManager.punchedObject.GetComponent<Punchable>();
        _timer = 0;
    }

    public override void OnStateExit()
    {
        rigidBody.isKinematic = false;
    }

    public override void HandleAttackInput()
    {
        // Do nothing
    }

    public override void HandleCrouchInput()
    {
        // Do nothing
    }

    public override void HandleDashInput()
    {
        // Do nothing
    }

    public override void HandleJumpInput()
    {
        // Do nothing
    }

    public override void HandleLungeInput()
    {
        _punchable.Punch();
        playerManager.SetState(PlayerStateIndex.Air);
        // TODO: will cause issues, after this an attack input will be queued up on release
    }

    public override void HandleUncrouchInput()
    {
        // Do nothing
    }

    public override void Looking(Vector2 lookVal)
    {
        // TODO: restrict looking to half-sphere
        float rotationYAmount = lookVal.x * attributes.lookSpeed * Time.deltaTime;
        float rotationXAmount = -1 * lookVal.y * attributes.lookSpeed * Time.deltaTime;
        playerObject.transform.localEulerAngles += new Vector3(0, rotationYAmount, 0);
        cameraObject.transform.localEulerAngles += new Vector3(rotationXAmount, 0, 0);
    }

    public override void Moving(Vector2 moveVal)
    {
        _timer += Time.deltaTime;
        _punchable.SetupPunch(cameraObject.transform.forward);
        if (_timer > attributes.punchTimer)
        {
            HandleAttackInput();
        }
    }
}
