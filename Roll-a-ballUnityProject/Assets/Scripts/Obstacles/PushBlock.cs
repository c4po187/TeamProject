#region Prerequisites

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

#endregion

#region Enumerators

public enum MoveAxis { 
    X_,
    Y_,
    Z_
}

#endregion

#region Objects

public class PushBlock : MonoBehaviour {

    #region Private Members

    private bool b_ballCollideBounds;
    private MoveAxis m_moveAxis;
    private string m_room;

    #endregion

    #region Public Members

    public float FrontBound;
    public float BackBound;

    #endregion

    #region Functions

	void Start () {
        b_ballCollideBounds = false;
        m_room = GetRoom();
        m_moveAxis = DetermineAxis();
	}

    private string GetRoom() {
        return this.transform.root.name; 
    }

    private MoveAxis DetermineAxis() {
        if (m_room != string.Empty) {
            if (m_room.Equals("Room_4")) return MoveAxis.Z_;
            if (m_room.Equals("Room_5")) return MoveAxis.X_;
        }
        return MoveAxis.Z_;
    }

    void Update() { ; }

    void FixedUpdate() {
        switch (m_moveAxis) { 
            case MoveAxis.X_:
                if ((this.transform.position.x <= this.FrontBound || this.transform.position.x >= this.BackBound) &&
                    !b_ballCollideBounds) {
                    this.GetComponent<Rigidbody>().isKinematic = true;
                }
                else {
                    this.GetComponent<Rigidbody>().isKinematic = false;
                }
                break;
            case MoveAxis.Y_:
                if ((this.transform.position.y >= this.FrontBound || this.transform.position.y <= this.BackBound) &&
                    !b_ballCollideBounds) {
                    this.GetComponent<Rigidbody>().isKinematic = true;
                }
                else {
                    this.GetComponent<Rigidbody>().isKinematic = false;
                }
                break;
            case MoveAxis.Z_:
                if ((this.transform.position.z >= this.FrontBound || this.transform.position.z <= this.BackBound) &&
                    !b_ballCollideBounds) {
                    this.GetComponent<Rigidbody>().isKinematic = true;
                }
                else {
                    this.GetComponent<Rigidbody>().isKinematic = false;
                }
                break;
        }
    }

    void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag.Equals("Player")) {
            switch (m_moveAxis) {
                case MoveAxis.X_:
                    if (other.gameObject.transform.position.x > this.BackBound ||
                        other.gameObject.transform.position.x < this.FrontBound) {
                        b_ballCollideBounds = true;
                    }
                    break;
                case MoveAxis.Y_:
                    if (other.gameObject.transform.position.y > this.FrontBound ||
                        other.gameObject.transform.position.y < this.BackBound) {
                        b_ballCollideBounds = true;
                    }
                    break;
                case MoveAxis.Z_:
                    if (other.gameObject.transform.position.z > this.FrontBound ||
                        other.gameObject.transform.position.z < this.BackBound) {
                        b_ballCollideBounds = true;
                    }
                    break;
            }
        } else {
            b_ballCollideBounds = false;
        }
    }

    void OnCollisionStay(Collision other) {
        b_ballCollideBounds = (other.gameObject.tag.Equals("Player") && 
            other.gameObject.transform.position.y > 0.6f);
    }

    #endregion
}

#endregion

// END OF FILE                                                                                                                                                                                                                                                                                                                                                                                       bbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                  vvvvvvvvvvvvdddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddd                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                          c                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                      cccccccccccccccccccccccccccccccc                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                       