using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingDroidBehavior : DroidBehavior
{



    // Start is called before the first frame update
    void Start()
    {
        rand = new System.Random();
        // todo
        // laser, move, laser, arc, enum toggle thing
    }

    void FixedUpdate()
    {

            if (!coroutineRun && !isDead)
            {   
                
                StartCoroutine(TrainingDroidSequence());
            }
    }

    public IEnumerator TrainingDroidSequence()
    {
        coroutineRun = true;
        StartCoroutine(rotatePosition(120.0f, 0.4f));
        yield return new WaitForSeconds(base.RandomTimeDelay());
        StartCoroutine(FireLaser());
        if (enableMovement)
        {
            yield return new WaitForSeconds(base.RandomTimeDelay());
            StartCoroutine(base.moveInArc(base.RandomRotation()));
        }
        yield return new WaitForSeconds(1.0f);
        coroutineRun = false;
    }

    protected override IEnumerator FireLaser()
    {
        yield return base.FireLaser();
    }


    protected override IEnumerator rotatePosition(float angle, float seconds)
    {
        yield return base.rotatePosition(angle, seconds);
    }

    protected override IEnumerator moveInArc(float angle)
    {
        yield return base.moveInArc(angle);
    }

}
