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

    [SerializeField] private int cameraXLock = 35;
    [SerializeField] private int cameraYLock = 20;

    private float currDist; //distance of the camera

    private bool locking;
    private float newX;
    private float newY;

    private bool gotten;

    private GameObject[] allPlayers;
    private bool enoughPlayers;

    private void Awake()
    {
        gotten = false;
    }

    // Start is called before the first frame update
    public void GetPlayers()
    {
        players = new List<GameObject>();
        enoughPlayers = true;

        locking = false;

        allPlayers = GameObject.FindGameObjectsWithTag("Player");

        foreach(GameObject p in allPlayers)
        {
            players.Add(p);
        }

        cam = GetComponent<Camera>();
        currDist = transform.position.z;

        gotten = true;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (gotten)
        {
            if (enoughPlayers)
            {
                int playersAlive = 0;
                foreach (GameObject p in allPlayers)
                {
                    if (p.activeSelf)
                    {
                        playersAlive++;
                    }
                }

                if(playersAlive <= 1)
                {
                    enoughPlayers = false;
                }
            }


            if (!locking)
            {
                Vector3 centerPoint = GetCenterPoint();
                Vector3 newPosiion = centerPoint;

                newX = Mathf.Lerp(transform.position.x, newPosiion.x, 0.5f);
                newY = Mathf.Lerp(transform.position.y, newPosiion.y, 0.5f);

                if (enoughPlayers)
                {
                    //Move Y
                    if (newY > -cameraYLock && newY < cameraYLock)
                    {
                        transform.position = new Vector3(transform.position.x, newY, 0f) + new Vector3(0f, 0f, currDist);
                    }
                    //Move X
                    if (newX > -cameraXLock && newX < cameraXLock)
                    {
                        transform.position = new Vector3(newX, transform.position.y, 0f) + new Vector3(0f, 0f, currDist);
                    }
                }
                else
                {
                    transform.position = new Vector3(newX, newY, 0f) + new Vector3(0f, 0f, currDist);
                }

                float newZoom = Mathf.Lerp(max, min, (GetGreatestXDistance() / 2 + GetGreatestYDistance() / 2) / zoomSpeed);
                cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, newZoom, Time.deltaTime);

            }
            else
            {
                float newZoom = Mathf.Lerp(max, min, eaterTeleportLockFOV);
                cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, newZoom, Time.deltaTime);

                float xTravel = Mathf.Lerp(transform.position.x, 0f, 0.5f);
                float yTravel = Mathf.Lerp(transform.position.y, 0f, 0.5f);

                transform.position = new Vector3(xTravel, yTravel, currDist);
            }
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
    }

    public void EaterIsNotTeleporting()
    {
        locking = false;
    }
}
