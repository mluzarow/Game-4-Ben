// textLibrarian.cs
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

///<summary>Uess a text tag to read all dialogue under said tag from dialogue.txt and saves it in requestedText.</summary>
public class textLibrarian {
	/// <summary>Stores raw read info.</summary>
	private Stack<string> reader = new Stack<string>();
	/// <summary>Stores organized read info.</summary>
	private List<string> buffer = new List<string>();
	/// <summary>Stores requested dialogue text.</summary>
	public List<string> requestedText = new List<string>();

	/// <summary>
	/// Gets the dialogue.
	/// </summary>
	/// <param name="dialogue">
	/// The dialogue tag used to find all the requested text.
	/// </param>
	public void getDialogue(string dialogue) {
		//Load text from file into buffer
		loadText ("Dialogue.txt", dialogue);
		int temp = reader.Count;
		
		for (int i = 0; i < temp; i++) {
			buffer.Add (reader.Pop ());
		}
		
		buffer.Reverse ();

		processDialogue ();
	}

	/// <summary>
	/// Adds all requested dialogue from buffer
	/// </summary>
	/// <returns>
	/// Void: Control.
	/// </returns>
	/// <param name="s">
	/// String: Holds a string from buffer as it is iterated through.
	/// </para>
	private void processDialogue() {
		bool found = false;

		foreach (string s in buffer) {
			if (!found) {
				if (s == "INIT") {
					found = true;
				}
			} else {
				if (s != "END DIALOGUE") {
					requestedText.Add (s);
				} else {
					break;
				}
			}
		}

		return;
	}


	/// <summary>
	/// Loads the requested text from dialogue.txt
	/// </summary>
	/// <returns>
	/// Int: Error value.
	/// </returns>
	/// <param name="filename">
	/// The filename of the file holding the text. This is dialogue.txt in 
	/// .../Assets/Files/dialogue.txt
	/// </param>
	/// <param name="dialogue">
	/// The dialogue tag used to find all the requested text.
	/// </param>
	private int loadText(string filename, string dialogue) {
		//Begin load block
		try {
			//Alter string to the entire line in the library
			dialogue = "DIALOGUE " + dialogue;
			string line;

			//Start filestream
			StreamReader f = new StreamReader(Application.dataPath + "/Files/" + filename);
			
			//Begin reading
			using(f) {
				//Find the corresponding dialogue set for the give name
				do {
					line = f.ReadLine();
				} while (line != dialogue);

				//Read diaglogue script text into buffer
				do {
					reader.Push(f.ReadLine());
				} while (reader.Peek() != "END DIALOGUE");
				
				//Close the file stream
				f.Close ();

				//Return OK
				return(0);
			}
		} catch (IOException e) {
			//print(e);
			//Return error
			return (1);
		}
	}
}

