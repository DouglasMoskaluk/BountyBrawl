using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteRendering : MonoBehaviour
{
    [SerializeField]
    private int sortingOrderBase = 5000;
    [SerializeField]
    private int offset = 0; //Put in the negatives to get trail behind bullet
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

            for(int i = 0; i < myRenderer.Length; i++)
            {
                if(i == 0)
                {
                    myRenderer[i].sortingOrder = (int)(sortingOrderBase - transform.position.y - offset*2);
                }
                else
                {
                    myRenderer[i].sortingOrder = (int)(sortingOrderBase - transform.position.y - offset);
                }
            }
            if (runOnlyOnce)
            {
                Destroy(this);
            }
        }
    }
}
