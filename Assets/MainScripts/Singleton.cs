using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton : MonoBehaviour {

    public static Singleton singletonInstance;

    public bool pause = false;

    private void Awake()
    {
        //Aca se crea el Singleton en caso de que no exista
        if (singletonInstance == null)
        {
            DontDestroyOnLoad(gameObject);
            Debug.Log("Creando nueva instancia de Singleton.");
            singletonInstance = this;
        }
    }


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape)){
            tooglePause();     
        }
	}

    private void tooglePause(){
        if(Time.timeScale == 1f){
            Debug.Log("Juego pausado.");
            Time.timeScale = 0f;
            pause = true;
        }
        else if(Time.timeScale == 0f){
            Debug.Log("Juego despausado.");
            Time.timeScale = 1f;
            pause = false;
        }
    }

}
