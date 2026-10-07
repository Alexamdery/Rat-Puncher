using UnityEngine;

public class PlayerStateCrouching : PlayerStateGrounded
{
    private float _colliderCrouchDist;
    private float _cameraCrouchDist;
    public PlayerStateCrouching(PlayerAttributes attributes, PlayerManager playerController)
        : base(attributes, playerController)
    {
        float centerHeight = collider.height / 2;
        _colliderCrouchDist = (1 - attributes.crouchHeight) * centerHeight;
        _cameraCrouchDist = (1 - attributes.crouchHeight) * centerHeight;
    }

    public override void OnStateEnter()
    {
        // Crouch
        base.OnStateEnter();
        cameraObject.transform.position -= new Vector3(0, _cameraCrouchDist);
        collider.center -= new Vector3(0, _colliderCrouchDist, 0);
        collider.height *= attributes.crouchHeight;
    }

    public override void OnStateExit()
    {
        // Uncrouch
        base.OnStateExit();
        cameraObject.transform.position += new Vector3(0, _cameraCrouchDist);
        collider.center = new Vector3(0, 0, 0);
        collider.height /= attributes.crouchHeight;
    }

    public override void Moving(Vector2 moveVal)
    {
        base.Moving(moveVal * attributes.crouchControl);
    }
    public override void HandleUncrouchInput()
    {
        playerManager.SetState(PlayerStateIndex.Grounded);
    }
}
