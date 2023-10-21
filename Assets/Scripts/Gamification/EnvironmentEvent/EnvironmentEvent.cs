using System;
using UnityEngine;

[Serializable]
public class EnvironmentEvent
{
    public EnvironmentEventType eventType;
    public GameObject targetObject;
    public int sharpenedKnifeForTrigger;
    public bool isApplied = false;
}
