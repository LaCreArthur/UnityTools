using UnityEditor;
using UnityEngine;

namespace AS.Toolbox.Editor
{
    public class ReplaceWithPrefab : EditorWindow
    {
        [SerializeField] GameObject prefab;
        [SerializeField] bool usePrefabScale;
        [SerializeField] bool usePrefabRot;

        [MenuItem("Tools/My Utilities/Replace With Prefab")]
        static void CreateReplaceWithPrefab() => GetWindow<ReplaceWithPrefab>();

        void OnGUI()
        {
            prefab = (GameObject)EditorGUILayout.ObjectField("Prefab", prefab, typeof(GameObject), false);
            usePrefabRot = EditorGUILayout.Toggle("Use Prefab Rotations", usePrefabRot);
            usePrefabScale = EditorGUILayout.Toggle("Use Prefab Scale", usePrefabScale);

            if (GUILayout.Button("Replace"))
            {
                GameObject[] selection = Selection.gameObjects;

                for (int i = selection.Length - 1; i >= 0; --i)
                {
                    var selected = selection[i];
                    PrefabUtility.GetPrefabAssetType(prefab);
                    var prefabType = PrefabUtility.GetPrefabAssetType(prefab);
                    GameObject newObject;

                    if (prefabType != PrefabAssetType.NotAPrefab)
                        newObject = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                    else
                    {
                        newObject = Instantiate(prefab);
                        newObject.name = prefab.name;
                    }

                    if (newObject == null)
                    {
                        Debug.LogError("Error instantiating prefab");
                        break;
                    }

                    Undo.RegisterCreatedObjectUndo(newObject, "Replace With Prefabs");
                    newObject.transform.SetParent(selected.transform.parent,
                        newObject.GetComponent<RectTransform>() ==
                        null); //worldPositionStays to be false if newObject has a rectTransform (UI)
                    newObject.transform.localPosition = selected.transform.localPosition;
                    newObject.transform.localRotation = usePrefabRot ? prefab.transform.localRotation : selected.transform.localRotation;
                    newObject.transform.localScale = usePrefabScale ? prefab.transform.localScale : selected.transform.localScale;
                    newObject.transform.SetSiblingIndex(selected.transform.GetSiblingIndex());
                    Undo.DestroyObjectImmediate(selected);
                }
            }

            GUI.enabled = false;
            EditorGUILayout.LabelField("Selection count: " + Selection.objects.Length);
        }
    }
}
