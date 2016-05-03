using UnityEngine;
using System.Collections;

public class BrightcenterButtonDelegation : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void DelegateClick() {
        BrightcenterController bcController = BrightcenterController.GetInstance(this.gameObject);
        bcController.OpenBrightcenterApp();
    }
}
