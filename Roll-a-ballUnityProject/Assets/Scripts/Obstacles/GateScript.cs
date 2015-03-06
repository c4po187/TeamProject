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
			if (this.renderer.material.color == other.gameObject.renderer.material.color) {
				this.collider.isTrigger = true;
                Destroy(this.gameObject);
			} else {
				this.collider.isTrigger = false;
			}
		}
	}

	void OnCollisionEnter(Collision other) {
		if (other.gameObject.tag == "Player") {
			if (this.renderer.material.color == other.gameObject.renderer.material.color) {
				this.collider.isTrigger = true;
                Destroy(this.gameObject);
			} else {
				this.collider.isTrigger = false;
			}
		}
	}
}
