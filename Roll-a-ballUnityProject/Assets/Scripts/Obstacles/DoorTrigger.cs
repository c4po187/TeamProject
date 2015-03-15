#region Prerequisites

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#endregion

#region Objects

public struct ColorMask {
    public Color color;
    public uint co_mask;
}

public class DoorTrigger : MonoBehaviour {

    #region Members

    private string m_previousRoom, m_currentRoom;
    private Color m_currentDoorColor, m_backDoorColor;
    private List<ColorMask> m_colorLookUp;
    private const uint BACK_MASK = 0x07;

    #endregion

    #region Functions

    void Awake() {
        m_colorLookUp = new List<ColorMask>() { 
            new ColorMask { color = new Color(1f, 0, 0), co_mask = 0x02 },  // Red
            new ColorMask { color = new Color(0, 1f, 0), co_mask = 0x04 },  // Green
            new ColorMask { color = new Color(0, 0, 1f), co_mask = 0x05 },  // Blue
            new ColorMask { color = new Color(1f, 1f, 0), co_mask = 0x03 }  // Yellow
        };
    }

	void Start () {
        m_previousRoom = m_currentRoom = string.Empty;
	}

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag.Equals("Player")) {
            // Grab the previous room string from the ball controller
            m_previousRoom = other.GetComponent<Controls>().CurrentRoom;
            // Get the current room string via this objects hierarchy
            m_currentRoom = this.transform.parent.parent.parent.parent.name;
            
            // Store the current room & previous room names in the ball controller
            other.gameObject.SendMessage("Set_PreviousRoom", m_previousRoom);
            other.gameObject.SendMessage("Set_CurrentRoom", m_currentRoom);

            // If we are not in the same room as before, return the ball to white
            if (!other.GetComponent<Controls>().CurrentRoom.Equals(
                other.GetComponent<Controls>().PreviousRoom)) {
                other.gameObject.SendMessage("Set_BallColour", Color.white);
                m_currentDoorColor = this.transform.parent.FindChild("DoorRight")
                    .GetComponent<Renderer>().material.color;
                m_backDoorColor = GetBackDoorColour(m_currentDoorColor);
            }
        }
    }

    /// <summary>
    /// Gets the base colour of the door that is behind the 
    /// this door in the current room).
    /// Uses bitmasking to match the set values associated
    /// with the base colors in a look up table.
    /// </summary>
    /// <param name="color">Represents the color of this 
    /// door (adjacent to the trigger).</param>
    /// <returns>The color of the door behind.</returns>
    private Color GetBackDoorColour(Color color) {
        uint masked = m_colorLookUp.Find(x => x.color.Equals(color)).co_mask ^ BACK_MASK;

        return m_colorLookUp.Find(x => x.co_mask.Equals(masked)).color;
    }

    #endregion
}

#endregion