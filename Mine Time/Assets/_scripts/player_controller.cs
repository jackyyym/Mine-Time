using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class player_controller : MonoBehaviour {

    public GameObject controller;
    
    public float fall_speed = 10;
    public float turn_speed = 5f;

    public float xmove = 0, ymove = 0;

	
	// Update is called once per frame
	void FixedUpdate () {
        Vector3 leftturn = new Vector3(0, 0, -turn_speed);
        Vector3 rightturn = new Vector3(0, 0, turn_speed);
        float rot = transform.rotation.eulerAngles.z;
        if (rot >= 180f)
            rot -= 360f;

        foreach (Touch t in Input.touches)
        {
            if (t.position.x <  Screen.width/2)
            {
                if (rot >= -70)
                    transform.Rotate(leftturn);
            }
            if (t.position.x > Screen.width/2)
            {
                if (rot <= 70)
                    transform.Rotate(rightturn);
            }
        }

        xmove = fall_speed *  rot;
        ymove = fall_speed + ((70 - Math.Abs(rot)) * 0.1f);
        xmove /= 10000; ymove /= 200;

        Vector3 move = new Vector3(-xmove, 0, 0);

        gameObject.transform.position -= move; // fall
	}

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "rock")
            controller.GetComponent<spawner>().SendMessage("colliderock", collision.gameObject);
        else if (collision.tag == "gold")
            controller.GetComponent<spawner>().SendMessage("collidegold", collision.gameObject);
    }
}
