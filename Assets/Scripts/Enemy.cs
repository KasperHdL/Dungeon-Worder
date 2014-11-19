using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

/////////////////////////////////////
//			 VARIABLES			   //
/////////////////////////////////////

	public Player player;
	public Room room;

	public float health = Settings.ENEMY_HEALTH;

/////////////////////////////////////
//			  BUILT-IN			   //
/////////////////////////////////////

	void Start () {
		transform.localScale = new Vector3(Settings.ENEMY_SIZE_X,Settings.ENEMY_SIZE_Y,1f);
	}
	
	void Update () {
		float angle = Vector2.Angle(Vector2.right,transform.position-player.transform.position);
		transform.rotation = Quaternion.Euler(0,0,angle);
	}

/////////////////////////////////////
//			   PUBLIC			   //
/////////////////////////////////////

	public void takeDamage(float amount){
		health -= amount;
		if(health <= 0){
			room.enemies.RemoveAt(player.selectedEnemyIndex);
			Destroy(gameObject);
		}
	}
}
