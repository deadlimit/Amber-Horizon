using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerLockedState")]
public class PlayerLockedState : State
{
    private PlayerController player;
    protected override void Initialize()
    {
        player = (PlayerController)owner;
    }
    public override void RunUpdate()
    {
        player.InputGrounded(Vector3.zero);
    }
}
