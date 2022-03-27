using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprayParticle : MonoBehaviour
{
    public Rigidbody body;
    Vector3 initPosition;
    float rotation = 0.1f;

    public void SetInitPos(Vector3 init)
    {
        initPosition = init;
        StartCoroutine(Kill());
    }

    IEnumerator Kill()
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y < initPosition.y)
        {
            body.AddForce(Vector3.up);
            transform.Rotate(rotation, rotation, rotation);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<RatCatcherController>())
        {
            other.GetComponent<RatCatcherController>().Hurt();
        }
    }
}
