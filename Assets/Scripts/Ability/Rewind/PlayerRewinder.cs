using System;
using UnityEngine;

public class PlayerRewinder : IRewind
{

    private PlayerController playerController;
    private RewindArray<Vector3> lastEulerAngles; //(y, z, x)
    private RewindArray<Vector3> lastPositions;

    private void Awake()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    public override void ResetRewindProperties()
    {
        rewindOffset = 0;
        lastEulerAngles = new RewindArray<Vector3>(rewindAbility.GetRewindDurationInFrames());
        lastPositions = new RewindArray<Vector3>(rewindAbility.GetRewindDurationInFrames());
    }

    public override RewindResponse Rewind()
    {
        RewindResponse cameraRes = lastEulerAngles.GetLast(rewindOffset);
        RewindResponse positionRes = lastPositions.GetLast(rewindOffset);
        positionRes.RewindingObject = gameObject;
        playerController.transform.localEulerAngles = (Vector3)cameraRes.Element;
        playerController.transform.position = (Vector3)positionRes.Element;
        return positionRes;
    }

    public override void UpdateRewindElements()
    {
        lastEulerAngles.Add(playerController.transform.localEulerAngles);
        lastPositions.Add(playerController.transform.position);
    }

}