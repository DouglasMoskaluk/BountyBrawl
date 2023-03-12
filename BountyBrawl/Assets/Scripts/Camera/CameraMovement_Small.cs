using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement_Small : MonoBehaviour
{
    private List<GameObject> players;

    private Camera cam;

    [Tooltip("The minimum distance the camera can grow")]
    [SerializeField] private float min = 40f;

    [Tooltip("The maximum distance the camera can grow")]
    [SerializeField] private float max = 10f;

    [Tooltip("Affects the speed of growth. Heigher is slower and lower is faster")]
    [SerializeField] private float zoomSpeed = 60f;

    // Start is called before the first frame update
    void Start()
    {
        players = new List<GameObject>();

        GameObject[] allPlayers = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject p in allPlayers)
        {
            players.Add(p);
        }

        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        float newZoom = Mathf.Lerp(max, min, (GetGreatestXDistance()/1.7f + GetGreatestYDistance()/1.7f) / zoomSpeed);
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, newZoom, Time.deltaTime);
    }

    //Finds the greatest x distance between all players
    private float GetGreatestXDistance()
    {
        var bounds = new Bounds(Vector3.zero, Vector3.zero);

        for(int i = 0; i < players.Count; i++)
        {
            bounds.Encapsulate(players[i].transform.position);
        }

        return bounds.size.x;
    }

    //Finds the greatest y distance between all players
    private float GetGreatestYDistance()
    {
        var bounds = new Bounds(Vector3.zero, Vector3.zero);

        for (int i = 0; i < players.Count; i++)
        {
            bounds.Encapsulate(players[i].transform.position);
        }

        return bounds.size.y * 2;
    }

    public void AddEater(GameObject eater) { players.Add(eater); }

    public void DeleteEater() { players.RemoveAt(players.Count - 1); }
}