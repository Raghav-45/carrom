﻿using System.Collections;
using UnityEngine;

public class pocket : MonoBehaviour
{
    public GameObject gameManager;
    Animator anim;
    Rigidbody2D rb;
    public GameObject striker;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        gameManager = GameObject.Find("Game Manager");
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    GameObject GetCurrentPlayingCharacter() {
        return gameManager.GetComponent<Game_Manager>().currentPlayingCharacter;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Object is Striker
        if (other.gameObject.tag == "pocket" && this.gameObject.tag == "striker")
        {
            rb.velocity = Vector2.zero;
            StartCoroutine("fall");
            if (this.gameObject.GetComponent<striker>().coveringTheQueen == true)
            {
                //TODO: return the queen
                this.gameObject.GetComponent<striker>().coveringTheQueen = false;
            }
        }

        // Object is Coin
        if (other.gameObject.tag == "pocket" && (this.gameObject.tag == "black" || this.gameObject.tag == "red" || this.gameObject.tag == "white"))
        {
            // Give one Bonus Turn
            // GameManager.getTurnPlayer.Striker...
            // striker.GetComponent<striker>().MoveStriker(true);

            if (this.gameObject.tag == "black")
            {
                striker.GetComponent<striker>().black++;
                striker.GetComponent<striker>().UpdateScore();
                if (striker.GetComponent<striker>().coveringTheQueen == true)
                {
                    //TODO: also can add red++ instead of setting it to 1 ( for freeplay mode )
                    striker.GetComponent<striker>().red = 1;
                    striker.GetComponent<striker>().coveringTheQueen = false;
                }
            }
            if (this.gameObject.tag == "red")
            {
                // striker.GetComponent<striker>().red++;
                striker.GetComponent<striker>().coveringTheQueen = true;
            }
            if (this.gameObject.tag == "white")
            {
                striker.GetComponent<striker>().white++;
                striker.GetComponent<striker>().UpdateScore();
                if (striker.GetComponent<striker>().coveringTheQueen == true)
                {
                    //TODO: also can add red++ instead of setting it to 1 ( for freeplay mode )
                    striker.GetComponent<striker>().red = 1;
                    striker.GetComponent<striker>().coveringTheQueen = false;
                }
            }

            rb.velocity = Vector2.zero;
            anim.SetTrigger("fall");
            Destroy(this.gameObject);
        }
    }

    IEnumerator fall()
    {
        anim.SetTrigger("fall");
        yield return new WaitForSeconds(1f);
        this.GetComponent<striker>().MoveStriker(false);
    }
}