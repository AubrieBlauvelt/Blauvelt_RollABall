﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float speed = 0;
    public TextMeshProUGUI countText;
    public GameObject winTextObject;

    private Rigidbody rb;
    private int count;
    private float movementX;
    private float movementY;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;

        SetCountText();
        winTextObject.SetActive(false);
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();

        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    void SetCountText()
    {
        //reduced amount required to win from 12 to 6 to make it easier for the player
        countText.text = "Count: " + count.ToString();
        if (count >= 6)
        {
            winTextObject.SetActive(true);
        }
    }

    void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);

        rb.AddForce(movement * speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            count = count + 1;

            SetCountText();

            // Code is used to grow my player character each time it picks up a cube. 
            float growthAmount = .4f;
            transform.localScale += new Vector3(growthAmount, growthAmount, growthAmount);
        }
        
        //Made a new tag and prefab. "decrease pickup" is what I named both the tag and prefab. 
        if (other.gameObject.CompareTag("Decrease Pickup"))
        {
            //Allows me to decrease the score upon collision with the decrease pickups
            other.gameObject.SetActive(false);
            count = count - 1;

            SetCountText();

            // On collosion with the decrease pickup, the player's size will decrease.  
            float growthAmount = -.4f;
            transform.localScale += new Vector3(growthAmount, growthAmount, growthAmount);
        }
    }

    //To enact a reset on falling off the map
    private void Update()
    {
        if (transform.position.y < -10f)  //sets the fall distance before reset
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }


        //resets game after getting -3 points
        if (count <= -3)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
