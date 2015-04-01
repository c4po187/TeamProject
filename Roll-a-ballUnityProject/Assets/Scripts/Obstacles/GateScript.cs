using UnityEngine;
using System.Collections;

public class GateScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Player") {
			if (this.GetComponent<Renderer>().material.color == other.gameObject.GetComponent<Renderer>().material.color) {
				this.GetComponent<Collider>().isTrigger = true;
                Destroy(this.gameObject);
			} else {
				this.GetComponent<Collider>().isTrigger = false;
			}
		}
	}

	void OnCollisionEnter(Collision other) {
		if (other.gameObject.tag == "Player") {
			if (this.GetComponent<Renderer>().material.color == other.gameObject.GetComponent<Renderer>().material.color) {
				this.GetComponent<Collider>().isTrigger = true;
                Destroy(this.gameObject);
			} else {
				this.GetComponent<Collider>().isTrigger = false;
			}
		}
	}
}
