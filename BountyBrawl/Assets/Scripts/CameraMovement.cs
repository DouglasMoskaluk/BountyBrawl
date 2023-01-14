using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private GameObject[] players;

    private Camera cam;

    [Tooltip("The minimum distance the camera can grow")]
    [SerializeField] private float min = 40f;

    [Tooltip("The maximum distance the camera can grow")]
    [SerializeField] private float max = 10f;

    [Tooltip("Affects the speed of growth. Heigher is slower and lower is faster")]
    [SerializeField] private float zoomSpeed = 50f;

    // Start is called before the first frame update
    void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        float newZoom = Mathf.Lerp(max, min, (GetGreatestXDistance() + GetGreatestYDistance()) / zoomSpeed);
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, newZoom, Time.deltaTime);
    }

    //Finds the greatest x distance between all players
    private float GetGreatestXDistance()
    {
        var bounds = new Bounds(Vector3.zero, Vector3.zero);

        for(int i = 0; i < players.Length; i++)
        {
            bounds.Encapsulate(players[i].transform.position);
        }

        return bounds.size.x;
    }

    //Finds the greatest y distance between all players
    private float GetGreatestYDistance()
    {
        var bounds = new Bounds(Vector3.zero, Vector3.zero);

        for (int i = 0; i < players.Length; i++)
        {
            bounds.Encapsulate(players[i].transform.position);
        }

        return bounds.size.y * 2;
    }
}
