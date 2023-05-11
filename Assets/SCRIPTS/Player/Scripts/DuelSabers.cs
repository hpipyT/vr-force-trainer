using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuelSabers : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject.Find("/Player/Camera Offset/LeftHand Controller/Saber").SetActive(true);
        StartCoroutine(KillAura());
        // spawn a second saber in left hand
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator KillAura()
    {
        GameObject.Find("/Player/Camera Offset/KillAura").GetComponent<SphereCollider>().radius = 2000.0f;
        yield return new WaitForSeconds(0.5f);
        GameObject.Find("/Player/Camera Offset/KillAura").GetComponent<SphereCollider>().radius = 0.0f;
        yield return null;
    }
}
