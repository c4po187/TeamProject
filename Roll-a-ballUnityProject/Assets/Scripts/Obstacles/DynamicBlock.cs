#region Prerequisites

using UnityEngine;
using System.Collections;
using System.Diagnostics;
using System;

#endregion

#region Enumerators

public enum DynamicDirection { 
    ToEnd,
    ToStart
}

public enum DynamicState { 
    Waiting,
    InitTimer,
    Moving
}

[Flags]
public enum DynamicAxis { 
    _Undef = 0x0,
    X_Axis = 0x01,
    Y_Axis = X_Axis << 1,
    Z_Axis = Y_Axis << 1
}

#endregion

#region Objects

public class DynamicBlock : MonoBehaviour {

    #region Members

    public Vector3 startPosition, endPosition;
    public float delay;
    public float speed;
    private Stopwatch m_timer;
    private DynamicDirection m_dDir;
    private DynamicState m_dState;
    private DynamicAxis m_dAxis;

    #endregion

    void Awake() {
        delay = 2000;
        speed = 2.5f;
        m_dDir = DynamicDirection.ToEnd;
        m_dState = DynamicState.Moving;
        m_dAxis = (DynamicAxis.X_Axis | DynamicAxis.Y_Axis | DynamicAxis.Z_Axis);
        startPosition = this.transform.position;
        this.transform.position = new Vector3(
            startPosition.x, startPosition.y, startPosition.z - 0.001f);
    }

    void Start() {
        m_timer = new Stopwatch();
    }

	// Update is called once per frame
	void Update () {
        // DBG
        if (this.transform.root.name.Equals("Room_3")) {
            UnityEngine.Debug.Log("X: " + this.transform.position.x.ToString() +
                    ", Y: " + this.transform.position.y.ToString() +
                    ", Z: " + this.transform.position.z.ToString());
        }
        
        if (m_dState.Equals(DynamicState.Moving)) {
            if (this.transform.position.Equals(startPosition) || 
                this.transform.position.Equals(endPosition)) {
                m_dState = DynamicState.InitTimer;
            }
        }
        if (m_dState.Equals(DynamicState.InitTimer)) {
            m_timer.Start();
            m_dState = DynamicState.Waiting;
        }
        if (m_dState.Equals(DynamicState.Waiting)) {
            if (m_timer.ElapsedMilliseconds >= delay) {
                m_timer.Reset();
                m_dState = DynamicState.Moving;
                m_dDir = (m_dDir.Equals(DynamicDirection.ToStart)) 
                    ? DynamicDirection.ToEnd : DynamicDirection.ToStart;
                /*
                 * Nudge the position slightly, otherwise we will keep on
                 * triggering a reset of the delay timer and movements will
                 * either not happen or glitch.
                 */
                this.transform.position = (m_dDir.Equals(DynamicDirection.ToEnd))
                    ? new Vector3(startPosition.x, startPosition.y, startPosition.z - 0.001f)
                    : new Vector3(endPosition.x, endPosition.y, endPosition.z + 0.001f);
            }
        }
	}

    void FixedUpdate() {
        if (m_dState.Equals(DynamicState.Moving)) {
            if (m_dDir.Equals(DynamicDirection.ToEnd)) {
                this.transform.position = Vector3.MoveTowards(
                    this.transform.position, endPosition, speed * Time.deltaTime);
            }
            if (m_dDir.Equals(DynamicDirection.ToStart)) {
                this.transform.position = Vector3.MoveTowards(
                    this.transform.position, startPosition, speed * Time.deltaTime);
            }
        }
    }
}


#endregion