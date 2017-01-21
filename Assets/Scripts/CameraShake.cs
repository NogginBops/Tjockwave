using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    Vector3 originalPos;
    
    public void ShakeCamera(float duration, float amount)
    {
        originalPos = transform.localPosition;

        StartCoroutine(Shake(duration, amount));
    }

    IEnumerator Shake(float duration, float amount)
    {
        while (duration > 0)
        {
            transform.localPosition = originalPos + Random.insideUnitSphere * amount;
            duration -= Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        duration = 0f;
        transform.localPosition = originalPos;

        yield break;
    }
}