using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Word : MonoBehaviour {


/////////////////////////////////////
//			 VARIABLES			   //
/////////////////////////////////////

	//prefab
	public GameObject letterPrefab;
	
	//ref
	public Player player;

	//word handling
	private Letter[] letters = new Letter[Settings.LONGEST_WORD];
	private string currentWord = "";
	private string nextWord;
	private int nextWordIndex = 0;
	private string nextChar;
	private bool elementTyped = false;
	private bool wrongLastTime = false;

	private Color[] colors = new Color[4];//{Normal,Current,Wrong,Correct};

	int worldLength;

/////////////////////////////////////
//			  BUILT-IN			   //
/////////////////////////////////////

	void Awake(){
		colors[0] = new Color(95/255f,95/255f,95/255f);
		colors[1] = new Color(200/255f,200/255f,20/255f);
		colors[2] = new Color(172/255f,34/255f,34/255f);
		colors[3] = new Color(34/255f,172/255f,75/255f);
	}


/////////////////////////////////////
//			  PUBLIC			   //
/////////////////////////////////////

	public void clearWord(){
		currentWord = "";
	}

	public void setNewWord(){
		clearWord();
		findNextWord();
	}

	public void restartWord(){
		clearWord();
		nextWordIndex = 0;
		updateLetters();
		findNextChar();
	}

	public void removeWord(){
		clearWord();
		for(int i = 0;i<letters.Length;i++)
			letters[i].gameObject.SetActive(false);
	}

	public void input(string inp){
		if(inp != nextChar){
			if(wrongLastTime){
				restartWord();
				StartCoroutine(setWordToWrongFor(.3f));
				return;
			}
			StartCoroutine(setLetterToWrongFor(nextWordIndex-1,.3f));
			wrongLastTime = true;
		}else{
			wrongLastTime = false;
			letters[nextWordIndex-1].image.color = colors[3];
			currentWord += inp;

			if(currentWord == nextWord){
				if(elementTyped){
					elementTyped = false;
					worldLength += nextWord.Length;

					player.Attack(worldLength);
					findNextWord();
				}else{
					elementTyped = true;
					worldLength = nextWord.Length;
					findNextWord();
				}
				currentWord = "";
			}else{
				findNextChar();
			}

		}
	}


/////////////////////////////////////
//			  PRIVATE			   //
/////////////////////////////////////

	private void createLetters(){
		for(int i = 0;i<letters.Length;i++){
			
				GameObject g = Instantiate(letterPrefab, Vector2.zero, Quaternion.identity) as GameObject;
				g.transform.parent = transform;
				RectTransform r = g.transform as RectTransform;
				r.localPosition = new Vector2(50*i,0);
				Letter l = g.GetComponent<Letter>();
				letters[i] = l;

			if(i<nextWord.Length){
				l.text.text = nextWord.Substring(i,1).ToUpper();
				l.image.color = colors[0];
			}else{
				letters[i].gameObject.SetActive(false);
			}
		}
	}

	private void updateLetters(){
		for(int i = 0;i<letters.Length;i++){
			if(i<nextWord.Length){
				letters[i].gameObject.SetActive(true);
				letters[i].text.text = nextWord.Substring(i,1).ToUpper();
				letters[i].image.color = colors[0];
			}else{
				letters[i].gameObject.SetActive(false);
			}
		}
	}


	private void findNextWord(){
		nextWord = (elementTyped) ? Settings.ATTACKS[Random.Range(0,Settings.ATTACKS.Length)]:Settings.ELEMENTS[Random.Range(0,Settings.ELEMENTS.Length)];
		nextWordIndex = 0;
		if(letters[0] == null)
			createLetters();
		else
			updateLetters();
		findNextChar();
		transform.position = new Vector2(Screen.width/2,35f);
	}

	private void findNextChar(){
		nextChar = nextWord.Substring(nextWordIndex,1);
		transform.position -= new Vector3(50,0,0);
		letters[nextWordIndex].image.color = colors[1];
		nextWordIndex++;
	}


/////////////////////////////////////
//			  CORUOTINES		   //
/////////////////////////////////////

	IEnumerator setLetterToWrongFor(int index, float s){
		letters[index].image.color = colors[2];
		yield return new WaitForSeconds(s);
		letters[index].image.color = colors[1];
	}

	IEnumerator setWordToWrongFor(float s){
		for(int i = 0;i<nextWord.Length;i++)
			letters[i].image.color = colors[2];
		yield return new WaitForSeconds(s);
		for(int i = 0;i<nextWord.Length;i++)
			letters[i].image.color = colors[0];


		letters[0].image.color = colors[1];
	}
}


	