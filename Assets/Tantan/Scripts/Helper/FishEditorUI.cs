using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Fish))]
public class FishEditorUI : Editor
{
    public override void OnInspectorGUI()
    {
        Fish fish = (Fish)target;

        EditorGUILayout.LabelField("Fish Type", EditorStyles.boldLabel);
        fish.fishType = (FishType)EditorGUILayout.EnumPopup("Fish Type", fish.fishType);
        switch (fish.fishType)
        {
            case FishType.Common:
                fish.commonFishType = (CommonFishType)EditorGUILayout.EnumPopup("Common Fish Type", fish.commonFishType);
                break;
            case FishType.Uncommon:
                fish.uncommonFishType = (UncommonFishType)EditorGUILayout.EnumPopup("Uncommon Fish Type", fish.uncommonFishType);
                break;
            case FishType.Rare:
                fish.rareFishType = (RareFishType)EditorGUILayout.EnumPopup("Rare Fish Type", fish.rareFishType);
                break;
            case FishType.Legendary:
                fish.legendaryFishType = (LegendaryFishType)EditorGUILayout.EnumPopup("Legendary Fish Type", fish.legendaryFishType);
                break;
        }
        EditorGUILayout.Separator();
        EditorGUILayout.LabelField("Fish Properties", EditorStyles.boldLabel);
        fish.swimSpeed = EditorGUILayout.FloatField("Swim Speed", fish.swimSpeed);
        fish.fishPoint = EditorGUILayout.IntField("Fish Point", fish.fishPoint);
        fish.resistanceForce = EditorGUILayout.FloatField("Resistance Force", fish.resistanceForce);
        fish.fishVisionRange = EditorGUILayout.FloatField("Fish Vision Range", fish.fishVisionRange);
    }
}
