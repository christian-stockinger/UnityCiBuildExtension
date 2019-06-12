// ----------------------------------------------------------------------
// File:           AutoBuilderCore.cs
// Organization:   iNTENCE automotive electronics GmbH 
// Copyright:      © 2018 iNTENCE GmbH. All rights reserved. 
// Author:         c.stockinger (c.stockinger@intence.de)
// LastChangedBy:  c.stockinger (c.stockinger@intence.de)
// ----------------------------------------------------------------------


using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class AutoBuilderCore
{
    private static AutoBuilderCore _instance = null;

    private AutoBuilderCore()
    {
    }

    public static AutoBuilderCore Instance
    {
        get
        {
            return _instance ?? (_instance = new AutoBuilderCore());
        }
    }

    public static string ProductName
    {
        get
        {
            return PlayerSettings.productName;
        }
    }

    private BuildTarget _buildTarget = BuildTarget.StandaloneWindows64;

    public void SetBuildTarget(BuildTarget buildTarget)
    {
        this._buildTarget = buildTarget;
    }

    public static string BuildPathRoot
    {
        get
        {
            string path = Path.Combine(Environment.CurrentDirectory, "Build");
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            return path;
        }
    }

    public static int AndroidLastBuildVersionCode
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

    public BuildTargetGroup ConvertBuildTarget()
    {
        switch (this._buildTarget)
        {
            case BuildTarget.StandaloneOSX:
            case BuildTarget.iOS: return BuildTargetGroup.iOS;
            case BuildTarget.StandaloneWindows:
            case BuildTarget.StandaloneLinux:
            case BuildTarget.StandaloneWindows64:
            case BuildTarget.StandaloneLinux64:
            case BuildTarget.StandaloneLinuxUniversal: return BuildTargetGroup.Standalone;
            case BuildTarget.Android: return BuildTargetGroup.Android;
            case BuildTarget.WebGL: return BuildTargetGroup.WebGL;
            case BuildTarget.WSAPlayer: return BuildTargetGroup.WSA;
            case BuildTarget.PSP2: return BuildTargetGroup.PSP2;
            case BuildTarget.PS4: return BuildTargetGroup.PS4;
            case BuildTarget.XboxOne: return BuildTargetGroup.XboxOne;
            case BuildTarget.N3DS: return BuildTargetGroup.N3DS;
            case BuildTarget.tvOS: return BuildTargetGroup.tvOS;
            case BuildTarget.Switch: return BuildTargetGroup.Switch;
            default: return BuildTargetGroup.Standalone;
        }
    }

    /// <summary>
    /// Returns teh Extension of the build Target
    /// </summary>
    /// <returns></returns>
    public string GetExtension()
    {
        switch (this._buildTarget)
        {
            case BuildTarget.StandaloneOSX: break;
            case BuildTarget.StandaloneWindows:
            case BuildTarget.StandaloneWindows64: return ".exe";
            case BuildTarget.iOS: break;
            case BuildTarget.Android: return ".apk";
            case BuildTarget.StandaloneLinux: break;
            case BuildTarget.WebGL: break;
            case BuildTarget.WSAPlayer: break;
            case BuildTarget.StandaloneLinux64: break;
            case BuildTarget.StandaloneLinuxUniversal: break;
            case BuildTarget.PSP2: break;
            case BuildTarget.PS4: break;
            case BuildTarget.XboxOne: break;
            case BuildTarget.N3DS: break;
            case BuildTarget.tvOS: break;
            case BuildTarget.Switch: break;
            case BuildTarget.NoTarget: break;
            default: break;
        }

        return ".unknown";
    }

    /// <summary>
    /// Gernerate Build Options with all Scenes from the Build Settings
    /// </summary>
    /// <returns></returns>
    private BuildPlayerOptions GetDefaultPlayerOptions()
    {
        BuildPlayerOptions buildPlayerOptions =
            new BuildPlayerOptions
            {
                //Collect All Scenes from the Build Settings
                scenes = (from s in EditorBuildSettings.scenes where s.enabled select s.path)
                    .ToArray(),
                options = BuildOptions.None
            };

        return buildPlayerOptions;
    }

    /// <summary>
    /// Get Environment Variable and log an error if not existing or empty
    /// </summary>
    /// <param name="variable"></param>
    /// <returns></returns>
    private static string GetEnvironmentVariable(string variable)
    {
        string value = Environment.GetEnvironmentVariable(variable, EnvironmentVariableTarget.Process);

        if (string.IsNullOrEmpty(value))
        {
            Debug.LogError(string.Format("The EnvironmentVariable {0} was null or Whitespace", variable));
        }
        else
        {
            Debug.Log(string.Format("Found {0} with the value {1}", variable, value));
        }

        return value;
    }

    /// <summary>
    /// Will Setup Android Keystone, SDK and NDK
    /// </summary>
    private void SetUpAndroidEnvironment()
    {
        string environmentProductName = ProductName.ToUpper();
        Debug.Log("Start Setup Android Environment");
        PlayerSettings.Android.keystoreName = GetEnvironmentVariable("ANDROID_KEYSTORE_" + environmentProductName + "_NAME");
        PlayerSettings.Android.keystorePass = GetEnvironmentVariable("ANDROID_KEYSTORE_" + environmentProductName + "_PASSWORD");
        PlayerSettings.Android.keyaliasName = GetEnvironmentVariable("ANDROID_KEYALIAS_" + environmentProductName + "_NAME");
        PlayerSettings.Android.keyaliasPass = GetEnvironmentVariable("ANDROID_KEYALIAS_" + environmentProductName + "_PASSWORD");
        EditorPrefs.SetString("AndroidSdkRoot", GetEnvironmentVariable("ANDROID_SDK_PATH"));
        EditorPrefs.SetString("AndroidNdkRoot", GetEnvironmentVariable("ANDROID_NDK_PATH"));
        Debug.Log("Finished Setup Android Environment");
    }

    public void PerformBuild()
    {
        SetBuildTarget(_buildTarget);
        BuildTargetGroup targetGroup = ConvertBuildTarget();

        if (_buildTarget == BuildTarget.Android)
        {
            AndroidLastBuildVersionCode = PlayerSettings.Android.bundleVersionCode;
            this.SetUpAndroidEnvironment();
        }

        string path = Path.Combine(BuildPathRoot, _buildTarget.ToString());
        string name = ProductName + GetExtension();

        string defineSymbole = PlayerSettings.GetScriptingDefineSymbolsForGroup(targetGroup);
        PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, defineSymbole + ";BUILD");

        BuildPlayerOptions buildPlayerOptions = GetDefaultPlayerOptions();

        buildPlayerOptions.locationPathName = Path.Combine(path, name);
        buildPlayerOptions.target = _buildTarget;

        EditorUserBuildSettings.SwitchActiveBuildTarget(targetGroup, _buildTarget);

        Debug.Log(buildPlayerOptions.locationPathName + ": " + BuildPipeline.BuildPlayer(buildPlayerOptions));
        PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, defineSymbole);
    }
}