using UnityEngine;
using System.Collections;

public class PlayMusic : MonoBehaviour {

	[SerializeField]protected AudioSource music;
	[SerializeField]protected AudioClip musicIdle;
	[SerializeField]protected AudioClip musicFight;

	protected float regularVolume = 1;
	protected float minimumVolume = 0;
	protected float targetVolume = 0;

	// Use this for initialization
	void Start () {
		regularVolume = music.volume;
		music.volume = 0;
		targetVolume = regularVolume;
		music.Play();
	}
	
	// Update is called once per frame
	void Update () {
		if((0 == GameManager.enemyCount && music.clip == musicFight)
		   || (0 != GameManager.enemyCount && music.clip == musicIdle)){
			targetVolume = minimumVolume;
			if(0 == music.volume){
				music.clip = 0 == GameManager.enemyCount ? musicIdle : musicFight;
				music.Play();
			}
		}
		else{
			targetVolume = regularVolume * (music.clip == musicFight ? 1 : 3); //necessary as combat music is much louder than idle music
		}

		if(Mathf.Abs(targetVolume - music.volume) > 0.05f){
			music.volume = Mathf.Lerp(music.volume, targetVolume, Time.deltaTime);
		}
		else{
			music.volume = targetVolume;
		}
	}

}
