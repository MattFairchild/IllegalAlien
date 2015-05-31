using UnityEngine;
using System.Collections;

public class LevelLoaderScript : MonoBehaviour {

	public void LoadLevel(int levelNum)
	{
		Application.LoadLevel (levelNum);
	}
	
	public void ExitGame()
	{
		Application.Quit ();
	}

}
