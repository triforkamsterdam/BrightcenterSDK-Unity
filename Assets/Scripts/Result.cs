using UnityEngine;
using System.Collections;

public class Result {

    public string assessmentId { get; set; }
    public string studentId { get; set; }
    public string questionId { get; set; }
    public double score { get; set; }
    public int duration { get; set; }
    public int attempts { get; set; }
    public string completionStatus { get; private set; }

    public Result() { 
    
    }


    public Result(string assessmentId, string studentId, string questionId, double score, int duration, int attempts, string completionStatus){
        this.assessmentId = assessmentId;
        this.questionId = questionId;
        this.score = score;
        this.duration = duration;
        this.attempts = attempts;
        this.completionStatus = completionStatus;
    }

    public void Log() {
        Debug.Log("result = "+ assessmentId +" "+ studentId +" "+ questionId +" "+ score +" "+ duration +" "+ attempts +" "+ completionStatus);
    }

    public void setCompleted() {
        this.completionStatus = "COMPLETED";
    }

    public void setInComplete() {
        this.completionStatus = "INCOMPLETE";
    }

}
