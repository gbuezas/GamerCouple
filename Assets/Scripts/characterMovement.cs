using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterMovement : MonoBehaviour {


    private Rigidbody2D characterRigidbody;
    public float speed;

	// Use this for initialization
	void Start () {
        characterRigidbody = GetComponent<Rigidbody2D>();
        
    }


    // Update is called once per frame
    void Update() {
        
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");
        Vector2 movement = new Vector2(moveHorizontal, moveVertical);
        
        if (Input.GetKey(KeyCode.A))
        {
            Debug.Log("Apretando un boton " + KeyCode.A);
            //speed = 10;
            characterRigidbody.AddForce(movement * speed);
        }
  
        if (Input.GetKey(KeyCode.D))
        {
            Debug.Log("Apretando boton = " + KeyCode.D);
           // speed = 10;
            characterRigidbody.AddForce(movement * speed);
        }

     

    }
}
