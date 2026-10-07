using UnityEngine;

public class PlayerStatePunching : PlayerState
{
    public PlayerStatePunching(PlayerAttributes attributes, PlayerManager playerController)
        : base(attributes, playerController)
    {
    }

    public override void OnStateChange()
    {
        throw new System.NotImplementedException();
    }
}
