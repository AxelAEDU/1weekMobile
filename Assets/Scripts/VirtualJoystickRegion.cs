using UnityEngine;
using System.Collections;

public class VirtualJoystickRegion : MonoBehaviour
{
	public Texture joystickBackground;
	public Texture joystickThumb;
	public float backgroundWidth, backgroundHeight, thumbWidth, thumbHeight, thumbMaxDistance;
	public bool dissapearOnRelease;
	//public int imageAspect;
	public Region region;
	public enum Region { LEFT, RIGHT, BOTTOMLEFT, BOTTOMRIGHT }; //bottomLeft and bottomRight use regionHeight
	public float regionHeight; //0-1
	public float x = 0;
	public float y = 0;
	bool invertY;


	Vector3 mouseDownPosition;
	bool draw = false;

	// Use this for initialization
	void Start()
	{



	}

	// Update is called once per frame
	void Update()
	{
		if (float.IsNaN(x))
		{
			x = 0;
		}
		if (float.IsNaN(y))
		{
			y = 0;
		}
	}

	void LateUpdate()
	{


	}

	void OnGUI()
	{
		Vector3 mouse = new Vector3(0, 0, 0);
		//PRESS
		if (Input.GetMouseButtonDown(0))
		{
			//set the draw position of the background
			mouseDownPosition = GUIUtility.ScreenToGUIPoint(Input.mousePosition);
			Debug.Log("Mouse: " + mouseDownPosition + " width:" + Screen.width / 2 + " height: " + Screen.height + " regionHeight: " + Screen.height * regionHeight);
			draw = false;
			switch (region)
			{
				case Region.LEFT:
					if (mouseDownPosition.x < Screen.width / 2)
					{
						draw = true;
						Debug.Log("LEFT");
					}
					break;
				case Region.RIGHT:
					if (mouseDownPosition.x >= Screen.width / 2)
					{
						draw = true;
						Debug.Log("Right");
					}
					break;
				case Region.BOTTOMLEFT:
					if (mouseDownPosition.x < Screen.width / 2 && mouseDownPosition.y < Screen.height * regionHeight)
					{
						draw = true;
						Debug.Log("BL");
					}
					break;
				case Region.BOTTOMRIGHT:
					if (mouseDownPosition.x >= Screen.width / 2 && mouseDownPosition.y < Screen.height * regionHeight)
					{
						draw = true;
						Debug.Log("BR");
					}
					break;
			}
		}


		//RELEASE
		if (Input.GetMouseButtonUp(0))
		{
			if (dissapearOnRelease)
			{
				draw = false;
			}
			x = 0;
			y = 0;
		}

		if (draw)
		{
			//get the screen positon to draw the backgrond and thumb textures at. Based on the mousedown position
			Vector2 backgroundDrawPos = new Vector2(mouseDownPosition.x - backgroundWidth / 2, Screen.height - mouseDownPosition.y - backgroundHeight / 2);
			Vector2 thumbDrawPos = new Vector2(mouseDownPosition.x - thumbWidth / 2, Screen.height - mouseDownPosition.y - thumbHeight / 2);

			//draw the background
			GUI.DrawTexture(new Rect(backgroundDrawPos.x, backgroundDrawPos.y, backgroundWidth, backgroundHeight), joystickBackground, ScaleMode.ScaleToFit, true, 1);

			//HOLD
			if (Input.GetMouseButton(0))
			{
				Vector3 mousePos = GUIUtility.ScreenToGUIPoint(Input.mousePosition); //get the current mouse positon in GUIspace
				thumbDrawPos = new Vector2(mousePos.x - thumbWidth / 2, Screen.height - mousePos.y - thumbHeight / 2);//update the thumb draw position
				Vector2 center1 = new Vector2(backgroundDrawPos.x + backgroundWidth / 2, backgroundDrawPos.y + backgroundHeight / 2);
				Vector2 center2 = new Vector2(thumbDrawPos.x + thumbWidth / 2, thumbDrawPos.y + thumbHeight / 2);
				Vector2 toThumb = center2 - center1; // get the vector from the mousedownPos to the currentMouse pos
				float distance = Vector2.Distance(center1, center2); // get the distance 
																	 //Debug.Log("Distance: " + distance);
				if (distance > thumbMaxDistance) // if we're < the max distance
				{
					//otherwise draw the texture at the max distance
					thumbDrawPos = center1 + toThumb.normalized * thumbMaxDistance - new Vector2(thumbWidth / 2, thumbHeight / 2);
				}

				if (invertY)
				{
					x = toThumb.x / toThumb.magnitude;
					y = toThumb.y / toThumb.magnitude;
				}
				else
				{
					x = toThumb.x / toThumb.magnitude;
					y = -toThumb.y / toThumb.magnitude;
				}
				//Debug.Log("("+x+","+y+")");
			}
			//draw the joystickThumb at the correct position
			GUI.DrawTexture(new Rect(thumbDrawPos.x, thumbDrawPos.y, thumbWidth, thumbHeight), joystickThumb, ScaleMode.ScaleToFit, true, 1);
		}

		if (float.IsNaN(x))
		{
			x = 0;
		}
		if (float.IsNaN(y))
		{
			x = 0;
		}
	}
}
