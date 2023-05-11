using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class LaserCollision : MonoBehaviour
{
    Vector3 target;
    Vector3 laserMovement;
    GameObject player;

    bool isDeflected = false;
    bool leftGun = false;
    Vector3 deflectTarget = new Vector3(0,0,0);
    Vector3 startingPosition;
    public float offset;
    float speed = 2.5f;

    // Start is called before the first frame update
    void Start()
    {
        // get player position and set as laser target
        player = GameObject.Find("/Player/Camera Offset/Main Camera");
        offset = -0.25f;
        startingPosition = gameObject.transform.position;
        target = new Vector3(player.transform.position.x, player.transform.position.y + offset, player.transform.position.z);
        laserMovement = (target - transform.position).normalized * speed;
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        var step = speed * Time.deltaTime;

        // initial shot from blaster
        if (gameObject != null && !isDeflected) 
        {
            // move laser in direction of player position
            transform.position += laserMovement * Time.deltaTime;

        }
        else
        {
            // move towards saber-directed target
            gameObject.transform.position += deflectTarget * step;

        }
    }

    void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.name == "Gun")
        {
            leftGun = true;
        }
    }

    // collision with objects
    void OnTriggerEnter(Collider collision)
    {
        // if (collision.gameObject.name == "Gun")
        // {
        //     Debug.Log("Laser left barrel");
        //     leftGun = true;
        // }

        if (gameObject != null)
        {
            // Debug.Log(collision.name);
            if (collision.gameObject.name == "Main Camera")
            {
                PlayerManager victim = collision.gameObject.transform.parent.transform.parent.gameObject.GetComponent<PlayerManager>();
                victim.TakeDamage();

                Destroy(gameObject);
            }

            if (collision.gameObject.name == "Saber")
            {
                Debug.Log("Saber hit!");
                // https://answers.unity.com/questions/1787394/how-do-i-determine-the-rotation-of-the-xr-unity-co.html

                // know orientation of saber
                Quaternion saberRotation = collision.gameObject.transform.GetChild(0).rotation;

                deflectTarget = collision.gameObject.transform.rotation * Vector3.forward;
                isDeflected = true;

                
                // rotate laser
                gameObject.transform.rotation = saberRotation;

                // Increase UltimateStatus by small amount
                PlayerManager playerScript = collision.GetComponent<PlayerManager>();

                IncreaseUltimate(collision, 5f);
            }

            if ((collision.gameObject.name == "Body" || collision.gameObject.name == "Gun") && leftGun)
            {
                Debug.Log("Training Droid hit!");

                // Access droid's script
                DroidBehavior droidScript = collision.gameObject.transform.parent.GetComponent<DroidBehavior>();

                if (!isDeflected)
                {

                    // Increase UltimateStatus by Medium amount
                    droidScript.HitDroid("Body");
                    PlayerManager playerScript = collision.GetComponent<PlayerManager>();
                    IncreaseUltimate(collision, 10f);
                }
                else
                {
                    droidScript.HitDroid("Head");
                }

                // Destroy laser
                Destroy(gameObject);
            }

            if (collision.gameObject.name == "Head" && leftGun)
            {
                Debug.Log("Training Droid headshot!");

                DroidBehavior droidScript = collision.gameObject.transform.parent.GetComponent<DroidBehavior>();
                droidScript.HitDroid("Head");
                
                PlayerManager playerScript = collision.GetComponent<PlayerManager>();
                IncreaseUltimate(collision, 15f);
                
                Destroy(gameObject);

                // Increase UltimateStatus by Large amount
                
            }

            // play a rad laser sound
        }
    }

    public void IncreaseUltimate(Collider collision, float amount)
    {
        PlayerManager playerScript = GameObject.Find("Player").GetComponent<PlayerManager>();
        Debug.Log("Added ult");
        playerScript.FillUltimate(amount);
    }

    // public void BlasterShot(Vector3 target)
    // {
        
    //     if (gameObject != null)
    //     {
    //         float speed = 0.001f;
    //         var step = speed * Time.deltaTime;
    //         while(gameObject.transform.position != target)
    //         {
    //         // move laser in direction of player position
    //         gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, target, step);
            
    //         }
    //     }
    // }

    public void ChangeTrajectory()
    {
        
    }

}
