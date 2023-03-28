using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    //Holds origin position
    private Vector3 origin;

    private bool shake;

    private void Awake()
    {
        shake = true;
    }

    public IEnumerator Shake(float duration, float magnitude)
    {

        float elapsed = 0f;

        while (elapsed < duration)
        {
            if (!shake) break;
            origin = transform.localPosition;

            float x = Random.Range(-1f, 1f) * magnitude + origin.x;
            float y = Random.Range(-1f, 1f) * magnitude + origin.y;

            transform.localPosition = new Vector3(x, y,-120);

            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = origin;
    }

    public void setShake(bool will) { shake = will; }
}
