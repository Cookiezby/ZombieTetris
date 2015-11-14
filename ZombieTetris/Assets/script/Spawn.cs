using UnityEngine;
using System.Collections;

public class Spawn : MonoBehaviour {

	public static Spawn instance;
	public  GameObject[] boxTemplate;
	private GameManager gmInstance;

	public Sprite zombieBlockSprite;
	public Sprite normalBlockSprite;

	public int frequencyOfZombie = 2; //  20% will have a zombie block 


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

	void generateZombieBlock(GameObject box){
		int index = Random.Range(0,4);
		GameObject block = box.transform.GetChild(index).gameObject;
		block.GetComponent<SpriteRenderer>().sprite = zombieBlockSprite;
		block.GetComponent<Block>().isZombie = true;
	}

	public  void SpawnBox(){
		int i = Random.Range(0,7);
		GameObject box = Instantiate(boxTemplate[i].gameObject,transform.position,Quaternion.identity) as GameObject;

		int zombie = Random.Range(1,11);
		if(zombie <= frequencyOfZombie){
		   generateZombieBlock(box);
		}

		gmInstance.currentBox = box;
		foreach(Transform child in box.transform){
			gmInstance.currentBoxChild.Add(child.gameObject);
		}
		gmInstance.haveCurrentBox = true;
	}
}
