// ----------------------------------------------------------------------
// File:           AutoBuilder.cs
// Organization:   iNTENCE automotive electronics GmbH 
// Copyright:      © 2018 iNTENCE GmbH. All rights reserved. 
// Author:         c.stockinger (c.stockinger@intence.de)
// LastChangedBy:  c.stockinger (c.stockinger@intence.de)
// ----------------------------------------------------------------------


using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

class AutoBuilder
{
    public static string ProductName => PlayerSettings.productName;

    private static string BuildPathRoot
    {
        get
        {
            string path = Path.Combine(Environment.CurrentDirectory, ProductName);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            return path;
        }
    }

    static int AndroidLastBuildVersionCode
    {
        get
        {
            return PlayerPrefs.GetInt("LastVersionCode", -1);
        }
        set
        {
            PlayerPrefs.SetInt("LastVersionCode", value);
        }
    }

    static BuildTargetGroup ConvertBuildTarget(BuildTarget buildTarget)
    {
        switch (buildTarget)
        {
            case BuildTarget.StandaloneOSX:
            case BuildTarget.iOS:
                return BuildTargetGroup.iOS;
            case BuildTarget.StandaloneWindows:
            case BuildTarget.StandaloneLinux:
            case BuildTarget.StandaloneWindows64:
            case BuildTarget.StandaloneLinux64:
            case BuildTarget.StandaloneLinuxUniversal:
                return BuildTargetGroup.Standalone;
            case BuildTarget.Android:
                return BuildTargetGroup.Android;
            case BuildTarget.WebGL:
                return BuildTargetGroup.WebGL;
            case BuildTarget.WSAPlayer:
                return BuildTargetGroup.WSA;
            case BuildTarget.PSP2:
                return BuildTargetGroup.PSP2;
            case BuildTarget.PS4:
                return BuildTargetGroup.PS4;
            case BuildTarget.XboxOne:
                return BuildTargetGroup.XboxOne;
            case BuildTarget.N3DS:
                return BuildTargetGroup.N3DS;
            case BuildTarget.tvOS:
                return BuildTargetGroup.tvOS;
            case BuildTarget.Switch:
                return BuildTargetGroup.Switch;
            default:
                return BuildTargetGroup.Standalone;
        }
    }

    static string GetExtension(BuildTarget buildTarget)
    {
        switch (buildTarget)
        {
            case BuildTarget.StandaloneOSX:
                break;
            case BuildTarget.StandaloneWindows:
            case BuildTarget.StandaloneWindows64:
                return ".exe";
            case BuildTarget.iOS:
                break;
            case BuildTarget.Android:
                return ".apk";
            case BuildTarget.StandaloneLinux:
                break;
            case BuildTarget.WebGL:
                break;
            case BuildTarget.WSAPlayer:
                break;
            case BuildTarget.StandaloneLinux64:
                break;
            case BuildTarget.StandaloneLinuxUniversal:
                break;
            case BuildTarget.PSP2:
                break;
            case BuildTarget.PS4:
                break;
            case BuildTarget.XboxOne:
                break;
            case BuildTarget.N3DS:
                break;
            case BuildTarget.tvOS:
                break;
            case BuildTarget.Switch:
                break;
            case BuildTarget.NoTarget:
                break;
            default:
                break;
        }

        return ".unknown";
    }

    static BuildPlayerOptions GetDefaultPlayerOptions()
    {
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();

        List<string> listScenes = new List<string>();
        foreach (EditorBuildSettingsScene s in EditorBuildSettings.scenes)
        {
            if (s.enabled)
                listScenes.Add(s.path);
        }

        buildPlayerOptions.scenes = listScenes.ToArray();
        buildPlayerOptions.options = BuildOptions.None;

        // To define
        // buildPlayerOptions.locationPathName = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) + "\\LightGunBuild\\Android\\LightGunMouseArcadeRoom.apk";
        // buildPlayerOptions.target = BuildTarget.Android;

        return buildPlayerOptions;
    }

    static void PerformBuild(BuildTarget buildTarget)
    {
        BuildTargetGroup targetGroup = ConvertBuildTarget(buildTarget);

        if (buildTarget == BuildTarget.Android)
        {
            AndroidLastBuildVersionCode = PlayerSettings.Android.bundleVersionCode;
            PlayerSettings.Android.keystoreName = Environment.GetEnvironmentVariable("ANDROID_KEYSTORE_NAME", EnvironmentVariableTarget.Process);
            PlayerSettings.Android.keystorePass = Environment.GetEnvironmentVariable("ANDROID_KEYSTORE_PASSWORD", EnvironmentVariableTarget.Process);
            PlayerSettings.Android.keyaliasName = Environment.GetEnvironmentVariable("ANDROID_KEYALIAS_NAME", EnvironmentVariableTarget.Process);
            PlayerSettings.Android.keyaliasPass = Environment.GetEnvironmentVariable("ANDROID_KEYALIAS_PASSWORD", EnvironmentVariableTarget.Process);
            EditorPrefs.SetString("AndroidSdkRoot", Environment.GetEnvironmentVariable("ANDROID_SDK_PATH", EnvironmentVariableTarget.Process));
            EditorPrefs.SetString("AndroidNdkRoot", Environment.GetEnvironmentVariable("ANDROID_NDK_PATH", EnvironmentVariableTarget.Process));

            Debug.Log("NDK Process " + Environment.GetEnvironmentVariable("ANDROID_NDK_PATH", EnvironmentVariableTarget.Process));
            Debug.Log("NDK Machine " + Environment.GetEnvironmentVariable("ANDROID_NDK_PATH", EnvironmentVariableTarget.Machine));
            Debug.Log("NDK User " + Environment.GetEnvironmentVariable("ANDROID_NDK_PATH", EnvironmentVariableTarget.User));
        }

        string path = Path.Combine(Path.Combine(BuildPathRoot, targetGroup.ToString()), ProductName + "_" + buildTarget);
        string name = ProductName + GetExtension(buildTarget);

        string defineSymbole = PlayerSettings.GetScriptingDefineSymbolsForGroup(targetGroup);
        PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, defineSymbole + ";BUILD");

        BuildPlayerOptions buildPlayerOptions = GetDefaultPlayerOptions();

        buildPlayerOptions.locationPathName = Path.Combine(path, name);
        buildPlayerOptions.target = buildTarget;

        EditorUserBuildSettings.SwitchActiveBuildTarget(targetGroup, buildTarget);

        string result = buildPlayerOptions.locationPathName + ": " + BuildPipeline.BuildPlayer(buildPlayerOptions);
        Debug.Log(result);
        PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, defineSymbole);

        EditorUtility.RevealInFinder(path);
    }

    [MenuItem("Tools/Build/Android")]
    static void BuildAndroid()
    {
        PerformBuild(BuildTarget.Android);
    }

    [MenuItem("Tools/Build/IOS")]
    static void BuildIos()
    {
        PerformBuild(BuildTarget.iOS);
    }
}