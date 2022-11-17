using UnityEditor;
using UnityEngine;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;

public class MyBuild : EditorWindow
{
    private const string defaultPath = "Assets/Scenes/";
    private const string publicPath = "public/";
    private const string unityPath = ".unity";
    private bool isPublic = false;
    private static List<string> Levels;
    private string sceneArray = "";
    private string sceneName = "";
    Vector2 scroll = new Vector2();
    public MyBuild()
    {
        Levels = new List<string>();
    }

    [MenuItem ("BKTools/BuildSettings")]
    public static void ShowWindow()
    {
        MyBuild build =  (MyBuild)EditorWindow.GetWindow(typeof(MyBuild));
        build.minSize = new Vector2(150f, 150f);
        build.maxSize = new Vector2(1000f, 1000f);
        build.Show();
    }

    private void OnGUI()
    {
        
        // Assign Scene
        GUILayout.Label("Input Scenes Path");
        GUILayout.BeginVertical();

        if (Selection.activeObject)
        {
            sceneName = GUILayout.TextField(Selection.activeObject.name);
            GUILayout.TextArea(sceneArray, GUILayout.Height(400));
        }

        if (GUILayout.Button("Chapter"))
        {
            Levels.Add(defaultPath + sceneName + unityPath);
            sceneArray += Levels[Levels.Count - 1] + "\n";
        }
        if (GUILayout.Button("Public"))
        {
            Levels.Add(defaultPath+ publicPath + sceneName + unityPath);
            sceneArray += Levels[Levels.Count - 1] + "\n";
        }
        if (GUILayout.Button("Delete"))
        {
            sceneArray = "";
            Levels.RemoveAt(Levels.Count - 1);
            for(int i = 0; i < Levels.Count; i++)
            {
                sceneArray += Levels[i] + "\n";
            }
        }
        
        // Start Build
        GUILayout.Label("Build Setting");
        if (GUILayout.Button("Standard Build"))
        {
            StandardBuild(BuildOptions.None);
        }
        if(GUILayout.Button("Standard Build And Run"))
        {
            StandardBuildAndRun(BuildOptions.None);
        }
        if (GUILayout.Button("Development Build"))
        {
            StandardBuild(BuildOptions.Development);
        }
        if (GUILayout.Button("Development Build And Run"))
        {
            StandardBuildAndRun(BuildOptions.Development);
        }
        GUILayout.EndVertical();
    }

    private string StandardBuild(BuildOptions option)
    {
        string path = EditorUtility.SaveFolderPanel("Choose Location of Built Game", "", "");
        // Get filename.
        string[] levels = Levels.ToArray();

        // Batch File Path
        string defaultPath = System.IO.Directory.GetCurrentDirectory();
        string makeFilePath = defaultPath + "/MakeFile.bat";
        string readFilePath = defaultPath + "/ReadFile.bat";
        string captureFolderPath = defaultPath + "/Capture";

        // Build player.
        BuildPipeline.BuildPlayer(levels, path + "/Merror.exe", BuildTarget.StandaloneWindows, option);

        FileInfo BulidFileInfo = new FileInfo(path + "/Merror.exe");
        FileInfo MakeFileInfo = new FileInfo(path + "/MakeFile.bat");
        FileInfo ReadFileInfo = new FileInfo(path + "/ReadFile.bat");
        FileInfo CaptureInfo = new FileInfo(path + "/Capture");
        // Copy a file from the project folder to the build folder, alongside the built game.]
        if (BulidFileInfo.Exists)
        {
            if(MakeFileInfo.Exists == false)
                FileUtil.CopyFileOrDirectory(makeFilePath, path + "/MakeFile.bat");
            if(ReadFileInfo.Exists == false)
                FileUtil.CopyFileOrDirectory(readFilePath, path + "/ReadFile.bat");
            if (CaptureInfo.Exists == false)
                FileUtil.CopyFileOrDirectory(captureFolderPath, path + "/Capture");
        }

        return path;

    }

    private void StandardBuildAndRun(BuildOptions option)
    {
        string path = StandardBuild(option);
        Process run = new Process();
        FileInfo BuildFile = new FileInfo(path + "/Merror.exe");

        if (BuildFile.Exists)
        {
            run.StartInfo.FileName = path + "/Merror.exe";
            run.Start();
        }
    }

    
}
