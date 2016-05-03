using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IBrightcenter {

    void handleResults(List<Result> results);
    void handleResultsError();

    void handleSaveResult();
    void handleSaveResultError();

}
