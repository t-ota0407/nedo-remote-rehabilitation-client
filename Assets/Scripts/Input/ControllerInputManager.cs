using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class ControllerInputManager : MonoBehaviour
{
    private bool isPressedButtonA = false;
    public bool IsPressedButtonA
    {
        get { return isPressedButtonA; }
    }

    private bool isPressedButtonB = false;
    public bool IsPressedButtonB
    {
        get { return isPressedButtonB; }
    }

    private bool isPressedTrigger = false;
    public bool IsPressedTrigger
    {
        get { return isPressedTrigger; }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        List<InputDevice> rightHandedDevices = new List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(XRNode.RightHand, rightHandedDevices);
        if (rightHandedDevices.Count == 1)
        {
            InputDevice device = rightHandedDevices[0];
            device.TryGetFeatureValue(CommonUsages.primaryButton, out isPressedButtonA);
            device.TryGetFeatureValue(CommonUsages.secondaryButton, out isPressedButtonB);
            device.TryGetFeatureValue(CommonUsages.triggerButton, out isPressedTrigger);
        }
    }
}
