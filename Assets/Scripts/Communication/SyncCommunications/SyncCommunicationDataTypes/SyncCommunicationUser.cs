using System;

[Serializable]
public class SyncCommunicationUser
{
    public string userUuid;
    public string userName;
    public string avatarType;
    public string rehabilitationCondition;
    public string avatarState;
    public float reachingProgress;
    public Posture headPosture;
    public Posture leftHandPosture;
    public Posture rightHandPosture;
    public Posture leftLegPosture;
    public Posture rightLegPosture;
}
