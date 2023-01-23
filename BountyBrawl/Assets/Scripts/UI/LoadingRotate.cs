using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingRotate : MonoBehaviour
{
    private float rotz;
    [SerializeField]
    public float RotSpeed;

    // Update is called once per frame
    void Update()
    {
        rotz += -Time.deltaTime * RotSpeed;
        transform.rotation = Quaternion.Euler(0, 0, rotz);
    }
}
