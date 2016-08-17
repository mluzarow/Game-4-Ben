using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class textPrinter : MonoBehaviour {

	public string textboxContent;
	public GUIStyle textboxStyle;
	public Rect textboxSize;
	public float crawlTime = 0.05f;

	public string dialogue2Get;

	public List<string> textboxStuff;

	private int index = 0;
	private int listTotal;

	private bool beginTextRoutine = false;
	private bool endTextRoutine = false;
	private bool beginTextPrint = false;
	private bool endTextPrint = false;
	
	private bool triggerTextbox = false;
	private bool triggerSkipCrawl = false;

	private textLibrarian librarian = new textLibrarian();

	void Start() {
		textboxSize = new Rect (Screen.width * 0.200f, Screen.height * 0.800f, Screen.width * 0.600f, Screen.height * 0.400f);

		librarian.getDialogue (dialogue2Get);

		listTotal = librarian.requestedText.Capacity - 1;
	}

	void Update() {
		//Player has not triggered the text routine
		if (!beginTextRoutine/*&&allowTextRoutine*/) {
			//Player has input commands to show the textbox
			if (playerInput.input.y == 1 && Input.GetKeyDown (menuMain.keys["ACTION"])) {
				beginTextRoutine = true;
				beginTextPrint = true;

				playerInput.triggerTextboxMovement = true;
				print ("Beginning text routine");
			}
		//Player has triggered the text routine
		} else if (beginTextRoutine) {
			//Coroutine has ended with no more text to show; waiting for player input
			if (endTextRoutine) {
				//Reset all textbox variables
				beginTextRoutine = false;
				endTextRoutine = false;
				beginTextPrint = false;
				endTextPrint = false;

				triggerTextbox = false;
				triggerSkipCrawl = false;
				index = 0;
				playerInput.triggerTextboxMovement = false;

				print ("Text routine has ended");
			//Coroutine has not ended, therefore start it
			} else if (beginTextPrint) {
				//Cannot print text while printing text
				beginTextPrint = false;
				//Read new line
				textboxContent = librarian.requestedText [index];

				//Start coroutine
				StartCoroutine(textCrawler());
				print ("Coroutine has started for text " + (index+1) + " out of " + listTotal);
			//Player pressed Action during text print
			} else if (Input.GetKeyDown (menuMain.keys["ACTION"]) && !endTextRoutine) {
				//The text has not printed to the end
				if (!endTextPrint) {
					//Skip text crawl timer
					triggerSkipCrawl = true;
					print ("Skipping text crawl");
				//Text has printed to the end
				} else if (endTextPrint) {
					//Increment index by one
					index++;

					//These is still something left to print
					if (index <= (listTotal - 1)) {
						endTextPrint = false;
						beginTextPrint = true;
						triggerSkipCrawl = false;
						print ("Additional text found");
					//Nothing left to print
					} else if (index > (listTotal - 1)) {
						endTextRoutine = true;
						print ("No additional text found");
					}
				}
			}
		}
	}

	//Text crawler coroutine to crawl through text of textboxContent
	IEnumerator textCrawler () {
		//Find the length of the texbox text string
		int stringLength = textboxContent.Length;
		//Save the text to a temporary variable
		string originalText = textboxContent;
		//Remove all text from the textbox
		textboxContent = "";

		//Allow textbox to be drawn on OnGUI()
		triggerTextbox = true;

		/*
		//Index of the array of words in string textboxContent
		int index = 0;
		int partsLength;
		string[] parts = textboxContent.Split (' ');
		partsLength = parts.Length;
		//Remove all text from the textbox
		textboxContent = "";

		//Allow textbox to be drawn on OnGUI()
		triggerTextbox = true;

		//Interate through the array of words
		for (int iArray = 0; iArray < partsLength; iArray++) {
			int wordLength = parts[iArray].Length;

		
			for (int iWord = 0; iWord < wordLength; iWord++) {

			}
		}
		*/




		//Interate through the string (originalText)
		for (int i = 0; i < stringLength; i++) {
			//Copy the next letter from the original text to output textbox text
			textboxContent += originalText[i];

			//If triggered, text will have no crawl timer
			if (!triggerSkipCrawl) {
				//Yield coroutine by seconds and continue
				yield return new WaitForSeconds(crawlTime);
			}
		}
		//Finished writing text
		//triggerFinished = true;
		endTextPrint = true;

		yield return 0;
	}

	void OnGUI() {
		if (triggerTextbox) {
			GUI.TextArea(textboxSize, textboxContent, textboxStyle);
		}
	}

	void OnDrawGizmos() {
		Gizmos.color = Color.blue; //Gizmo color
		float size = 0.5f; //Gizmo size
		
		//Draw gizmos for the start and end positions of the door teleports
		Gizmos.DrawLine ((Vector2)transform.position - Vector2.up * size, (Vector2)transform.position + Vector2.up * size);
		Gizmos.DrawLine ((Vector2)transform.position - Vector2.right * size, (Vector2)transform.position + Vector2.right * size);
	}
}
