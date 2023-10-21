using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class ControllerInputManager : MonoBehaviour
{
    public bool IsPressedButtonA { get { return isPressedButtonA; } }
    private bool isPressedButtonA = false;

    public bool IsPressedButtonB { get { return isPressedButtonB; } }
    private bool isPressedButtonB = false;

    public bool IsPressedButtonX { get { return isPressedButtonX; } }
    private bool isPressedButtonX = false;

    public bool IsPressedRightHandTrigger { get { return isPressedRightHandTrigger; } }
    private bool isPressedRightHandTrigger = false;


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
            device.TryGetFeatureValue(CommonUsages.triggerButton, out isPressedRightHandTrigger);
        }

        List<InputDevice> leftHandedDevices = new List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(XRNode.LeftHand, leftHandedDevices);

        if (leftHandedDevices.Count == 1)
        {
            InputDevice device = leftHandedDevices[0];
            device.TryGetFeatureValue(CommonUsages.primaryButton, out isPressedButtonX);
        }
    }
}
