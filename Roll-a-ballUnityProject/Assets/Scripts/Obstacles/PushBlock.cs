using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PushBlock : MonoBehaviour {

    #region Private Members

    private bool b_ballCollideBounds;

    #endregion

    #region Public Members

    public float FrontBound_Z;
    public float BackBound_Z;

    #endregion

    #region Functions

	void Start () {
        b_ballCollideBounds = false;
	}

    void Update() {
        if ((this.transform.position.z >= this.FrontBound_Z || this.transform.position.z <= this.BackBound_Z) && 
            !b_ballCollideBounds) {
            this.GetComponent<Rigidbody>().isKinematic = true;
        }
        else {
            this.GetComponent<Rigidbody>().isKinematic = false;
        }
    }

    void FixedUpdate() { ; }

    void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag.Equals("Player")) {
            if (other.gameObject.transform.position.z > this.FrontBound_Z ||
                other.gameObject.transform.position.z < this.BackBound_Z) {
                b_ballCollideBounds = true;
            }
        }
        else {
            b_ballCollideBounds = false;
        }
    }

    #endregion
}