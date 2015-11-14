using UnityEngine;
using System.Collections;

public class SpawnBox : MonoBehaviour {

	
	public static int bornPosX  = 8;
	public static int bornPosY  = 36;
	private GameManager boxesInstance;
 
	// Use this for initialization
	void Start () {
		//boxesInstance = Boxes.getInstance;
		//SpawnNewBox();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	/*public void SpawnNewBox(){
		int i = Random.Range(1,7);
		int[,] boxPosition;
		switch (i) {
		case 1:
			boxPosition = new int[4,4]{
				{1,0,0,0},
				{1,0,0,0},		
				{1,0,0,0},
				{1,0,0,0}
		    };
			break;
		case 2:
			boxPosition = new int[4,4]{
				{0,0,0,0},
				{1,0,0,0},		
				{1,0,0,0},
				{1,1,0,0}
			};
			break;
		case 3:
			boxPosition = new int[4,4]{
				{0,0,0,0},
				{0,1,0,0},		
				{0,1,0,0},
				{1,1,0,0}
			};
			break;
		case 4:
			boxPosition = new int[4,4]{
				{0,0,0,0},
				{1,0,0,0},		
				{1,1,0,0},
				{0,1,0,0}
			};
			break;
		case 5:
			boxPosition = new int[4,4]{
				{0,0,0,0},
				{0,1,0,0},		
				{1,1,0,0},
				{1,0,0,0}
			};
			break;
		case 6:
			boxPosition = new int[4,4]{
				{0,0,0,0},
				{1,0,0,0},		
				{1,1,0,0},
				{1,0,0,0}
			};
			break;
		case 7:
			boxPosition = new int[4,4]{
				{0,0,0,0},
				{0,0,0,0},		
				{1,1,0,0},
				{1,1,0,0}
			};
			break;
		default:
			break;
		}

		//boxesInstance.currentBox = boxPosition;
		//boxesInstance.currentPosX = bornPosX;
		//boxesInstance.currentPoxY = bornPosY;
		//Instantiate(boxList[i],transform.position,Quaternion.identity);
	}*/
}
