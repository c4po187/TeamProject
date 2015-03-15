#region Prerequisites

using UnityEngine;
using System.Collections;
using System.Diagnostics;

#endregion

public class AcidBath : MonoBehaviour {

    #region Members

    private GameObject[] m_grounds;
    private Stopwatch m_respawnTimer;
    private long m_delay;

    #endregion

    #region Functions

    // Use this for initialization
	void Start () {
        m_grounds = GameObject.FindGameObjectsWithTag("ground");
        m_respawnTimer = new Stopwatch();
        m_delay = 2500;
	}
	
	// Update is called once per frame
	void Update () {
        if (m_respawnTimer.IsRunning) {
            if (m_respawnTimer.ElapsedMilliseconds >= m_delay) {
                m_respawnTimer.Reset();
                GameObject.FindGameObjectWithTag("Player").GetComponent<SpawnPoint>().Respawn();
            }
        }
	}

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag.Equals("Player")) {
            foreach (var g in m_grounds) {
                Physics.IgnoreCollision(other, g.GetComponent<Collider>());
            }
        }
    }

    void OnTriggerStay(Collider other) {
        if (other.gameObject.tag.Equals("Player")) {  
            other.gameObject.SendMessage("Set_BallRelativity", Relativity.Sinking);
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.gameObject.tag.Equals("Player")) {
            other.gameObject.SendMessage("Set_BallRelativity", Relativity.Ground);
            foreach (var g in m_grounds) {
                Physics.IgnoreCollision(other, g.GetComponent<Collider>(), false);
            }
            // Init delay timer for respawn
            m_respawnTimer.Start();
        }
    }

    #endregion
}
