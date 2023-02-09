using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteRendering : MonoBehaviour
{
    [SerializeField]
    private int sortingOrderBase = 5000;
    [SerializeField]
    private int offset = 0;
    [SerializeField]
    private bool runOnlyOnce = false;

    private float timer;
    private float timerMax = 0.1f;
    private Renderer[] myRenderer;

    private void Awake()
    {
        myRenderer = GetComponents<Renderer>();
    }

    private void LateUpdate()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            timer = timerMax;
            foreach (Renderer r in myRenderer)
            {
                r.sortingOrder = (int)(sortingOrderBase - transform.position.y - offset);
            }
            if (runOnlyOnce)
            {
                Destroy(this);
            }
        }
    }
}
