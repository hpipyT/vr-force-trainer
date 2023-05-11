using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroidBehavior : MonoBehaviour
{

    // control
    
    public int spawnNumber;
    public bool coroutineRun = false;
    public System.Random rand;
    
    // For firing lasers at player
    public GameObject projectile;
    public GameObject gun;
    public GameObject player;

    public GameObject remains;

    protected GameObject laser;

    // 

    

    public int health = 2;
    
    public bool isScrappable;
    public bool isDead = false;
    public bool isMoving = false;
    public bool isStunned = false;
    public bool animationPlaying = false;
    public bool allowFireLaser = false;


    // movement
    public bool enableMovement = true;
    public float arcSpeed = 10.0f;
    
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        

    }

    protected virtual IEnumerator FireLaser()
    {
        Vector3 center = player.transform.position;
        Quaternion rot = Quaternion.FromToRotation(Vector3.forward, center - gun.transform.position) * Quaternion.Euler(90,90,90);
        laser = Instantiate(projectile, gun.transform.position, rot);
        // Gun Animation
        yield return null;
    }

    protected virtual IEnumerator rotatePosition(float angle, float seconds)
    {
        float time = seconds;

        float startRotation = transform.eulerAngles.y;
        float endRotation = startRotation + angle;

        float t = 0.0f;
        
        while (t < time)
        {
            t += Time.deltaTime;

            float step = Mathf.Lerp(startRotation, endRotation, t / time) % angle;
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, step, transform.eulerAngles.z);

            yield return null;
        }
    }

    protected virtual IEnumerator moveInArc(float angle)
    {
        float time = 1.0f;
        float speed = arcSpeed;

        float t = 0.0f;
        int clock = 0;
        clock = (RandomRotation() > 0) ? -1 : 1;

        while (t < time)
        {
            t += Time.deltaTime;
            float step = (Time.deltaTime * speed)  % Mathf.Abs(angle);

            transform.RotateAround(player.transform.position, clock * Vector3.up, step);
            rotatePosition(angle, 0.4f);

            yield return null;
        }
    }

    public float RandomRotation()
    {
        return Random.Range(-30.0f, 30.0f);
    }

    public float RandomTimeDelay()
    {
        return rand.Next(1, 4) / 2.0f;
    }


    public void OnTriggerEnter(Collider collision)
    {
        if (collision.name == "Main Camera")
        {
            Debug.Log("Touching Player");
            StartCoroutine(DamagePlayer(collision));
        }
        
    }

    IEnumerator DamagePlayer(Collider collision)
    {

        // get player script

        // subtract player's health

        yield return new WaitForSeconds(2.0f);

    }


    public void SubtractHealth(string part)
    {
        if (part == "Body")
        {
            health--;
            Debug.Log("Droid " +  "_ lost 1 health");
        }
        if (part == "Head")
        {
            health--;
            health--;
            Debug.Log("Droid " +  "_ lost 2 health");
        }
    }

    public void HitDroid(string part)
    {
        SubtractHealth(part);
        if (health > 0)
        {
            if(!animationPlaying)
                StartCoroutine(PlayHurtAnimation());

            // play hurtsound
        }
        else
        {
            // StartCoroutine(PlayDeathAnimation());
            

            // get droidspawner script
            DroidSpawner droids = GameObject.Find("DroidSpawner").GetComponent<DroidSpawner>();

            // get wraith at same position as this droid
            Instantiate(remains, droids.wraithTracker[spawnNumber].transform.position, Quaternion.identity);
            Destroy(droids.wraithTracker[spawnNumber]);
            Destroy(gameObject);

            // destroy wraith and physical

            // play deathsound
        }
    }


    public IEnumerator PlayDeathAnimation()
    {
        int i = 100;
        animationPlaying = true;


        while (i > 0)
        {
            gameObject.transform.Rotate(0.0f,1.0f,0.0f);
            i--;
        }
        yield return null;
        Debug.Log("AAAAAAAAAAAAAAAH");
    }

    public IEnumerator PlayHurtAnimation()
    {
        // spin animation
        // glow red
        // ouch sound
        int i = 100;
        while (i > 0)
        {
            gameObject.transform.Rotate(0.0f,1.0f,0.0f);
            i--;
        }
        Debug.Log("OOOOUCH");
        yield return null;
    }

}
