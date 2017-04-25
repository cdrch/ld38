using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    private Transform player;

	void Start ()
	{
        player = GameObject.FindGameObjectWithTag("Player").transform;
	}

	void Update ()
	{
        transform.position = player.position;
	}
}
