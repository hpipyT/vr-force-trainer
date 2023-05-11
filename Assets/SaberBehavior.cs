using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class SaberBehavior : MonoBehaviour
{

    System.Random rand;

    public GameObject reflect1;
    public GameObject reflect2;
    public GameObject reflect3;

    public GameObject swing1;
    public GameObject swing2;
    public GameObject swing3;

    InputDevice rightHand;
    InputDevice leftHand;

    bool coroutine = false;

    // Start is called before the first frame update
    void Start()
    {
        rand = new System.Random();
        rightHand = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        leftHand = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
    }

    // Update is called once per frame
    void Update()
    {
        rightHand.TryGetFeatureValue(CommonUsages.deviceVelocity, out Vector3 accel);
        rightHand.TryGetFeatureValue(CommonUsages.triggerButton, out bool yee);
        if(yee)
        if (!coroutine)
        {
            coroutine = true;
            //StartCoroutine(PlaySwing());
        }

        //swing1.GetComponent<AudioSource>().Play();
    }

    public IEnumerator PlaySwing()
    {
        if (rand.Next(0, 2) == 0)
            reflect1.GetComponent<AudioSource>().Play(0);
        else
            reflect2.GetComponent<AudioSource>().Play(0);

        yield return new WaitForSeconds(2.0f);
        coroutine = false;
        yield return null;
    }

    public void OnTriggerEnter(Collider collision)
    {
        if (collision.name == "Laser(Clone)")
        {
            StartCoroutine(PlaySwing());
        }
        if (collision.name == "Body" || collision.name == "Head")
            collision.transform.parent.gameObject.GetComponent<DroidBehavior>().HitDroid("Body");
    }
}
