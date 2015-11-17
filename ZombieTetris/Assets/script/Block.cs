using UnityEngine;
using System.Collections;

public class Block : MonoBehaviour {

	public bool isZombie = false;

	void Start(){

	}


	public void turnToZombie(){
		gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("image/blockZombie");
		isZombie = true;
	}


}
