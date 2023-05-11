using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waveCollision : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider collision)
    {
        if (collision.name == "Body" || collision.name == "Head")
        {
            Debug.Log("Wave hit droid");
            // collision.transform.parent.gameObject.GetComponent<DroidBehavior>().HitDroid("Head");
            // get droidspawner script
            DroidSpawner droids = GameObject.Find("DroidSpawner").GetComponent<DroidSpawner>();
            Debug.Log(collision.transform.parent.gameObject.GetComponent<DroidBehavior>().spawnNumber);
            StartCoroutine(AllowWait(collision.transform.parent.gameObject.GetComponent<DroidBehavior>().spawnNumber));

        }
    }

    public IEnumerator AllowWait(int num)
    {
        DroidSpawner droids = GameObject.Find("DroidSpawner").GetComponent<DroidSpawner>();
        yield return StartCoroutine(droids.StunDroid(num));
    }
}
