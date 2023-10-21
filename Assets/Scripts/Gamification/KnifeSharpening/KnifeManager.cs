using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeManager : MonoBehaviour
{
    [SerializeField] private GameObject particleEffect;
    [SerializeField] private int requiredReachingTimes;
    [SerializeField] private double particleEffectShowingMiliSeconds;

    public bool IsSharpeningFinished { get { return isSharpeningFinished; } }
    private bool isSharpeningFinished;
    private DateTime sharpeningFinishedTime;

    public bool IsParticleEffectFinished { get { return isParticleEffectFinished; } }
    private bool isParticleEffectFinished;

    private int reachingTimes = 0;

    // Start is called before the first frame update
    void Start()
    {
        InitializeKnife();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isSharpeningFinished)
        {
            CheckSharpeningFinished();
        }

        if (isSharpeningFinished)
        {
            CheckParticleEffectFinished();
        }
    }

    public void InitializeKnife()
    {
        particleEffect.SetActive(false);
        reachingTimes = 0;
        isSharpeningFinished = false;
        isParticleEffectFinished = false;
    }

    public void IncrementReachingTimes()
    {
        reachingTimes += 1;
    }

    private void CheckSharpeningFinished()
    {
        if (reachingTimes > requiredReachingTimes)
        {
            isSharpeningFinished = true;
            sharpeningFinishedTime = DateTime.Now;

            particleEffect.SetActive(true);
        }
    }

    private void CheckParticleEffectFinished()
    {
        DateTime now = DateTime.Now;
        if ((now - sharpeningFinishedTime).TotalMilliseconds > particleEffectShowingMiliSeconds)
        {
            isParticleEffectFinished = true;
            particleEffect.SetActive(false);
        }
    }
}
