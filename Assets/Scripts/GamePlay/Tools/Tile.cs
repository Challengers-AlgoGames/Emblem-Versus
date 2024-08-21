using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Tile : MonoBehaviour
{
    [SerializeField] private Color _baseColor, _offsetColor;

    [SerializeField] private SpriteRenderer _renderer;


    [SerializeField] private GameObject _highlight;


    public void Spawn(bool isOffset)
    {
        _renderer.color = isOffset ? _offsetColor : _baseColor;
    }
    void OnMouseEnter()
    {
        _highlight.SetActive(true);
    }
    void OnMouseExit()
    {
        _highlight.SetActive(false);
    }
}
