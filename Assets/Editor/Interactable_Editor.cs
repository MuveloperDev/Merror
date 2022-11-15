using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor (typeof(Interactable), true), CanEditMultipleObjects]
public class Interactable_Editor : Editor
{
    private SerializedProperty MyType = null;
    private SerializedProperty AudioPlayable = null;
    private List<SerializedProperty> Audio = new List<SerializedProperty>();
    private string[] Audio_Menu;

    private SerializedProperty Moveable = null;
    private List<SerializedProperty> Move = new List<SerializedProperty>();
    private string[] Move_Menu;

    private SerializedProperty Rotatable = null;
    private List<SerializedProperty> Rotate = new List<SerializedProperty>();
    private string[] Rotate_Menu;

    private List<SerializedProperty> Lighting = new List<SerializedProperty>();
    private string[] Light_Menu;
    //private SerializedProperty Outlinable = null;
    private void Awake()
    {
        Move_Menu = new string[]
        {
            "Movement_Axis", "Target_Movement", "Movement_Speed", "InvertMovement"
        };
        Rotate_Menu = new string[] // Menu List
        {
            "Rotation_Axis", "Target_Angle", "Rotate_Speed",
            "Invert", "IsUsed", "AutoReset", "ResetTimer", "Reset_Speed"
        };
        Audio_Menu = new string[]
        {
            "_MyClip", "Clip_Name"
        };
        Light_Menu = new string[]
        {
            "_Light"
        };
    }
    private void OnEnable()
    {
        Move.Clear();
        Rotate.Clear(); // Clear List
        Audio.Clear();
        Lighting.Clear();

        MyType = serializedObject.FindProperty("myType");

        Moveable = serializedObject.FindProperty("Moveable");
        for (int i = 0; i < Move_Menu.Length; i++)
        {
            Move.Add(serializedObject.FindProperty(Move_Menu[i]));
        }

        Rotatable = serializedObject.FindProperty("Rotatable");
        for(int i = 0; i < Rotate_Menu.Length; i++) // Find property by menu name in string[]
        {
            Rotate.Add(serializedObject.FindProperty(Rotate_Menu[i]));
        }

        AudioPlayable = serializedObject.FindProperty("Audio_Playable");
        for (int i = 0; i < Audio_Menu.Length; i++)
        {
            Audio.Add(serializedObject.FindProperty(Audio_Menu[i]));
        }
        for (int i = 0; i < Light_Menu.Length; i++)
        {
            Lighting.Add(serializedObject.FindProperty(Light_Menu[i]));
        }
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        var iterator = serializedObject.GetIterator();

        if(iterator.NextVisible(true))
        {
            do
            {
                bool visible = true;
                foreach(SerializedProperty audioMenu in Audio) // Check in audio menu
                {
                    if(iterator.name == audioMenu.name) // If audio menu,
                    {
                        visible = AudioPlayable.boolValue; // Get audio playable bool value
                        break;
                    }
                }
                foreach (var moveMenu in Move)
                {
                    if (iterator.name == moveMenu.name)
                    {
                        visible = Moveable.boolValue;
                        break;
                    }
                }
                foreach(var rotateMenu in Rotate)
                {
                    if(iterator.name == rotateMenu.name)
                    {
                        visible = Rotatable.boolValue;
                        break;
                    }
                }
                foreach (var lightMenu in Lighting)
                {
                    if (iterator.name == lightMenu.name)
                    {
                        if (MyType.enumValueIndex != (int)Interactable.ObjectType.Switch)
                        {
                            iterator.objectReferenceValue = null;
                            visible = false;
                            break;
                        }
                    }
                }
                if(visible) // Is visible menu?
                {
                    EditorGUILayout.PropertyField(iterator, true); // Then show up
                }
            }
            while(iterator.NextVisible(false));
        }
        serializedObject.ApplyModifiedProperties(); // Apply modified.
    }
}
