using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pocket : MonoBehaviour
{
    Animator anim;
    Rigidbody2D rb;
    bool striker_stat = false;
    GameObject start;
    public GameObject striker;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        start = GameObject.Find("start");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.gameObject.tag);
        // Object is Striker
        if (other.gameObject.tag == "pocket" && this.gameObject.tag == "striker")
        {
            rb.velocity = Vector2.zero;
            StartCoroutine("fall");
            if (striker.GetComponent<striker>().coveringTheQueen == true)
            {
                //TODO: return the queen
                striker.GetComponent<striker>().coveringTheQueen = false;
            }
        }

        // Object is Any Pawn
        if (other.gameObject.tag == "pocket" && (this.gameObject.tag == "black" || this.gameObject.tag == "red" || this.gameObject.tag == "white"))
        {
            // Give one Bonus Chance
            striker.GetComponent<striker>().MoveStriker(true);

            if (this.gameObject.tag == "black")
            {
                striker.GetComponent<striker>().black++;
                Debug.Log("Black Fall by Player");
                if (striker.GetComponent<striker>().coveringTheQueen == true)
                {
                    //TODO: also can add red++ instead of setting it to 1 ( for freeplay mode )
                    striker.GetComponent<striker>().red = 1;
                    striker.GetComponent<striker>().coveringTheQueen = false;
                }
            }
            if (this.gameObject.tag == "red")
            {
                // Debug.Log(striker.GetComponent<striker>().red);
                // striker.GetComponent<striker>().red++;
                striker.GetComponent<striker>().coveringTheQueen = true;
                Debug.Log("Red Fall by Player");
            }
            if (this.gameObject.tag == "white")
            {
                // Debug.Log(striker.GetComponent<striker>().red);
                striker.GetComponent<striker>().white++;
                Debug.Log("white Fall by Player");
                if (striker.GetComponent<striker>().coveringTheQueen == true)
                {
                    //TODO: also can add red++ instead of setting it to 1 ( for freeplay mode )
                    striker.GetComponent<striker>().red = 1;
                    striker.GetComponent<striker>().coveringTheQueen = false;
                }
            }

            striker_stat = true;
            rb.velocity = Vector2.zero;
            anim.SetTrigger("fall");
            Destroy(this.gameObject);
        }
    }

    IEnumerator fall()
    {
        anim.SetTrigger("fall");
        yield return new WaitForSeconds(1f);
        this.GetComponent<striker>().ReturnStriker();
    }

    // Update is called once per frame
    // void Update()
    // {

    // }
}