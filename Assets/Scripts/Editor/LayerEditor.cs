﻿using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

public class LayerEditor : EditorWindow {
    //parameters for LayerChanger
    private Transform floor;
    private Transform min;
    private Transform max;
    private int sensitivity = 3;


    [MenuItem("Tools/SetupLayer", false)]
    public static void ShowWindow() {
        GetWindow(typeof(LayerEditor));
    }

    private void OnGUI() {
        GUILayout.Label("Sprite Layer Setup", EditorStyles.boldLabel);
        //floor
        floor = EditorGUILayout.ObjectField("Floor", floor, typeof(Transform), true) as Transform;
        //min e max
        min = EditorGUILayout.ObjectField("Min", min, typeof(Transform), true) as Transform;
        max = EditorGUILayout.ObjectField("Max", max, typeof(Transform), true) as Transform;
        //sensitivity
        sensitivity = EditorGUILayout.IntField(sensitivity);
        //button to bake layer
        if (GUILayout.Button("Bake")) {
            StaticLayerChanger lc = new StaticLayerChanger(floor, min, max, sensitivity);
            lc.UpdateOrder();
        }
    }
}
