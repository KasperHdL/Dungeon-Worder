using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Room : MonoBehaviour {


	public Player player;
	public Transform[] walls;
	public Transform floor;
	private Vector2 type;

	public Transform enemyContainer;
	public GameObject enemyPrefab;
	public List<Transform> enemies = new List<Transform>();

	public Vector2 playerSpawn;

	// Use this for initialization
	void Start () {
		type = new Vector2(Random.Range(Settings.ROOM_MIN_WIDTH,Settings.ROOM_MAX_WIDTH),Random.Range(Settings.ROOM_MIN_HEIGHT,Settings.ROOM_MAX_HEIGHT));
		setWallsFromType(type);
		//spawn enemies
		spawnEnemies(10);

		playerSpawn = new Vector2(0,-type.y + Settings.PLAYER_SPAWN_BUFFER_Y/2);
		player.transform.position = playerSpawn;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Return)){
			spawnEnemies(10);
			playerSpawn = new Vector2(0,-type.y + Settings.PLAYER_SPAWN_BUFFER_Y/2);
			player.transform.position = playerSpawn;
		}
	}

	void spawnEnemies(int c){
		for(int i = 0;i<c;i++){
			bool spawnPos = false;
			Vector2 pos = Vector2.zero;

			float w = Settings.WALL_SIZE/2;
			float eW = Settings.ENEMY_SIZE_X;
			float eH = Settings.ENEMY_SIZE_Y;
			float sbW = Settings.PLAYER_SPAWN_BUFFER_X;
			float sbH = Settings.PLAYER_SPAWN_BUFFER_Y;

			while(!spawnPos){
				pos = new Vector2(	Random.Range(-type.x + w + eW/2,type.x - w - eW/2),
									Random.Range(-type.y + w + eH/2,type.y - w - eH/2));
				if(!(pos.x < player.transform.position.x + sbW && 
					pos.x > player.transform.position.x - sbW &&
					pos.y < player.transform.position.y + sbH &&
					pos.y > player.transform.position.y - sbH))
					spawnPos = true;
			}
			GameObject g = Instantiate(enemyPrefab,pos,Quaternion.identity) as GameObject;
			Enemy e = g.GetComponent<Enemy>();
			e.player = player;
			e.room = this;
			enemies.Add(g.transform);
			g.transform.parent = enemyContainer;
		}
	}

	void setWallsFromType(Vector2 type){
		walls[0].position = new Vector2(-type.x,0);
		walls[1].position = new Vector2( type.x,0);
		walls[2].position = new Vector2(0,-type.y);
		walls[3].position = new Vector2(0, type.y);

		float w = Settings.WALL_SIZE;
		walls[0].localScale = new Vector3(w ,type.y*2, 1);
		walls[1].localScale = new Vector3(w ,type.y*2, 1);
		walls[2].localScale = new Vector3(type.x*2 + w ,w , 1);
		walls[3].localScale = new Vector3(type.x*2 + w ,w , 1);

		floor.localScale = type*2;
	}

	//public
	public void killAllEnemies(){
		for(int i = 0;i<enemies.Count;i++)
			Destroy(enemies[i].gameObject);
		enemies.Clear();
	}
}