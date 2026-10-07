using System.Collections.Generic;
using UnityEngine;

public enum Inputs
{
    Lunge,
    Attack,
    Jump,
    Crouch,
    Uncrouch,
    Dash
}

public enum PlayerStateIndex
{
    Grounded,
    Air,
    Punching,
    Crouching
}

[CreateAssetMenu(fileName = "PlayerAttributes", menuName = "Scriptable Objects/PlayerAttributes")]
public class PlayerAttributes : ScriptableObject
{
    public float moveSpeed = 4f;
    public float lookSpeed = 4f;
    public float jumpStrength = 4f;
    public float crouchHeight = 0.6f;
    public float attackDist = 10f;
    public float lungeForce = 10f;
    public float airControl = 0.5f;
    public float crouchControl = 0.5f;
    public float groundFriction = 1.0f;
    public float punchTimer = 5.0f;
    public GameObject attackObject;
}
