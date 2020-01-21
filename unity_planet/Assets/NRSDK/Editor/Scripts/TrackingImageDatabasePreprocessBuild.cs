/****************************************************************************
* Copyright 2019 Nreal Techonology Limited. All rights reserved.
*                                                                                                                                                          
* This file is part of NRSDK.                                                                                                          
*                                                                                                                                                           
* https://www.nreal.ai/        
* 
*****************************************************************************/

namespace NRKernal
{
    using UnityEditor;

    internal class TrackingImageDatabasePreprocessBuild : PreprocessBuildBase
    {
        public override void OnPreprocessBuild(BuildTarget target, string path)
        {
            // Set NRSDK eveironment.
            SetNRSDKEveironment();

            var augmentedImageDatabaseGuids = AssetDatabase.FindAssets("t:NRTrackingImageDatabase");
            foreach (var databaseGuid in augmentedImageDatabaseGuids)
            {
                var database = AssetDatabase.LoadAssetAtPath<NRTrackingImageDatabase>(
                    AssetDatabase.GUIDToAssetPath(databaseGuid));

                TrackingImageDatabaseInspector.BuildDataBase(database);
                database.BuildIfNeeded();
            }
        }

        // Auto set the sdcard permission、portrait oritention、
        public static void SetNRSDKEveironment()
        {
#if UNITY_ANDROID
            PlayerSettings.Android.forceSDCardPermission = true;
            PlayerSettings.defaultInterfaceOrientation = UIOrientation.Portrait;
            //PlayerSettings.SetGraphicsAPIs(BuildTarget.Android, new GraphicsDeviceType[1] { GraphicsDeviceType.OpenGLES3 });
            //PlayerSettings.allowUnsafeCode = true;
#endif
        }
    }
}
