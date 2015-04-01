#region Prerequisites

using System;
using System.Collections.Generic;
using UnityEngine;

#endregion

#region Objects

[Serializable]
public class Map : Dictionary<string, Vector3> {
    public string key;
    public Vector3 location;
}

#endregion
