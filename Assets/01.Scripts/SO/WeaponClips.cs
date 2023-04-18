using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;
using Tools;
using UnityEditor;
using UnityEditor.UIElements;

// Character Clip
[CreateAssetMenu(fileName = "ClipsData", menuName = "ScriptableObject/ClipsData")]
public class WeaponClips : ScriptableObject
{
    [SerializeField] private string type;
    [SerializeField] private string actorName;
    [SerializeField]
    private int weaponID = -1;
    [SerializeField] private string weaponName;
    public int WeaponID => weaponID;

    [ContextMenuItem("Add Sprite", "AddSprite")]
    [SerializeField]
    private List<ClipBase> clips;
    private void AddSprite()
    {
        foreach (var clip in clips)
        {
            if(weaponID != -1)
                clip.texture = Resources.Load<Texture2D>($"Sprites/{type}/{actorName}/{weaponName}/{clip.name}");
            else
            {
                clip.texture = Resources.Load<Texture2D>($"Sprites/{type}/{actorName}/{clip.name}");
            }
        }
        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
    public List<ClipBase> Clips => clips;
}