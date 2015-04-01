using UnityEngine;
using System.Collections;

public class Wall : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag.Equals("Player"))
            this.GetComponent<AudioSource>().Play();
    }
}
