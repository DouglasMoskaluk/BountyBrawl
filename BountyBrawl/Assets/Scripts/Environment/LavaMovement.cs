using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaMovement : MonoBehaviour
{
    [SerializeField] private float scrollSpeed = 8f;
    private Vector3 StartPosition;

    private void Awake()
    {
        StartPosition = transform.position;
    }

    private void Update()
    {
        transform.Translate(Vector3.up * scrollSpeed * Time.deltaTime);
        if(transform.position.y > 153.3526)
        {
            transform.position = StartPosition;
        }
        
    }
}
