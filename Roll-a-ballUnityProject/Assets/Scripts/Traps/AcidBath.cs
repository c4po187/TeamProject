#region Prerequisites

using UnityEngine;
using System.Collections;
using System.Diagnostics;

#endregion

/*
 * TODO:
 * We need to implement some sort of penalty system,
 * which we can inflict upon the player if they do 
 * happen to fall into this here trap.
 * We could use it for other things we deem as a
 * penalty also.
 * 
 * Perhaps a separate class somewhere that is 
 * either static or follows the singleton pattern?
 */

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
                // Reset the timer, respawn and rescale the player
                m_respawnTimer.Reset();
                GameObject.FindGameObjectWithTag("Player").GetComponent<SpawnPoint>().Respawn();
                GameObject.FindGameObjectWithTag("Player").SendMessage("Rescale");
                foreach (var c in DoorTrigger.ColorLookUp) {
                    if (GameObject.FindGameObjectWithTag("Player").GetComponent<Renderer>().material.color.Equals(c.color))
                        GameObject.FindGameObjectWithTag("Player").GetComponent<Renderer>().material.color = Color.white;
                }
            }
        }
	}

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag.Equals("Player")) {
            foreach (var g in m_grounds) {
                // Disable collisions between the player and floor, so we can sink
                Physics.IgnoreCollision(other, g.GetComponent<Collider>());
                other.gameObject.GetComponent<ParticleSystem>().enableEmission = true;
            }
        }
    }

    void OnTriggerStay(Collider other) {
        if (other.gameObject.tag.Equals("Player")) {  
            other.gameObject.SendMessage("Set_BallRelativity", Relativity.Sinking);
            // Ensure the ball faces towards the node, so we can see the ball's dissolve particle animation
            other.gameObject.transform.LookAt(this.GetComponent<SpawnPoint>().GetSpawnLocation());
            if (!this.GetComponent<AudioSource>().isPlaying) {
                this.GetComponent<AudioSource>().pitch = 1.5f;
                this.GetComponent<AudioSource>().Play();
            }
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.gameObject.tag.Equals("Player")) {
            other.gameObject.SendMessage("Set_BallRelativity", Relativity.Ground);
            foreach (var g in m_grounds) {
                // Turn collisions back on between the player and the floor
                Physics.IgnoreCollision(other, g.GetComponent<Collider>(), false);
            }
            // Init delay timer for respawn
            m_respawnTimer.Start();
        }
    }

    #endregion
}
