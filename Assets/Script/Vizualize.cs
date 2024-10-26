using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Vizualize : MonoBehaviour
{
    public float distanceTarget;
    public LayerMask layermask;

    private RaycastHit hit;

    [Header("Events")]
    public UnityEvent enterHitEvent;
    public UnityEvent exitHitEvent;




    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward),
                out hit, distanceTarget, layermask))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.red);
            enterHitEvent.Invoke();
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.green);
            exitHitEvent.Invoke();
        }
    }
}

