using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour
{

    Vector3 offset;
    Transform player;
    // Start is called before the first frame update
    void Start()
    {
        //bu şekilde de alabiliriz player transformunu
        player = FindObjectOfType<PlayerController>().transform;

        //aradaki farkı alıyoruz
        offset = player.position - transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {   
        // transform.position + offset = player.position;

        transform.position = player.position-offset;
        
    }
}
