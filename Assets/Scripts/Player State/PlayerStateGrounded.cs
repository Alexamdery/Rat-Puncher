using UnityEngine;

public class PlayerStateGrounded : PlayerState
{
    public PlayerStateGrounded(PlayerAttributes attributes, PlayerController playerController)
        : base(attributes, playerController)
    {
    }

    public override void Moving(Vector2 moveVal)
    {
        if (!IsGrounded())
        {
            playerController.SetState(PlayerStateIndex.Air);
        }

        base.Moving(moveVal);
        Vector3 xzVelocity = Vector3.Scale(rigidBody.linearVelocity, new Vector3(1, 0, 1));
        rigidBody.AddForce(-xzVelocity * attributes.groundFriction);
    }
}
