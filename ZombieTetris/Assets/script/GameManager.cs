using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {


	public static GameManager instance;
	public static int gridWidth = 15;
	public static int gridHeight = 24;
	private Spawn spawnInstance;
	public bool haveCurrentTetris = false;


	public List<GameObject> currentTetrisBlockList = new List<GameObject>();
	private List<GameObject> stoppedTetrisBlockList = new List<GameObject>();
	private List<GameObject> stoppedZombieBlockList = new List<GameObject>();


	private BlockKind[,] blockKindArray = new BlockKind[gridWidth,gridHeight];
	

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
	   InvokeRepeating("moveDown",0f,0.4f);
	}
	// Update is called once per frame
	void Update () {
	    checkKey();
	}



//=======================================================
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
				updateInfectedPercentageText();
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
				float maxX = (float)gridWidth;
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
					padding = ((float)gridWidth - maxX);
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
		}
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

		int[] temp = new int[gridHeight]; //the amount of block in each row //the spawn place is uper the scene
		int[] movedis = new int[gridHeight+1]; // after we check all the row the distace that each row need to move
		for (int i = 0; i < stoppedTetrisBlockList.Count; i++) {
			int y = (int)stoppedTetrisBlockList[i].transform.position.y;
			if(y >=1 && y <= gridWidth){
				temp[y]++;
				if(temp[y] == gridWidth){
					for(int j = gridHeight; j >= y+1 ;j--){
						movedis[j]++;
					}
					haveRowFull = true;
				}
			}
		}

		if(haveRowFull){
			List<GameObject> needDestroy = new List<GameObject>();
			for (int i = 0; i < stoppedTetrisBlockList.Count; i++) {
				GameObject stoppedTetrisBlock = stoppedTetrisBlockList[i];
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

	//=======================================================

	void moveToStoppedList(){
		foreach (GameObject tetrisBlock in currentTetrisBlockList) {
			stoppedTetrisBlockList.Add(tetrisBlock);
			if(tetrisBlock.GetComponent<Block>().kind == BlockKind.zombie){
				stoppedZombieBlockList.Add(tetrisBlock);
			}
			
			if(tetrisBlock.transform.position.y >= gridHeight){
				gameOver();
			}
		}
		
		foreach (GameObject item in blockKindArray) {
			int positionX = item.transform.position.x;
			int positionY = item.transform.position.y;

			int realX = positionX / 1;
			int realY = positionY / 1;

			blockKindArray[realX,realY] = item.GetComponent<Block>().kind;
			item.GetComponent<Block>().setBlockPosition(realX,realY);

		}
		
		
		//update the blokkind array here
	}


	void SpawnBox(){
		currentTetrisBlockList.Clear();
		spawnInstance.SpawnBox();
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
							stoppedTetrisBlock.GetComponent<Block>().getHurt();
						    if(stoppedTetrisBlock.GetComponent<Block>().kind == BlockKind.zombie){
								newZombieBlockList.Add(stoppedTetrisBlock);
							}
							//
					}
					
					else if(Math.Abs(normaly - zombiey) == 1 && normalx == zombiex){
						stoppedTetrisBlock.GetComponent<Block>().getHurt();
						if(stoppedTetrisBlock.GetComponent<Block>().kind == BlockKind.zombie){
							newZombieBlockList.Add(stoppedTetrisBlock);
						}
					}
				}
			}
		}
		for (int i = 0; i < newZombieBlockList.Count; i++) {
			stoppedZombieBlockList.Add(newZombieBlockList[i]);
		}

		if(stoppedTetrisBlockList.Count == stoppedZombieBlockList.Count && stoppedTetrisBlockList.Count > 0){
			gameOver();
		}
	}

	void updateInfectedPercentageText(){
		Text infectedPercentageText = GameObject.Find("InfectedPercentage").GetComponent<Text>();
		float percentage = (float)stoppedZombieBlockList.Count / (float)stoppedTetrisBlockList.Count * 100;

		if(percentage < 50){
			infectedPercentageText.color = Color.green;
		}else if(percentage < 80){
			infectedPercentageText.color = Color.yellow;
		}else{
			infectedPercentageText.color = Color.red;
		}

		infectedPercentageText.text = "Infected: "+percentage.ToString("f2")+"%";
	}


	void gameOver(){
		//if the block has reach the top, the game is over
		Time.timeScale = 0;
		UIController.instance.setGameOverTextVisible();
	}

	//======get  set 

	public BlockKind[,] getBlockKindArray(){
		return blockKindArray;
	}


}
