#if UNITY_EDITOR
using System;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public static class CopyPathMenuItem
{
    [MenuItem("GameObject/Copy Path", false, 0)]
    private static void CopyPath()
    {
        var go = Selection.activeGameObject;
 
        if (go == null)
        {
            return;
        }
 
        var path = go.name;
 
        while (go.transform.parent != null)
        {
            go = go.transform.parent.gameObject;
            path = string.Format("{0}/{1}", go.name, path);
        }
 
        EditorGUIUtility.systemCopyBuffer = path;
    }
 
    [MenuItem("GameObject/Copy Path", true)]
    private static bool CopyPathValidation()
    {
        // We can only copy the path in case 1 object is selected
        return Selection.gameObjects.Length == 1;
    }

    [MenuItem("GameObject/Copy CodeRef", false, 0)]
    private static void CopyCodeRef()
    {
        var gos = Selection.gameObjects;
 
        if (gos == null || gos.Length == 0)
            return;

        string finalPath = "";

        foreach(var goItem in gos)
        {
            var go = goItem;

            //Get component type
            Type goType = GetSpecifyCompType(go);
            var variableName = string.Format(goType == typeof(GameObject) ? "m_Go{0}" : "m_{0}", go.name);
            string compPath = goType == typeof(GameObject) ? "gameObject" : string.Format("GetComponent<{0}>()", goType.Name.ToString());

            //Get gameobject path
            var path = go.name;
            while (go.transform.parent != null)
            {
                go = go.transform.parent.gameObject;
                path = string.Format("{0}/{1}", go.name, path);
            }

            //Merge reference
            finalPath += string.Format("{0} = transform.Find(\"{1}\").{2};\n", variableName, path, compPath);
        }
        
        EditorGUIUtility.systemCopyBuffer = finalPath;
    }

    private static Type GetSpecifyCompType(GameObject go)
    {
        if(go.name.Contains("Btn"))
        {
            return typeof(Button);
        }
        else if(go.name.Contains("Txt"))
        {
            return go.GetComponent<Text>() != null ? typeof(Text) : typeof(TextMeshProUGUI);
        }
        else if(go.name.Contains("Tmp"))
        {
            return typeof(TextMeshProUGUI);
        }
        else if(go.name.Contains("Rimg"))
        {
            return typeof(RawImage);
        }
        else if(go.name.Contains("Img"))
        {
            return typeof(Image);
        }
        else
        {
            return typeof(GameObject);
        }
    }
}
#endif