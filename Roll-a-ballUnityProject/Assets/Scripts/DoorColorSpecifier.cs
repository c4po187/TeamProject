#region Prerequisites

using System;
using UnityEngine;

#endregion

#region Component

[ExecuteInEditMode]
[AddComponentMenu("Doors/Door Color Specifier")]
public class DoorColorSpecifier : MonoBehaviour {

    #region Properties

    public Color inside, outside;

    #endregion

    void Start() { ; }
}

#endregion
