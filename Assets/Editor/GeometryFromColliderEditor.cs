using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GeometryFromColliderScript))]
public class GeometryFromColliderEditor : Editor {

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		GeometryFromColliderScript myScript = (GeometryFromColliderScript)target;

		if(GUILayout.Button("Generate Geometry")) {
			myScript.GenerateGeometry();
		}
	}
}
