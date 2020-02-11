using UnityEditor;
using UnityEngine;

public class CSVView : EditorWindow
{
    private TextAsset csv;
    private string[][] arr;

    [MenuItem("Window/CSV View")]
    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        GetWindow(typeof(CSVView));
    }

    private void OnGUI()
    {
        var newCsv = EditorGUILayout.ObjectField("CSV", csv, typeof(TextAsset), false) as TextAsset;
        if (newCsv != csv)
        {
            csv = newCsv;
            arr = CsvParser2.Parse(csv.text);
        }

        if (GUILayout.Button("Refresh") && csv != null)
            arr = CsvParser2.Parse(csv.text);

        if (csv == null)
            return;

        if (arr == null)
            arr = CsvParser2.Parse(csv.text);

        foreach (var t in arr)
        {
            EditorGUILayout.BeginHorizontal();
            foreach (var t1 in t)
            {
                EditorGUILayout.TextField(t1);
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}