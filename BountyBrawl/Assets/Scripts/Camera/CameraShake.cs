using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    //Holds origin position
    private Vector2 origin;

    public IEnumerator Shake(float duration, float magnitude)
    {

        float elapsed = 0f;

        while (elapsed < duration)
        {
            origin = transform.localPosition;

            float x = Random.Range(-1f, 1f) * magnitude + origin.x;
            float y = Random.Range(-1f, 1f) * magnitude + origin.y;

            transform.localPosition = new Vector2(x, y);

            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = origin;
    }
}
