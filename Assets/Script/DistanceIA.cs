using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DistanceIA : MonoBehaviour

{
    public float speed;
    public float distanceTrigger;
    public Transform target;


    // Update is called once per frame
    void Update()
    {
        Vector3 targetPosition = new Vector3(target.position.x, transform.position.y, target.position.z);
        float distance = Vector3.Distance(transform.position, targetPosition);

        if (distance <= distanceTrigger)
        {
            Vector3 direction = (targetPosition - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * speed);
        }



    }

    private void OnCollisionter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene("SampleScene");
        }
    }


}
