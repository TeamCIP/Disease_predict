using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using Unity.VisualScripting;

[System.Serializable]
/*public class StringListWrapper        //for normal json
{
    public string[] strings;
}*/
public class HandleResponses : MonoBehaviour
{

    //----------------------------user input----------------------------
    [SerializeField] private Button submitSymptoms;
    [SerializeField] private List<FeatureSO> featureList;
    public List<string> userAnswers;

    //-------------Output-------------------------
    [SerializeField] private GameObject resultWindow;
    [SerializeField] private TextMeshProUGUI diseaseName;
    [SerializeField] private Button retryButton;
    private string Disease;


    //---------connecting to flask server
    private string serverURL; //--------------------fill this in


    //----------deal with multiple pages
    private int numberOfPages;
    [SerializeField] private int featurePerPage;
    private int featureCount;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button backButton;
    private int currentPageCount;
    [SerializeField] private List<GameObject> toggleOptionList;
    private int x;

    //UI variables
    [SerializeField] private GameObject symptomsScrollView;

    private void Awake()
    {
        featureCount = featureList.Count;
        numberOfPages = featureCount / featurePerPage;
        currentPageCount = 1;
        for (int i = 0; i < toggleOptionList.Count; i++)
        {
            toggleOptionList[i].SetActive(false);
        }



        submitSymptoms.onClick.AddListener(() =>
        {
            symptomsScrollView.SetActive(false);
            resultWindow.SetActive(true);
            int j = 0;
            for (int i = 0; i < featureList.Count; i++)
            {
                if (featureList[i].featureTrue)
                {
                    userAnswers.Add(featureList[i].featureNameInDataset);
                    j++;
                }
            }
            string jsonData= JsonUtility.ToJson(userAnswers);


            //for StringListWrapper for normal json
            /*var listOfStrings = new string[] {"Hello there"};

            string jsonData1=JsonUtility.ToJson(new StringListWrapper { strings=listOfStrings});*/


            // Create a JSON array manually with square brackets
            string json1 = "[[";

            for (int i = 0; i < userAnswers.Count; i++)
            {
                json1 += "\"" + userAnswers[i] + "\"";

                if (i < userAnswers.Count - 1)
                {
                    json1 += ",";
                }
            }

            json1 += "]]";

            Debug.Log("data part 1 "+json1);
            //Debug.Log(jsonData);    //issue is in the conversion from normal to json an 400d bad request
            
            StartCoroutine(SendArrayToServer(jsonData));
        });
        retryButton.onClick.AddListener(() =>
        {
            symptomsScrollView.SetActive(true);
            resultWindow.SetActive(false);
            userAnswers.Clear();
        });
        backButton.onClick.AddListener(() =>
        {
            currentPageCount--;
            for (int i = 0; i < toggleOptionList.Count; i++)
            {
                toggleOptionList[i].SetActive(false);
            }
        });
        nextButton.onClick.AddListener(() =>
        {
            currentPageCount++;
            for (int i = 0; i < toggleOptionList.Count; i++)
            {
                toggleOptionList[i].SetActive(false);
            }
        });
    }

    private void Update()
    {
        x = currentPageCount * featurePerPage;
        for (int i = x-featurePerPage;i<x;i++)
        {
            toggleOptionList[i].SetActive(true);
        }
        if(currentPageCount==1)
        {
            backButton.gameObject.SetActive(false);
            submitSymptoms.gameObject.SetActive(false);
        }
        else if (currentPageCount == numberOfPages)
        {
            nextButton.gameObject.SetActive(false);
            submitSymptoms.gameObject.SetActive(true);
        }
        else
        {
            nextButton.gameObject.SetActive(true);
            backButton.gameObject.SetActive(true);
            submitSymptoms.gameObject.SetActive(false);
        }
    }

    IEnumerator SendArrayToServer(string data)
    {
        // Replace with the actual URL of your Flask server
        string url = "http://127.0.0.1:5000/predict";

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(data);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();

        // Set the content type to indicate JSON data
        request.SetRequestHeader("Content-Type", "application/json");

        // Send the POST request
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            // Request was successful, process the response
            Debug.Log("Response: " + request.downloadHandler.text);
            diseaseName.text = "Your disease is "+request.downloadHandler.text;
        }
        else
        {
            // Request encountered an error
            Debug.LogError("Error sending request: " + request.error);
        }


        /*using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(data);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();

            // Set the content type to indicate JSON data
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                // Request was successful, process the response
                Debug.Log("Response: " + request.downloadHandler.text);
            }
            else
            {
                // Request encountered an error
                Debug.LogError("Error sending request: " + request.error);
            }
        }*/
    }


}
