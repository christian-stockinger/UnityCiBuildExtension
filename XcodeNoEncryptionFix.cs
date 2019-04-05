// ----------------------------------------------------------------------
// File:           XcodeNoEncryptionFix.cs
// Author:         c.stockinger (c.stockinger@intence.de)
// LastChangedBy:  c.stockinger (c.stockinger@intence.de)
// ----------------------------------------------------------------------

using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using UnityEngine;

public class XcodeNoEncryptionFix : MonoBehaviour
{
    [PostProcessBuild]
    public static void ChangeXcodePlist(BuildTarget buildTarget, string pathToBuiltProject)
    {
        if (buildTarget == BuildTarget.iOS)
        {
            // Get plist
            string plistPath = pathToBuiltProject + "/Info.plist";
            PlistDocument plist = new PlistDocument();
            plist.ReadFromString(File.ReadAllText(plistPath));

            // Get root
            PlistElementDict rootDict = plist.root;

            string buildKey = "ITSAppUsesNonExemptEncryption";
            rootDict.SetBoolean(buildKey, false);

            // Write to file
            File.WriteAllText(plistPath, plist.WriteToString());
        }
    }
}