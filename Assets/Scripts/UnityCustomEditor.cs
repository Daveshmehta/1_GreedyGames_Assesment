using System.IO;
using UnityEditor;
using UnityEngine;

public class ObjectTemplateEditorWindow : EditorWindow
{


    private string newTemplateName = "";
    private Vector2 scrollPosition;

    private void OnGUI()
    {
        GUILayout.Label("Object Template Editor", EditorStyles.boldLabel);



        GUILayout.Space(10);


        GUILayout.Label("Create New Template", EditorStyles.boldLabel);

        GUILayout.BeginHorizontal();
        newTemplateName = EditorGUILayout.TextField("Template Name", newTemplateName);

        if (GUILayout.Button("Create"))
        {

            CreateNewTemplate(newTemplateName);
        }
        GUILayout.EndHorizontal();


    }

    private void CreateNewTemplate(string templateName)
    {
        if (string.IsNullOrEmpty(templateName))
        {
            Debug.LogError("Template name cannot be empty.");
            return;
        }

        

       
        string filePath = "Assets/Resources/Templates/" + templateName + ".json";

        try
        {
            File.WriteAllText(filePath, templateName);

            UnityEditor.AssetDatabase.Refresh();

            Debug.Log("JSON file saved: " + filePath);

            
        }
        catch (IOException e)
        {
            Debug.LogError("Error saving JSON file: " + e.Message);
        }
    }
}
