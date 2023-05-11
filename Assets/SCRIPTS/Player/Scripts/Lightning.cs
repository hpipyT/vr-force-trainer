using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Lightning : MonoBehaviour
{

    public ParticleSystem lightning;
    public GameObject lightningSpawnPt;
    public ParticleSystem zap;

    PowerManager player;
    InputDevice leftHand;

    public float lightningLength;
    bool isFiring;
    public bool lightningActive = false;

    // Start is called before the first frame update
    void OnEnable()
    {
        // // lightningSpawnPt = gameObject.transform.parent.Find("LeftHand Controller/LightningPt").gameObject;
        // StartCoroutine(chargeLightning());
        // StartCoroutine(fireLightning());
        player = transform.parent.GetComponent<PowerManager>();
        lightningLength = 5.0f;
        isFiring = false;
        
        // start particle system
    }

    // Update is called once per frame

    
    void Update()
    {

        leftHand = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        leftHand.TryGetFeatureValue(CommonUsages.triggerButton, out bool triggerPressed);
        if (triggerPressed)
        {
            // charge lightning
            // lightning ball particle
            // increases size and distance of lightning

            // on Ultimate ends or Trigger release, stop charging

            // while trigger is held down
            // while(InputDevices.GetDeviceAtXRNode(XRNode.LeftHand).TryGetFeatureValue(CommonUsages.gripButton, out bool gripPressed))
            // {
            //     // add flat increase to length of lightning and thickness of tendrils
            //     yield return null;
            // }
        }
        else
        {
            if (!isFiring)
            {
                isFiring = true;
                Debug.Log("Lightning started");
                StartCoroutine(chargeLightning());
            }            
        }
        // if (lightning)
        // {
        //     ParticleSystem lightning
        // }
        // add collisions to lightning particles

        

    }

    // buildup charge over time

    IEnumerator chargeLightning()
    {
        // lightning ball particle
        // increases size and distance of lightning

        // on Ultimate ends or Trigger release, stop charging

        // while trigger is held down
        // while(InputDevices.GetDeviceAtXRNode(XRNode.LeftHand).TryGetFeatureValue(CommonUsages.gripButton, out bool gripPressed))
        // {
        //     // add flat increase to length of lightning and thickness of tendrils
        //     yield return null;
        // }
        lightningLength = lightningLength;
        StartCoroutine(fireLightning(lightningLength));
        
        yield return null;
    }


    public IEnumerator fireLightning(float lightningLength)
    {
        // lightning bolt particle
        lightningSpawnPt = GameObject.Find("Player/Camera Offset/LeftHand Controller/LightningPt");
        Debug.Log("Firing lightning");

        // lightning particles
        
        // access particle sys, adjust length
        var maine = lightning.main;
        maine.startSpeed = lightningLength;

        zap = Instantiate(lightning, lightningSpawnPt.transform.position, lightningSpawnPt.transform.rotation);
        lightningSpawnPt.GetComponent<CapsuleCollider>().height = 10;
        lightningSpawnPt.GetComponent<CapsuleCollider>().radius = 0.25f;


        yield return new WaitForSeconds(1.0f);
        zap.Stop();
        Destroy(zap);

        lightningSpawnPt.GetComponent<CapsuleCollider>().height = 0;
        lightningSpawnPt.GetComponent<CapsuleCollider>().radius = 0;


        StartCoroutine(returnToManager());
        
        // get parent script, change lightning firing to false;


    }

    public IEnumerator returnToManager()
    {

        yield return new WaitForSeconds(1.0f);
        Debug.Log("Shutting off lightning");
        player.startLightning = false;
        gameObject.SetActive(false);
    }

    // if button is held down 
    // charge Lightning effect
    // drains player's Ultimate meter longer it's held down 

    // on release
    // activate cone and quickly dissipate particle effect

    // Lightning colliders
    
    void OnTriggerEnter(Collider collision)
    {
        // check if the droid hit was physical

        // get droid's script

        // DroidBehavior droidScript 

        // subtract droid health

        // make droid vibrate and add sparkle effect

        // play zap noise
    }

}
