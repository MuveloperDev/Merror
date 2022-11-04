using UnityEditor;
using UnityEngine;
using System.Diagnostics;

public class MyBuild
{
    [MenuItem ("MyTools/BKBuild")]
    public static void Build()
    {
        // Get filename.
        string path = EditorUtility.SaveFolderPanel("Choose Location of Built Game", "", "");
        string[] levels = new string[] { "Assets/Chapter1/Chapter1.unity" };

        // Batch File Path
        string defaultPath = System.IO.Directory.GetCurrentDirectory();
        string makeFilePath = defaultPath + "/MakeFile.bat";
        string readFilePath = defaultPath + "/ReadFile.bat";

        // Build player.
        BuildPipeline.BuildPlayer(levels, path + "/BuiltGame.exe", BuildTarget.StandaloneWindows, BuildOptions.None);

        // Copy a file from the project folder to the build folder, alongside the built game.
        FileUtil.CopyFileOrDirectory(makeFilePath, path + "/MakeFile.bat");
        FileUtil.CopyFileOrDirectory(readFilePath, path + "/ReadFile.bat");
    }
}
