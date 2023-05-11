using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroidSpawner : DroidBehavior
{
    public GameObject droid;
    public GameObject wraith;
    public GameObject boss;

    //public GameObject projectile;

    int totalDroids;
    public int droidsSpawned;
    
    public bool nextMovement = false;

    public GameObject[] wraithTracker;
    GameObject[] physicalTracker;
    DroidBehavior[] physicalScript;
    DroidBehavior[] wraithScript;

    Vector3 oldPos;

    GameObject spawnCenter;
    System.Random random;
    
    // Start is called before the first frame update
    void Start()
    {
        spawnCenter = GameObject.Find("/Player/Camera Offset/Main Camera");
        random = new System.Random();

        totalDroids = 10;
        droidsSpawned = 0;

        wraithTracker = new GameObject[totalDroids];
        physicalTracker = new GameObject[totalDroids];

        wraithScript = new DroidBehavior[totalDroids];
        physicalScript = new DroidBehavior[totalDroids];
        
        
        StartCoroutine(gameControl());


    }

    // Update is called once per frame
    //int count = 0;
    bool routineRunning = false;
    bool gameStart = false;
    void FixedUpdate()
    {
        // if (!gameStart)
        // {
        //     StartCoroutine(gameControl());
        //     gameStart = true;
        // }
        
    }

    public IEnumerator gameControl()
    {
        int droidsToSpawn = 2;

        StartCoroutine(SpawnWave(droidsToSpawn, 7.0f));
        // StartCoroutine(SpawnWave(2, 7.0f));



        // 10 seconds after last spawn before next wave
        yield return new WaitForSeconds(28.0f);

        // then some grunts too 
        StartCoroutine(SpawnWave(2, 1.0f));


        // 11 seconds
        yield return new WaitForSeconds(20.0f);

        StartCoroutine(SpawnWave(1, 3.0f));

        StartCoroutine(MoveBossDroid());

        yield return null;
    }

    public IEnumerator SpawnWave(int numDroids, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        if (droidsSpawned >= totalDroids)
            yield break;
        
        for (int i = 0; i < numDroids; i++)
        {
            if (droidsSpawned + 1 > totalDroids)
                yield break;
            yield return StartCoroutine(SpawnDroid());
            // Debug.Log("Spawned a droid");
            // Debug.Log("SpawnedDroids: " + droidsSpawned);
            
        }

        while(true)
        {
            yield return new WaitForSeconds(7.5f);
            yield return StartCoroutine(MoveSpawnedDroids());
            
        }

        yield return null;

    }

    public IEnumerator MoveSpawnedDroids()
    {
        for (int i = 0; i < droidsSpawned; i++)
        {
            if (wraithScript[i] != null)
                if (!(wraithScript[i].isMoving || physicalScript[i].isMoving) && !physicalScript[i].isStunned)
                {
                    yield return new WaitForSeconds(Random.Range(0.5f, 1.0f));
                    StartCoroutine(FireLaser(physicalTracker[i]));
                    StartCoroutine(NextMove(i));                
                }
            
        }

        yield return new WaitForSeconds(1.0f);
    }

    protected IEnumerator FireLaser(GameObject physical)
    {
        gun = physical.transform.GetChild(1).transform.gameObject;
        Vector3 center = player.transform.position;
        Quaternion rot = Quaternion.FromToRotation(Vector3.forward, center - gun.transform.position) * Quaternion.Euler(90,90,90);
        laser = Instantiate(projectile, gun.transform.position, rot);
        // Gun Animation
        yield return null;
    }

    public IEnumerator StunDroid(int droidNum)
    {
        DroidBehavior script = physicalScript[droidNum];

        script.isStunned = true;
        Debug.Log("Waiting for good time");
        // wait til their movement is not in the middle of an animation

        yield return new WaitForSeconds(2.5f);
        Debug.Log("Starting stun");
        
        // rotate them backwards and onto ground
        Quaternion rotation = Quaternion.AngleAxis(90, Vector3.forward);
        physicalTracker[droidNum].transform.rotation = rotation;

        // wait a moment
        yield return new WaitForSeconds(5.0f);
        // get back up
        rotation = Quaternion.AngleAxis(0, Vector3.forward);
        physicalTracker[droidNum].transform.rotation = rotation;

        Debug.Log("Stun complete");
        // proceed and allow new movement
        script.isMoving = false;
        script.isStunned = false;
        yield return null;
    }

    public IEnumerator SpawnDroid()
    {
        Vector3 center = spawnCenter.transform.position;
        Vector3 pos = RandomCircle(center, 5.0f);
        Quaternion rot = Quaternion.FromToRotation(Vector3.forward, center-pos) * Quaternion.Euler(0,210f,0);

        StartCoroutine(SpawnWraith(droidsSpawned, pos, rot));
        // spawns physical at same place as wraith

        StartCoroutine(SpawnPhysical(droidsSpawned, pos, rot));

        droidsSpawned++;

        yield return null;
    }

    // Spawn wraith in random spot
    // Spawns with a droid#, a position, and rotation
    public IEnumerator SpawnWraith(int spawnNum, Vector3 pos, Quaternion rot)
    {        
        // store wraith as GameObject

        wraithTracker[spawnNum] = Instantiate(wraith, pos, rot) as GameObject;

        // spawn it as invisible
        wraithTracker[spawnNum].transform.localScale = new Vector3(0,0,0);
        // wraithTracker[spawnNum] 
        
        // give it a script
        wraithScript[spawnNum] = wraithTracker[spawnNum].GetComponent<DroidBehavior>();
        wraithScript[spawnNum].spawnNumber = spawnNum;


        yield return null;
    }

    // spawn droid at same place as wraith
    // spawns with a droid#, the wraith it belongs to, and rotation
    public IEnumerator SpawnPhysical(int spawnNum, Vector3 pos, Quaternion rot)
    {
        physicalTracker[spawnNum] = Instantiate(droid, pos, rot) as GameObject;

        physicalScript[spawnNum] = physicalTracker[spawnNum].GetComponent<DroidBehavior>();
        physicalScript[spawnNum].spawnNumber = spawnNum;

        yield return null;
    }



    public IEnumerator NextMove(int droidNum)
    {
        // move forward or in an arc

        int clock = 0;

        float whichOne = base.RandomRotation();

        Vector3 distanceFromSpawn = spawnCenter.transform.position - wraithTracker[droidNum].transform.position;

        routineRunning = true;
        physicalScript[droidNum].isMoving = true;

        clock = (whichOne > 0) ? 1 : -1;
        if (clock == -1 && !(distanceFromSpawn.magnitude < 1.0f))
        {
            Debug.Log("Movement made");
            Vector3 moveUp = (distanceFromSpawn) * 0.5f;
            yield return StartCoroutine(MoveDroidForward(wraithTracker[droidNum], moveUp));
        }
        else
        {   Debug.Log("Movement made");
            yield return StartCoroutine(MoveDroidArc(wraithTracker[droidNum], whichOne * 60.0f));
        }


        physicalScript[droidNum].isMoving = false;
        routineRunning = false;
        yield return null;

    }

    public IEnumerator MoveDroidForward(GameObject wraith, Vector3 pos)
    {
        DroidBehavior wraithInsScript = wraith.GetComponent<DroidBehavior>();
        GameObject physical = physicalTracker[wraithInsScript.spawnNumber];
        DroidBehavior physInsScript = physical.GetComponent<DroidBehavior>();

        if (physInsScript.isStunned == false)
        {
            yield return StartCoroutine(MoveDroidForwardHelper(wraith, pos));
            yield return new WaitForSeconds(0.1f);
            yield return StartCoroutine(MoveDroidForwardHelper(physical, pos));
        }
        yield return null;
    }

    public IEnumerator MoveDroidArc(GameObject wraith, float angle)
    {
        DroidBehavior wraithInsScript = wraith.GetComponent<DroidBehavior>();
        GameObject physical = physicalTracker[wraithInsScript.spawnNumber];
        DroidBehavior physInsScript = physical.GetComponent<DroidBehavior>();

        int clock = 0;
        clock = (base.RandomRotation() > 0) ? -1 : 1;

        if (physInsScript.isStunned == false)
        {
            yield return MoveDroidArcHelper(wraith, angle, clock);
            yield return new WaitForSeconds(10.0f * NormalizeAngle(angle));
            yield return MoveDroidArcHelper(physical, angle, clock);
            // yield return new WaitForSeconds(1.0f);
        }
        yield return null;
    }

    public IEnumerator MoveDroidForwardHelper(GameObject wraith, Vector3 pos)
    {
        // mark droids as moving

        DroidBehavior wraithInsScript = wraith.GetComponent<DroidBehavior>();
        GameObject physical = physicalTracker[wraithInsScript.spawnNumber];
        DroidBehavior physInsScript = physical.GetComponent<DroidBehavior>();

        physicalScript[physInsScript.spawnNumber].isMoving = true;
        wraithScript[wraithInsScript.spawnNumber].isMoving = true;

        // move wraith
        float t = 0.0f;
        float time = 1.0f;

        while (t < time)
        {
            // where we want to go divided by time
            Vector3 step = pos * (Time.deltaTime % pos.magnitude);
            // interpolate where the droid will be at time (t/time), given start, and end (start + step)
            wraith.transform.position = Vector3.Lerp(wraith.transform.position, wraith.transform.position + step, t / time);

            t += Time.deltaTime;

            yield return null;
        }
        
        physicalScript[physInsScript.spawnNumber].isMoving = false;
        wraithScript[wraithInsScript.spawnNumber].isMoving = false;

        yield return null;
    }



    public float NormalizeAngle(float a)
    {
        return a - 1f * Mathf.Floor((a + 1f) / 1f);
    }

    public IEnumerator MoveDroidArcHelper(GameObject wraith, float angle, int clock)
    {
        // mark droids as moving

        DroidBehavior wraithInsScript = wraith.GetComponent<DroidBehavior>();
        GameObject physical = physicalTracker[wraithInsScript.spawnNumber];
        DroidBehavior physInsScript = physical.GetComponent<DroidBehavior>();

        physicalScript[physInsScript.spawnNumber].isMoving = true;
        wraithScript[wraithInsScript.spawnNumber].isMoving = true;

        
        float t = 0.0f;
        float time = 1.0f;
        float speed = 15.0f;



        Vector3 centerPt = spawnCenter.transform.position;

        while (t < time)
        {
            // move wraith
            float step = (Time.deltaTime * speed) % angle;
            wraith.transform.RotateAround(centerPt, clock * Vector3.up, step);
            rotatePosition(angle, 0.4f);

            t += Time.deltaTime;
            yield return null;
        }

        physicalScript[physInsScript.spawnNumber].isMoving = false;
        wraithScript[wraithInsScript.spawnNumber].isMoving = false;

        yield return null;

    }






    public IEnumerator MoveBossDroid()
    {
        float speed = 1.0f;
        float step = speed * Time.deltaTime;

        boss.transform.position = Vector3.MoveTowards(boss.transform.position, spawnCenter.transform.position, step);

        yield return null;
    }




    // yoinked from stackexchange
    Vector3 RandomCircle(Vector3 center, float radius)
    {
        
        int ang = random.Next(1, 179);

        Vector3 pos;
        pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
        pos.y = center.y;
        pos.z = center.z + radius * Mathf.Cos(ang * Mathf.Deg2Rad);

        return pos;
    }

}
