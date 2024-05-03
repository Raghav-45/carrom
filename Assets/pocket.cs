using System.Collections;
using UnityEngine;

public class pocket : MonoBehaviour
{
    GameObject gameManager;
    Animator anim;
    Rigidbody2D rb;

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

    GameObject GetCurrentPlayingCharacter()
    {
        return gameManager.GetComponent<Game_Manager>().striker;
    }

    striker GetStriker()
    {
        return GetCurrentPlayingCharacter().GetComponent<striker>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // // Object is Striker
        // if (other.gameObject.tag == "pocket" && this.gameObject.tag == "striker")
        // {
        //     rb.velocity = Vector2.zero;
        //     StartCoroutine("fall");
        //     if (this.gameObject.GetComponent<striker>().coveringTheQueen == true)
        //     {
        //         //TODO: return the queen
        //         this.gameObject.GetComponent<striker>().coveringTheQueen = false;
        //     }
        // }

        // // Object is Coin
        // if (other.gameObject.tag == "pocket" && (this.gameObject.tag == "black" || this.gameObject.tag == "red" || this.gameObject.tag == "white"))
        // {
        //     // Give one Bonus Turn
        //     gameManager.GetComponent<Game_Manager>().giveTurn();

        //     if (this.gameObject.tag == "black")
        //     {
        //         GetCurrentPlayingCharacter().GetComponent<striker>().black++;
        //         GetCurrentPlayingCharacter().GetComponent<striker>().UpdateScore();
        //         if (GetCurrentPlayingCharacter().GetComponent<striker>().coveringTheQueen == true)
        //         {
        //             //TODO: also can add red++ instead of setting it to 1 ( for freeplay mode )
        //             GetCurrentPlayingCharacter().GetComponent<striker>().red = 1;
        //             GetCurrentPlayingCharacter().GetComponent<striker>().coveringTheQueen = false;
        //         }
        //     }
        //     if (this.gameObject.tag == "red")
        //     {
        //         // striker.GetComponent<striker>().red++;
        //         GetCurrentPlayingCharacter().GetComponent<striker>().coveringTheQueen = true;
        //     }
        //     if (this.gameObject.tag == "white")
        //     {
        //         GetCurrentPlayingCharacter().GetComponent<striker>().white++;
        //         GetCurrentPlayingCharacter().GetComponent<striker>().UpdateScore();
        //         if (GetCurrentPlayingCharacter().GetComponent<striker>().coveringTheQueen == true)
        //         {
        //             //TODO: also can add red++ instead of setting it to 1 ( for freeplay mode )
        //             GetCurrentPlayingCharacter().GetComponent<striker>().red = 1;
        //             GetCurrentPlayingCharacter().GetComponent<striker>().coveringTheQueen = false;
        //         }
        //     }

        //     rb.velocity = Vector2.zero;
        //     anim.SetTrigger("fall");
        //     Destroy(this.gameObject);
        // }

        if (other.gameObject.tag != "pocket") return;

        switch (this.gameObject.tag)
        {
            case "striker":
                rb.velocity = Vector2.zero;
                // StartCoroutine("fall");
                if (this.gameObject.GetComponent<striker>().coveringTheQueen == true)
                {
                    //TODO: return the queen
                    this.gameObject.GetComponent<striker>().coveringTheQueen = false;
                }
                break;
            case "black":
            case "white":
                // Give one Bonus Turn
                gameManager.GetComponent<Game_Manager>().giveTurn();

                if (this.gameObject.tag == "black")
                {
                    GetStriker().black++;
                    gameManager.GetComponent<Game_Manager>().deltaCoins++;
                }
                else if (this.gameObject.tag == "white")
                {
                    GetStriker().white++;
                    gameManager.GetComponent<Game_Manager>().deltaCoins++;
                }

                GetStriker().UpdateScore();

                if (GetStriker().coveringTheQueen)
                {
                    //TODO: also can add red++ instead of setting it to 1 ( for freeplay mode )
                    GetStriker().red = 1;
                    GetStriker().coveringTheQueen = false;
                }

                rb.velocity = Vector2.zero;
                // anim.SetTrigger("fall");
                Destroy(this.gameObject);

                break;
            case "red":
                // Give one Bonus Turn
                gameManager.GetComponent<Game_Manager>().giveTurn();
                if (this.gameObject.tag == "red")
                {
                    GetStriker().red++;
                    gameManager.GetComponent<Game_Manager>().deltaCoins++;
                }

                GetStriker().coveringTheQueen = true;

                rb.velocity = Vector2.zero;
                // anim.SetTrigger("fall");
                Destroy(this.gameObject);

                break;
            default:
                break;
        }
    }

    IEnumerator fall()
    {
        anim.SetTrigger("fall");
        yield return new WaitForSeconds(1f);
        // this.GetComponent<striker>().MoveStriker(false); // Dont Give Bonus Chance
    }
}