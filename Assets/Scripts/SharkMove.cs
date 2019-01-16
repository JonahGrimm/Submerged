using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkMove : MonoBehaviour
{
    public Transform sharkTransform;
    public Vector3 start;
    public Vector3 end;
    private BoxCollider bc;
    public float moveTime = 3f;

    void Start()
    {
        sharkTransform.position = start;
        bc = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            bc.enabled = false;
            StartCoroutine("Move");
        }
    }

    IEnumerator Move()
    {
        sharkTransform.gameObject.SetActive(true);

        sharkTransform.position = start;
        float timeElapsed = 0f;
        while (timeElapsed < moveTime)
        {
            Vector3.Lerp(start, end, timeElapsed / moveTime);
            yield return null;
        }
        sharkTransform.position = end;

        sharkTransform.gameObject.SetActive(false);
    }
}
