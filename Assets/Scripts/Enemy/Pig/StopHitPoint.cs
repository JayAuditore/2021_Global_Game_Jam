using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopHitPoint : MonoBehaviour
{

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag=="Player"|| collision.tag == "Ground")
        {
            transform.parent.GetComponent<Pig>().isStop = true;

        }



    }
}
