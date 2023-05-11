using UnityEngine.XR;
using UnityEngine;

public class ThrustDetection : MonoBehaviour
{
    InputDevice left;
    InputDevice right;
    // Start is called before the first frame update
    void Start()
    {
        left = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        right = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (right.TryGetFeatureValue(CommonUsages.triggerButton, out bool isPressed))
        {
            Debug.Log(isPressed);
        }
    }
}