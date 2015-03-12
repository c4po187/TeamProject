using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

public enum SuperCubeState {
    Switching,
    Still
}

public enum SwitchPhase {
    Initial,
    Next,
    Final
}

/*
 * Quick and dirty class that holds a triple of any type.
 */
public class Triple<T1, T2, T3> {

    private T1 m_a;
    private T2 m_b;
    private T3 m_c;

    protected Triple(T1 a, T2 b, T3 c) {
        this.m_a = a;
        this.m_b = b;
        this.m_c = c;
    }

    public T1 Item1 {
        get { return this.m_a; }
    }

    public T2 Item2 {
        get { return this.m_b; }
    }

    public T3 Item3 {
        get { return this.m_c; }
    }

    /// <summary>
    /// Accessor for creating instances of Triple.
    /// </summary>
    /// <param name="item1">
    /// The first item.
    /// </param>
    /// <param name="item2">
    /// The second item.
    /// </param>
    /// <param name="item3">
    /// The third item.
    /// </param>
    /// <returns>
    /// A new instance of Triple.
    /// </returns>
    public static Triple<T1, T2, T3> Create(T1 item1, T2 item2, T3 item3) {
        return new Triple<T1, T2, T3>(item1, item2, item3);
    }

    /// <summary>
    /// Returns a string that represents the current object.
    /// </summary>
    /// <returns>A string that represents the current object.</returns>
    /// <filterpriority>2</filterpriority>
    public override string ToString() {
        return string.Concat(
            '{', this.Item1.ToString(), ", ", this.Item2.ToString(), ", ", this.Item3.ToString(), '}');
    }
}

/*
 * Scene starts from this class.
 */
public class Entry : MonoBehaviour {

    // Positioning Parameters
    private const float STEP_X = 20f,
                        STEP_Y = 5f,
                        SWITCH_SPEED = 12f;

    // References to the cubling prefabs
    public GameObject Room0, Room1, Room2, Room3, Room4, Room5, Room6, Room7, Room8, Room9,
                     Room10, Room11, Room12, Room13, Room14, Room15, Room16, Room17, Room18,
                     Room19, Room20, Room21, Room22, Room23, Room24, Room25;

    public GameObject m_switcher;
    private Vector3 m_centralVector, m_lastPosition, m_targetPosition;
    private Triple<int, int, int> m_lastLocation;

    // Multidimensional array to hold each cubling
    private GameObject[, ,] m_cublings;

    // Enum Variables
    private SuperCubeState m_scState;
    private SwitchPhase m_phase;

    private int m_switchCounter;

    // Countdown params
    private Stopwatch m_timer;
    private const long COUNTDOWN_DURATION = 120000;

    public long Countdown_ms {
        get { return m_timer.ElapsedMilliseconds; }
    }

    // Use this for initialization
    void Start() {
                
        m_scState = SuperCubeState.Still;
        m_timer = new Stopwatch();
        m_switchCounter = 0;

        /* 
         * Instantiate & fill array
         */
        m_cublings = new GameObject[3, 3, 3];

        // Bottom Layer
        m_cublings[0, 0, 0] = Room0;      // Start Room
        m_cublings[1, 0, 0] = Room1;
        m_cublings[2, 0, 0] = Room2;
        m_cublings[0, 0, 1] = Room3;
        m_cublings[1, 0, 1] = Room4;
        m_cublings[2, 0, 1] = Room5;
        m_cublings[0, 0, 2] = Room6;
        m_cublings[1, 0, 2] = Room7;
        m_cublings[2, 0, 2] = Room8;

        // Middle Layer
        m_cublings[0, 1, 0] = Room9;
        m_cublings[1, 1, 0] = Room10;
        m_cublings[2, 1, 0] = Room11;
        m_cublings[0, 1, 1] = Room12;
        m_cublings[1, 1, 1] = null;       // Centre Room: left empty for switching
        m_cublings[2, 1, 1] = Room13;
        m_cublings[0, 1, 2] = Room14;
        m_cublings[1, 1, 2] = Room15;
        m_cublings[2, 1, 2] = Room16;

        // Top Layer
        m_cublings[0, 2, 0] = Room17;
        m_cublings[1, 2, 0] = Room18;
        m_cublings[2, 2, 0] = Room19;
        m_cublings[0, 2, 1] = Room20;
        m_cublings[1, 2, 1] = Room21;
        m_cublings[2, 2, 1] = Room22;
        m_cublings[0, 2, 2] = Room23;
        m_cublings[1, 2, 2] = Room24;
        m_cublings[2, 2, 2] = Room25;     // End Room

        // Instantiate all the cubling prefabs
        for (int y = 0; y < 3; ++y) {
            for (int z = 0; z < 3; ++z) {
                for (int x = 0; x < 3; ++x) {
                    if (m_cublings[x, y, z] != null) {
                        m_cublings[x, y, z] =
                            (GameObject)Instantiate(
                                m_cublings[x, y, z],
                                new Vector3(x * STEP_X, y * STEP_Y, z * STEP_X),
                                Quaternion.identity);
                    }
                }
            }
        }

        m_centralVector = GetCublingVector(1, 1, 1);
    }

