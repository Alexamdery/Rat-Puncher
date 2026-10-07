using UnityEditor.Rendering;
using UnityEngine;

public abstract class PlayerState
{
    protected PlayerAttributes attributes;
    protected PlayerManager playerManager;
    protected GameObject playerObject;
    protected GameObject cameraObject;
    protected Rigidbody rigidBody;
    protected CapsuleCollider collider;
    public PlayerState(PlayerAttributes attributes, PlayerManager playerController)
    {
        this.attributes = attributes;
        this.playerManager = playerController;
        playerObject = playerController.playerObject;
        cameraObject = playerController.cameraObject;
        rigidBody = playerObject.GetComponent<Rigidbody>();
        collider = playerObject.GetComponent<CapsuleCollider>();
    }

    protected PlayerState(PlayerAttributes attributes, GameObject playerObject, GameObject cameraObject)
    {
        this.attributes = attributes;
        this.playerObject = playerObject;
        this.cameraObject = cameraObject;
    }

    public abstract void OnStateEnter();

    public abstract void OnStateExit();

    public abstract void Moving(Vector2 moveVal);
    public abstract void Looking(Vector2 lookVal);

    public void HandleInput(Inputs input)
    {
        switch (input)
        {
        case Inputs.Lunge:
            HandleLungeInput();
            break;
        case Inputs.Attack:
            HandleAttackInput();
            break;
        case Inputs.Crouch:
            HandleCrouchInput();
            break;
        case Inputs.Uncrouch:
            HandleUncrouchInput();
            break;
        case Inputs.Dash:
            HandleDashInput();
            break;
        case Inputs.Jump:
            HandleJumpInput();
            break;
        }
    }

    public abstract void HandleLungeInput();
    public abstract void HandleAttackInput();
    public abstract void HandleCrouchInput();
    public abstract void HandleUncrouchInput();
    public abstract void HandleDashInput();
    public abstract void HandleJumpInput();

    protected void Jump()
    {
        rigidBody.linearVelocity =
            new Vector3(rigidBody.linearVelocity.x, attributes.jumpStrength, rigidBody.linearVelocity.z);
    }

    protected void Lunge()
    {
        rigidBody.linearVelocity = cameraObject.transform.forward * attributes.lungeForce +
                                   cameraObject.transform.up * attributes.lungeForce / 2;
    }

    protected void Attack()
    {
        AttackController attackController =
            Object
                .Instantiate(attributes.attackObject,
                             cameraObject.transform.position + cameraObject.transform.forward * attributes.attackDist,
                             cameraObject.transform.rotation, cameraObject.transform)
                .GetComponent<AttackController>();
    }

    protected bool IsGrounded()
    {
        return Physics.Raycast(playerObject.transform.position, -Vector3.up, collider.height / 2 + 0.1f);
    }
}
