using UnityEngine;
using System.Collections;

public class Room_Handler : MonoBehaviour {

	public Player player;

	public GameObject roomPrefab;
	public Room room;


	// Use this for initialization
	void Start () {
		newRoom();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Space)){
			newRoom();
		}
	}

	void newRoom(){
		if(room != null){
			room.killAllEnemies();
			Destroy(room.gameObject);
		}
		
		GameObject g = Instantiate(roomPrefab, Vector3.zero, Quaternion.identity) as GameObject;
		room = g.GetComponent<Room>();
		room.player = player;
		g.transform.parent = transform;
	}
}
