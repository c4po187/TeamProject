#region Prerequisites

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#endregion

#region Objects

[ExecuteInEditMode]
[AddComponentMenu("Location/Spawn Point")]
public class SpawnPoint : MonoBehaviour {

    #region Members

    private GameObject actor;
    public string actorTag;
    public Vector3 spawnLocation;
    public List<Vector3> spawnLocations;
    public int startIndex;

    #endregion

    #region Functions

    void Awake() {
        spawnLocations = new List<Vector3>();
    }

    void Start() {
        actor = GameObject.FindGameObjectWithTag(actorTag);
        startIndex = 0;
    }

    public void SetTag(string s) {
        actorTag = s;
    }

    public string GetTag() {
        return actorTag;
    }

    public void AddSpawnLocation(Vector3 v) {
        spawnLocations.Add(v);
    }

    public void AddSpawnLocation(float x, float y, float z) {
        AddSpawnLocation(new Vector3(x, y, z));
    }

    public List<Vector3> GetSpawnLocations() {
        return spawnLocations;
    }

    public Vector3 GetSpawnLocationAt(int index) {
        return spawnLocations[index];
    }

    public void RemoveSpawnLocationAt(int index) {
        spawnLocations.RemoveAt(index);
    }

    public void ClearSpawnLocations() {
        spawnLocations.Clear();
    }

    public void SetSpawnLocation(Vector3 v) {
        spawnLocation = v;
    }

    public void SetSpawnlocation(float x, float y, float z) {
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
        if (spawnLocations.Count == 0) {
            actor.transform.position = spawnLocation;
        }
        else {
            actor.transform.position = spawnLocations[startIndex];
            if (startIndex < spawnLocations.Count - 1)
                ++startIndex;
        }
    }

    public void Respawn(Vector3 newLocation) {
        actor.transform.position = newLocation;
    }

    public void Respawn(float x, float y, float z) {
        actor.transform.position = new Vector3(x, y, z);
    }

    #endregion
}

#endregion
