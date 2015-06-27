using UnityEngine;
using System.Collections;

public class BackgroundScript : MonoBehaviour {

	public bool fancyBackground = true;
	[SerializeField]protected Material mat1;
	[SerializeField]protected Material mat2;
	[SerializeField]protected Renderer rend;
	[SerializeField]protected GameObject particles;
	[Range(0.0f, 10.0f)][SerializeField]protected float emissionFactor = 1;

	// Use this for initialization
	void Start () {
		rend.material = fancyBackground ? mat1 : mat2;
		ParticleSystem[] systems = particles.GetComponentsInChildren<ParticleSystem>();
		foreach(ParticleSystem system in systems){
			system.emissionRate *= emissionFactor;
			system.maxParticles = (int)(system.maxParticles * emissionFactor);
		}
	}
}
