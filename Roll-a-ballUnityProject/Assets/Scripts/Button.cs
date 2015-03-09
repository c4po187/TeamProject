using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Button : MonoBehaviour 
{
	public List<GameObject> allDoorsThisRoom;
	GameObject thisRoom;
	bool doorsOpen;
    Color m_ballColour;

	// Use this for initialization
	void Start () 
	{
		doorsOpen = false;
	}

	void OnCollisionEnter(Collision other)
	{
        if (other.gameObject.tag == "Player") {
            m_ballColour = other.gameObject.GetComponent<Renderer>().material.color;
            //if (!doorsOpen) {
            //    Debug.Log("button pressed");
            //    GetDoors();
            //    OpenDoors();
            //    doorsOpen = true;
            //}
            GetDoors();
            OpenDoors();
        }
	}

	void GetDoors()
	{
		thisRoom = this.transform.parent.parent.gameObject;
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
		foreach (GameObject door in allDoorsThisRoom)
		{
			Vector3 open; 

            if (door.GetComponent<Renderer>().material.color == m_ballColour) {
                open = door.transform.position;
                open.y += 3;
                door.transform.position += open;
                //doorsOpen = false;
            }
		}
	}
}
