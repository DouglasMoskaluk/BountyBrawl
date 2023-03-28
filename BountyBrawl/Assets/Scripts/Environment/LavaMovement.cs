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
        transform.Translate(Vector3.right * scrollSpeed * Time.deltaTime);
        if(transform.position.x > 270.9395f)
        {
            transform.position = StartPosition;
        }
    }
}
