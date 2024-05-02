﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Game_Manager : MonoBehaviour
{
    public byte white = 0;
    public byte black = 0;
    [SerializeField] private Text whiteTextRenderer;
    [SerializeField] private Text blackTextRenderer;

    public Transform[] startPoints;
    public GameObject striker;
    public Transform currentPlayingCharacterResetPos;

    public GameObject[] strikers;
    public GameObject currentPlayingCharacter;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        striker = GameObject.FindGameObjectsWithTag("striker")[0];
        currentPlayingCharacterResetPos = startPoints[0];

        strikers = GameObject.FindGameObjectsWithTag("striker");
        currentPlayingCharacter = strikers[1];
    }

    // Start is called just before any of the Update methods is called the first time
    // private void Start()
    // {
    //     currentPlayingCharacter.GetComponent<striker>().isPlayerTurn = true;
    // }

    // private void Update()
    // {
    //     if (currentPlayingCharacter.GetComponent<striker>().startObserving && currentPlayingCharacter.GetComponent<striker>().GetComponent<Rigidbody2D>().velocity.magnitude <= currentPlayingCharacter.GetComponent<striker>().resetThresholdVelocity)
    //     {
    //         Debug.Log("sd");
    //         currentPlayingCharacter.GetComponent<striker>().startObserving = false;
    //     }
    // }

    // private void Update()
    // {
    //     if (striker.GetComponent<striker>().GetComponent<Rigidbody2D>().velocity.magnitude <= striker.GetComponent<striker>().resetThresholdVelocity)
    //     {
    //         Debug.Log("sd");
    //         // striker.GetComponent<striker>().startObserving = false;
    //         striker.GetComponent<striker>().ResetStrikerPos();
    //     }
    // }

    public void SwitchToNextPlayer()
    {
        // Find the index of the current player
        int currentIndex = System.Array.IndexOf(startPoints, currentPlayingCharacterResetPos);

        // Increment the index to switch to the next player
        currentIndex = (currentIndex + 1) % startPoints.Length;

        // Update currentPlayingCharacter to the next player
        currentPlayingCharacterResetPos = startPoints[currentIndex];
    }

    public void UpdateScoreUI()
    {
        if (whiteTextRenderer != null && blackTextRenderer != null)
        {
            whiteTextRenderer.text = white.ToString();
            blackTextRenderer.text = black.ToString();
        }
    }

    public void restart()
    {
        // Get the current active scene and Reload it
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
}