using UnityEngine;
using System.Collections;

public class Controls : MonoBehaviour 
{
	public float currentSpeed;
	public float maxSpeed;
	private float distToGround;

    private GameObject m_currentSurface;
    public ConstantForce GravityOffset;

    /*
    public ConstantForce GravityOffset {
        get {
            if (m_gravityOffset == null)
                m_gravityOffset = this.gameObject.AddComponent<ConstantForce>();
            return m_gravityOffset;
        } set { m_gravityOffset = value; }
    } 
    */
	// Use this for initialization
	void Start () 
	{
		//Physics.gravity.Set(0.0f,-1000f,0.0f);
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
		distToGround = GetComponent<Collider>().bounds.extents.y;
		maxSpeed = 10f;
	}

	// Update is called once per frame
	void Update () 
	{
		currentSpeed = GetComponent<Rigidbody>().velocity.magnitude;

        //if (!IsGrounded()) Debug.Log("Airbourne");
        //else Debug.Log("Grounded");

        #if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
        /* 
         * Check for player keyboard input and move ball accordingly
         */
        maxSpeed = 20f;
		if (GetComponent<Rigidbody>().velocity.magnitude < maxSpeed && IsGrounded())
		{
        	if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
			{
				GetComponent<Rigidbody>().AddForce(Vector3.forward * maxSpeed);
			}
			if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
			{
				GetComponent<Rigidbody>().AddForce(Vector3.back * maxSpeed);
			}
			if (Input.GetKey(KeyCode.A) | Input.GetKey(KeyCode.LeftArrow))
			{
				GetComponent<Rigidbody>().AddForce(Vector3.left * maxSpeed);
			}
			if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
			{
				GetComponent<Rigidbody>().AddForce(Vector3.right * maxSpeed);
			}
		}
        #endif

        #if UNITY_ANDROID
        /*
         * Move ball based on accelerometer values
         */
        Vector3 movement = new Vector3(Input.acceleration.x, 0f, Input.acceleration.y); //-y

//        if (movement.sqrMagnitude > 1)
//            movement.Normalize();
        
		if (rigidbody.velocity.magnitude < maxSpeed && IsGrounded())
		{
        	rigidbody.AddForce(movement * 50f);
		}
        #endif
	}

	bool IsGrounded()
	{
		return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
	}

	//added by adam
	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Pickup")
		{
			this.GetComponent<Renderer>().material.color = other.GetComponent<Renderer>().material.color;
			other.gameObject.SetActive(false);
		}
	}

	void OnCollisionEnter(Collision other)
	{
		// todo: there is a problem with the relative position where it is taking the 
		//local y coordinate of the sphere to do the above/bellow calculation meaning 
		//sometimes it triggers above and sometimes not depending on which way up the sphere is.

		Vector3 contactPoint = other.contacts[0].point;
		var relativePosition = transform.InverseTransformPoint(contactPoint);
		//Debug.Log ("CP= " + contactPoint + " RP= " + relativePosition);

        // Check to see if we have hit the magnetic strip
        if (other.gameObject.tag.Equals("magstrip")) 
            m_currentSurface = other.gameObject;

		// dont want to colide with objects we are rollling on
		if (!(relativePosition.y > 0))
		{
            
            
            //Debug.Log("The object is not above.");

			
			if (other.gameObject.name != "FloorTL" &&
			    other.gameObject.name != "FloorBL" &&
			    other.gameObject.name != "FloorTR" &&
			    other.gameObject.name != "FloorBR")
			{
				Debug.Log(other.gameObject.name);
				this.GetComponent<AudioSource>().Play();
				#if UNITY_ANDROID
				Handheld.Vibrate();
				#endif
			}
            
		}
	}

    void OnCollisionStay(Collision other) {
        if (other.gameObject.Equals(m_currentSurface)) {
            ContactPoint cp = other.contacts[0];
            GravityOffset.force = (-1f * Physics.gravity) + (-1f * cp.normal);
        }
    }

    void OnCollisionExit(Collision other) {
        if (other.gameObject.Equals(m_currentSurface)) {
            m_currentSurface = null;
            GravityOffset.force = Vector3.zero;
        }
    }
}
