using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class LegacyAnimationTool : EditorWindow
{
    [MenuItem("Tools/Convert Animations to Legacy")]
    public static void ShowWindow()
    {
        // 툴 창을 엽니다.
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
        // 애셋 폴더 내의 모든 애니메이션 클립을 찾습니다.
        string[] guids = AssetDatabase.FindAssets("t:AnimationClip", new[] { "Assets" });

        foreach (string guid in guids)
        {
            // 각 애니메이션 클립의 경로를 얻습니다.
            string path = AssetDatabase.GUIDToAssetPath(guid);
            AnimationClip clip = AssetDatabase.LoadAssetAtPath<AnimationClip>(path);

            if (clip != null)
            {
                clip.legacy = true;
            }
        }

        // 애셋 데이터베이스 갱신
        AssetDatabase.Refresh();
    }
}
