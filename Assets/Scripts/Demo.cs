using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Demo : MonoBehaviour, IBrightcenter{


    BrightcenterController bcController = null;


	// Use this for initialization
	void Start () {
        Debug.Log("start demo");
        bcController = BrightcenterController.GetInstance(this.gameObject);
        bcController.setDelegate(this);
        bcController.appUrl = "demoApp";
        bcController.handleUrl("tracy://?data=eyJsYXN0TmFtZSI6IkJlZGFmIiwicGVyc29uSWQiOiI1MmIzMGI0YjMwMDQ3Y2Y5ZGVkOThjNzgiLCJmaXJzdE5hbWUiOiJCcmFjaGEifQ**&cookie=EEB0640F6353B46587EB9E3A9403A660&assessmentId=e027be54-a4a1-4df3-9ae3-8f43ab6aa4b3");
	}
	
	// Update is called once per frame
	void Update () {

    }

    public void OnClickGetResults()
    {
        bcController.GetResult();
        Debug.Log("clicked");
    }

    public void OnClickSaveResult() {
        Result result = new Result();
        result.assessmentId = bcController.assessmentId;
        result.studentId = bcController.student.id;
        result.questionId = "0";
        result.score = 4.0;
        result.duration = 21;
        result.setCompleted();
        bcController.SaveResult(result);
    }

    public void handleResults(List<Result> results) {
        Debug.Log("handling results");
        foreach(Result result in results){
            result.Log();
        }
    }

    public void handleResultsError() {
        Debug.Log("error with getting results");
    }

    public void handleSaveResult() {
        Debug.Log("result is saved");
    }

    public void handleSaveResultError() {
        Debug.Log("error while saving result");
    }
}
