using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class LegacyAnimationTool : EditorWindow
{
    [MenuItem("Tools/Convert Animations to Legacy")]
    public static void ShowWindow()
    {
        // �� â�� ���ϴ�.
        EditorWindow.GetWindow<LegacyAnimationTool>("Legacy Animation Converter");
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Convert All Animations to Legacy"))
        {
            ConvertAnimationsToLegacy();
        }
    }

    private void ConvertAnimationsToLegacy()
    {
        // �ּ� ���� ���� ��� �ִϸ��̼� Ŭ���� ã���ϴ�.
        string[] guids = AssetDatabase.FindAssets("t:AnimationClip", new[] { "Assets" });

        foreach (string guid in guids)
        {
            // �� �ִϸ��̼� Ŭ���� ��θ� ����ϴ�.
            string path = AssetDatabase.GUIDToAssetPath(guid);
            AnimationClip clip = AssetDatabase.LoadAssetAtPath<AnimationClip>(path);

            if (clip != null)
            {
                clip.legacy = true;
            }
        }

        // �ּ� �����ͺ��̽� ����
        AssetDatabase.Refresh();
    }
}
