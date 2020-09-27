using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeckTextUI : MonoBehaviour
{
    public Deck deck;
    public DeckManager DeckManager;
    private Transform cam;
    private TextMeshProUGUI text;

    private void Awake()
    {
        DeckManager = FindObjectOfType<DeckManager>();
        text = GetComponent<TextMeshProUGUI>();
        if (Camera.main != null) cam = Camera.main.transform;
        transform.LookAt(transform.position+ cam.forward);
        transform.parent.localPosition+= Vector3.up*(DeckManager.deckSize+1);
    }

    private void Update()
    {
        text.text = deck.steps.ToString();
    }
}
