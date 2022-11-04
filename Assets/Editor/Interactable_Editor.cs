using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;

[CustomEditor (typeof(Interactable), true), CanEditMultipleObjects]
public class Interactable_Editor : Editor
{
    private SerializedProperty MyType = null;
    private SerializedProperty AudioPlayable = null;
    private List<SerializedProperty> Audio = new List<SerializedProperty>();
    private string[] Audio_Menu;

    private SerializedProperty Rotatable = null;
    private List<SerializedProperty> Rotate = new List<SerializedProperty>();
    private string[] Rotate_Menu;

    private List<SerializedProperty> Lighting = new List<SerializedProperty>();
    private string[] Light_Menu;
    //private SerializedProperty Outlinable = null;
    private void Awake()
    {
        Rotate_Menu = new string[] // Menu List
        {
            "Rotation_Axis", "Target_Angle", "Rotate_Speed",
            "Invert", "IsUsed", "AutoReset", "ResetTimer", "Reset_Speed"
        };
        Audio_Menu = new string[]
        {
            "_MyClip", "Type", "Clip_Name"
        };
        Light_Menu = new string[]
        {
            "_Light"
        };
    }
    private void OnEnable()
    {
        Rotate.Clear(); // Clear List
        Audio.Clear();
        Lighting.Clear();

        MyType = serializedObject.FindProperty("myType");
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
                foreach(var audioMenu in Audio) // Check in audio menu
                {
                    if(iterator.name == audioMenu.name) // If audio menu,
                    {
                        visible = AudioPlayable.boolValue; // Get audio playable bool value
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
