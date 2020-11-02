using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    private GameObject player;
    void Start()
    {
        player = GameObject.Find("Car");
    }

    void Update()
    {
        Vector3 camPos = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z-45);
        transform.position = camPos;
        
    }
}
