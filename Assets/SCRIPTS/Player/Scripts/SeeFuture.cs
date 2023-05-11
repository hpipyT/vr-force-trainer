using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class SeeFuture : MonoBehaviour
{
    GameObject healthMan;
    PowerManager player;
    InputDevice leftHand;

    GameObject droids;
    bool[] wraithsLowered;
    // Start is called before the first frame update
    void OnEnable()
    {
        healthMan = GameObject.Find("Player");
        player = transform.parent.GetComponent<PowerManager>();
        droids = GameObject.Find("DroidSpawner");

        wraithsLowered = new bool[droids.GetComponent<DroidSpawner>().droidsSpawned];

        for (int i = 0; i < droids.GetComponent<DroidSpawner>().droidsSpawned; i++)
        {
            wraithsLowered[i] = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        leftHand = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        leftHand.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryPressed);

        float t = 0.0f;
        float time = 1.0f;
        if (primaryPressed)
        {

            Debug.Log("Primary is held down");
            // make all the wraiths visible
            StartCoroutine(ShowWraiths());
            // 1 second
            // while (t < time)
            // {
            //     // speed/ult per increment
            //     float step = (5.0f * player.heatScale) * Time.deltaTime;

            //     if (player.currentHealth <= player.maxHealth / 10.0f);
                
            //     healthMan.GetComponent<PlayerManager>().ultimate -= step;
            //     t += Time.deltaTime;
            // }
            
        }
        else
        {
            // make all wraiths invisible
            StartCoroutine(HideWraiths());
        }

    }

    public IEnumerator ShowWraiths()
    {
        // get droidSpawn script
        DroidSpawner script = droids.GetComponent<DroidSpawner>();
        
        // loop through each droid in wraithTracker and set scale to 0.25

        for (int i = 0; i < script.droidsSpawned; i++)
        {
            if (script.wraithTracker[i] != null)
            {
                script.wraithTracker[i].transform.localScale = new Vector3(0.25f,0.25f,0.25f);
                // if wraiths havent been adjusted, adjust them
                if (!wraithsLowered[i])
                {
                    script.wraithTracker[i].transform.position -= new Vector3(0,0.4f,0);
                    wraithsLowered[i] = true;
                }
            }
        }

        yield return null;
    }


    public IEnumerator HideWraiths()
    {
        DroidSpawner script = droids.GetComponent<DroidSpawner>();
        for (int i = 0; i < script.droidsSpawned; i++)
        {
            if (script.wraithTracker[i] != null)
            {
            script.wraithTracker[i].transform.localScale = new Vector3(0,0,0);
            }
        }
        yield return null;
    }

    public IEnumerator returnToManager()
    {

        yield return new WaitForSeconds(1.0f);
        Debug.Log("Shutting off See");
        player.startSee = false;
        gameObject.SetActive(false);
        yield return null;
    }
}
