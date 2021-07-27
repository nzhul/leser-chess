﻿using System.Collections.Generic;
using UnityEngine;

public class BoardHighlights : MonoBehaviour
{
    private List<GameObject> _highlights;

    public GameObject highlightPrefab;

    public GameObject hoverPrefab;

    public GameObject hoverGraphic;

    public static BoardHighlights Instance { get; set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        this._highlights = new List<GameObject>();
        InitHoverHighlight();
    }

    public void HighlightAllowedMoves(bool[,] moves)
    {
        for (int i = 0; i < moves.GetLength(0); i++)
        {
            for (int j = 0; j < moves.GetLength(1); j++)
            {
                if (moves[i, j])
                {
                    GameObject go = this.GetHighlightObject();
                    go.SetActive(true);
                    go.transform.position = new Vector3(i + .5f, 0, j + .5f); //TODO: Use TILE_SIZE / 2;
                }
            }
        }
    }

    public void HighlightSelection(int x, int y)
    {
        GameObject go = this.GetHighlightObject();
        go.SetActive(true);
        go.GetComponent<MeshRenderer>().material.color = new Color(0.5764706f, 0.8313726f, 0, 1); // yellow-green
        go.transform.position = new Vector3(x + .5f, 0, y + .5f); //TODO: Use TILE_SIZE / 2;
    }

    public void HideHighlights()
    {
        foreach (GameObject go in this._highlights)
        {
            go.GetComponent<MeshRenderer>().material.color = Color.white;
            go.SetActive(false);
        }
    }

    private GameObject GetHighlightObject()
    {
        GameObject instance = _highlights.Find(x => !x.activeSelf);
        if (instance == null)
        {
            instance = Instantiate(highlightPrefab);
            this._highlights.Add(instance);
        }

        return instance;
    }

    public void UpdateHover(int selectionX, int selectionY)
    {
        hoverGraphic.SetActive(true);
        hoverGraphic.transform.position = new Vector3(selectionX + 0.5f, 0.001f, selectionY + 0.5f);
    }

    public void DisableHover()
    {
        hoverGraphic.SetActive(false);
    }

    private void InitHoverHighlight()
    {
        hoverGraphic = Instantiate(hoverPrefab);
        hoverGraphic.SetActive(false);
    }
}