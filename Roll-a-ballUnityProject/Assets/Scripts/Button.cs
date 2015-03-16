using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Button : MonoBehaviour 
{
	public List<GameObject> allDoorsThisRoom;
	GameObject thisRoom;
    public static bool doorsOpen;
    Color m_ballColour;

	// Use this for initialization
	void Start () 
	{
		doorsOpen = false;
	}

    public void EnableDoors() {
        doorsOpen = false;
    }

	void OnCollisionEnter(Collision other)
	{
        if (other.gameObject.tag.Equals("Player")) {
            switch (this.tag) { 
                case "btnDoor":
                    m_ballColour = other.gameObject.GetComponent<Renderer>().material.color;
                    if (!doorsOpen) {
                        //GetDoors();
                        OpenDoors();
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

    /*
     * This function is no longer needed. 
     * Plus you guys were wasting resources and never clearing the list after querying it,
     * thus adding more and more duplicate entries on each call....
     * Duh!!
     */
	void GetDoors()
	{
        // Added another level of parent as it is now within a new hierarchy
        thisRoom = this.transform.parent.parent.parent.gameObject;  
		Debug.Log (thisRoom.name);
		allDoorsThisRoom.Add(thisRoom.transform.Find("Walls&Floor/WallT/Door/DoorLeft").gameObject);
		allDoorsThisRoom.Add(thisRoom.transform.Find("Walls&Floor/WallB/Door/DoorLeft").gameObject);
		allDoorsThisRoom.Add(thisRoom.transform.Find("Walls&Floor/WallL/Door/DoorLeft").gameObject);
		allDoorsThisRoom.Add(thisRoom.transform.Find("Walls&Floor/WallR/Door/DoorLeft").gameObject);

		allDoorsThisRoom.Add(thisRoom.transform.Find("Walls&Floor/WallT/Door/DoorRight").gameObject);
		allDoorsThisRoom.Add(thisRoom.transform.Find("Walls&Floor/WallB/Door/DoorRight").gameObject);
		allDoorsThisRoom.Add(thisRoom.transform.Find("Walls&Floor/WallL/Door/DoorRight").gameObject);
		allDoorsThisRoom.Add(thisRoom.transform.Find("Walls&Floor/WallR/Door/DoorRight").gameObject);
	}

	void OpenDoors()
	{

        GameObject[] surrounds = GameObject.FindGameObjectsWithTag("doorSurround");
        var sList = new List<GameObject>(surrounds);
        sList.RemoveAll(x => x.transform.root != this.transform.root);

        GameObject parent = null;

        foreach (var s in sList) {
            if (s.GetComponent<Renderer>().materials[0].color.Equals(m_ballColour)) {
                parent = s.transform.parent.gameObject;
            }
        }
        /*
        for (int i = 0; i < surrounds.Length; ++i) {
            if (surrounds[i].GetComponent<Renderer>().material.color.Equals(m_ballColour)) { 
                parent = surrounds[i].transform.parent.gameObject;
            }
        }
        */
        if (parent != null) {
            if (m_ballColour.Equals(Color.green) || m_ballColour.Equals(Color.red)) {
                parent.transform.FindChild("DoorLeft").Rotate(new Vector3(0, -90f, 0), Space.Self);
                parent.transform.FindChild("DoorRight").Rotate(new Vector3(0, 90f, 0), Space.Self);
            }
            else {
                parent.transform.FindChild("DoorLeft").Rotate(new Vector3(0, 90f, 0), Space.Self);
                parent.transform.FindChild("DoorRight").Rotate(new Vector3(0, -90f, 0), Space.Self);
            }
            doorsOpen = true;
        }
        
        /*
        foreach (GameObject door in allDoorsThisRoom)
		{
            if (door.GetComponent<Renderer>().material.color == m_ballColour) {
                open = door.transform.position;
                open.y += 3;
                door.transform.position += open;
                //doorsOpen = false;
            }
            
		}
        allDoorsThisRoom.Clear();
        */
	}
}
