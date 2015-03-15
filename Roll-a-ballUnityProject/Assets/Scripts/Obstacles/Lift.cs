#region Prerequisites

using UnityEngine;
using System.Collections;
using System.Diagnostics;

#endregion

#region Enumerators

public enum LiftState { 
    Idle = 0,
    Up = 1,
    AtTop = Up << 1,
    DropDelay = Up << 2,
    Down = Up << 3
}

#endregion

#region Objects

public class Lift : MonoBehaviour {

    #region Members

    public long delay;
    public float top, speed;
    private const float BOTTOM = 1.05f;
    private LiftState m_liftState;
    private Stopwatch m_timer;

    #endregion

    #region Functions

    // Called on Script creation - Init
    void Awake() {
        top = 8f;
        speed = 2.5f;
        delay = 750;
        m_liftState = LiftState.Idle;
    }

    // Called on runtime, and the object is enabled - Init
	void Start () {
        m_timer = new Stopwatch();
	}

	// Update is called once per frame
    void Update() { ; }

    /*
     * FixedUpdate gets called relative to the current framerate, 
     * best for physics, translations, etc..
     */
    void FixedUpdate() {
        if (m_liftState.Equals(LiftState.Idle) && this.transform.position.y > BOTTOM) {
            this.transform.Translate(Vector3.down * (Time.deltaTime * speed), Space.World);
        }
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag.Equals("Player")) {
            if (m_timer.IsRunning)
                m_timer.Reset();
            m_timer.Start();
        }
    }

    void OnTriggerStay(Collider other) {
        if (other.gameObject.tag.Equals("Player")) {
            if (m_timer.ElapsedMilliseconds >= delay) {
                delay = 0;

                if (m_liftState.Equals(LiftState.Idle)) {
                    m_liftState = LiftState.Up;                   
                }

                if (this.transform.position.y >= top) {
                    m_liftState = LiftState.AtTop;
                }

                if (m_liftState.Equals(LiftState.AtTop)) {
                    m_timer.Reset();
                    m_liftState = LiftState.DropDelay;
                }

                if (m_liftState.Equals(LiftState.DropDelay)) {
                    delay = 2000;
                    m_liftState = LiftState.Down;
                    m_timer.Start();
                }

                if (m_liftState.Equals(LiftState.Up) && this.transform.position.y < top) {
                    this.transform.Translate(Vector3.up * (Time.deltaTime * speed), Space.World);
                }

                if (m_liftState.Equals(LiftState.Down) && this.transform.position.y > BOTTOM) {
                    this.transform.Translate(Vector3.down * (Time.deltaTime * speed), Space.World);
                }
            }
        }
    }

    void OnTriggerExit(Collider other) {
        m_liftState = LiftState.Idle;
        delay = 750;
        m_timer.Reset();
    }

    #endregion

}

#endregion

// END OF FILE