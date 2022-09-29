using System;
using UnityEditor;
using System.IO;

public class CreateAssetBundle {
    public static string AssetBundleDirectory = "Assets/AssetBundles";

    public static string BuildTargetName(BuildTarget platform) =>
        platform switch {
            BuildTarget.StandaloneWindows => "Win",
            BuildTarget.WebGL => "WebGL",
            _ => throw new ArgumentOutOfRangeException(nameof(platform), platform, null)
        };

    [MenuItem("Assets/Build AssetBundles")]
    static void BuildAllAssetBundles() {
        BuildAllAssetBundlesForPlatform(BuildTarget.StandaloneWindows);
        BuildAllAssetBundlesForPlatform(BuildTarget.WebGL);
    }

    static void BuildAllAssetBundlesForPlatform(BuildTarget platform) {
        var directory = AssetBundleDirectory + "/" + BuildTargetName(platform);
        if (!Directory.Exists(directory)) {
            Directory.CreateDirectory(directory);
        }

        BuildPipeline.BuildAssetBundles(directory,
                                        BuildAssetBundleOptions.ChunkBasedCompression,
                                        platform);
    }
}