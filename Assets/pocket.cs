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
                GameManager.Instance.GetCurrentPlayer().OnFoul();
                Vector2 currentDirection = rb.velocity.normalized;
                rb.velocity = currentDirection.normalized * 0.07f;
                break;
            case "black":
                GameManager.Instance.AddCoinToCurrentPlayer(CoinType.Black);
                rb.velocity = Vector2.zero;
                Destroy(this.gameObject);
                break;
            case "red":
                GameManager.Instance.AddCoinToCurrentPlayer(CoinType.Red);
                rb.velocity = Vector2.zero;
                Destroy(this.gameObject);
                break;
            case "white":
                GameManager.Instance.AddCoinToCurrentPlayer(CoinType.White);
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
    }
}