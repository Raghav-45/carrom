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
    public int red, white, black;
    public GameObject striker;
    // public GameObject game_over;
    // public Text game_over_text;
    // public Text black_no;
    // public Text white_no;


    // Start is called before the first frame update
    void Start()
    {
        // red=1;
        // black_no.text = "1";
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        start = GameObject.Find("start");
        // black_no.text = "0";
        // black_no.text = "0";
        // if (this.gameObject.tag == "black")
        // {
        //     Debug.Log(striker.GetComponent<striker>().get_striker());
        // }
    }

    // Update is called once per frame

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
            striker.GetComponent<striker>().move_striker(true);

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

        // striker.GetComponent<striker>().coveringTheQueen = false;

        // Object is Red Pawn
        // if (other.gameObject.tag == "pocket" && this.gameObject.tag == "red")
        // {
        //     striker.GetComponent<striker>().red++;
        // }

        // if (other.gameObject.tag == "pocket" && (this.gameObject.tag == "black" || this.gameObject.tag == "red"))
        // {
        //     // Give one Bonus Chance
        //     striker.GetComponent<striker>().move_striker(true);

        //     if (this.gameObject.tag == "black")
        //     {
        //         striker.GetComponent<striker>().black++;
        //     }

        //     striker_stat = true;
        //     rb.velocity = Vector2.zero;
        //     anim.SetTrigger("fall");
        //     // Debug.Log("Black: " + get_black() + " White: " + get_white());
        //     Debug.Log("Black Fall by Player");
        //     Destroy(this.gameObject);
        // }

        // if (other.gameObject.tag == "pocket" && (this.gameObject.tag == "white" || this.gameObject.tag == "red"))
        // {
        //     // Give one Bonus Chance
        //     striker.GetComponent<striker>().move_striker(false);

        //     if (this.gameObject.tag == "white")
        //     {
        //         striker.GetComponent<striker>().white++;
        //     }

        //     striker_stat = true;
        //     rb.velocity = Vector2.zero;
        //     anim.SetTrigger("fall");
        //     // Debug.Log("Black: " + get_black() + " White: " + get_white());
        //     Debug.Log("White Fall by Player");
        //     Destroy(this.gameObject);
        // }
    }

    IEnumerator fall()
    {
        anim.SetTrigger("fall");
        yield return new WaitForSeconds(1f);
        // Destroy(this.gameObject);
        this.GetComponent<striker>().return_striker();
        // yield return new WaitForSeconds(.5f);
        // start.GetComponent<start>().init();
    }

    public bool get_st()
    {
        return striker_stat;
    }
    public int get_black()
    {
        return black;
    }
    public int get_white()
    {
        return white;
    }
    public int get_red()
    {
        return red;
    }

    void Update()
    {
        // if (this.gameObject.tag == "striker")
        // {
        //          if(get_black()==1)
        //         {
        //             game_over.SetActive(true);
        //             game_over_text.text = "Player Two Won";

        //         }
        //         else if(get_white()==1){
        // game_over.SetActive(true);
        //             game_over_text.text = "Player One Won";
        //         }
        // Debug.Log("Black: "+get_black()+" White: "+get_white());
        //     if(other.gameObject.tag=="pocket" && (this.gameObject.tag=="red"||this.gameObject.tag=="black"||this.gameObject.tag=="white"))
        // {
        //     rb.velocity = Vector2.zero;
        //     anim.SetTrigger("fall");
        //     Destroy(this.gameObject);
        // }
        // }
        // if(this.gameObject.tag=="striker")
        // {
        // black_no.text = black+"";
        // white_no.text = white+"";
        // }
        // striker_control();
    }
    // void striker_control()
    // {
    // if(get_st()==true)
    // {
    //     striker.GetComponent<striker>().move_striker(true);
    // }
    // else
    // {
    //   striker.GetComponent<striker>().move_striker(false);  
    // }
    // }
    // private void OnTriggerEnter2D(Collider2D other) {
    //     if(other.gameObject.tag=="pocket")
    //     {
    //         Destroy(this.gameObject);
    //     }
    // }
}
