using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public bool As;
    private Deck deck;
    public bool moving = false;
    private float speed;
    private DeckManager DeckManager;

    private void Awake()
    {
        DeckManager = FindObjectOfType<DeckManager>();
        deck = GetComponentInParent<Deck>();
        RefreshSpeed();
    }

    public void RefreshSpeed()
    {
        speed = DeckManager.speedMultiplier;
    }

    public void Leave()
    {
        StartCoroutine(LeavePile());
    }
    public void Down()
    {
        StartCoroutine(GoDown());
    }

    public void Enter()
    {
        StartCoroutine(EnterPile());
    }

    public void GoTo(int index)
    {
        StartCoroutine(GoToPosition(index));
    }
    
    IEnumerator LeavePile()
    {
        moving = true;
        while (transform.localPosition.x < 2)
        {
            transform.localPosition += Vector3.right * Time.deltaTime * speed;
            yield return null;
        }
        transform.localPosition = new Vector3(2, transform.localPosition.y,0);
        moving = false;
    }
    
    IEnumerator EnterPile()
    {
        moving = true;
        while (transform.localPosition .x >0)
        {
            transform.localPosition  += Vector3.left * Time.deltaTime * speed;
            yield return null;
        }
        transform.localPosition = new Vector3(0, transform.localPosition.y,0);
        moving = false;
    }
    
    IEnumerator GoDown()
    {
        moving = true;
        float startPos = transform.localPosition.y;
        while (transform.localPosition.y >startPos-1)
        {
            transform.localPosition += Vector3.down * Time.deltaTime * speed;
            yield return null;
        }
        transform.localPosition = new Vector3(0, startPos-1,0);
        moving = false;
    }
    
    IEnumerator GoToPosition(int position)
    {
        moving = true;
        while (transform.localPosition.y < position)
        {
            transform.localPosition += Vector3.up * Time.deltaTime*position * speed;
            yield return null;
        }
        transform.localPosition = new Vector3(2, position,0);
        moving = false;
    }
}
