using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

	// Use this for initialization
	public static UIController instance;
	public Text gameOverText;
	void Awake(){
		if(instance == null){
			instance = this;
			DontDestroyOnLoad(gameObject);
			initUI();
		}else{
			DestroyImmediate(this);
		}
	}

	void initUI(){
		gameOverText.gameObject.SetActive(false);
	}


	public void setGameOverTextVisible(){
		gameOverText.gameObject.SetActive(true);
	}

}
