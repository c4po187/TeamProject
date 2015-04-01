#region Prerequisites

using UnityEngine;
using System.Collections;
using System.Diagnostics;
using System.Threading;

#endregion

#region Enumerators

public enum Polarity { 
    Negative = 0,
    Positive = 1
}

#endregion

public class MagStrip : MonoBehaviour {

    #region Members

    private const long POS_DURATION = 10000;  
    private const long DELAY = 100;              
    private Stopwatch m_timer, m_delayTimer;
    private ContactPoint m_contact;
    private bool mb_isRepel;
    private Vector3 m_force;

    #endregion

    #region Properties

    public Polarity StripPolarity;

    #endregion

    #region Functions

    // Use this for initialization
	void Start () {
        mb_isRepel = false;
        m_timer = new Stopwatch();
        m_delayTimer = new Stopwatch();
        StripPolarity = Polarity.Negative;
	}
	
	// Update is called once per frame
	void Update () {  
        if (m_timer.IsRunning) {
            if (m_timer.ElapsedMilliseconds >= POS_DURATION) {
                m_timer.Reset();
                StripPolarity = Polarity.Negative;
                GameObject.FindGameObjectWithTag("Player").SendMessage("Set_BallRelativity", Relativity.Ground);
            }
        }

        if (m_delayTimer.IsRunning) {
            if (m_delayTimer.ElapsedMilliseconds >= DELAY) {
                m_delayTimer.Reset();
                m_timer.Reset();
                StripPolarity = Polarity.Negative;
                GameObject.FindGameObjectWithTag("Player").SendMessage("Set_BallRelativity", Relativity.Ground);
            }
        }
    }

    void FixedUpdate() {
        if (StripPolarity.Equals(Polarity.Positive)) {
            this.GetComponent<WindZone>().windMain = 2.5f;
            this.GetComponent<WindZone>().windTurbulence = 1f;
            ParticleSystem.Particle[] particles =
                new ParticleSystem.Particle[this.GetComponent<ParticleSystem>().maxParticles];
            this.GetComponent<ParticleSystem>().GetParticles(particles);
            for (int i = 0; i < particles.Length; ++i) {
                particles[i].color = new Color32((byte)12, (byte)82, (byte)144, (byte)180);
            }
            this.GetComponent<ParticleSystem>().SetParticles(particles, particles.Length);
        }
        else {
            this.GetComponent<WindZone>().windMain = 0f;
            this.GetComponent<WindZone>().windTurbulence = 0f;
        } 
    }

    public void ModifyPolarity(Polarity polarity) {
        StripPolarity = polarity;
        if (polarity.Equals(Polarity.Positive))
            m_timer.Start();
    }

    void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag.Equals("Player")) {
            m_contact = other.contacts[0];
        }
            
    }

    void OnCollisionStay(Collision other) {
        if (other.gameObject.tag.Equals("Player")) {
            if (StripPolarity.Equals(Polarity.Positive)) {
                other.gameObject.SendMessage("Set_BallRelativity", Relativity.Wall);
                other.gameObject.SendMessage("Set_WallDirection", m_contact.normal);
            }
        }            
    }

    void OnCollisionExit(Collision other) {
        if (other.gameObject.tag.Equals("Player")) {
            if (other.transform.position.y > 6f) {
                m_delayTimer.Start();
            } else {
                StripPolarity = Polarity.Negative;
                m_timer.Reset();
                other.gameObject.SendMessage("Set_BallRelativity", Relativity.Ground);
            }
        }
    }

    void OnTriggerEnter(Collider other) {
        if (StripPolarity.Equals(Polarity.Negative)) {
            if (other.gameObject.tag.Equals("Player")) {
                mb_isRepel = true;
                m_force = other.gameObject.GetComponent<Rigidbody>().velocity.normalized;
            }
        }
    }
    
    void OnTriggerStay(Collider other) {
        if (StripPolarity.Equals(Polarity.Negative)) {
            if (other.gameObject.tag.Equals("Player") && mb_isRepel) {
                float xk = .5f, zk = .01f;
                float x = (m_force.x >= 0f) ? m_force.x : -m_force.x;
                other.gameObject.GetComponent<Rigidbody>()
                    .AddForce(new Vector3(x * xk, 0f, -m_force.z * zk), ForceMode.Impulse);
            }
        }
    }

    void OnTriggerExit(Collider other) { 
        if (other.gameObject.tag.Equals("Player"))
            mb_isRepel = false;
    }

    #endregion
}
