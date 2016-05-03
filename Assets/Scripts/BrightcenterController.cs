using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using SimpleJSON;
using System.Collections.Generic;
using System;
using System.Text;
using System.Text.RegularExpressions;

public class BrightcenterController : MonoBehaviour {


    private static BrightcenterController instance = null;

    public static BrightcenterController GetInstance(GameObject gameObject) {
        Debug.Log("get instance");
        if (instance == null) {
            instance = gameObject.AddComponent<BrightcenterController>();
            Debug.Log(instance);
            return instance;
        }
        return instance;
    }

    IBrightcenter delegateInstance;

    string baseUrl = "https://brightcenter.nl/dashboard/api";
    public Student student  { get; set; }
    public string assessmentId { get; set; }
    public string appUrl { get; set; }
    string cookie = null;

	// Use this for initialization
	void Start () {
        Debug.Log("start bccontroller");
	}
	
	// Update is called once per frame
	void Update () {
      
	}

    public void setDelegate(IBrightcenter delegateInstance)
    {
        this.delegateInstance = delegateInstance;
    }


    //url: tracy://?data=eyJsYXN0TmFtZSI6IkJvbm5pZXIiLCJwZXJzb25JZCI6IjUyYjMwYjRiMzAwNDdjZjlkZWQ5OGM3NSIsImZpcnN0TmFtZSI6Ik1heCJ9&cookie=2144920132B322812674396DD1BBD462&assessmentId=e027be54-a4a1-4df3-9ae3-8f43ab6aa4b3
    public void handleUrl(string url) {
        string urlParamsString = Regex.Replace(url, ".*://\\?", "");
        List<String> urlParamsList = new List<string>(urlParamsString.Split('&'));
        foreach (string param in urlParamsList)
        {
            if (param.Contains("assessmentId"))
            {
                this.assessmentId = param.Replace("assessmentId=", "");
                Console.WriteLine("assessmentId: " + this.assessmentId);
            }
            else if (param.Contains("cookie"))
            {
                this.cookie = "JSESSIONID=" + param.Replace("cookie=", "");
                Console.WriteLine("cookie: " + this.cookie);
            }
            else if (param.Contains("data"))
            {
                string dataString = param.Replace("data=", "");
                dataString = dataString.Replace("*", "=");
                Console.WriteLine("data: " + dataString);
                var jsonStudent = JSON.Parse(Encoding.UTF8.GetString(Convert.FromBase64String(dataString)));
                this.student = new Student(jsonStudent["personId"], jsonStudent["firstName"], jsonStudent["lastName"]);
            }
        }        

        Debug.Log("assessmentId: " + this.assessmentId);
        Debug.Log("Cookie: " + this.cookie);
        Debug.Log("student: " + this.student.firstName);
    }

    public void GetResult() {
        string url = baseUrl + "/assessment/" + this.assessmentId + "/student/" + student.id + "/assessmentItemResult";
        Debug.Log(url);
        Hashtable headers = new Hashtable();
        headers.Add("Cookie", this.cookie);
        WWW results = new WWW(url, null, headers);
        StartCoroutine(WaitForResultsRequest(results));
    }

    IEnumerator WaitForResultsRequest(WWW www)
    {
        Debug.Log("get results with cookie: " + this.cookie);
        yield return www;
        if (www.error != null)
        {
            Debug.Log("error getting: " + www.error);
            delegateInstance.handleResultsError();
        }
        else
        {
            // [{"questionId":"Tijdrace_Provincies","date":1421614265074,"attempts":1,"duration":10.0,"score":0.0,"completionStatus":"COMPLETED"}]
            Debug.Log(www.text);
            var resultsJson = JSON.Parse(www.text);
            List<Result> resultsList = new List<Result>();
            for (var i = 0; i < resultsJson.Count; i++)
            {
                var resultJ = resultsJson[i];
                Result result = new Result(this.assessmentId, this.student.id, resultJ["questionId"], resultJ["score"].AsDouble, resultJ["duration"].AsInt, resultJ["attempts"].AsInt, resultJ["completionStatus"]);
                resultsList.Add(result);
            }
            delegateInstance.handleResults(resultsList);
        }
    }

    public void SaveResult(Result result) {
        string url = baseUrl + "/assessment/" + result.assessmentId + "/student/" + this.student.id + "/assessmentItemResult/" + result.questionId;
        Debug.Log("saving result: " + url);
        Hashtable headers = new Hashtable();
        headers.Add("Cookie", this.cookie);
        headers.Add("Content-Type", "application/json");

        string jsonString = "{\"score\" : \"" + result.score +  "\", \"duration\" : \"" + result.duration + "\", \"completionStatus\" : \"" + result.completionStatus + "\"}";
        WWW saveResult = new WWW(url, System.Text.Encoding.UTF8.GetBytes(jsonString), headers);
        StartCoroutine(WaitForResultPost(saveResult));
    }

    IEnumerator WaitForResultPost(WWW www) {
        yield return www;
        Debug.Log("post result with cookie: " + this.cookie);
        if (www.error != null)
        {
            Debug.Log("error posting: " + www.error);
            delegateInstance.handleSaveResultError();
        }
        else
        {
            Debug.Log("success posting: " + www.text);
            delegateInstance.handleSaveResult();
        }
    }

    public void OpenBrightcenterApp() {
        Debug.Log("opening bc app with url: " + this.appUrl + "assessmentId: " + this.assessmentId);
        if (this.appUrl != null) {
            Application.OpenURL("brightcenterApp://?protocolName=" + this.appUrl + "&assessmentId=" + this.assessmentId);
        }        
    }
}
