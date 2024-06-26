using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

public partial class BuildRunner
{
    public static void BuildPlayer()
    {
        var commandLineArgs = System.Environment.GetCommandLineArgs();

        foreach (var arg in commandLineArgs)
        {
            Debug.Log($"arg : {arg}");
        }

        var buildVersion = GetArgValue("-APPVERSION");
        PlayerSettings.bundleVersion = buildVersion;
        Debug.Log($"BuildVersion : {buildVersion}");
        
        CreateDirectory();
        
        var outPath = GetArgValue("-OUTPUTPATH");
        Debug.Log($"output Path : {outPath}");
        
        BuildTarget buildTarget = EvaluateBuildTarget();
        var fileName = buildTarget == BuildTarget.Android ? "Rohan2.apk" : "Rohan2.exe";
        
        var fullPath = $"{outPath}/{fileName}";
        Debug.Log($"fullPath : {fullPath}");
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions
        {
            scenes = new [] { "Assets/Scenes/SampleScene.unity" },
            locationPathName = fullPath,
            target = buildTarget,
            options = BuildOptions.None
        };
        BuildPipeline.BuildPlayer (buildPlayerOptions);

        // var report = BuildPipeline.BuildPlayer(levels, 
        //     fileName,
        //     buildTarget, 
        //     BuildOptions.None);

        //ReportBuildSummary(report);
    }

    static void CreateDirectory()
    {
        var dir = GetArgValue("-OUTPUTPATH");
        
        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);
    }
    
    public static void BuildAddressablesAndPlayer()
    {
        BuildAddressables();
        BuildPlayer();
    }

    static BuildTarget EvaluateBuildTarget()
    {
        var value = GetArgValue("-buildTarget");

        switch (value)
        {
            case "Win64":
                EditorUserBuildSettings.standaloneBuildSubtarget = StandaloneBuildSubtarget.Player;
                EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows64);
                break;
            // case "Android":
            default:
                EditorUserBuildSettings.standaloneBuildSubtarget = StandaloneBuildSubtarget.Player;
                EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);
                return BuildTarget.Android;
        }

        return (BuildTarget)0;
    }
}