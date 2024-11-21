using System;
using UnityEngine;

public class PlayerRewinder : IRewind
{

    private PlayerController playerController;
    private PlayerHealthController healthController;
    private RewindArray<Vector3> lastEulerAngles; //(y, z, x)
    private RewindArray<Vector3> lastPositions;
    private RewindArray<float> lastHealthStates;

    private void Awake()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        healthController = playerController.gameObject.GetComponent<PlayerHealthController>();
    }

    public override void ResetRewindProperties()
    {
        rewindOffset = 0;
        lastEulerAngles = new RewindArray<Vector3>(rewindAbility.GetRewindDurationInFrames());
        lastPositions = new RewindArray<Vector3>(rewindAbility.GetRewindDurationInFrames());
        lastHealthStates = new RewindArray<float>(rewindAbility.GetRewindDurationInFrames());
    }

    public override RewindResponse Rewind()
    {
        RewindResponse cameraRes = lastEulerAngles.GetLast(rewindOffset);
        RewindResponse positionRes = lastPositions.GetLast(rewindOffset);
        RewindResponse healthRes = lastHealthStates.GetLast(rewindOffset);
        positionRes.RewindingObject = gameObject;
        playerController.transform.localEulerAngles = (Vector3)cameraRes.Element;
        playerController.transform.position = (Vector3)positionRes.Element;
        healthController.AlterateHealthInstantly((float)healthRes.Element); //needs to be fixed
        return positionRes;
    }

    public override void UpdateRewindElements()
    {
        lastEulerAngles.Add(playerController.transform.localEulerAngles);
        lastPositions.Add(playerController.transform.position);
        lastHealthStates.Add(healthController.CurrentHealth);
    }

}