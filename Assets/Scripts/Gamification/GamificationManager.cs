using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GamificationManager : MonoBehaviour
{
    private const int REACHING_KNIFE_INCREMENT_COEFFICIENT = 3;
    private const int FACILITY_AUTO_KNIFE_INCREMENT_COEFFICIENT = 1;

    private int sharpenedKnife = 0;
    
    [SerializeField] private List<EnvironmentEvent> environmentEvents = new();
    [SerializeField] private List<LogEvent> logEvents = new();
    [SerializeField] private List<FacilityEvent> facilityEvents = new();

    [SerializeField] private MyAvatarManager myAvatarManager;

    [SerializeField] private GameUIManager gameUIManager;

    private readonly List<ReleasedFacility> releacedFacilities = new();

    private KnifeSharpeningSetupManager targetKnifeSharpeningSetupManager;
    private bool isPlayingGame = false;
    private bool isAscending = true;

    private bool hasKnifeSharpenedDetected = false;

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

            if (!hasKnifeSharpenedDetected && targetKnifeSharpeningSetupManager.KnifeManager.IsSharpeningFinished)
            {
                sharpenedKnife += REACHING_KNIFE_INCREMENT_COEFFICIENT;
                gameUIManager.UpdateSharpenedKnifeNumber(sharpenedKnife);

                hasKnifeSharpenedDetected = true;
            }

            if (targetKnifeSharpeningSetupManager.KnifeManager.IsParticleEffectFinished)
            {
                targetKnifeSharpeningSetupManager.KnifeManager.InitializeKnife();

                hasKnifeSharpenedDetected = false;
            }
        }

        CheckSharpenedKnifeAutoIncrement();

        CheckEnvironmentEvent();
        CheckLogEvent();
        CheckFacilityEvent();
    }

    public void ContinueGame(KnifeSharpeningSetupManager targetSharpeningSetup)
    {
        Quaternion targetRotation = Quaternion.LookRotation(targetSharpeningSetup.transform.position - gameUIManager.transform.position, Vector3.up) * Quaternion.Euler(0, 180, 0);
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

    private void CheckSharpenedKnifeAutoIncrement()
    {

    }

    private void CheckEnvironmentEvent()
    {
        environmentEvents = environmentEvents
            .OrderBy(environmentEvent => environmentEvent.sharpenedKnifeForTrigger)
            .ToList();

        foreach (EnvironmentEvent environmentEvent in environmentEvents)
        {
            if (environmentEvent.isApplied)
            {
                continue;
            }

            if (environmentEvent.sharpenedKnifeForTrigger <= sharpenedKnife)
            {
                switch (environmentEvent.eventType)
                {
                    case EnvironmentEventType.Appear:
                        environmentEvent.targetObject.SetActive(true);
                        break;

                    case EnvironmentEventType.Disappear:
                        environmentEvent.targetObject.SetActive(false);
                        break;
                }
                environmentEvent.isApplied = true;
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
            if (logEvent.isApplied)
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
            if (facilityEvent.isApplied)
            {
                continue;
            }

            if (facilityEvent.sharpenedKnifeForTrigger <= sharpenedKnife)
            {
                ReleasedFacility targetReleasedFacility = releacedFacilities.Find(item => item.facilityType == facilityEvent.facilityType);

                if (targetReleasedFacility == null)
                {
                    targetReleasedFacility = new ReleasedFacility(facilityEvent.facilityType, 0);
                    releacedFacilities.Add(targetReleasedFacility);
                }

                targetReleasedFacility.amount += 1;

                gameUIManager.UpdateFacilityCard(targetReleasedFacility.facilityType, targetReleasedFacility.amount);

                facilityEvent.isApplied = true;
            }
            else
            {
                break;
            }
        }
    }
}
