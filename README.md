# Brightcenter SDK-Unity 
SDK for Unity 4.6.9 or higher
=============================

In this repo you'll find the the SDK for Brightcenter. In this file I'll describe how you can use the SDK.

###Download the project
First of all you need to download the project. You can either check it out with git or download the zip. The project-folder wil be located in the Assets folder. 

###Use the Button in your project
How to include the Brightcenter SDK in your project:

1. Open the scene where you will use the brightcenter button.
2. Drag and drop the contents of assets inside your assets-folder.
3. Drag the BrigtcenterButton.prefab on your canvas it should automatic allign to bottom-left.
4. Drag the BrightcenterButton to its on onclick in the inspector windows.
5. Appoint the BrightcenterButtonDelegate script to the BrightcenterButton, and select from the dropdown menu from the onclick component          BrightcenterButtonDeligate.DelegateClick.
6. Add the button to your scene, your scene should have its own eventhandeler if you make and canvas it should be added automatically, if not add it manualy.

###Log in automatically

Add the following code to the start method when your app starts.
``` C#
void Start () {
        bcController = BrightcenterController.GetInstance(this.gameObject);
        bcController.setDelegate(this);
        bcController.appUrl = "You app name here";
        bcController.handleUrl("Your Url here");
}
```


###How to use the SDK

Use the BrightcenterController to make an connection with the Brightcenter API. You can get an instance by using `BrightcenterController.GetInstance(gameObject)`.

To initialise the BrightcenterController you need to call the handleUrl. This method is ussualy called when the bc app is opend from the brightcenterApp. However this is not yet implemented, to workaround this you need to generate an url. This can be done by accesing the dashbaord got to the personal menu en press 'genereer url voor sdk'. You can chose a appurl and an student. Copy the generated Url and use that to call  `handleUrl(url).

The class that uses the BrightcenterController needs to implement the IBrightcenter interface. This interface needs to be set in de BrightcenterController by calling  `bcController.setDelegate()`.

When a GetResult call is made from BrightcenterController all the results for the assessment of the current student will be fetched. When this is resolved without errors the interface method `handleResults(List<Result> result)` will be called to handle the results, if there is an error the `handleResultsError()`will be called instead.
You can use `bcController.GetResult()` to retrieve results from the current student. 


You can use `SaveResult(Result result)`to save an result of a student. The Results should have:
```C#
assessmentId: the id of the assessment
studentId: the id of the student
questionId: the id of the question
score: the score of the student
duration: the duration in seconds
attempts: the amount of attempts
CompletionStatus: a completionStatus enum
```


Use `bcController.SaveResult(result);` to save the results from the current student. 

``` C#
//this is a code block!
Result result = new Result()
result.assessmentId = bcController.assessmentId;
result.studentId = bcController.student.id;
result.questionId = "0";
result.score = 4.0;
result.duration = 21;
result.setCompleted();
bcController.SaveResult(result);
```


Inside the Demo.cs you can look at examples to implement the SDK.

###Notes

-if you have problems using the sdk you can create an issue on github or with the jira issue tracker on tst-brightcenter.trifork.nl
