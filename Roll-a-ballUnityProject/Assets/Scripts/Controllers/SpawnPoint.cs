#region Prerequisites

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

#endregion

#region Objects

/*
 * USAGE:
 * The easiest way to use this component is from the inspector, as a component.
 * There are of course public functions if access is required via a script.
 * 
 * If all you need is a single spawn point, use Spawn Location.
 * If you require multiple spawn points or reference nodes, then use nodes. 
 * You may assign as many as you wish. Always give each node a unique key
 * which is simply a string, as each node is accessed by this key.
 */

[ExecuteInEditMode]
[AddComponentMenu("Location/Spawn Point")]
public class SpawnPoint : MonoBehaviour {

    /*
     * NOTE:
     * Never instantiate a list, Vector among other types
     * if used as a public property in a custom component
     * such as this SpawnPoint class. The list will be
     * re-allocated memory on runtime, erasing the values
     * you previously entered.
     * Just declare, and enter the values in the inspector,
     * Unity will take care of the rest.
     */

    #region Members

    private const float STEP = 20f;

    private GameObject m_currentRoom;
    public GameObject actor;
    public string actorTag;
    public Vector3 spawnLocation;
    public List<Map> nodes;
    
    [ReadOnly]
    public string currentRoom;

    #endregion

    #region Functions

    void Awake() {
        try {
            actor = this.gameObject;
        }
        catch (Exception) {
            throw new Exception("Could not attach actor!");
        }
        if (actor == null)
            actor = GameObject.FindGameObjectWithTag(actorTag);
        GetCurrentRoom();
    }

    private void GetCurrentRoom() {
        GameObject[] floors = GameObject.FindGameObjectsWithTag("ground");
        foreach (var f in floors) {
            if (f.GetComponent<Collider>().bounds.Intersects(actor.GetComponent<Collider>().bounds)) {
                m_currentRoom = f.transform.root.gameObject;
                break;
            }
        }
        if (m_currentRoom != null)
            currentRoom = m_currentRoom.name;
    }

    public void SetTag(string s) {
        actorTag = s;
    }

    public string GetTag() {
        return actorTag;
    }

    public void AddNode(string room, Vector3 location) {
        Map map = new Map();
        map.Add(room, location);
        nodes.Add(map);
    }

    public Vector3 GetLocationFrom(string room) {
        return nodes.Find(x => x.key.Equals(room)).location;
    }

    public string GetRoomFrom(Vector3 location) {
        return nodes.Find(x => x.location.Equals(location)).key;
    }

    public void RemoveNodeAtKey(string room) {
        nodes.RemoveAll(x => x.key.Equals(room));
    }

    public void ClearNodes() {
        nodes.Clear();
    }

    public void SetSpawnLocation(Vector3 v) {
        spawnLocation = v;
    }

    public void SetSpawnLocation(float x, float y, float z) {
        spawnLocation = new Vector3(x, y, z);
    }

    public Vector3 GetSpawnLocation() {
        return spawnLocation;
    }

    public void SetActor(GameObject _actor) {
        actor = _actor;
    }

    public GameObject GetActor() {
        return actor;
    }

    public void Respawn() {
        if (nodes.Count == 0) 
            actor.transform.position = spawnLocation;
        else 
            actor.transform.position = nodes.Find(x => x.key.Equals(currentRoom)).location;
    }

    public void Respawn(Vector3 newLocation) {
        actor.transform.position = newLocation;
    }

    public void Respawn(float x, float y, float z) {
        actor.transform.position = new Vector3(x, y, z);
    }

    public bool ApplyLocationTransformation(string key, Vector3 direction, bool applyToChildren) {
        bool loc_transformed = false;
        bool obj_transformed = false;

        if (nodes.Count == 0 &&
            (direction.Equals(Vector3.up) || direction.Equals(Vector3.down) ||
            direction.Equals(Vector3.left) || direction.Equals(Vector3.right))) {
            return false;
        } else {
            nodes.Find(x => x.key.Equals(key)).location =
                (nodes.Find(x => x.key.Equals(key)).location + (direction * STEP));
            loc_transformed = true;
            if (applyToChildren) {
                if (GameObject.Find(key)) {
                    foreach (Transform child in GameObject.Find(key).transform) {
                        child.position = (child.position + (direction * STEP));                    
                    }
                    obj_transformed = true;
                }
            }
        }

        // Finally
        if (applyToChildren)
            return loc_transformed && obj_transformed;
        else
            return loc_transformed;
    }



    #endregion
}

#endregion
