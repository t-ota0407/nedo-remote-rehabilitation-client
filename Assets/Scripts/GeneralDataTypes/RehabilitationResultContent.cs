using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RehabilitationResultContent
{
    public string rehabilitationCondition;
    public string rehabilitationStartedAt;
    public string rehabilitationFinishedAt;
    public int reachingTimes;
    public int sharpenedKnifeBefore;
    public int sharpenedKnifeAfter;

    public RehabilitationResultContent(
        string rehabilitationCondition,
        string rehabilitationStartedAt,
        string rehabilitationFinishedAt,
        int reachingTimes,
        int sharpenedKnifeBefore,
        int sharpenedKnifeAfter)
    {
        this.rehabilitationCondition = rehabilitationCondition;
        this.rehabilitationStartedAt = rehabilitationStartedAt;
        this.rehabilitationFinishedAt = rehabilitationFinishedAt;
        this.reachingTimes = reachingTimes;
        this.sharpenedKnifeBefore = sharpenedKnifeBefore;
        this.sharpenedKnifeAfter = sharpenedKnifeAfter;
    }
}
