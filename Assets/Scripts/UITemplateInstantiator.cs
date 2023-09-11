using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;

[System.Serializable]
public class UITemplate
{
    public string templateName;
    public List<UIObject> objects;
}

[System.Serializable]
public class UIObject
{
    public bool bUseScreenSpaceCoordinates;
    public string name;
    public Vector3 position;
    public Vector3 rotation;
    public Vector3 scale;
    public Color color;
}

public class UITemplateInstantiator : MonoBehaviour
{
    public List<UITemplate> templates = new List<UITemplate>();
    public Transform canvasTransform;
    public int selectedTemplateIndex = 0;


    [HideInInspector]
    public int previousSelectedTemplateIndex = 0;

    private void Start()
    {

        LoadSelectedTemplate();
    }

    private void Update()
    {

        if (selectedTemplateIndex != previousSelectedTemplateIndex)
        {

            SaveSelectedTemplate();


            LoadSelectedTemplate();


            previousSelectedTemplateIndex = selectedTemplateIndex;
        }
    }

    public void InstantiateTemplate()
    {
        if (selectedTemplateIndex >= 0 && selectedTemplateIndex < templates.Count)
        {
            UITemplate selectedTemplate = templates[selectedTemplateIndex];

            GameObject templateRoot = new GameObject(selectedTemplate.templateName);
            templateRoot.transform.SetParent(canvasTransform);

            foreach (UIObject uiObject in selectedTemplate.objects)
            {
                GameObject uiElement = new GameObject(uiObject.name);
                uiElement.transform.SetParent(templateRoot.transform);

                RectTransform rectTransform = uiElement.AddComponent<RectTransform>();
                rectTransform.anchoredPosition = uiObject.position;
                rectTransform.localEulerAngles = uiObject.rotation;
                rectTransform.localScale = uiObject.scale;

               
            }
        }
        else
        {
            Debug.LogWarning("Invalid template index.");
        }
    }

    public void SaveSelectedTemplate()
    {
        if (selectedTemplateIndex >= 0 && selectedTemplateIndex < templates.Count)
        {
            UITemplate selectedTemplate = templates[selectedTemplateIndex];

            string jsonTemplate = JsonUtility.ToJson(selectedTemplate);


            string filePath = Path.Combine(Application.dataPath, "Templates", selectedTemplate.templateName + ".json");
            File.WriteAllText(filePath, jsonTemplate);

            Debug.Log("Template " + selectedTemplate.templateName + " saved to file: " + filePath);
        }
        else
        {
            Debug.LogWarning("Invalid template index.");
        }
    }

    public void LoadSelectedTemplate()
    {
        if (selectedTemplateIndex >= 0 && selectedTemplateIndex < templates.Count)
        {
            UITemplate selectedTemplate = templates[selectedTemplateIndex];

            
            string filePath = Path.Combine(Application.dataPath, "Templates", selectedTemplate.templateName + ".json");

            if (File.Exists(filePath))
            {
                string jsonTemplate = File.ReadAllText(filePath);

                
                selectedTemplate = JsonUtility.FromJson<UITemplate>(jsonTemplate);

               
                templates[selectedTemplateIndex] = selectedTemplate;

                Debug.Log("Template " + selectedTemplate.templateName + " loaded from file: " + filePath);
            }
            else
            {
                Debug.LogWarning("Template file does not exist: " + filePath);
            }
        }
        else
        {
            Debug.LogWarning("Invalid template index.");
        }
    }


}

