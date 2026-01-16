using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FishPossibilities))]
public class FishPossibilityEditorUI : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        var common = serializedObject.FindProperty("common");
        var uncommon = serializedObject.FindProperty("uncommon");
        var rare = serializedObject.FindProperty("rare");
        var legendary = serializedObject.FindProperty("legendary");

        EditorGUILayout.PropertyField(common);

        float maxUncommon = 100f - common.floatValue;
        maxUncommon = Mathf.Max(0f, maxUncommon);

        uncommon.floatValue = EditorGUILayout.Slider(
            "Uncommon",
            uncommon.floatValue,
            0f,
            maxUncommon
        );

        float maxRare = 100f - common.floatValue - uncommon.floatValue;
        maxRare = Mathf.Max(0f, maxRare);

        rare.floatValue = EditorGUILayout.Slider(
            "Rare",
            rare.floatValue,
            0f,
            maxRare
        );

        float maxLegendary = 100f - common.floatValue - uncommon.floatValue - rare.floatValue;
        maxLegendary = Mathf.Max(0f, maxLegendary);

        legendary.floatValue = EditorGUILayout.Slider(
            "Legendary",
            legendary.floatValue,
            0f,
            maxLegendary
        );
        serializedObject.ApplyModifiedProperties();
    }
}
