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

            if (isAscending && progress > 0.95f)
            {
                targetKnifeSharpeningSetupManager.KnifeManager.IncrementReachingTimes();
                isAscending = false;
            }
            if (!isAscending && progress < 0.05f)
            {
                isAscending = true;
            }
        }

        if (targetKnifeSharpeningSetupManager.KnifeManager.IsParticleEffectFinished)
        {
            // todo: 増加量はていねいに設定する
            sharpenedKnife += 1;
            gameUIManager.UpdateSharpenedKnifeNumber(sharpenedKnife);
            
            targetKnifeSharpeningSetupManager.KnifeManager.InitializeKnife();
        }

        CheckEnvironmentEvent();
        CheckLogEvent();
        CheckFacilityEvent();
    }

    public void ContinueGame(KnifeSharpeningSetupManager targetSharpeningSetup)
    {
        Quaternion gameUITargetPosture = Quaternion.LookRotation(targetSharpeningSetup.transform.position, Vector3.up) * Quaternion.Euler(0, -90, 0);
        gameUIManager.StartRotation(gameUITargetPosture);

        targetKnifeSharpeningSetupManager = targetSharpeningSetup;
        isAscending = true;
        isPlayingGame = true;
    }

    public void StopGame()
    {
        isPlayingGame = false;
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
