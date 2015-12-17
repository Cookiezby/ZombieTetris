using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum BlockKind{
	normal = 0,
	zombie = 1,
	medical = 2,
	police = 3,
	fire = 4,
}

public class Block : MonoBehaviour {
	
	public BlockKind kind = BlockKind.normal;
	public int fullHealth = 4;
	public GameObject healthBar;
	public Animator blockAnimator;
	private int currentHealth;

	private int positionX;
	private int positionY;

	private BlockKind[,] blockKindArray;

	void Awake(){
		currentHealth = 4;
		blockAnimator = gameObject.GetComponent<Animator>();
		blockKindArray = GameObject.Find("BoxGenerater").GetComponent<GameManager>();
	}

	void Start(){

	}


	public void turnToZombie(){
		gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("image/zombieBlock");
		gameObject.GetComponent<Animator>().runtimeAnimatorController = Resources.Load("animation/zombieAnimation/zombieAnimator") as RuntimeAnimatorController;
		kind = BlockKind.zombie;
		healthBar.transform.localScale = new Vector3(1,1,1);
	}

	public void turnToNormal(){
		gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("image/humanBlock");
		gameObject.GetComponent<Animator>().runtimeAnimatorController = Resources.Load("animation/defaultAnimation/defaultAnimator") as RuntimeAnimatorController;
		kind = BlockKind.normal;

	}

	public void turnToMedical(){

	}

	public void turnToPolice(){

	}

	public void turnToFire(){
		gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("image/fireBlock");
	}

	public void turnToKind(BlockKind kind){

	}


	public void setBlockPosition(int x, int y){
		positionX = x;
		positionY = y;
	}

	public void getHurt(){
		currentHealth -= 1;
		blockAnimator.SetTrigger("attacked");
		if(currentHealth == 0){
			turnToZombie();
			return;
		}
		float scaleX  =(float)currentHealth / (float)fullHealth;
		healthBar.transform.localScale = new Vector3(scaleX,1,1);
	}

	void excute(){
		switch(kind){
		case BlockKind.normal:
			break;
		case BlockKind.medical:
			break;
		case BlockKind.police:
			break;
		case BlockKind.fire:
			break;
		case BlockKind.zombie:
			behaviorZombie();
			break;
		}
	}

	void behaviorZombie(){
		// attack block aroud it
	}

	void behaviorFire(){

	}

	void behaviorMedical(){

	}

}















