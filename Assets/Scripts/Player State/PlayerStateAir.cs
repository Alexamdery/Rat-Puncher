using UnityEngine;

public class PlayerStateAir : PlayerState
{
    private bool canLunge;
    private bool canAttack;
    public PlayerStateAir(PlayerAttributes attributes, PlayerManager playerController)
        : base(attributes, playerController)
    {
    }
    public override void OnStateChange()
    {
        canLunge = true;
        canAttack = true;
    }

    public override void Moving(Vector2 moveVal)
    {
        if (IsGrounded())
        {
            playerManager.SetState(PlayerStateIndex.Grounded);
        }

        Vector3 input = new Vector3(moveVal.x, 0, moveVal.y);
        Vector3 xzVelocity = Vector3.Scale(rigidBody.linearVelocity, new Vector3(1, 0, 1));
        if (Mathf.Abs(Vector3.SignedAngle(xzVelocity, playerObject.transform.rotation * input, Vector3.up)) >= 90)
        {
            // Only allow input if it's in the opposite direction (outside of a 180 deg range)
            rigidBody.AddForce(playerObject.transform.rotation * input * attributes.moveSpeed * attributes.airControl,
                               ForceMode.VelocityChange);
        }
    }

    public override void HandleLungeInput()
    {
        if (canLunge)
        {
            base.HandleLungeInput();
            canLunge = false;
        }
    }

    public override void HandleAttackInput()
    {
        if (canAttack)
        {
            base.HandleAttackInput();
            canLunge = false;
            canAttack = false;
        }
    }
    public override void HandleJumpInput()
    {
        return;
    }
}
