using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throw : MonoBehaviour
{
    [SerializeField] private float throwSpeed = 60f;
    [SerializeField] private float throwTime = 0.2f; //How long the weapon will be thrown for
    [SerializeField] private float rotationSpeed = 5f;

    private bool isThrown = false;

    private Vector2 traj;

    // Update is called once per frame
    void Update()
    {
        if (isThrown)
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = traj.normalized * throwSpeed;
            transform.Rotate(new Vector3(0f, 0f, -rotationSpeed * Time.deltaTime));
            return;
        }
    }

    public IEnumerator Cooldown(Vector2 direction)
    {
        isThrown = true;
        traj = direction;
        yield return new WaitForSeconds(throwTime);
        isThrown = false;
        gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    } //When the weapon has done being thrown


    //collision detection for when it hits a wall...
}
