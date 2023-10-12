using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeSharpeningSetupManager : MonoBehaviour
{
    [SerializeField] private GameObject enteringAreaRendering;

    [SerializeField] private Material emptyAreaMaterial;
    [SerializeField] private Material isInAreaMaterial;
    [SerializeField] private Material usedAreaMaterial;

    public KnifeManager KnifeManager { get { return knifeManager; } }
    [SerializeField] private KnifeManager knifeManager;

    public GameObject StandingOrigin { get { return standingOrigin; } }
    [SerializeField] private GameObject standingOrigin;
    public GameObject MinReachingOrigin { get { return minReachingOrigin; } }
    [SerializeField] private GameObject minReachingOrigin;
    public GameObject MaxReachingOrigin { get { return maxReachingOrigin; } }
    [SerializeField] private GameObject maxReachingOrigin;

    private int playerInArea = 0;
    public bool IsEnterd
    {
        get { return playerInArea > 0; }
    }

    private MeshRenderer enteringAreaMeshRenderer;

    private Material beforeUnvisibleMaterial;

    // Start is called before the first frame update
    void Start()
    {
        enteringAreaMeshRenderer = enteringAreaRendering.GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            playerInArea += 1;

            enteringAreaMeshRenderer.material = isInAreaMaterial;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            playerInArea -= 1;

            if (playerInArea == 0)
            {
                enteringAreaMeshRenderer.material = emptyAreaMaterial;
            }
        }
    }

    public void SetVisibility(bool visibility)
    {
        if (visibility)
        {
            enteringAreaMeshRenderer.material = beforeUnvisibleMaterial;
        }
        else
        {
            beforeUnvisibleMaterial = enteringAreaMeshRenderer.material;
            enteringAreaMeshRenderer.material = usedAreaMaterial;
        }

    }
}
