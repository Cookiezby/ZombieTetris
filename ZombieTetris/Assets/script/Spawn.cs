using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spawn : MonoBehaviour {

	public static Spawn instance;
	public  GameObject[] boxTemplate;
	private GameManager gmInstance;


    private List<List<Vector3>> tetrisTemplate = new List<List<Vector3>>();


	public int frequencyOfZombie = 2; //  20% will have a zombie block 

	void templatePosition(){
		List<Vector3> templateA = new List<Vector3>();
		Vector3 A_1 = new Vector3(0f,0f,0f)+transform.position;
		Vector3 A_2 = new Vector3(0f,1f,0f)+transform.position;
		Vector3 A_3 = new Vector3(0f,-1f,0f)+ transform.position;
		Vector3 A_4 = new Vector3(1f,0f,0f)+transform.position;
		templateA.Add(A_1);
		templateA.Add(A_2);
		templateA.Add(A_3);
		templateA.Add(A_4);


		List<Vector3> templateB = new List<Vector3>();
		Vector3 B_1 = new Vector3(0f,0f,0f)+transform.position;
		Vector3 B_2 = new Vector3(0f,1f,0f)+transform.position;
		Vector3 B_3 = new Vector3(1f,1f,0f)+ transform.position;
		Vector3 B_4 = new Vector3(1f,0f,0f)+transform.position;
		templateB.Add(B_1);
		templateB.Add(B_2);
		templateB.Add(B_3);
		templateB.Add(B_4);

		List<Vector3> templateC = new List<Vector3>();
		Vector3 C_1 = new Vector3(0f,0f,0f)+transform.position;
		Vector3 C_2 = new Vector3(0f,1f,0f)+transform.position;
		Vector3 C_3 = new Vector3(0f,2f,0f)+ transform.position;
		Vector3 C_4 = new Vector3(0f,-1f,0f)+transform.position;
		templateC.Add(C_1);
		templateC.Add(C_2);
		templateC.Add(C_3);
		templateC.Add(C_4);

		List<Vector3> templateD = new List<Vector3>();
		Vector3 D_1 = new Vector3(0f,0f,0f)+transform.position;
		Vector3 D_2 = new Vector3(0f,1f,0f)+transform.position;
		Vector3 D_3 = new Vector3(1f,0f,0f)+ transform.position;
		Vector3 D_4 = new Vector3(1f,-1f,0f)+transform.position;
		templateD.Add(D_1);
		templateD.Add(D_2);
		templateD.Add(D_3);
		templateD.Add(D_4);

		List<Vector3> templateE = new List<Vector3>();
		Vector3 E_1 = new Vector3(0f,0f,0f)+transform.position;
		Vector3 E_2 = new Vector3(1f,0f,0f)+transform.position;
		Vector3 E_3 = new Vector3(1f,1f,0f)+ transform.position;
		Vector3 E_4 = new Vector3(0f,-1f,0f)+transform.position;
		templateE.Add(E_1);
		templateE.Add(E_2);
		templateE.Add(E_3);
		templateE.Add(E_4);

		List<Vector3> templateF = new List<Vector3>();
		Vector3 F_1 = new Vector3(0f,0f,0f)+transform.position;
		Vector3 F_2 = new Vector3(0f,1f,0f)+transform.position;
		Vector3 F_3 = new Vector3(0f,-1f,0f)+ transform.position;
		Vector3 F_4 = new Vector3(1f,1f,0f)+transform.position;
		templateF.Add(F_1);
		templateF.Add(F_2);
		templateF.Add(F_3);
		templateF.Add(F_4);

		List<Vector3> templateG = new List<Vector3>();
		Vector3 G_1 = new Vector3(0f,0f,0f)+transform.position;
		Vector3 G_2 = new Vector3(0f,1f,0f)+transform.position;
		Vector3 G_3 = new Vector3(0f,-1f,0f)+ transform.position;
		Vector3 G_4 = new Vector3(-1f,1f,0f)+transform.position;
		templateG.Add(G_1);
		templateG.Add(G_2);
		templateG.Add(G_3);
		templateG.Add(G_4);
	
		tetrisTemplate.Add(templateA);
		tetrisTemplate.Add(templateB);
		tetrisTemplate.Add(templateC);
		tetrisTemplate.Add(templateD);
		tetrisTemplate.Add(templateE);
		tetrisTemplate.Add(templateF);
		tetrisTemplate.Add(templateG);

	}

	void Awake(){
		if(instance == null){
			instance = this;
			templatePosition();
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

	/*void generateZombieBlock(GameObject tetris){
		int index = Random.Range(0,4);
		GameObject tetrisBlock = tetris.transform.GetChild(index).gameObject;
		Sprite zombieSprite = Resources.Load<Sprite>("image/blockZombie");
		tetrisBlock.GetComponent<SpriteRenderer>().sprite = zombieSprite;
		tetrisBlock.GetComponent<Block>().kind = BlockKind.zombie;
	}

	void generateMedicalBlock(GameObject tetris){
		int index = Random.Range(0,4);
		GameObject tetrisBlock = tetris.transform.GetChild(index).gameObject;
		Sprite medicalSprite = Resources.Load<Sprite>("image/blockMedical");
		tetrisBlock.GetComponent<SpriteRenderer>().sprite = medicalSprite;
		tetrisBlock.GetComponent<Block>().kind = BlockKind.medica;
	}*/

	/*public  void SpawnBox(){
		int i = Random.Range(0,7);
		GameObject tetris = Instantiate(boxTemplate[i].gameObject,transform.position,Quaternion.identity) as GameObject;


		int randomKind = Random.Range(0,5);
		switch(randomKind){
		case 1:
			generateZombieBlock(tetris);
			break;
		case 2:
			generateMedicalBlock(tetris);
			break;
		}

		gmInstance.currentTetris = tetris;
		foreach(Transform child in tetris.transform){
			gmInstance.currentTetrisBlockList.Add(child.gameObject);
		}

		gmInstance.haveCurrentTetris = true;
	}*/

	public void SpawnBox(){
		int templateKind = Random.Range(0,7);
		List<Vector3> blockPositions = tetrisTemplate[templateKind];




	    for (int i = 0; i < blockPositions.Count; i++) {
			GameObject block = Instantiate(Resources.Load("prefab/basicBlock")) as GameObject;
			block.transform.position = blockPositions[i];
			gmInstance.currentTetrisBlockList.Add(block);
			int randomKind = Random.Range(0,5);
			switch(randomKind){
			case 1:
				block.GetComponent<Block>().turnToZombie();
				break;
			case 2:
				block.GetComponent<Block>().turnToMedical();
				break;
			}
		}

		gmInstance.haveCurrentTetris = true;

	}
}
