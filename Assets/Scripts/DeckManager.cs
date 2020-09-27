using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckManager : MonoBehaviour
{
    public int deckAmount;
    public int deckSize;
    public int spacing;

    public Slider amountSlider;
    public Slider SizeSlider;
    public Text averageText;
    public Text maxText;
    public Text minText;
    public Text deviationText;
    public Text estimateText;

    public int speedMultiplier = 1;

    public GameObject deckPrefab;

    private List<Deck> decks;
    private bool isRunning = false;

    public void RefreshStats()
    {
        averageText.text = "Average Steps: " + CurrentAverage();
        maxText.text = "Max Steps: " + CurrentMax();
        minText.text = "Min Steps: " + CurrentMin();
        deviationText.text = "Standard Deviation: " + CurrentStandartDeviation();
        estimateText.text = "Estimated Average n*H(n-1): " + EstimatedAverage() + " (n = " +deckSize + ")";
    }
    private void Awake()
    {
        decks = new List<Deck>();
        spacing *= (int)Mathf.Sqrt(deckSize)/2;
    }

    public void RefreshAllSpeedComand()
    {
        foreach (Deck deck in decks)
        {
            deck.RefreshAllSpeed();
        }
    }
    private void Start()
    {
        InstantiateDecks();
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.E))
        {
            speedMultiplier = Mathf.Clamp(speedMultiplier*2, 1, 16);
            Debug.Log(speedMultiplier);
            RefreshAllSpeedComand();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            speedMultiplier = Mathf.Clamp(speedMultiplier/2, 1, 16);
            RefreshAllSpeedComand();
        }
        
    }

    public void RunSimulation()
    {
       
        if (isRunning)
        {
            return;
        }
        isRunning = true;
        
        foreach (Deck deck in decks)
        {
            deck.StartAlgorithm();
        }
    }
    

    public void InstantiateDecks()
    {
        
        foreach (Deck deck in decks)
        {
            Destroy(deck.gameObject);
        }
        
        decks = new List<Deck>();

        deckAmount = (int)amountSlider.value;
        amountSlider.GetComponentInChildren<Text>().text = "Number of decks: " + deckAmount;
        
        deckSize = (int)SizeSlider.value;
        SizeSlider.GetComponentInChildren<Text>().text = "Cards on each deck: " + deckSize;
        
        
        
        int x = 0;
        int dx = 0;
        int y = 0;
        int dy = -1;

        int width = (int) Mathf.Sqrt(deckAmount)+2;
        int height = (int) Mathf.Sqrt(deckAmount)+1;
        
        

        for (int i = 0; i<deckAmount; i++)
        {
            if (-width/2 < x && x <= width/2 && -height/2 < y && y <= height/2) {
                //do stuff with x,y
                //Debug.Log("x: " + x + "y: " + y);
                decks.Add(Instantiate(deckPrefab, transform).GetComponent<Deck>());
                decks[i].transform.position = new Vector3(x*spacing,0,y*spacing);
            }

            if (x == y 
                || (x < 0 && x == -y) 
                || (x > 0 && x == 1-y)){
                // change direction
                int aux = dx;
                dx = -dy;
                dy = aux;
                
            }
            x += dx;
            y += dy;        
        }
        

        foreach (Deck deck in decks)
        {
            deck.SetUpDeck(deckSize);
        }
        isRunning = false;
    }

    private float Harmonic(int n)
    {
        float sum = 0f;
        for (float k = 1 ; k <= n ; k+=1)
        {
            sum += 1 / k;
        }
        return sum;
    }
    private float EstimatedAverage()
    {
        return Harmonic(deckSize - 1) * deckSize;
    }
    private float CurrentAverage()
    {
        float sum = 0f;
        foreach (Deck deck in decks)
        {
            sum += deck.steps;
        }
        return sum / decks.Count;
    }

    private int CurrentMin()
    {
        int min = -1;
        foreach (Deck deck in decks)
        {
            if (deck.steps < min || min == -1) min = deck.steps;
        }

        return min;
    }
    
    private int CurrentMax()
    {
        int max = -1;
        foreach (Deck deck in decks)
        {
            if (deck.steps > max || max == -1) max = deck.steps;
        }

        return max;
    }
    private float CurrentVariance()
    {
        float avg = CurrentAverage();
        float sumSquares = 0f;

        foreach (Deck deck in decks)
        {
            sumSquares += Mathf.Pow((deck.steps - avg), 2.0f);
        }

        return sumSquares / decks.Count;
    }

    private float CurrentStandartDeviation()
    {
        return Mathf.Sqrt(CurrentVariance());
    }
    

   
}
