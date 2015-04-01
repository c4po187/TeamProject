using UnityEngine;
using System.Collections;

public class ColourSwitchParticles : MonoBehaviour {

    private Color[] m_colors;

    private const float DURATION = 5f;
    private float m_currentTime = 0f;
    private int m_index = 0;

	// Use this for initialization
	void Start () {
        // Fill up array with our colours
        m_colors = new Color[4];
        m_colors[0] = new Color(1, 0, 0, 1);    // Red
        m_colors[1] = new Color(0, 1, 0, 1);    // Green
        m_colors[2] = new Color(0, 0, 1, 1);    // Blue
        m_colors[3] = new Color(1, 1, 0, 1);    // Yellow

        // Start the particles as blue
        this.GetComponent<ParticleSystem>().startColor = m_colors[0];
	}
	
	// Update is called once per frame
	void Update () {
        m_currentTime += Time.deltaTime;

        if (m_currentTime > DURATION) {
            m_currentTime = 0f;
            ++m_index;
        }

        if (m_index > 3) m_index = 0;

        this.GetComponent<ParticleSystem>().startColor = m_colors[m_index];
	}

    void OnParticleCollision(GameObject other) {
        if (other.gameObject.tag == "Player") {
            Rigidbody body = other.GetComponent<Rigidbody>();
            if (body) {
                other.gameObject.GetComponent<Renderer>().material.color = m_colors[m_index];
            }
        }
    }
}
