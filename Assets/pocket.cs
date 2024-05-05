using System.Collections;
using UnityEngine;

public class pocket : MonoBehaviour
{
    Animator anim;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag != "pocket") return;

        switch (this.gameObject.tag)
        {
            case "striker":
                rb.velocity = Vector2.zero;
                break;
            case "black":
            case "red":
            case "white":
                // TODO: Give one Bonus Turn

                switch (this.gameObject.tag)
                {
                    case "black":
                        GameManager.Instance.AddCoinToCurrentPlayer(CoinType.Black);
                        break;
                    case "red":
                        GameManager.Instance.AddCoinToCurrentPlayer(CoinType.Red);
                        break;
                    case "white":
                        GameManager.Instance.AddCoinToCurrentPlayer(CoinType.White);
                        break;
                }

                GameManager.Instance.UpdateScoreUI();

                rb.velocity = Vector2.zero;
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