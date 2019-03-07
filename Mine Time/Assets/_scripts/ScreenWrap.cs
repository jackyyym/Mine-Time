using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenWrap : MonoBehaviour
{
	
	
		//real margin beyond the viewport (1 is the size of the screen, see below)
		public float overflow = .05f; //<<<<<REMEMBER THE STUPID f
	
		// Use this for initialization
		void Start ()
		{
		
		}
	
		// Update is called once per frame
		void Update ()
		{
				//convert the ship's world position to viewport position
				//viewport coordinates are relative to the camera and range from 0 to 1 for everything that is on screen
				//x = 0 is the coordinate of the left edge of the screen.
				//x = 1 is the coordinate of the right edge of the screen.
				var cam = Camera.main;
				var viewportPosition = cam.WorldToViewportPoint (transform.position);
				//new position is the current position in case nothing changes
				var newPosition = viewportPosition;
		
				if (viewportPosition.x > 1 + overflow)
						newPosition.x = 0 - overflow;
		
				if (viewportPosition.x < 0 - overflow)
						newPosition.x = 1 + overflow;
		
				if (viewportPosition.y > 1 + overflow)
						newPosition.y = 0 - overflow;
		
				if (viewportPosition.y < 0 - overflow)
						newPosition.y = 1 + overflow;
		
				//convert back to world coordinates
				transform.position = cam.ViewportToWorldPoint (newPosition);
				
				//you can also convert to the position in pixels on the screen but beware it's relative to the camera
				//Vector2 screenPos = cam.WorldToScreenPoint (transform.position);
				//Debug.Log ("triangle is " + screenPos.x + " pixels from the left");
		}
}

