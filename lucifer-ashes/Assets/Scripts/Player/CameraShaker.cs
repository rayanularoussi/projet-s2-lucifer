using System.Collections;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    public bool start = false;
    public float duration = 1f;

    void Update()
    {
        if(start)
        {
            start = false;
            StartCoroutine(Shaking());
        }
    }

    IEnumerator Shaking()
    {
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            transform.position = startPosition + Random.insideUnitSphere;
            yield return null;
            startPosition = transform.position;
        }
        transform.position = startPosition;
    }
}