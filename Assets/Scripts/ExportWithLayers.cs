#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using UnityEditor;

public static class ExportWithLayers {

	[MenuItem("Asset Store Tools/Export package with tags and physics layers")]
	public static void ExportPackage()
	{
		string[] projectContent = AssetDatabase.GetAllAssetPaths();  
	          AssetDatabase.ExportPackage(projectContent, "RamboClone.unitypackage", ExportPackageOptions.Recurse | ExportPackageOptions.IncludeLibraryAssets );  
	          Debug.Log("Project Exported"); 
	}

}
#endif
