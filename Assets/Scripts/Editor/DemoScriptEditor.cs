using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DemoScript))]
public class DemoScriptEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Perform Bad (not) Background Task"))
        {
            DemoScript demoScript = (DemoScript)target;
            demoScript.PerformBackgroundTask();
        }

        if (GUILayout.Button("Perform C# Async Task"))
        {
            DemoScript demoScript = (DemoScript)target;
            demoScript.PerformCSharpAsync_Task();
        }

        if (GUILayout.Button("Perform Editor Coroutine Task"))
        {
            DemoScript demoScript = (DemoScript)target;
            demoScript.PerformEditorCoroutine_Task();
        }

        if (GUILayout.Button("Perform Compound Task"))
        {
            DemoScript demoScript = (DemoScript)target;
            demoScript.PerformCompoundTask();
        }
    }
}
