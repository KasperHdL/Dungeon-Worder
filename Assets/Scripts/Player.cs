using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Player : MonoBehaviour {

	public Room_Handler roomHandler;
	public Word word;

	public Enemy selectedEnemy;
	public int selectedEnemyIndex = -1;

	// Use this for initialization
	void Start () {
		transform.localScale = new Vector2(Settings.PLAYER_SIZE_X,Settings.PLAYER_SIZE_Y);
	}
	
	// Update is called once per frame
	void Update () {
		if(selectedEnemy == null && roomHandler.room.enemies.Count != 0){
			selectedEnemyIndex = 0;
			selectedEnemy = roomHandler.room.enemies[0].GetComponent<Enemy>();
			word.setNewWord();
		}else if(roomHandler.room.enemies.Count == 0){
			transform.rotation = Quaternion.identity;


			return;
		}

		float angle = Vector2.Angle(Vector2.right,selectedEnemy.transform.position-transform.position);
		transform.rotation = Quaternion.Euler(0,0,angle);

		string input = Input.inputString.ToLower();
		if(input != ""){
			if(input == " "){
				selectedEnemyIndex++;
				if(selectedEnemyIndex == roomHandler.room.enemies.Count)
					selectedEnemyIndex = 0;
				
				selectedEnemy = roomHandler.room.enemies[0].GetComponent<Enemy>();
				word.setNewWord();
			}else{
				word.input(input);
			}
		}
	}

	public void Attack(int wordLength){
		float damage = Settings.PLAYER_DAMAGE;
		damage *= wordLength / Settings.LONGEST_WORD;

		selectedEnemy.takeDamage(damage * Settings.PLAYER_DAMAGE_MULTIPLIER);
	}

}