using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(Prism))]
public class PrismEditor : Editor {

    [MenuItem("GameObject/Create Other/Prism")]
    static void Create() {
        GameObject gameObject = new GameObject("Prism");
        Prism prism = gameObject.AddComponent<Prism>();
        MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();
        meshFilter.mesh = new Mesh();
        prism.Rebuild();
    }

    public override void OnInspectorGUI() {
        Prism obj;
        obj = target as Prism;
        if (obj == null)
            return;

        base.DrawDefaultInspector();
        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Rebuild"))
            obj.Rebuild();

        EditorGUILayout.EndHorizontal();
    }
}
