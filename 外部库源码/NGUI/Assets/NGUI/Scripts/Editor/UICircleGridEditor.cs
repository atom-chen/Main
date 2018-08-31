//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2015 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(UICircleGrid), true)]
public class UICircleGridEditor : UIWidgetContainerEditor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        NGUIEditorTools.DrawProperty("Angle Offset", serializedObject, "angleOffset");
        NGUIEditorTools.DrawProperty("Center Radius", serializedObject, "centerRadius");
        NGUIEditorTools.DrawProperty("Ring Between", serializedObject, "ringBetween");

        SerializedProperty sp;
        GUILayout.BeginHorizontal();
        {
            sp = NGUIEditorTools.DrawProperty("Circle Limit", serializedObject, "maxPerLine");
            if (sp.intValue < 0) sp.intValue = 0;
            if (sp.intValue == 0) GUILayout.Label("Unlimited");
        }
        GUILayout.EndHorizontal();

        NGUIEditorTools.DrawProperty("Sorting", serializedObject, "sorting");
        NGUIEditorTools.DrawProperty("Smooth Tween", serializedObject, "animateSmoothly");
        NGUIEditorTools.DrawProperty("Hide Inactive", serializedObject, "hideInactive");
        NGUIEditorTools.DrawProperty("Constrain to Panel", serializedObject, "keepWithinPanel");
        serializedObject.ApplyModifiedProperties();

        /*SerializedProperty sp = NGUIEditorTools.DrawProperty("Arrangement", serializedObject, "arrangement");

        NGUIEditorTools.DrawProperty("  Cell Radius", serializedObject, "cellRadius");

		if (sp.intValue < 2)
		{
			bool columns = (sp.hasMultipleDifferentValues || (UIGrid.Arrangement)sp.intValue == UIGrid.Arrangement.Horizontal);

			GUILayout.BeginHorizontal();
			{
				sp = NGUIEditorTools.DrawProperty(columns ? "  Column Limit" : "  Row Limit", serializedObject, "maxPerLine");
				if (sp.intValue < 0) sp.intValue = 0;
				if (sp.intValue == 0) GUILayout.Label("Unlimited");
			}
			GUILayout.EndHorizontal();

			UIGrid.Sorting sort = (UIGrid.Sorting)NGUIEditorTools.DrawProperty("Sorting", serializedObject, "sorting").intValue;

			if (sp.intValue != 0 && (sort == UIGrid.Sorting.Horizontal || sort == UIGrid.Sorting.Vertical))
			{
				EditorGUILayout.HelpBox("Horizontal and Vertical sortinig only works if the number of rows/columns remains at 0.", MessageType.Warning);
			}
		}*/
    }
}
