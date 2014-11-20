public static class Settings {

	//WORDS
	public static string[] ATTACKS = {"arrow", "ball", "missile"};
	public static string[] ELEMENTS = {"fire", "water", "wind", "earth", "light"};
	public static int LONGEST_ATTACK_WORD = 7;
	public static int LONGEST_ELEMENT_WORD = 5;
	public static int LONGEST_WORD = LONGEST_ATTACK_WORD + LONGEST_ELEMENT_WORD;

	//PLAYER
	public static float PLAYER_SPAWN_BUFFER_X = 2;
	public static float PLAYER_SPAWN_BUFFER_Y = 2;

	public static float PLAYER_SIZE_X = 0.5f;
	public static float PLAYER_SIZE_Y = 0.5f;

	public static float PLAYER_DAMAGE = 1f;
	public static float PLAYER_DAMAGE_MULTIPLIER = 1f;

	//ROOMS
	public static float WALL_SIZE = 0.5f;

	public static float ROOM_MIN_WIDTH = 3f;
	public static float ROOM_MIN_HEIGHT = 2f;
	
	public static float ROOM_MAX_WIDTH = 6f;
	public static float ROOM_MAX_HEIGHT = 4f;

	//ENEMIS
	public static float ENEMY_SIZE_X = 0.4f;
	public static float ENEMY_SIZE_Y = 0.4f;

	public static float ENEMY_HEALTH = 1f;
	public static float ENEMY_HEALTH_INCREMENT = 1f;

}
