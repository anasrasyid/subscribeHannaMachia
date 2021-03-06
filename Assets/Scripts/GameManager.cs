﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] public CharacterSkin[] playerSkins;
    [SerializeField] public float BombExplode = 3;
    [SerializeField] public float delayTouch = 0.5f;

    [SerializeField] public GameObject panelGameOver;

    public static GameManager Manager { get; private set; }

    private void Awake()
    {
        if (Manager == null)
            Manager = this;
        else
            Destroy(gameObject);
    }

    public void GameOver()
    {
        panelGameOver.SetActive(true);
    }
}
