using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public static GameManager instance;
	public static int gridWidth = 20;
	public static int gridHeight = 32;
	private Spawn spawnInstance;
	public bool haveCurrentBox = false;
	public GameObject currentBox;
	public List<GameObject> currentBoxChild = new List<GameObject>();
	private List<GameObject> boxChildStopped = new List<GameObject>();
	private int[] eachRowBlockAmount = new int[33];
	private GameObject[,] boxChildStoppedArray = new GameObject[20,32];
	private int[,] boxStoppedTag = new int[20,32];
	
	void Awake(){
		if(instance == null){
			instance = this;
			DontDestroyOnLoad(gameObject);
		}else{
			DestroyImmediate(this);
		}
	}
	// Use this for initialization
	void Start () {
	   spawnInstance = Spawn.instance;
	   InvokeRepeating("moveDown",0f,0.5f);
	}
	// Update is called once per frame
	void Update () {
	    checkKey();
	}

//check the key
	void checkKey(){
		if(Input.GetKeyDown("a")){
			moveLeft();
		}

		if(Input.GetKeyDown("w")){
		    rotate();
		}

		if(Input.GetKeyDown("s")){
			moveDown();
		}

		if(Input.GetKeyDown("d")){
			moveRight();
		}
	}
// the key function
	void moveDown(){
		if(haveCurrentBox){
			Vector3 prePosition = currentBox.transform.position;
			currentBox.transform.position += new Vector3(0,-1,0);
			if(!checkMoveable()){
				currentBox.transform.position = prePosition;
				moveToStoppedList();
				checkRowFull();
				SpawnBox();
			}
		}
	}
	void moveRight(){
		if(haveCurrentBox){
			Vector3 prePoisition = currentBox.transform.position;
			currentBox.transform.position += new Vector3(1,0,0);
			if(!checkMoveable()){
				currentBox.transform.position = prePoisition;
			}
		}
	}
	
	void moveLeft(){
		if(haveCurrentBox){
			Vector3 prePosition = currentBox.transform.position;
			currentBox.transform.position += new Vector3(-1,0,0);
			if(!checkMoveable()){
				currentBox.transform.position = prePosition;
			}
		}
	}
	void rotate(){
		//fake rotate
		/*if(haveCurrentBox){
			currentBox.transform.Rotate(0,0,90);
		}*/
		if(haveCurrentBox){
			Vector3[] prePosition = new Vector3[currentBoxChild.Count];
			for(int i = 0; i < currentBoxChild.Count ;i++){
				prePosition[i] = currentBoxChild[i].transform.position;
			}

			Vector3 centerPosition = currentBoxChild[0].transform.position;
			foreach (GameObject boxChild in currentBoxChild) {
				Vector3 dir = boxChild.transform.position - centerPosition;
				dir = Quaternion.Euler(new Vector3(0,0,90))*dir;
				boxChild.transform.position =  centerPosition + dir;
			}
			if(!checkInBound()){
				//adjust to the right position, not refuse raotate;

				float minX = 1f;
				float maxX = 20f;
				foreach (GameObject child in currentBoxChild) {
					Vector3 position = child.transform.position;
					if(position.x > maxX){
						maxX = position.x;
					}
					if(position.x < minX){
						minX = position.x;
					}
				}
				float padding;
				if(maxX > gridWidth){
					padding = (20f - maxX);
					//right over
				}else{
					padding = (1f - minX);
				    //left over
				}
				currentBox.transform.position += new Vector3 (padding,0,0);

			}else if(!checkOverlap()){
				for(int i = 0; i < currentBoxChild.Count ;i++){
					currentBoxChild[i].transform.position = prePosition[i];
				}
			}
		}


	}

	bool checkInBound(){

		foreach (GameObject boxChild in currentBoxChild) {
			Vector3 position = boxChild.transform.position;
			if(!(position.x >= 1 && position.x <= gridWidth && position.y >= 1)){
			  return false;
			}
			//Debug.Log(position.x+" "+position.y);
		}
		//Debug.Log("inBound");
		return true;
	}

	bool checkOverlap(){
		foreach (GameObject stoppedBoxChild in boxChildStopped){
			foreach (GameObject boxChild in currentBoxChild) {
				Vector3 positionA = boxChild.transform.position;
				Vector3 positionB = stoppedBoxChild.transform.position;
				float dis = Mathf.Pow((positionA.x - positionB.x),2) + Mathf.Pow((positionA.y - positionB.y),2);
				if(dis < 1){
					return false;
				}
			}
		}
		//Debug.Log("Overlap True");
		return true;
	}

	bool checkMoveable(){
		if(checkInBound()&&checkOverlap()){
			return true;
		}else{
			return false;
		}
	}

	void checkRowFull(){
		for(int i = 1; i <= gridHeight ; i++){
			if(eachRowBlockAmount[i] == gridWidth){
				moveRowDown(i);
				for(int j = i; j<= gridHeight-1;j++){
					eachRowBlockAmount[j] = eachRowBlockAmount[j+1];
				}
				i = 0; 
				//every time when a row fall down scan from the begin when there is no full line the for will break
			}
		}
	}

	void moveRowDown(int row){
		List<GameObject> finishDelete = new List<GameObject>();
		List<GameObject> needDelete = new List<GameObject>();
		for(int i = 0; i < boxChildStopped.Count ; i++){
			if((int)boxChildStopped[i].transform.position.y > row){
				boxChildStopped[i].transform.position += new Vector3(0,-1,0);
				finishDelete.Add(boxChildStopped[i]);
			}else if((int)boxChildStopped[i].transform.position.y == row){
				needDelete.Add(boxChildStopped[i]);
			}
		}
		foreach (GameObject item in needDelete) {
			DestroyImmediate(item.gameObject);
		}
		boxChildStopped = finishDelete;
	}

	void SpawnBox(){
		currentBoxChild.Clear();
		spawnInstance.SpawnBox();
	}

	void moveToStoppedList(){
		foreach (GameObject boxChild in currentBoxChild) {
			boxChildStopped.Add(boxChild);
			int y = (int)boxChild.transform.position.y;
			int x = (int)boxChild.transform.position.x;
			boxChildStoppedArray[x-1,y-1] = boxChild;
			boxStoppedTag[x-1,y-1] = 1;
			eachRowBlockAmount[y]++; // there is one block be added to this line
		}
	}


}
