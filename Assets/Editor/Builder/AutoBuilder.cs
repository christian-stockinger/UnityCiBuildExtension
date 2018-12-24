// ----------------------------------------------------------------------
// File:           AutoBuilder.cs
// Author:         c.stockinger (c.stockinger@intence.de)
// LastChangedBy:  c.stockinger (c.stockinger@intence.de)
// ----------------------------------------------------------------------

using UnityEditor;

internal class AutoBuilder
{
    [MenuItem("Tools/Build/Android")]
    private static void BuildAndroid()
    {
        EditorUserBuildSettings.exportAsGoogleAndroidProject = false;
        AutoBuilderCore.Instance.SetBuildTarget(BuildTarget.Android);
        AutoBuilderCore.Instance.PerformBuild();
    }

    [MenuItem("Tools/Build/AndroidGradle")]
    private static void BuildAndroidGradle()
    {
        EditorUserBuildSettings.exportAsGoogleAndroidProject = true;
        AutoBuilderCore.Instance.SetBuildTarget(BuildTarget.Android);
        AutoBuilderCore.Instance.PerformBuild();
    }

    [MenuItem("Tools/Build/IOS")]
    private static void BuildIos()
    {
        AutoBuilderCore.Instance.SetBuildTarget(BuildTarget.iOS);
        AutoBuilderCore.Instance.PerformBuild();
    }

    [MenuItem("Tools/Build/Linux")]
    private static void BuildLinux()
    {
        AutoBuilderCore.Instance.SetBuildTarget(BuildTarget.StandaloneLinuxUniversal);
        AutoBuilderCore.Instance.PerformBuild();
    }

    [MenuItem("Tools/Build/PS4")]
    private static void BuildPS4()
    {
        AutoBuilderCore.Instance.SetBuildTarget(BuildTarget.PS4);
        AutoBuilderCore.Instance.PerformBuild();
    }

    [MenuItem("Tools/Build/XboxOne")]
    private static void BuildXboxOne()
    {
        AutoBuilderCore.Instance.SetBuildTarget(BuildTarget.XboxOne);
        AutoBuilderCore.Instance.PerformBuild();
    }

    [MenuItem("Tools/Build/Windows64")]
    private static void BuildWindows64()
    {
        AutoBuilderCore.Instance.PerformBuild();
    }
}