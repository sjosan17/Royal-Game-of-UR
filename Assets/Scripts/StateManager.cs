using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    public int NumberOfPlayers = 2;
    public int CurrentPlayerID = 0;

    public int DiceTotal;

    public bool IsDoneRolling = false;
    public bool IsDoneClicking = false;
    public bool IsDoneAnimating = false;

    public GameObject NoLegalMovesPopup;

    public void NewTurn()
    {
        // This is the start of a player's turn.
        //We don't have a roll for them yet.
        IsDoneRolling = false;
        IsDoneClicking = false;
        IsDoneAnimating = false;

        CurrentPlayerID = (CurrentPlayerID + 1) % NumberOfPlayers;
    }

    public void RollAgain()
    {
        IsDoneRolling = false;
        IsDoneClicking = false;
        IsDoneAnimating = false;
    }

    // Update is called once per frame
    void Update () {

        // Is the turn done?
        if(IsDoneRolling && IsDoneClicking && IsDoneAnimating)
        {
            Debug.Log("Turn is done!");
            NewTurn();
        }
	}

    public void CheckLegalMoves()
    {
        // If we rolled a zero, then we clearly have no legal moves.
        if(DiceTotal == 0)
        {
            StartCoroutine( NoLegalMoveCoroutine() );
            return;
        }

        // Loop through all of a player's stones
        PlayerStone[] pss = GameObject.FindObjectsOfType<PlayerStone>();
        bool hasLegalMove = false;
        foreach(PlayerStone ps in pss)
        {
            if(ps.PlayerId == CurrentPlayerID)
            {
                if( ps.CanLegallyMoveAhead(DiceTotal))
                {
                    // TODO: Highlight stones that can be legally moved
                    hasLegalMove = true;
                }
            }
        }

        // If no legal moves are possible, wait sec then move to next player with message.
        if(hasLegalMove == false)
        {
            StartCoroutine(NoLegalMoveCoroutine());
            return;
        }
    }

    IEnumerator NoLegalMoveCoroutine()
    {
        // Display message
        NoLegalMovesPopup.SetActive(true);

        // Wait 1 second
        yield return new WaitForSeconds(1f);

        NoLegalMovesPopup.SetActive(false);

        NewTurn();
    }
}
