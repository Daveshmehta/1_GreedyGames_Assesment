using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;


#if UNITY_EDITOR
using UnityEditor;
#endif


public class JsonRead : MonoBehaviour
{
    private List<UIObjectView> spawnedUIObjList = new();
    private const string FilePath = "UITemplate";

    public string templateData;

    private UITemplate deserializedData;

    public Canvas rootCanvas;
    public UIObjectView uiObjectPrefab;



    public void LoadData()
    {
        try
        {
            string filePath = Application.dataPath + "/" + FilePath + ".json";

            if (File.Exists(filePath))
            {
                templateData = File.ReadAllText(filePath);

            }
            else
            {
                Debug.LogError("File do not exist");
            }
        }


        catch (System.Exception e)
        {
            Debug.LogError("File Loading failed due to : " + e.Message + "    " + e.StackTrace);
            throw e;
        }
    }

    public void Spawntemplate()
    {
        if (string.IsNullOrEmpty(templateData))
            return;

        deserializedData = JsonConvert.DeserializeObject<UITemplate>(templateData);
        foreach (UIObject uiobj in deserializedData.objects)
        {
            InstantiateUIObject(uiobj);
        }

        
    }

    public void SaveData()
    {
        if (string.IsNullOrEmpty(templateData))
            return;


        Debug.Log(Application.dataPath);
        string filePath = Application.dataPath + "/" + FilePath + ".json";
        File.WriteAllText(filePath, templateData);
        AssetDatabase.Refresh();
      
    }

    public void InstantiateUIObject(UIObject uiObject)
    {
        if (uiObjectPrefab == null)
        {
            return;
        }

        UIObjectView spawnedPrefab = Instantiate(uiObjectPrefab, rootCanvas.transform);
        spawnedPrefab.InitializeUIObject(uiObject);
        spawnedUIObjList.Add(spawnedPrefab);

        

    }

    public void ClearSpawnedObjects()
    {
        foreach (var obj in spawnedUIObjList)
        {
            if (obj != null)
                DestroyImmediate(obj.gameObject);

        }
        spawnedUIObjList.Clear();


    }


}

#if UNITY_EDITOR
[CustomEditor(typeof(JsonRead))]
public class FusionNetworkManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.Space(10);

        JsonRead manager = (JsonRead)target;

        var areaStyle = new GUIStyle(GUI.skin.textArea);
        areaStyle.wordWrap = true;

        var width = 100;
        areaStyle.fixedHeight = 0;

        areaStyle.fixedHeight = areaStyle.CalcHeight(new GUIContent(manager.templateData), width);

        manager.templateData = GUILayout.TextArea(manager.templateData, areaStyle = new GUIStyle(GUI.skin.textArea));



        GUILayout.Space(20);

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Load Data"))
        {
            manager.LoadData();
        }
        if (GUILayout.Button("Save Data"))
        {

            manager.SaveData();
        }

        GUILayout.EndHorizontal();
        GUILayout.Space(10);
        GUILayout.BeginHorizontal();


        if (GUILayout.Button("Destroy Objects"))
        {

            manager.ClearSpawnedObjects();
        }

        if (GUILayout.Button("Spawn Template"))
        {

            manager.Spawntemplate();
        }
        EditorGUILayout.EndHorizontal();
    }
}
#endif

