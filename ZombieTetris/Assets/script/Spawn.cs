using UnityEngine;
using System.Collections;

public class Spawn : MonoBehaviour {

	public static Spawn instance;
	public  GameObject[] boxTemplate;
	private GameManager gmInstance;

	public int frequencyOfZombie = 8; //  20% will have a zombie block 


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
	  gmInstance = GameManager.instance;
	  SpawnBox(); 
	}
	
	// Update is called once per frame
	void Update () {
	  
	}

	void generateZombieBlock(GameObject tetris){
		int index = Random.Range(0,4);
		GameObject tetrisBlock = tetris.transform.GetChild(index).gameObject;
		Sprite zombieSprite = Resources.Load<Sprite>("image/blockZombie");
		tetrisBlock.GetComponent<SpriteRenderer>().sprite = zombieSprite;
		tetrisBlock.GetComponent<Block>().isZombie = true;
	}

	public  void SpawnBox(){
		int i = Random.Range(0,7);
		GameObject tetris = Instantiate(boxTemplate[i].gameObject,transform.position,Quaternion.identity) as GameObject;

		int zombie = Random.Range(1,9);
		if(zombie <= frequencyOfZombie){
		   generateZombieBlock(tetris);
		}
		gmInstance.currentTetris = tetris;
		foreach(Transform child in tetris.transform){
			gmInstance.currentTetrisBlockList.Add(child.gameObject);
		}

		gmInstance.haveCurrentTetris = true;
	}
}
