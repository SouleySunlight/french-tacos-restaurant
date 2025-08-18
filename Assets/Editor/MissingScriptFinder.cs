using UnityEngine;
using UnityEditor;

public class MissingScriptFinder
{
    [MenuItem("Tools/Find Missing Scripts")]
    static void FindMissingScripts()
    {
        GameObject[] allObjects = Object.FindObjectsByType<GameObject>(FindObjectsSortMode.None);
        int count = 0;

        foreach (GameObject go in allObjects)
        {
            Component[] components = go.GetComponents<Component>();

            foreach (Component c in components)
            {
                if (c == null)
                {
                    Debug.LogWarning("Missing script on GameObject: " + GetFullPath(go));
                    count++;
                }
            }
        }

        Debug.Log("Scan terminé. Scripts manquants trouvés: " + count);
    }

    static string GetFullPath(GameObject obj)
    {
        string path = obj.name;
        while (obj.transform.parent != null)
        {
            obj = obj.transform.parent.gameObject;
            path = obj.name + "/" + path;
        }
        return path;
    }
}