#region Prerequisites

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#endregion

#region Objects

public class Button : MonoBehaviour {
    
    #region Members

    public static bool doorsOpen;
    private GameObject m_door, m_backDoor;
    private Vector3 m_doorPos, m_backDoorPos;

    #endregion

    #region Functions

    // Use this for initialization
	void Start () {
        doorsOpen = false;
        m_door = m_backDoor = null;
        m_doorPos = m_backDoorPos = new Vector3();
	}

    public void EnableDoors() {
        doorsOpen = false;
    }

	void OnCollisionEnter(Collision other) {
        GameObject collidingObject = other.gameObject;
        if (collidingObject.tag.Equals("Player")) {
            switch (this.tag) { 
                case "btnDoor":
                    if (!doorsOpen) {
                        OpenDoors(ref collidingObject);
                    }
                    break;
                case "btnMagstrip":
                    GameObject.FindGameObjectWithTag("MagStrip")
                        .SendMessage("ModifyPolarity", Polarity.Positive);
                    break;
                default:
                    break;
            }
            
        }
	}

    void FixedUpdate() {
        if (m_door != null && m_backDoor != null) {
            m_door.transform.position = Vector3.MoveTowards(
                m_door.transform.position, m_doorPos, Time.deltaTime * 0.65f);
            m_backDoor.transform.position = Vector3.MoveTowards(
                m_backDoor.transform.position, m_backDoorPos, Time.deltaTime * 0.65f);
            if (m_door.transform.position.y >= m_doorPos.y
                && m_backDoor.transform.position.y >= m_backDoorPos.y) {
                m_door = null;
                m_backDoor = null;
            }
        }
    }

	private void OpenDoors(ref GameObject player) {

        Color ballColor = player.GetComponent<Renderer>().material.color;

        if (!ballColor.Equals(Color.white) && !doorsOpen) {
            string room = this.transform.root.name;

            GameObject[] doors = GameObject.FindGameObjectsWithTag("door");
            var dList = new List<GameObject>(doors);
            dList.RemoveAll(x => !x.transform.root.name.Equals(room));


            foreach (var d in dList) {
                if (d.GetComponent<Renderer>().materials[1].color.Equals(ballColor)) {
                    m_doorPos = d.transform.position;
                    d.GetComponent<AudioSource>().volume = 0.35f;
                    d.GetComponent<AudioSource>().Play();
                    m_door = d;
                    break;
                }
            }
            
            var iList = new List<GameObject>(doors);
            iList.RemoveAll(x => ReferenceEquals(x, m_door));
            foreach (var i in iList) {
                if (i.GetComponent<Renderer>().materials[1].color.Equals(
                    DoorTrigger.GetBackDoorColour(ballColor)) &&
                    !i.transform.root.name.Equals(room)) { 
                    
                }
            }
            
            m_backDoor = iList.Find(
                x => x.GetComponent<Renderer>().materials[1].color.Equals(
                    DoorTrigger.GetBackDoorColour(ballColor)) &&
                    !x.transform.root.name.Equals(room));
            m_backDoorPos = m_backDoor.transform.position;

            m_doorPos.y += 2f;
            m_backDoorPos.y += 2f;
            
            doorsOpen = true;
        }

        return;
    }

    #endregion
}

#endregion

// END OF FILE