    /// <summary>
    /// Updates this instance.
    /// </summary>
    void Update() {
        /***** DEBUG: PERFORM SWITCH *****/
        if (Input.GetKeyDown(KeyCode.Home)) {
            StartShift();
        }

        if (this.Countdown_ms >= COUNTDOWN_DURATION) {
            ResetTimer();
            StartShift();
        }
    }

    /// <summary>
    /// Called to the fixed frame rate.
    /// </summary>
    void FixedUpdate() {
        if (m_scState.Equals(SuperCubeState.Switching)) {

            m_switcher.transform.position = Vector3.MoveTowards(
                m_switcher.transform.position, m_targetPosition, (SWITCH_SPEED * Time.deltaTime));

            if (m_switcher.transform.position == m_targetPosition)
                Next();
        }
    }

    /// <summary>
    /// Starts the shift.
    /// </summary>
    public void StartShift() {
        m_scState = SuperCubeState.Switching;
        m_phase = SwitchPhase.Initial;
        CubeSwitch();
    }

    /// <summary>
    /// Gets a Vector based on the row, column and aisle specified.
    /// </summary>
    /// <param name="x">
    /// Represents the horizontal cube location.
    /// </param>
    /// <param name="y">
    /// Represents the vertical cube location.
    /// </param>
    /// <param name="z">
    /// Represents the depth of the cube location.
    /// </param>
    /// <returns>
    /// An instance of Vector3.
    /// </returns>
    private Vector3 GetCublingVector(int x, int y, int z) {
        return new Vector3(x * STEP_X, y * STEP_Y, z * STEP_X);
    }

    /// <summary>
    /// Gets the array location of the cube based on its positional vector.
    /// </summary>
    /// <param name="position">
    /// A vector representing the 3-axis position.
    /// </param>
    /// <returns>
    /// A tuple of integers.
    /// </returns>
    private Triple<int, int, int> GetCublingLocation(Vector3 position) {
        return Triple<int, int, int>.Create(
            (int)(position.x % (STEP_X - 1f)), (int)(position.y % (STEP_Y - 1f)), (int)(position.z % (STEP_X - 1f)));
    }

    /// <summary>
    /// Changes to the next stage in the switch.
    /// </summary>
    private void Next() {
        // Make sure the array matches the current supercube configuration
        Triple<int, int, int> newLocation = GetCublingLocation(m_switcher.transform.position);
        m_cublings[newLocation.Item1, newLocation.Item2, newLocation.Item3] = m_switcher;

        if (m_phase.Equals(SwitchPhase.Initial)) m_phase = SwitchPhase.Next;

        if (m_phase.Equals(SwitchPhase.Final)) {
            m_switchCounter = 0;
            m_scState = SuperCubeState.Still;
            m_switcher = null;
        }
        else
            CubeSwitch();
    }

    /// <summary>
    /// Determines which cube in the matrix will be switching.
    /// </summary>
    private void CubeSwitch() {

        if (m_phase.Equals(SwitchPhase.Initial)) {
            List<GameObject> centrals = new List<GameObject>() { 
				m_cublings[1, 0, 1],
				m_cublings[1, 1, 0],
				m_cublings[0, 1, 1],
				m_cublings[2, 1, 1],
				m_cublings[1, 1, 2],
				m_cublings[1, 2, 1]
			};

            System.Random r = new System.Random();
            m_switcher = centrals[r.Next(0, centrals.Count)];
            m_targetPosition = m_centralVector;
        }

        if (m_phase.Equals(SwitchPhase.Next)) {
            bool useCentral = (m_switchCounter >= 3);
            Triple<int, int, int> nextLocation = GetRandomLocation(ref useCentral);
            m_switcher = m_cublings[nextLocation.Item1, nextLocation.Item2, nextLocation.Item3];
            m_targetPosition = m_lastPosition;

            // If we are to move the central cube, this is our last run in the cycle
            if (GetCublingVector(nextLocation.Item1, nextLocation.Item2, nextLocation.Item3).Equals(m_centralVector))
                m_phase = SwitchPhase.Final;
        }

        m_lastPosition = m_switcher.transform.position;

        ++m_switchCounter;
    }

