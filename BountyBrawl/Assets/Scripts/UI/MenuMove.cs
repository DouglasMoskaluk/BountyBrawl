using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMove : MonoBehaviour
{
    [SerializeField] private float scrollSpeed = 8f;
    private Vector3 StartPosition;

    private void Awake()
    {
        StartPosition = transform.position;
    }

    private void Update()
    {
        transform.Translate(Vector3.left * scrollSpeed * Time.deltaTime);
        if (transform.position.x <= -28.6f)
        {
            transform.position = StartPosition;
        }
    }
}
