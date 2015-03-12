using UnityEngine;
using System.Collections;

public class ColourChange : MonoBehaviour {

	public string colour;

	// Use this for initialization
	void Start () {
		switch (colour)
		{
		case "Orange":
                this.GetComponent<Renderer>().material.color = Color.Lerp(Color.yellow, Color.red, 0.5f);
			break;

		case "Candy":
            this.GetComponent<Renderer>().material.color = Color.Lerp(Color.magenta, Color.green, 0.3725f);
			break;

		case "Green":
			this.GetComponent<Renderer>().material.color = Color.green;
			break;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
