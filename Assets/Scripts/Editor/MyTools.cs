using UnityEngine;
using UnityEditor;

public static class MyTools {
	[MenuItem("My Tools/1. Add Defaults to Report %q")]
	static void DEV_AppendDefaultsToReport() {
		CSVManager.AppendToReport(
			new string[11] {
				Random.Range(0,11).ToString(),				
				Random.Range(0,11).ToString(),
				Random.Range(0,11).ToString(),
				Random.Range(0,11).ToString(),
				Random.Range(0,11).ToString(),
				Random.Range(0,11).ToString(),
				Random.Range(0,11).ToString(),
				Random.Range(0,11).ToString(),
				Random.Range(0,11).ToString(),
				Random.Range(0,11).ToString(),
				Random.Range(0,11).ToString()
			}
		);
		EditorApplication.Beep();
		Debug.Log("Report updated successfully!");
	}

	[MenuItem("My Tools/2. Reset Report %k")]
	static void DEV_ResetReport() {
		CSVManager.CreateReport();
		EditorApplication.Beep();
		Debug.Log("The report has been reset... ");
	}


	

	
}
