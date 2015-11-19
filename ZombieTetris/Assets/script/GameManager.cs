using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public static GameManager instance;
	public static int gridWidth = 20;
	public static int gridHeight = 32;
	private Spawn spawnInstance;
	public bool haveCurrentTetris = false;


	public List<GameObject> currentTetrisBlockList = new List<GameObject>();
	private List<GameObject> stoppedTetrisBlockList = new List<GameObject>();
	private int[] eachRowBlockAmount = new int[gridHeight + 1];
	private List<GameObject> stoppedZombieBlockList = new List<GameObject>();

	
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
	   InvokeRepeating("moveDown",0f,0.2f);
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
		if(haveCurrentTetris){
			for(int i = 0; i < currentTetrisBlockList.Count;i++){
				currentTetrisBlockList[i].transform.position += new Vector3(0,-1,0);
			}
			if(!checkMoveable()){
				for(int i = 0; i < currentTetrisBlockList.Count;i++){
					currentTetrisBlockList[i].transform.position += new Vector3(0,1,0);
				}
				moveToStoppedList();
				zombieInfect();
				checkRowFull();
				SpawnBox();
			}
		}
	}
	void moveRight(){
		if(haveCurrentTetris){
			for(int i = 0; i < currentTetrisBlockList.Count;i++){
				currentTetrisBlockList[i].transform.position += new Vector3(1,0,0);
			}
			if(!checkMoveable()){
				for(int i = 0; i < currentTetrisBlockList.Count;i++){
					currentTetrisBlockList[i].transform.position += new Vector3(-1,0,0);
				}
			}
		}
	}
	
	void moveLeft(){
		if(haveCurrentTetris){
			for(int i = 0; i < currentTetrisBlockList.Count;i++){
				currentTetrisBlockList[i].transform.position += new Vector3(-1,0,0);
			}
			if(!checkMoveable()){
				for(int i = 0; i < currentTetrisBlockList.Count;i++){
					currentTetrisBlockList[i].transform.position += new Vector3(1,0,0);
				}
			}
		}
	}
	void rotate(){
		//fake rotate
		/*if(haveCurrentBox){
			currentBox.transform.Rotate(0,0,90);
		}*/
		if(haveCurrentTetris){
			Vector3[] prePosition = new Vector3[currentTetrisBlockList.Count];
			for(int i = 0; i < currentTetrisBlockList.Count ;i++){
				prePosition[i] = currentTetrisBlockList[i].transform.position;
			}

			Vector3 centerPosition = currentTetrisBlockList[0].transform.position;
			foreach (GameObject boxChild in currentTetrisBlockList) {
				Vector3 dir = boxChild.transform.position - centerPosition;
				dir = Quaternion.Euler(new Vector3(0,0,90))*dir;
				boxChild.transform.position =  centerPosition + dir;
			}
			if(!checkInBound()){
				//adjust to the right position, not refuse raotate;

				float minX = 1f;
				float maxX = 20f;
				foreach (GameObject child in currentTetrisBlockList) {
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

				for(int i = 0; i < currentTetrisBlockList.Count;i++){
					currentTetrisBlockList[i].transform.position += new Vector3(padding,0,0);
				}

			}else if(!checkOverlap()){
				for(int i = 0; i < currentTetrisBlockList.Count ;i++){
					currentTetrisBlockList[i].transform.position = prePosition[i];
				}
			}
		}


	}

	bool checkInBound(){
		foreach (GameObject boxChild in currentTetrisBlockList) {
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
		foreach (GameObject stoppedBoxChild in stoppedTetrisBlockList){
			foreach (GameObject boxChild in currentTetrisBlockList) {
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
		/*for(int i = 1; i <= gridHeight; i++){
			if(eachRowBlockAmount[i] == gridWidth){
				moveRowDown(i);
				for(int j = i; j<= gridHeight-1;j++){
					eachRowBlockAmount[j] = eachRowBlockAmount[j+1];
				}
				i = 0;
				//every time when a row fall down scan from the begin when there is no full line the for will break
			}
		}*/
		bool haveRowFull = false;

		int[] temp = new int[gridHeight]; //the amount of block in each row
		int[] movedis = new int[gridHeight+1]; // after we check all the row the distace that each row need to move
		for (int i = 0; i < stoppedTetrisBlockList.Count; i++) {
			int y = (int)stoppedTetrisBlockList[i].transform.position.y;
			temp[y]++;
			if(temp[y] == gridWidth){
				for(int j = gridHeight; j >= y+1 ;j--){
					movedis[j]++;
				}
				haveRowFull = true;
			}
		}

		if(haveRowFull){
			List<GameObject> needDestroy = new List<GameObject>();
			for (int i = 0; i < stoppedTetrisBlockList.Count; i++) {
				GameObject stoppedTetrisBlock = stoppedTetrisBlockList[i];
				int x = (int)stoppedTetrisBlockList[i].transform.position.x;
				int y = (int)stoppedTetrisBlockList[i].transform.position.y;

				if(temp[y] < gridWidth){
					stoppedTetrisBlockList[i].transform.position += new Vector3(0,-movedis[y],0);
				}
				if(temp[y] == gridWidth){
					needDestroy.Add(stoppedTetrisBlock);
				}
			}

			foreach (GameObject item in needDestroy) {
				if(item.GetComponent<Block>().kind == BlockKind.zombie){
					stoppedZombieBlockList.Remove(item);
				}
				stoppedTetrisBlockList.Remove(item);
				DestroyImmediate(item.gameObject);
			}
		}

	}

	void SpawnBox(){
		currentTetrisBlockList.Clear();
		spawnInstance.SpawnBox();
	}

	void moveToStoppedList(){
		foreach (GameObject tetrisBlock in currentTetrisBlockList) {
			stoppedTetrisBlockList.Add(tetrisBlock);
			if(tetrisBlock.GetComponent<Block>().kind == BlockKind.zombie){
				stoppedZombieBlockList.Add(tetrisBlock);
			}
			int y = (int)tetrisBlock.transform.position.y;
			eachRowBlockAmount[y]++; // there is one block be added to this line
		}
	}

	void zombieInfect(){
		List<GameObject> newZombieBlockList = new List<GameObject>();
		foreach (GameObject zombieBlock  in stoppedZombieBlockList) {
			int zombiex = (int)zombieBlock.transform.position.x;
			int zombiey = (int)zombieBlock.transform.position.y;
			foreach (GameObject stoppedTetrisBlock in stoppedTetrisBlockList) {
				int normalx = (int)stoppedTetrisBlock.transform.position.x;
				int normaly = (int)stoppedTetrisBlock.transform.position.y;
				if(stoppedTetrisBlock.GetComponent<Block>().kind != BlockKind.zombie){
					if(Math.Abs(normalx - zombiex) == 1 && normaly == zombiey){
							stoppedTetrisBlock.GetComponent<Block>().turnToZombie();
							newZombieBlockList.Add(stoppedTetrisBlock);
					}
					
					else if(Math.Abs(normaly - zombiey) == 1 && normalx == zombiex){
						stoppedTetrisBlock.GetComponent<Block>().turnToZombie();
						newZombieBlockList.Add(stoppedTetrisBlock);
					}
				}
			}
		}
		for (int i = 0; i < newZombieBlockList.Count; i++) {
			stoppedZombieBlockList.Add(newZombieBlockList[i]);
		}
	}


}
