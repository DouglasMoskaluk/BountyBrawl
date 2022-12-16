using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    private PlayerBody[] players;
    private float distance;
    private GameObject target;

    [SerializeField] private float enemySpeed = 2f;

    // Start is called before the first frame update
    void Start()
    {
        players = FindObjectsOfType<PlayerBody>();
    }

    // Update is called once per frame
    void Update()
    {


        distance = float.PositiveInfinity;

        foreach(var player in players)
        {
            float dist = Vector3.Distance(player.transform.position, transform.position);

            if(dist < distance)
            {
                distance = dist;
                target = player.gameObject;
            }
        }
        
    }

    private void FixedUpdate()
    {
        transform.position += transform.right * enemySpeed * Time.deltaTime;
    }

    private void LateUpdate()
    {

        if (target != null)
        {
            Vector2 face = target.transform.position - transform.position; //Get 2d position of mouse position
            float angle = Mathf.Atan2(face.y, face.x) * Mathf.Rad2Deg; //Get angle of where to face
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward); //Get the rotation based on angle
            transform.rotation = rotation;
        }

    }

}
