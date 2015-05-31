using UnityEngine;
using System.Collections;

public class LevelLoaderScript : MonoBehaviour {

	public void loadLevel(int levelNum)
	{
		Application.LoadLevel (levelNum);
	}
	
	public void exitGame()
	{
		Application.Quit ();
	}

}
