using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BoxOutlinerScript))]
public class BoxOutlinerEditor : Editor {

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		BoxOutlinerScript myScript = (BoxOutlinerScript)target;
		if(GUILayout.Button("Fix outline"))
		{
			myScript.DoOutline();
		}
	}
}
