using UnityEngine;
using System.Collections;

#region Enumerators

public enum Relativity { 
    Ground = 1,
    Wall = Ground << 1,
    Ceiling = Ground << 2
}

#endregion

public class Controls : MonoBehaviour 
{
	public float currentSpeed;
	public float maxSpeed;
	private float distToGround;

    private GameObject m_currentSurface;
    public ConstantForce GravityOffset;

    public Relativity BallRelativity { get; private set; }
    public Vector3 WallDirectional { get; private set; }

    public void Set_BallRelativity(Relativity r) {
        BallRelativity = r;
    }

    public void Set_WallDirection(Vector3 v) {
        WallDirectional = v;
    }

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
        BallRelativity = Relativity.Ground;
        WallDirectional = Vector3.zero;
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

        Debug.Log(this.transform.position.y.ToString());

        #if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
        /* 
         * Check for player keyboard input and move ball accordingly
         */
        maxSpeed = 12.5f;
		if (GetComponent<Rigidbody>().velocity.magnitude < maxSpeed)
		{
            if (isGrounded() || BallRelativity.Equals(Relativity.Wall)) {
                if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
                    GetComponent<Rigidbody>().AddForce(Vector3.forward * maxSpeed);
                }
                if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
                    GetComponent<Rigidbody>().AddForce(Vector3.back * maxSpeed);
                }
                if (Input.GetKey(KeyCode.A) | Input.GetKey(KeyCode.LeftArrow)) {
                    if (BallRelativity.Equals(Relativity.Wall)) 
                        GetComponent<Rigidbody>().AddForce(Vector3.up * (maxSpeed * 0.5f));
                    else
                        GetComponent<Rigidbody>().AddForce(Vector3.left * maxSpeed);
                }
                if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
                    GetComponent<Rigidbody>().AddForce(Vector3.right * maxSpeed);
                }
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

    void FixedUpdate() {
        switch (BallRelativity) { 
            case Relativity.Ground:
                // Simulates normal gravity
                GetComponent<Rigidbody>().AddForce(-Vector3.up * Physics.gravity.magnitude);
                break;
            case Relativity.Wall:
                GetComponent<Rigidbody>().AddForce(WallDirectional * Physics.gravity.magnitude);
                break;
            case Relativity.Ceiling:
                GetComponent<Rigidbody>().AddForce(Vector3.up * Physics.gravity.magnitude);
                break;
        }
    }

    public bool isGrounded() {
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
        /*
        if (other.gameObject.tag.Equals("magstrip")) 
            m_currentSurface = other.gameObject;
        */
          
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
    /*
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
     * */
}
