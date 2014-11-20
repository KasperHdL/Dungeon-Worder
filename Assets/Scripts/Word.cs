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
	private Letter[] attLetters = new Letter[Settings.LONGEST_ATTACK_WORD];
	private Letter[] eleLetters = new Letter[Settings.LONGEST_ELEMENT_WORD];

	private string elementWord;
	private string attackWord;

	private string currentWord = "";
	private string nextWord;
	private int wordIndex = 0;
	private string nextChar;
	private bool elementTyped = false;
	private bool wrongLastTime = false;

	private Color[] colors = new Color[5];//{Normal,Current,Wrong,Correct,Previous};

/////////////////////////////////////
//			  BUILT-IN			   //
/////////////////////////////////////

	void Awake(){
		colors[0] = new Color(95/255f,95/255f,95/255f);
		colors[1] = new Color(200/255f,200/255f,20/255f);
		colors[2] = new Color(172/255f,34/255f,34/255f);
		colors[3] = new Color(34/255f,172/255f,75/255f);
		colors[4] = new Color(40/255f,40/255f,40/255f);
	}


/////////////////////////////////////
//			  PUBLIC			   //
/////////////////////////////////////

	///<summary>clears the current word </summary>
	public void clearWord(){
		currentWord = "";
	}

	///<summary>sets new word</summary>
	public void setNewWord(){
		clearWord();
		elementTyped = false;
		findNextWord();
	}

	///<summary>restart the word</summary>
	public void restartWord(){
		clearWord();
		wordIndex = 0;
		transform.position = new Vector2(Screen.width/2,35f);
		updateLetters();
		findNextChar();
	}

	///<summary>remvoes the currennt word</summary>
	public void removeWord(){
		clearWord();
		for(int i = 0;i<eleLetters.Length;i++)
			eleLetters[i].gameObject.SetActive(false);
		for(int i = 0;i<attLetters.Length;i++)
			attLetters[i].gameObject.SetActive(false);
	}

	///<summary>give input to the word</summary>
	public void input(string inp){
		if(inp != nextChar){
			if(wrongLastTime){
				restartWord();
				StartCoroutine(setWordToWrongForSecondsThenNormal(elementTyped,.3f));
				return;
			}
			StartCoroutine(setLetterToWrongForSecondsThenNormal(wordIndex-1,elementTyped,.3f));
			wrongLastTime = true;
		}else{
			wrongLastTime = false;

			StartCoroutine(setLetterToCorrectForSecondsThenPrevious(wordIndex-1,elementTyped,.5f));

			currentWord += inp;

			if(currentWord == (elementTyped ? attackWord : elementWord)){
				if(elementTyped){
					elementTyped = false;

					player.Attack(attackWord.Length + elementWord.Length);
					findNextWord();
				}else{
					elementTyped = true;
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
		for(int i = 0;i<eleLetters.Length;i++){
				GameObject g = Instantiate(letterPrefab, Vector2.zero, Quaternion.identity) as GameObject;
				g.transform.parent = transform;
				RectTransform r = g.transform as RectTransform;
				r.localPosition = new Vector2(50*i,0);
				Letter l = g.GetComponent<Letter>();
				eleLetters[i] = l;

			if(i<elementWord.Length){
				l.text.text = elementWord.Substring(i,1).ToUpper();
				l.image.color = colors[0];
			}else{
				eleLetters[i].gameObject.SetActive(false);
			}
		}
		for(int i = 0;i<attLetters.Length;i++){
				GameObject g = Instantiate(letterPrefab, Vector2.zero, Quaternion.identity) as GameObject;
				g.transform.parent = transform;
				RectTransform r = g.transform as RectTransform;
				r.localPosition = new Vector2(50*i,-50f);
				Letter l = g.GetComponent<Letter>();
				attLetters[i] = l;

			if(i<attackWord.Length){
				l.text.text = attackWord.Substring(i,1).ToUpper();
				l.image.color = colors[0];
			}else{
				attLetters[i].gameObject.SetActive(false);
			}
		}
	}
// TODO
	private void updateLetters(){
		for(int i = 0;i<eleLetters.Length;i++){
			if(i < elementWord.Length){
				eleLetters[i].gameObject.SetActive(true);
				eleLetters[i].text.text = elementWord.Substring(i,1).ToUpper();
				eleLetters[i].image.color = colors[0];
			}else{
				eleLetters[i].gameObject.SetActive(false);
			}
		}
		for(int i = 0;i<attLetters.Length;i++){
			if(i < attackWord.Length){
				attLetters[i].gameObject.SetActive(true);
				attLetters[i].text.text = attackWord.Substring(i,1).ToUpper();
				attLetters[i].image.color = colors[0];
			}else{
				attLetters[i].gameObject.SetActive(false);
			}
		}
	}


	private void findNextWord(){
		elementWord = Settings.ELEMENTS[Random.Range(0,Settings.ELEMENTS.Length)];
		attackWord = Settings.ATTACKS[Random.Range(0,Settings.ATTACKS.Length)];

		wordIndex = 0;

		if(eleLetters[0] == null)
			createLetters();
		else
			updateLetters();
		findNextChar();
		if(elementTyped){
			transform.position = new Vector2(Screen.width/2,125f);
		}else{
			transform.position = new Vector2(Screen.width/2,85f);
		}
	}

	private void findNextChar(){
		if(elementTyped)
			nextChar = attackWord.Substring(wordIndex,1);
		else
			nextChar = elementWord.Substring(wordIndex,1);

		transform.position -= new Vector3(50,0,0);
		if(elementTyped){
			attLetters[wordIndex].image.color = colors[1];
		}else{
			eleLetters[wordIndex].image.color = colors[1];
		}
		wordIndex++;
	}


/////////////////////////////////////
//			  CORUOTINES		   //
/////////////////////////////////////

	IEnumerator setLetterToCorrectForSecondsThenPrevious(int index,bool isAttack, float s){
		if(isAttack)
			attLetters[index].image.color = colors[3];
		else
			eleLetters[index].image.color = colors[3];

		yield return new WaitForSeconds(s);

		if(isAttack)
			attLetters[index].image.color = colors[4];
		else
			eleLetters[index].image.color = colors[4];

	}

	IEnumerator setLetterToWrongForSecondsThenNormal(int index,bool isAttack, float s){
		if(isAttack)
			attLetters[index].image.color = colors[2];
		else
			eleLetters[index].image.color = colors[2];

		yield return new WaitForSeconds(s);
		if(isAttack)
			attLetters[index].image.color = colors[0];
		else
			eleLetters[index].image.color = colors[0];

	}

	IEnumerator setWordToWrongForSecondsThenNormal(bool isAttack,float s){
		for(int i = 0;i<(isAttack ? attLetters.Length: eleLetters.Length);i++)
			if(isAttack)
				attLetters[i].image.color = colors[2];
			else
				eleLetters[i].image.color = colors[2];
		yield return new WaitForSeconds(s);

		for(int i = 0;i<(isAttack ? attLetters.Length: eleLetters.Length);i++)
			if(isAttack)
				attLetters[i].image.color = colors[0];
			else
				eleLetters[i].image.color = colors[0];
		if(isAttack)
			attLetters[0].image.color = colors[1];
		else
			eleLetters[0].image.color = colors[1];

	}
}