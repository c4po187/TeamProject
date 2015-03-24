#region Prerequisites

using UnityEngine;
using System.Collections;

#endregion

#region Objects

public class Pendulum : MonoBehaviour {

    #region Members

    public float angle, speed;
    public string left_right;

    private Quaternion m_qStart, m_qEnd;
    private bool m_bDroppedIn;

    #endregion

    #region Functions

    void Awake() { m_bDroppedIn = false; }

    // Use this for initialization
	void Start () {
        m_qStart = Quaternion.AngleAxis(-angle, Vector3.forward);
        m_qEnd = Quaternion.AngleAxis(angle, Vector3.forward);
        
        if (!left_right.Equals(string.Empty)) {
            if (left_right.Equals("Left") || left_right.Equals("left")) 
                this.transform.rotation = m_qStart;
            else if (left_right.Equals("Right") || left_right.Equals("right")) 
                this.transform.rotation = m_qEnd;
            else 
                this.transform.rotation = m_qStart;
        } else 
            this.transform.rotation = m_qStart;
        
        if (angle.Equals(null)) angle = 45f;

        if (speed.Equals(null)) speed = 1.5f;
	}
	
	// Update is called once per frame
    void Update() { ; }

    void FixedUpdate() {
        if (!m_bDroppedIn) {
            if (left_right.Equals("Left") || left_right.Equals("left")) {
                this.transform.rotation = Quaternion.RotateTowards(m_qStart, m_qEnd, Time.time * Mathf.Pow(speed, 8.4f));
                if (this.transform.rotation.x >= m_qEnd.x)
                    m_bDroppedIn = true;
            }
            else {
                this.transform.rotation = Quaternion.RotateTowards(m_qEnd, m_qStart, Time.time * Mathf.Pow(speed, 8.4f));
                if (this.transform.rotation.x <= m_qStart.x)
                    m_bDroppedIn = true;
            }
        } else {
            if (left_right.Equals("Left") || left_right.Equals("left")) {
                this.transform.rotation = Quaternion.Lerp(
                    m_qEnd, m_qStart, (Mathf.Sin(Time.time * speed) + 1f) / 2f);
            } else {
                this.transform.rotation = Quaternion.Lerp(
                    m_qStart, m_qEnd, (Mathf.Sin(Time.time * speed) + 1f) / 2f);
            }
        }
    }

    /*
    void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag.Equals("Player")) {
            if (other.gameObject.transform.position.z <= (this.GetComponent<Collider>().bounds.center.z
                + this.GetComponent<Collider>().bounds.extents.z) &&
                other.gameObject.transform.position.z >= (this.GetComponent<Collider>().bounds.center.z
                - this.GetComponent<Collider>().bounds.extents.z)) {
                if (other.gameObject.transform.position.x < this.GetComponent<Collider>().bounds.center.x)
                    other.gameObject.SendMessage("NegateLaunchSpeed");
            }
        }
    }

    void OnCollisionExit(Collision other) {
        if (other.gameObject.tag.Equals("Player")) {
            other.gameObject.SendMessage("Set_BallRelativity", Relativity.Launched);
        }
    }
    */

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag.Equals("Player")) {
            if (other.gameObject.transform.position.z <= (this.GetComponent<Collider>().bounds.center.z
                + this.GetComponent<Collider>().bounds.extents.z) &&
                other.gameObject.transform.position.z >= (this.GetComponent<Collider>().bounds.center.z
                - this.GetComponent<Collider>().bounds.extents.z)) {
                float force = 15f;
                if (other.gameObject.transform.position.x < this.GetComponent<Collider>().bounds.center.x)
                    force = -force;        
                other.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(force, 0f, 0f), ForceMode.Impulse);
            }
        } 
    }

    /*
    void OnTriggerExit(Collider other) {
        if (other.gameObject.tag.Equals("Player")) {
            other.gameObject.SendMessage("Set_BallRelativity", Relativity.Launched);
        }
    }
    */
    #endregion
}

#endregion

// END OF FILE