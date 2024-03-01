using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//Credit: Sebastian Lague, champion of the people
//https://www.youtube.com/watch?v=rQG9aUWarwE

[CustomEditor(typeof(NPCSensory))]
public class EditorNPCSensors : Editor
{
    private void OnSceneGUI()
    {
        NPCSensory sensory = (NPCSensory)target;
        Handles.color = Color.white;
        Vector3 pos = sensory.transform.position;
        float angle = sensory.LookAngle;
        float distance = sensory.LookDistance;

        Handles.DrawWireArc(pos, Vector3.up, Vector3.forward, 360f, distance);

        Vector3 dirA = sensory.DirFromAngle(-angle / 2f);
        Vector3 dirB = sensory.DirFromAngle(angle / 2f);

        Handles.DrawLine(pos, pos + dirA * distance);
        Handles.DrawLine(pos, pos + dirB * distance);
    }
}
