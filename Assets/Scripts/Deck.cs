using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Deck : MonoBehaviour
{
    public int cardsNumber;
    public GameObject asPrefab; 
    public GameObject regularCardPrefab;

    public float scale;
    public int steps;
    private List<Card> cards;

    public void SetUpDeck(int amount)
    {
        cards=new List<Card>();
        cardsNumber = amount;
        GameObject currentGO;
        for (int i = 0; i < amount-1; i++)
        {
            currentGO = Instantiate(regularCardPrefab, transform,false);
            currentGO.transform.position += Vector3.up * i * scale;
            cards.Add(currentGO.GetComponent<Card>());
        }
        currentGO = Instantiate(asPrefab, transform,false);
        currentGO.transform.position += Vector3.up * (amount-1) * scale;
        cards.Add(currentGO.GetComponent<Card>());
    }

    public void RefreshAllSpeed()
    {
        foreach (Card card in cards)
        {
            card.RefreshSpeed();
        }
    }

    private void Awake()
    {
        transform.localScale = new Vector3(scale,scale,scale);
    }

    public void StartAlgorithm()
    {
        StartCoroutine(Step());
    }

    
    private Card GetIndex(int index)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position + Vector3.up * index * scale, 0.001f);
        if (colliders.Length <= 0) return null;
        return colliders[0].GetComponent<Card>();
    }

    private Card GetFirst()
    {
        return GetIndex(0);
    }

    private List<Card> GetThisAndAllBelow(int index)
    {
        List<Card> cardsToMove = new List<Card>();
        for (int i = index; i >= 1; i--)
        {
            cardsToMove.Add(GetIndex(i));
        }
        return cardsToMove;
    }

    IEnumerator Step()
    {
        Card first = GetFirst();
       
        if (first.As)
        {
            StopAllCoroutines();
            yield return null;
        }
        
        

        steps++;
        first.Leave();
        while (first.moving)
        {
            yield return null;
        }
        
        int randomIndex = Random.Range(0, cardsNumber);
        //Debug.Log(randomIndex);
        
        first.GoTo(randomIndex);
        if (randomIndex != 0)
        {
            List<Card> cardsToMove = GetThisAndAllBelow(randomIndex);
            foreach (Card card in cardsToMove)
            {
                card.Down();
            }
        }
        
        
        while (first.moving)
        {
            yield return null;
        }
        first.Enter();
        while (first.moving)
        {
            yield return null;
        }

        StartCoroutine(Step());
    }

   
}
