using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GamificationManager : MonoBehaviour
{
    private const int REACHING_KNIFE_INCREMENT_COEFFICIENT = 5;

    public int SharpenedKnife { get { return sharpenedKnife; } }
    private int sharpenedKnife = 0;
    private float autoIncrementSharpenedKnifePerSecond = 0;

    private DateTime lastAutoIncrementedAt = new();
    private bool isLastAutoIncrementEvenNumber = true;

    [SerializeField] private List<EnvironmentEvent> environmentEvents = new();
    [SerializeField] private List<LogEvent> logEvents = new();
    [SerializeField] private List<FacilityEvent> facilityEvents = new();
    [SerializeField] private List<BirdViewUpdateEvent> birdViewUpdateEvents = new();

    [SerializeField] private List<FacilityAutoIncrementAmount> facilityAutoIncrementAmounts = new();

    [SerializeField] private MyAvatarManager myAvatarManager;

    [SerializeField] private GameUIManager gameUIManager;

    [SerializeField] private SoundManager soundManager;

    private readonly List<ReleasedFacility> releacedFacilities = new();

    private KnifeSharpeningSetupManager targetKnifeSharpeningSetupManager;
    private bool isPlayingGame = false;
    private bool isAscending = true;

    private bool hasKnifeSharpenedDetected = false;

    public int TotalReachingTimes { get { return totalReachingTimes; } }
    private int totalReachingTimes = 0;

    // Start is called before the first frame update
    void Start()
    {
        sharpenedKnife = SingletonDatabase.Instance.loadedSaveData.sharpenedKnife;
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
                totalReachingTimes += 1;
                isAscending = false;
            }
            if (!isAscending && progress < 0.05f)
            {
                isAscending = true;
            }

            if (!hasKnifeSharpenedDetected && targetKnifeSharpeningSetupManager.KnifeManager.IsSharpeningFinished)
            {
                sharpenedKnife += REACHING_KNIFE_INCREMENT_COEFFICIENT;
                gameUIManager.UpdateSharpenedKnife(sharpenedKnife);

                soundManager.PlayCoinSE();

                hasKnifeSharpenedDetected = true;
            }

            if (targetKnifeSharpeningSetupManager.KnifeManager.IsParticleEffectFinished)
            {
                targetKnifeSharpeningSetupManager.KnifeManager.InitializeKnife();

                hasKnifeSharpenedDetected = false;
            }

            CheckSharpenedKnifeAutoIncrement();

            UpdateKnifeSharpeningTarget();
        }

        CheckEnvironmentEvent();
        CheckLogEvent();
        CheckFacilityEvent();
        CheckBirdViewUpdateEvents();
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

    private void UpdateKnifeSharpeningTarget()
    {
        environmentEvents = environmentEvents
            .OrderBy(environmentEvent => environmentEvent.sharpenedKnifeForTrigger)
            .ToList();

        int previousEnvironmentEventSharpenedKnifeForTrigger = 0;
        foreach (EnvironmentEvent environmentEvent in environmentEvents)
        {
            if (environmentEvent.isApplied)
            {
                previousEnvironmentEventSharpenedKnifeForTrigger = environmentEvent.sharpenedKnifeForTrigger;
                continue;
            }
            else
            {
                int current = sharpenedKnife;
                int target = environmentEvent.sharpenedKnifeForTrigger;
                int offset = previousEnvironmentEventSharpenedKnifeForTrigger;

                gameUIManager.UpdateTargetSharpenedKnifeUI(current, target, offset);
                break;
            }
        }
    }

    private void CheckSharpenedKnifeAutoIncrement()
    {
        DateTime now = DateTime.Now;
        if ((now - lastAutoIncrementedAt).TotalMilliseconds > 1000)
        {
            
            if (isLastAutoIncrementEvenNumber)
            {
                int incrementAmount = Mathf.CeilToInt(autoIncrementSharpenedKnifePerSecond);
                sharpenedKnife += incrementAmount;
                isLastAutoIncrementEvenNumber = false;
            }
            else
            {
                int incrementAmount = Mathf.FloorToInt(autoIncrementSharpenedKnifePerSecond);
                sharpenedKnife += incrementAmount;
                isLastAutoIncrementEvenNumber = true;
            }

            gameUIManager.UpdateSharpenedKnife(sharpenedKnife);

            lastAutoIncrementedAt = now;
        }
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

                        GameObject particle = (environmentEvent.targetObject.gameObject.name.Contains("Building")) ? (GameObject)Resources.Load("Prefabs/Particles/ParticleBuilding")
                                : (environmentEvent.targetObject.gameObject.name.Contains("House")) ? (GameObject)Resources.Load("Prefabs/Particles/ParticleHouse")
                                : (GameObject)Resources.Load("Prefabs/Particles/ParticleTree");
                        Vector3 spawnPosition = environmentEvent.targetObject.gameObject.transform.position;
                        Quaternion spawnDirection = Quaternion.AngleAxis(-90, Vector3.right);
                        particle = Instantiate(particle, spawnPosition, spawnDirection);

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
                gameUIManager.UpdateLogText(logEvent.message);
                logEvent.isApplied = true;
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

                FacilityAutoIncrementAmount targetFacilityAutoIncrementAmount = facilityAutoIncrementAmounts.Find(item => item.facilityType == facilityEvent.facilityType);
                autoIncrementSharpenedKnifePerSecond += targetFacilityAutoIncrementAmount.autoIncrementAmount;

                gameUIManager.UpdateAutoIncrementSharpenedKnifePerSecond(autoIncrementSharpenedKnifePerSecond);

                facilityEvent.isApplied = true;

                soundManager.PlayWindChimeSE();
            }
            else
            {
                break;
            }
        }
    }

    private void CheckBirdViewUpdateEvents()
    {
        foreach (var birdViewUpdateEvent in birdViewUpdateEvents)
        {
            if (!birdViewUpdateEvent.isApplied)
            {
                if(birdViewUpdateEvent.requiredSharpenedKnife <= sharpenedKnife)
                {
                    gameUIManager.UpdateToNextBirdView();
                    birdViewUpdateEvent.isApplied = true;
                }
                else
                {
                    return;
                }
            }
        }
    }
}
