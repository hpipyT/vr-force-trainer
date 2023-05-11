using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillAura : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.name == "Body" || collision.name == "Head")
        {
            Debug.Log("Killing droids");
            collision.transform.parent.gameObject.GetComponent<DroidBehavior>().HitDroid("Head");
        }
    }
}
