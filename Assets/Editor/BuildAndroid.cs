using UnityEditor;
using System.IO;

public class BuildScript
{
    [MenuItem("Build/Build Android")]
    public static void BuildAndroidFunc()
    {
        string[] scenes = { "Assets/Scenes/MainMenu.unity" }; 
        string buildDirectory = "Build/Android"; 
        string pathToBuild = buildDirectory + "/2dGameBuild.apk"; 

        if (!Directory.Exists(buildDirectory))
        {
            Directory.CreateDirectory(buildDirectory);
        }

        BuildPipeline.BuildPlayer(scenes, pathToBuild, BuildTarget.Android, BuildOptions.None);
    }
}