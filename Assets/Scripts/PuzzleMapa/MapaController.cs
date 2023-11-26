using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MapaController : MonoBehaviour
{
    public static MapaController THIS;

    public List<PiezasMapa> listaDePiezas;

    public bool insideMapa = false;
    public bool controlPuzzle;
    public int mapSizeX;
    public int mapSizeY;
    public GameObject[,] tiles;
    public GameObject tilePrefab;

    public Image firstKey;

    private void Awake()
    {
        THIS = this;

        //GenerateAllTiles(tileSize, TileCountX, TileCountY);
    }

    private void Update()
    {
        CheckPuzzleStatus();
    }

    private void Start()
    {
        GenerateMap();
    }

    private void GenerateMap()
    {
        tiles = new GameObject[mapSizeX, mapSizeY];

        for (int x = 0; x < mapSizeX; x++)
        {
            for (int y = 0; y < mapSizeY; y++)
            {
                GameObject tile = Instantiate(tilePrefab, transform);
                tile.transform.position = new Vector3(x, 0, y);
                tiles[x, y] = tile;
            }
        }
    }

    // Método para verificar si todas las piezas están correctamente colocadas
    private bool CheckAllPiecesCorrectlyPlaced()
    {
        foreach (PiezasMapa pieza in listaDePiezas)
        {
            if (!pieza.isCorrectlyPlaced)
            {
                return false; // Si alguna pieza no está correctamente colocada, retorna false
            }
        }

        return true; // Todas las piezas están correctamente colocadas
    }

    private void CheckPuzzleStatus()
    {
        if (CheckAllPiecesCorrectlyPlaced())
        {
            firstKey.gameObject.SetActive(true);
            GameController.THIS.firstKeyObtained = true;
            Debug.Log("¡Todas las piezas están correctamente colocadas!");         
        }
        else
        {
            Debug.Log("Aún hay piezas sin colocar correctamente.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        GameController.THIS.insidePuzzle = true;
        insideMapa = true;
    }

    private void OnTriggerExit(Collider other)
    {
        GameController.THIS.insidePuzzle = false;
        insideMapa = false;
    }
}
