using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class EnergyWave : MonoBehaviour
{
    public ParticleSystem wave;
    public GameObject waveSpawnPt;
    public ParticleSystem sploosh;

    // same signature as lightning but add a timed knockdown animation 
    bool isFiring;
    public bool waveActive = false;

    PowerManager player;
    InputDevice leftHand;
    InputDevice rightHand;
    
    // Start is called before the first frame update
    void OnEnable()
    {
        // spawn a wave
        player = transform.parent.GetComponent<PowerManager>();
        isFiring = false;
    }

    // Update is called once per frame
    void Update()
    {
        leftHand = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        leftHand.TryGetFeatureValue(CommonUsages.gripButton, out bool leftGripPressed);
        rightHand.TryGetFeatureValue(CommonUsages.gripButton, out bool rightGripPressed);

        if (!leftGripPressed && !rightGripPressed)
        {
            if (!isFiring)
            {
                isFiring = true;
                Debug.Log("Wave started");
                StartCoroutine(throwWave());
            }  
        }

    }

    public IEnumerator throwWave()
    {
        // lightning bolt particle
        waveSpawnPt = GameObject.Find("Player/Camera Offset/Main Camera/EnergyWavePt");
        Debug.Log("Throwing wave");

        // wave particles

        sploosh = Instantiate(wave, waveSpawnPt.transform.position, waveSpawnPt.transform.rotation);
        waveSpawnPt.GetComponent<BoxCollider>().size = new Vector3(2.75f,0.2f,5.0f);


        yield return new WaitForSeconds(1.0f);
        sploosh.Stop();
        Destroy(sploosh);

        waveSpawnPt.GetComponent<BoxCollider>().size = new Vector3(0,0,0);


        StartCoroutine(returnToManager());
        yield return null;
    }

    public IEnumerator returnToManager()
    {

        yield return new WaitForSeconds(1.0f);
        Debug.Log("Shutting off Wave");
        player.startWave = false;
        gameObject.SetActive(false);
        yield return null;
    }
}
