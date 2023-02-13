using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement_Big : MonoBehaviour
{
    private List<GameObject> players;

    private Camera cam;

    [Tooltip("The minimum distance the camera can grow")]
    [SerializeField] private float min = 40f;

    [Tooltip("The maximum distance the camera can grow")]
    [SerializeField] private float max = 10f;

    [Tooltip("Affects the speed of growth. Heigher is slower and lower is faster")]
    [SerializeField] private float zoomSpeed = 70f;

    [SerializeField] private float eaterTeleportLockFOV = 50f;

    private float currDist; //distance of the camera

    private bool locking;

    // Start is called before the first frame update
    void Start()
    {
        players = new List<GameObject>();

        locking = false;

        GameObject[] allPlayers = GameObject.FindGameObjectsWithTag("Player");

        foreach(GameObject p in allPlayers)
        {
            players.Add(p);
        }

        cam = GetComponent<Camera>();
        currDist = transform.position.z;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!locking)
        {
            Vector3 centerPoint = GetCenterPoint();
            Vector3 newPosiion = centerPoint;
            transform.position = newPosiion + new Vector3(0f, 0f, currDist);

            float newZoom = Mathf.Lerp(max, min, (GetGreatestXDistance() / 2 + GetGreatestYDistance() / 2) / zoomSpeed);
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, newZoom, Time.deltaTime);
        }
    }

    Vector3 GetCenterPoint()
    {
        var bounds = new Bounds(players[0].transform.position, Vector3.zero);
        
        for(int i = 0; i < players.Count; i++)
        {
            bounds.Encapsulate(players[i].transform.position);
        }

        return bounds.center;
    }

    //Finds the greatest x distance between all players
    private float GetGreatestXDistance()
    {
        var bounds = new Bounds(players[0].transform.position, Vector3.zero);

        for(int i = 0; i < players.Count; i++)
        {
            bounds.Encapsulate(players[i].transform.position);
        }

        return bounds.size.x;
    }

    //Finds the greatest y distance between all players
    private float GetGreatestYDistance()
    {
        var bounds = new Bounds(players[0].transform.position, Vector3.zero);

        for (int i = 0; i < players.Count; i++)
        {
            bounds.Encapsulate(players[i].transform.position);
        }

        return bounds.size.y * 2;
    }

    public void AddEater(GameObject eater) { players.Add(eater); }

    public void DeleteEater() { players.RemoveAt(players.Count-1); }

    public void EaterIsTeleporting() 
    {
        locking = true;
        transform.position = new Vector3(0f, 0f, -120f);
        cam.fieldOfView = eaterTeleportLockFOV;
    }

    public void EaterIsNotTeleporting()
    {
        locking = false;
    }
}