    /// <summary>
    /// Gets a random location.
    /// </summary>
    /// <returns>A Triple of integers representing a random location.</returns>
    /// <param name="enableCentral">If set to <c>true</c> enable the central cube.</param>
    private Triple<int, int, int> GetRandomLocation(ref bool enableCentral) {
        System.Random r = new System.Random();

        m_lastLocation = GetCublingLocation(m_lastPosition);
        Triple<int, int, int> location = null;
        Triple<int, int, int> start = Triple<int, int, int>.Create(0, 0, 0);
        Triple<int, int, int> central = Triple<int, int, int>.Create(1, 1, 1);
        Triple<int, int, int> end = Triple<int, int, int>.Create(2, 2, 2);

        // First lets determine whether we are to add or subtract
        bool isAdd = (r.Next(0, 2) % 2 == 1);
        // Now pick a random axis to modify
        switch (r.Next(0, 3)) {
            case 0:
                location = (isAdd) ?
                    Triple<int, int, int>.Create((m_lastLocation.Item1 + 1), m_lastLocation.Item2, m_lastLocation.Item3) :
                        Triple<int, int, int>.Create((m_lastLocation.Item1 - 1), m_lastLocation.Item2, m_lastLocation.Item3);
                break;
            case 1:
                location = (isAdd) ?
                    Triple<int, int, int>.Create(m_lastLocation.Item1, (m_lastLocation.Item2 + 1), m_lastLocation.Item3) :
                        Triple<int, int, int>.Create(m_lastLocation.Item1, (m_lastLocation.Item2 - 1), m_lastLocation.Item3);
                break;
            case 2:
                location = (isAdd) ?
                    Triple<int, int, int>.Create(m_lastLocation.Item1, m_lastLocation.Item2, (m_lastLocation.Item3 + 1)) :
                        Triple<int, int, int>.Create(m_lastLocation.Item1, m_lastLocation.Item2, (m_lastLocation.Item3 - 1));
                break;
            default:
                break;
        }
        
        // Run function recursively if we meet the following conditions...
        if ((!enableCentral && IsSameLocation(location, central)) ||
            IsSameLocation(location, start) || IsSameLocation(location, end) || OutOfBounds(location)) {
            location = GetRandomLocation(ref enableCentral);
        }

        return location;
    }

    /// <summary>
    /// Determines whether two locations are the same.
    /// </summary>
    /// <returns><c>true</c> if both locations are the same otherwise, <c>false</c>.</returns>
    /// <param name="lhs">Left hand side Triple..</param>
    /// <param name="rhs">Right hand side Triple.</param>
    private bool IsSameLocation(Triple<int, int, int> lhs, Triple<int, int, int> rhs) {
        return (lhs.Item1.Equals(rhs.Item1) && lhs.Item2.Equals(rhs.Item2) && lhs.Item3.Equals(rhs.Item3));
    }

    /// <summary>
    /// Checks to see if the cube location is out of the bounds of the supercube matrix.
    /// </summary>
    /// <returns><c>true</c>, if location exceeds any of the boundaries, <c>false</c> otherwise.</returns>
    /// <param name="location">Represents the location of the cube.</param>
    private bool OutOfBounds(Triple<int, int, int> location) {
        return (location.Item1 < 0 || location.Item1 > 2 ||
                location.Item2 < 0 || location.Item2 > 2 ||
                location.Item3 < 0 || location.Item3 > 2);
    }

    public bool BeginShiftCountdown() {
        if (!m_timer.IsRunning) {
            m_timer.Start();
            return true;
        }
        return false;
    }

    public void ResetTimer() {
        m_timer.Stop();
        m_timer.Reset();
    }

    public void StopTimer() {
        m_timer.Stop();
    }
}
