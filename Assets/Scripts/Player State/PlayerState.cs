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

    public abstract void OnStateChange();

    public virtual void Moving(Vector2 moveVal)
    {
        Vector3 input = new Vector3(moveVal.x, 0, moveVal.y);
        rigidBody.AddForce(playerObject.transform.rotation * input * attributes.moveSpeed, ForceMode.VelocityChange);
    }
    public virtual void Looking(Vector2 lookVal)
    {
        // TODO: DO NOT let player exceed top and bottom
        // Keep the euler angle between 90 and 270 degrees
        float rotationYAmount = lookVal.x * attributes.lookSpeed * Time.deltaTime;
        float rotationXAmount = -1 * lookVal.y * attributes.lookSpeed * Time.deltaTime;
        playerObject.transform.localEulerAngles += new Vector3(0, rotationYAmount, 0);
        cameraObject.transform.localEulerAngles += new Vector3(rotationXAmount, 0, 0);
    }

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

    public virtual void HandleLungeInput()
    {
        rigidBody.linearVelocity = cameraObject.transform.forward * attributes.lungeForce +
                                   cameraObject.transform.up * attributes.lungeForce / 2;
    }
    public virtual void HandleAttackInput()
    {
        AttackController attackController =
            Object
                .Instantiate(attributes.attackObject,
                             cameraObject.transform.position + cameraObject.transform.forward * attributes.attackDist,
                             cameraObject.transform.rotation, cameraObject.transform)
                .GetComponent<AttackController>();
    }

    public virtual void OnAttack(GameObject punched)
    {
        if (punched != null)
        {
            Punchable punchable = punched.GetComponent<Punchable>();
            punchable.SetupPunch(cameraObject.transform.forward);
        }
    }

    public virtual void HandleCrouchInput()
    {
    }
    public virtual void HandleUncrouchInput()
    {
    }
    public virtual void HandleDashInput()
    {
    }
    public virtual void HandleJumpInput()
    {
        rigidBody.linearVelocity =
            new Vector3(rigidBody.linearVelocity.x, attributes.jumpStrength, rigidBody.linearVelocity.z);
    }

    protected bool IsGrounded()
    {
        return Physics.Raycast(playerObject.transform.position, -Vector3.up, collider.height / 2 + 0.1f);
    }
}
