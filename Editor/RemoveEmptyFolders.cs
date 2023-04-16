using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class RemoveEmptyFolders : Editor
{
    [MenuItem("Tools/My Utilities/Remove Empty Folders in Assets")]
    public static void RemoveEmptyFoldersInAssets()
    {
        var emptyFolders = new List<string>();
        FindEmptyFoldersRecursively(Application.dataPath, emptyFolders);

        if (emptyFolders.Count > 0)
        {
            string emptyFolderList = string.Join("\n", emptyFolders);
            Debug.Log("Empty folders found:\n" + emptyFolderList);

            if (EditorUtility.DisplayDialog("Remove Empty Folders",
                "The following empty folders were found:\n\n" +
                emptyFolderList +
                "\n\nDo you want to delete them?", "Yes", "No"))
                DeleteFoldersAndRefresh(emptyFolders);
        }
        else
            Debug.Log("No empty folders found in Assets.");
    }

    static void FindEmptyFoldersRecursively(string folderPath, List<string> emptyFolders)
    {
        foreach (string subFolder in Directory.GetDirectories(folderPath))
        {
            FindEmptyFoldersRecursively(subFolder, emptyFolders);

            if (Directory.GetFiles(subFolder).Length == 0 && Directory.GetDirectories(subFolder).Length == 0)
            {
                string assetPath = subFolder.Replace(Application.dataPath, "Assets");
                emptyFolders.Add(assetPath);
            }
        }
    }

    static void DeleteFoldersAndRefresh(List<string> foldersToDelete)
    {
        foreach (string folder in foldersToDelete)
            AssetDatabase.DeleteAsset(folder);

        AssetDatabase.Refresh();
        Debug.Log("Empty folders removed.");
    }
}
