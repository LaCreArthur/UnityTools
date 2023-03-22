

//todo: WIP, enum values are not assigned automatically after generation
// public class GameStateEnumGenerator : AssetPostprocessor
// {
//     // Replace this with the folder containing your GameStateSO assets
//     const string GameStateSOAssetFolder = "Assets/_Doge/Scriptable Objects/Game States";
//
//     // The path where the generated enum script will be saved
//     const string GeneratedEnumScriptPath = "Assets/_Toolbox/Runtime/Scriptable Objects/Core/Game State/GameStateEnum.cs";
//
//     static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets,
//         string[] movedFromAssetPaths)
//     {
//         bool gameStateModified = false;
//
//         foreach (string asset in importedAssets)
//         {
//             if (asset.StartsWith(GameStateSOAssetFolder) && asset.EndsWith(".asset"))
//             {
//                 gameStateModified = true;
//                 break;
//             }
//         }
//
//         foreach (string asset in deletedAssets)
//         {
//             if (asset.StartsWith(GameStateSOAssetFolder) && asset.EndsWith(".asset"))
//             {
//                 gameStateModified = true;
//                 break;
//             }
//         }
//
//         if (gameStateModified)
//         {
//             UpdateGameStateEnum();
//         }
//     }
//
//     static void UpdateGameStateEnum()
//     {
//         StringBuilder enumBuilder = new StringBuilder();
//         enumBuilder.AppendLine("public enum GameStateEnum");
//         enumBuilder.AppendLine("{");
//         // add the None value to the enum
//         enumBuilder.AppendLine("    None,");
//
//         // find all GameStateSO assets in the GameStateSOAssetFolder
//         string[] gameStateSOAssetGuids = AssetDatabase.FindAssets("t:GameStateSO", new[] { GameStateSOAssetFolder });
//
//         Dictionary<GameStateSO, string> gameStates = new Dictionary<GameStateSO, string>();
//
//         // add an enum value for each GameStateSO asset
//         foreach (string guid in gameStateSOAssetGuids)
//         {
//             string assetPath = AssetDatabase.GUIDToAssetPath(guid);
//             GameStateSO gameStateSO = AssetDatabase.LoadAssetAtPath<GameStateSO>(assetPath);
//             // Remove the "GS " prefix from the name and replace spaces with camel case
//             string enumValue = Regex.Replace(gameStateSO.name, "^GS ", "", RegexOptions.IgnoreCase);
//             enumValue = Regex.Replace(enumValue, @"\s+", "");
//             enumValue = char.ToUpperInvariant(enumValue[0]) + enumValue[1..];
//             enumBuilder.AppendLine($"    {enumValue},");
//             gameStates.Add(gameStateSO, enumValue);
//         }
//
//         enumBuilder.AppendLine("}");
//         File.WriteAllText(GeneratedEnumScriptPath, enumBuilder.ToString());
//         AssetDatabase.Refresh();
//         // Wait for the code to finish recompiling before assigning the GameStateEnum field
//         EditorApplication.update += () =>
//         {
//             bool isCompiling = EditorApplication.isCompiling;
//             if (!isCompiling)
//             {
//                 foreach (var gameState in gameStates)
//                 {
//                     Debug.Log($"Setting {gameState.Key.name} to {gameState.Value}");
//                     // gameState.Key.gameStateEnum = (GameStateEnum)Enum.Parse(typeof(GameStateEnum), gameState.Value);
//                     EditorUtility.SetDirty(gameState.Key);
//                 }
//                 // Remove the update event handler
//                 EditorApplication.update -= null;
//             }
//         };
//     }
// }