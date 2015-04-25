#region Prerequisites

using System;
using System.Collections.Generic;
using UnityEngine;

#endregion

#region Objects

public class Teleporter : MonoBehaviour {

    #region Enumerators

    private enum TeleportAction { 
        Source,
        Destination
    }
    
    #endregion

    #region Members

    private TeleportAction m_action;

    #endregion

    #region Functions

    void Awake() {
        m_action = TeleportAction.Source;
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag.Equals("Player")) {
            if (m_action.Equals(TeleportAction.Source)) { 
                // Get a new Teleporter
                Teleporter d = GetDestination();
                other.gameObject.transform.position = d.gameObject.transform.position;
            }
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.gameObject.tag.Equals("Player"))
            m_action = TeleportAction.Source;
    }

    private void SetAction(TeleportAction action) {
        m_action = action;
    }

    private Teleporter GetDestination() {
        GameObject t_destination = null;
        // First off lets compile an array of Teleporters, and make a list from it
        List<GameObject> teleporters = new List<GameObject>(
            GameObject.FindGameObjectsWithTag("teleporter"));
        // Remove this teleporter from list
        teleporters.RemoveAll(x => ReferenceEquals(x, this.gameObject));
        // Remove all teleporters that are lower than this, (we wanna ascend after all)
        teleporters.RemoveAll(x => x.transform.position.y <= this.transform.position.y);

        if (teleporters.Count > 0) {
            // Grab a random Teleporter from what is left, and set its action to destination
            System.Random r = new System.Random();
            t_destination = teleporters[r.Next(0, teleporters.Count)];
            t_destination.GetComponent<Teleporter>().SetAction(TeleportAction.Destination);
        }

        return t_destination.GetComponent<Teleporter>();
    }

    #endregion
}

#endregion