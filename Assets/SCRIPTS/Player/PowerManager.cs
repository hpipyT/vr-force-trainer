using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class PowerManager : MonoBehaviour
{

    public GameObject player;
    public PlayerManager playerScript;

    public GameObject lightning;
    public GameObject wave;
    public GameObject grab;
    public GameObject heal;
    public GameObject see;
    public GameObject duel;

    // ultimate:
    // lighting
        // Left hand Button for lightning
    // pick up objects and throw
        // Grip and trigge 
    // energy wave
        // button and punch
        // drains ulti
    // heal
        // button and spin ? 
        // drains ulti 
    // future
        // briefly displays time until next shot
        // activate wraith enemies
            // store next move of droid and create wraith instance before they move, move to new position as they move
    // call on jedi
        // duel lightsaber 

    InputDevice leftHand;
    InputDevice rightHand;

    // power flags

    public bool startLightning = false;
    public bool startWave = false;
    public bool startSee = false;

    // increases cost of abilities
    public bool startDuel = false;
    public float heatScale;

    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.Find("Player");
        playerScript = player.GetComponent<PlayerManager>();

        heatScale = 1.0f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        // Get Button Press and check for gesture

        rightHand = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        leftHand = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);

        // Lightning power, activate with Left Grip
        if (leftHand.TryGetFeatureValue(CommonUsages.triggerButton, out bool triggerPressed))
        {

            // if button is held down 
            // Lightning effect
            // drains player's Ultimate meter 
            // Debug.Log(triggerPressed);
            if (triggerPressed && !startLightning && (playerScript.ultimate >= 40f * heatScale))
            {
                playerScript.ultimate -= 40f * heatScale;
                startLightning = true;
                // instantiate a lightning object with a script to access its methods
                if (startLightning)
                    lightning.SetActive(true);
            }
        }

        // force push
        if (leftHand.TryGetFeatureValue(CommonUsages.gripButton, out bool leftGripPressed))
        {
            if (rightHand.TryGetFeatureValue(CommonUsages.gripButton, out bool rightGripPressed))
            {
                // energy wave at point on camera
                if (leftGripPressed && rightGripPressed && !startWave && (playerScript.ultimate >= 20f * heatScale))
                {
                    playerScript.ultimate -= 20f * heatScale;
                    startWave = true;
                    if (startWave)
                        wave.SetActive(true);
                }
            }
            else
            {
                // enable grab component on left hand 
            }

        }

        // future sight, usable even in the direst of times
        if (leftHand.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryPressed))
        {

            if (primaryPressed && !startSee && (playerScript.ultimate > 0.0f))
            {
                startSee = true;
                
                float step = 15.0f;

                if (playerScript.currentHealth <= playerScript.maxHealth / 10.0f)
                    step = 0f;

                playerScript.ultimate -= step * Time.deltaTime;

                if (startSee)
                    see.SetActive(true);
            }
        }

        // heal, can't use in final form
        if (rightHand.TryGetFeatureValue(CommonUsages.triggerButton, out bool rightTriggerPressed))
        {

            if (rightTriggerPressed && !startDuel && (playerScript.ultimate >= 10 * heatScale))
            {
                playerScript.ultimate -= 30f * Time.deltaTime;
                playerScript.currentHealth += 10.0f * Time.deltaTime;
            }
        }

        // activate duelSabers, drop to 1/4 hp, losing ability to regen, but for 1/4 cost of abilities
        if (leftHand.TryGetFeatureValue(CommonUsages.secondaryButton, out bool secondaryButtonPressed))
        {
            
            if (secondaryButtonPressed && !startDuel)
            {
                playerScript.ultimate = 0.0f;
                playerScript.currentHealth /= 4;
                heatScale = 0.25f;
                startDuel = true;
                // instantiate a lightning object with a script to access its methods
                if (startDuel)
                    duel.SetActive(true);
            }
        }

        if(startDuel)
        {
            playerScript.ultimate += 1.0f * Time.deltaTime;
        }



        // player.ultimate = ultimate;
        


        // can drop saber if want to use extra powers 
        
    }

}



    // stretch

    // Red blue purple
    // Red: Darkside build
        // Force choke
        // Cool lightning 
    // Blue: Sturdy build
        // More health
        // Stronger knockdown
    // Purple: Badass build
        // Hold droid and turn to fire at others
    // Green: Heal build