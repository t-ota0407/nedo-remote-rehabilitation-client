using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GamificationManager : MonoBehaviour
{
    private int sharpenedKnife = 0;
    
    [SerializeField] private List<EnvironmentEvent> environmentEvents = new();
    [SerializeField] private List<LogEvent> logEvents = new();
    [SerializeField] private List<FacilityEvent> facilityEvents = new();

    [SerializeField] private MyAvatarManager myAvatarManager;

    [SerializeField] private GameUIManager gameUIManager;

    private KnifeSharpeningSetupManager targetKnifeSharpeningSetupManager;
    private bool isPlayingGame = false;
    private bool isAscending = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayingGame)
        {
            float progress = myAvatarManager.ReachingProgress();

            UpdateKnifePosition(progress);

            if (isAscending && progress > 0.95f)
            {
                targetKnifeSharpeningSetupManager.KnifeManager.IncrementReachingTimes();
                isAscending = false;
            }
            if (!isAscending && progress < 0.05f)
            {
                isAscending = true;
            }

            if (targetKnifeSharpeningSetupManager.KnifeManager.IsParticleEffectFinished)
            {
                // todo: 増加量はていねいに設定する
                sharpenedKnife += 1;
                gameUIManager.UpdateSharpenedKnifeNumber(sharpenedKnife);

                targetKnifeSharpeningSetupManager.KnifeManager.InitializeKnife();
            }
        }

        CheckEnvironmentEvent();
        CheckLogEvent();
        CheckFacilityEvent();
    }

    public void ContinueGame(KnifeSharpeningSetupManager targetSharpeningSetup)
    {
        Quaternion targetRotation = Quaternion.LookRotation(targetSharpeningSetup.transform.position - gameUIManager.transform.position, Vector3.up) * Quaternion.Euler(0, 180, 0);
        // Quaternion gameUITargetPosture = targetRotation * gameUIManager.transform.rotation;
        gameUIManager.StartRotation(targetRotation);

        targetKnifeSharpeningSetupManager = targetSharpeningSetup;
        isAscending = true;
        isPlayingGame = true;
    }

    public void StopGame()
    {
        isPlayingGame = false;
    }

    private void UpdateKnifePosition(float reachingProgress)
    {
        Vector3 minReachingPosition = targetKnifeSharpeningSetupManager.MinReachingOrigin.transform.position;
        Vector3 maxReachingPosition = targetKnifeSharpeningSetupManager.MaxReachingOrigin.transform.position;
        Vector3 reachingTargetPosition = Vector3.Lerp(minReachingPosition, maxReachingPosition, reachingProgress);

        GameObject knifeObject = targetKnifeSharpeningSetupManager.KnifeManager.gameObject;
        knifeObject.transform.position = reachingTargetPosition;
        knifeObject.transform.localPosition = knifeObject.transform.localPosition + new Vector3(0, -0.04f, -0.24f);
    }

    private void CheckEnvironmentEvent()
    {
        environmentEvents = environmentEvents
            .OrderBy(environmentEvent => environmentEvent.sharpenedKnifeForTrigger)
            .ToList();

        foreach (EnvironmentEvent environmentEvent in environmentEvents)
        {
            if (!environmentEvent.isApplied)
            {
                continue;
            }

            if (environmentEvent.sharpenedKnifeForTrigger <= sharpenedKnife)
            {
                // todo:イベントの実行
            }
            else
            {
                break;
            }
        }
    }

    private void CheckLogEvent()
    {
        logEvents = logEvents
            .OrderBy(logEvent => logEvent.sharpenedKnifeForTrigger)
            .ToList();

        foreach(LogEvent logEvent in logEvents)
        {
            if (!logEvent.isApplied)
            {
                continue;
            }

            if (logEvent.sharpenedKnifeForTrigger <= sharpenedKnife)
            {
                // todo:イベントの実行
            }
            else
            {
                break;
            }
        }
    }

    private void CheckFacilityEvent()
    {
        facilityEvents = facilityEvents
            .OrderBy(facilityEvent => facilityEvent.sharpenedKnifeForTrigger)
            .ToList();

        foreach(FacilityEvent facilityEvent in facilityEvents)
        {
            if (!facilityEvent.isApplied)
            {
                continue;
            }

            if (facilityEvent.sharpenedKnifeForTrigger <= sharpenedKnife)
            {
                // todo:イベントの実行
            }
            else
            {
                break;
            }
        }
    }
}
