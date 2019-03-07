using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skycontroller : MonoBehaviour {

    public GameObject player;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        float ymove = player.GetComponent<player_controller>().ymove;
        transform.position += new Vector3(0, ymove, 0);
        if (transform.position.y >= 9)
            Destroy(gameObject);
    }
}
