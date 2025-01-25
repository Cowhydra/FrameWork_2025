using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;
using UnityEditor.Animations;
using UnityEditor;

public static partial class AssetServer
{
    // AnimatorController ĳ��
    private static readonly Dictionary<string, AnimatorController> _cachedAnimatorControllers = new Dictionary<string, AnimatorController>();

    public static bool GetAnimatorController(string controllerPath,out AnimatorController ca)
    {
        ca = null;

        // ĳ�� Ȯ��
        if (_cachedAnimatorControllers.TryGetValue(controllerPath, out var controller))
        {
            ca= controller;
            return true;
        }

        // ���ο� AnimatorController ����
        ca = Load<AnimatorController>(controllerPath);
        if (ca == null)
        {
            return false;
        }

        _cachedAnimatorControllers[controllerPath] = controller;
        return true;
    }
}
