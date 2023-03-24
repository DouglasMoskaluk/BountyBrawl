using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSlowness : MonoBehaviour
{

    private PolygonCollider2D collider;
    [SerializeField] private float waterEaterSlowness = 200f;
    [SerializeField] private float waterLostSlowness = 150f;
    [SerializeField] private float waterPlayerSlowness = 5f;

    private void Awake()
    {
        collider = GetComponent<PolygonCollider2D>();
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            collision.GetComponent<PlayerBody>().EnterWater(waterPlayerSlowness);
        }else if(collision.tag == "Lost")
        {
            collision.GetComponent<TheLost>().EnterWater(waterLostSlowness);
        }else if(collision.tag == "Eater")
        {
            collision.GetComponent<TheEater>().EnterWater(waterEaterSlowness);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<PlayerBody>().ExitWater(waterPlayerSlowness);
        }
        else if (collision.tag == "Lost")
        {
            collision.GetComponent<TheLost>().ExitWater(waterLostSlowness);
        }
        else if (collision.tag == "Eater")
        {
            collision.GetComponent<TheEater>().ExitWater(waterEaterSlowness);
        }
    }
}
