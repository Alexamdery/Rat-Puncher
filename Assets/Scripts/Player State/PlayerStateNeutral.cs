using UnityEngine;

public abstract class PlayerStateNeutral : PlayerState
{
    public PlayerStateNeutral(PlayerAttributes attributes, PlayerManager playerController)
        : base(attributes, playerController)
    {
    }

    public override void Moving(Vector2 moveVal)
    {
        Vector3 input = new Vector3(moveVal.x, 0, moveVal.y);
        rigidBody.AddForce(playerObject.transform.rotation * input * attributes.moveSpeed, ForceMode.VelocityChange);
    }

    public override void Looking(Vector2 lookVal)
    {
        float rotationYAmount = lookVal.x * attributes.lookSpeed * Time.deltaTime;
        float rotationXAmount = -1 * lookVal.y * attributes.lookSpeed * Time.deltaTime;
        playerObject.transform.localEulerAngles += new Vector3(0, rotationYAmount, 0);
        cameraObject.transform.localEulerAngles += new Vector3(rotationXAmount, 0, 0);
    }

    public override void HandleAttackInput()
    {
        Attack();
    }

    public override void HandleCrouchInput()
    {
        // Do nothing
    }

    public override void HandleDashInput()
    {
        // TODO: implement dash
    }

    public override void HandleJumpInput()
    {
        Jump();
    }

    public override void HandleLungeInput()
    {
        Lunge();
    }

    public override void HandleUncrouchInput()
    {
        // Do Nothing
    }

    public override void OnStateEnter()
    {
        // Do nothing
    }

    public override void OnStateExit()
    {
        // Do nothing
    }
}
