using UnityEngine;
using System.Collections;

public enum BlockKind{
	normal = 0,
	zombie = 1,
	medical = 2,
}

public class Block : MonoBehaviour {
	
	public BlockKind kind = BlockKind.normal;
	public int health = 10;
	
	void Start(){

	}


	public void turnToZombie(){
		gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("image/blockZombie");
		kind = BlockKind.zombie;
	}

	public void turnToNormal(){
		gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("image/block");
		kind = BlockKind.normal;
	}

	public void turnToMedical(){
		gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("image/blockMedical");
		kind = BlockKind.medical;
	}


}
