using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class Tablero : MonoBehaviour
{
    public static Tablero THIS;

    [Header("Art Stuff")]

    //Inidca si esta dentro del puzzle de Ajedrez
    [SerializeField] public bool insideChessPuzzle = false;
    // tileMateral es el material que se usará para dibujar las casillas del tablero.
    [SerializeField] private Material tileMateral;
    /// <summary>
    /// tileSize es el tamño de las casillas
    /// </summary>
    [SerializeField] private float tileSize = 1.0f;
    /// <summary>
    /// y0ffset es la altura que ajustamos en el eje Y a las casillas
    /// </summary>
    [SerializeField] private float yOffset = 0.2f;
    /// <summary>
    /// boardCenter es donde ajustamos el centro del tablero al modelo del tablero
    /// </summary>
    [SerializeField] private Vector3 boardCenter = Vector3.zero;
    /// <summary>
    /// Offset a cuanto de alto esta la pieza
    /// </summary>
    [SerializeField] private float dragOffset = 1.5f;
    /// <summary>
    /// Camara del puzzle
    /// </summary>
    [SerializeField] private Camera chessCamera;

    [Header("Prefabs & Materials")]
    [SerializeField] private GameObject[] prefabs;
    [SerializeField] private Material[] teamMaterials;

    /// <summary>
    /// Variable que contiene las piezas
    /// </summary>
    public PiezasAjedrez[,] chessPieces;

    /// <summary>
    /// Arrastrando actualemnte
    /// </summary>
    private PiezasAjedrez currentlyDragging;

    /// <summary>
    /// Dimensión del tableo en X
    /// </summary>
    private const int TileCountX = 8;

    /// <summary>
    /// Dimensión del tablero en Y
    /// </summary>
    private const int TileCountY = 8;

    /// <summary>
    /// Almacena los GameObjects que representan cada una de las casillas del tablero.
    /// </summary>
    private GameObject[,] tiles;

    /// <summary>
    /// Referencia a la cámara principal en la escena.
    /// </summary>
    private Camera currentCamera;

    /// <summary>
    /// Casilla del tablero sobre la que el mouse está posicionado.
    /// </summary>
    private Vector2Int currentHover;

    /// <summary>
    /// Declaración de los límites
    /// </summary>
    private Vector3 bounds;

    /// <summary>
    /// Referencia al script del player
    /// </summary>
    public FirstPersonController player;


    private void Awake()
    {
        THIS = this;

        //Genera todas las casillas del tablero.
        GenerateAllTiles(tileSize, TileCountX, TileCountY);

        //Spawnea las piezas
        SpawnAllPieces(); 

        //Posiciona las piezas
        PositionAllPieces();

        // Encuentra el objeto con el script FirstPersonController
        player = FindObjectOfType<FirstPersonController>();
    }
    private void Update()
    {
        if (!currentCamera)
        {
            //Si currentCamera no está asignada, se asigna la cámara principal de la escena.
            currentCamera = chessCamera;
            return;
        }

        //RaycastHit se utiliza para almacenar información sobre el objeto con el que ha colisionado el rayo
        RaycastHit info;

        //Ray se genera desde la posición del ratón en la cámara actual
        Ray ray = currentCamera.ScreenPointToRay(Input.mousePosition);

        //Si hay una colisión entre el rayo y un objeto en la capa "Tile"
        if (Physics.Raycast(ray, out info, 100, LayerMask.GetMask("Tile", "Hover")))
        {
            //Si se detecta una colisión con un objeto en la capa "Tile", buscamos el índice de la tile en la matriz de tiles.
            Vector2Int hitPosition = LookupTileIndex(info.transform.gameObject);

            //Si no hay una tile resaltada actualmente, resaltamos la tile detectada por el raycast.
            if (currentHover == -Vector2Int.one)
            {
                //Establecer la tile actual como la tile resaltada
                currentHover = hitPosition;

                //Cambiar la capa de la tile a "Hover" para resaltarla
                tiles[hitPosition.x, hitPosition.y].layer = LayerMask.NameToLayer("Hover");

            }

            //Si el ratón se ha movido a otra tile, desresaltamos la tile anterior y resaltamos la nueva
            if (currentHover != hitPosition)
            {
                //Cambiar la capa de la tile anteriormente resaltada a "Tile" para desactivar el resaltado
                tiles[currentHover.x, currentHover.y].layer = LayerMask.NameToLayer("Tile");

                //Establecer la nueva tile como la tile resaltada
                currentHover = hitPosition;

                //Cambiar la capa de la nueva tile a "Hover" para resaltarla
                tiles[hitPosition.x, hitPosition.y].layer = LayerMask.NameToLayer("Hover");
            }

            //Si estamos presionando el ratón
            if (Input.GetMouseButtonDown(0))
            {
                if (chessPieces[hitPosition.x, hitPosition.y] != null)
                {
                    //Es nuestro turno?
                    if (true)
                    {
                        currentlyDragging = chessPieces[hitPosition.x, hitPosition.y];
                    }
                }
            }

            //Si estamos soltando el botón del ratón
            if (currentlyDragging != null && Input.GetMouseButtonUp(0))
            {
                Vector2Int previousPosition = new Vector2Int(currentlyDragging.currentX, currentlyDragging.currentY);

                bool validMove = MoveTo(currentlyDragging, hitPosition.x, hitPosition.y);
                if (!validMove)
                {
                    currentlyDragging.SetPosition(GetTileCenter(previousPosition.x, previousPosition.y));
                    currentlyDragging = null;
                }
                else
                {
                    currentlyDragging = null;
                }
            }


        }
        //Si se hace clic en la misma tile, desactiva el resaltado.
        else
        {
            if (currentHover != -Vector2Int.one)
            {
                //Cambiar la capa de la tile a "Tile" para resaltarla
                tiles[currentHover.x, currentHover.y].layer = LayerMask.NameToLayer("Tile");
                currentHover = -Vector2Int.one;
            }

            if (currentlyDragging && Input.GetMouseButtonUp(0))
            {
                currentlyDragging.SetPosition(GetTileCenter(currentlyDragging.currentX, currentlyDragging.currentY));
                currentlyDragging = null;
            }
        }

        if (currentlyDragging)
        {
            Plane horizontalPlane = new Plane(Vector3.up, Vector3.up * yOffset);
            float distance = 0.0f;
            if (horizontalPlane.Raycast(ray, out distance))
                currentlyDragging.SetPosition(ray.GetPoint(distance) + Vector3.up * dragOffset);
        }
    }

    /// <summary>
    /// Genera las celdas
    /// </summary>
    /// <param name="tileSize"></param>
    /// <param name="tileCountX"></param>
    /// <param name="tileCountY"></param>
    private void GenerateAllTiles(float tileSize, int tileCountX, int tileCountY)
    {

        yOffset += transform.position.y;

        bounds = new Vector3((tileCountX / 2) * tileSize, 0, (tileCountX / 2) * tileSize) + boardCenter;

        tiles = new GameObject[tileCountX,tileCountY];

        for (int x = 0; x < tileCountX; x++)
        {
            for (int y = 0; y < tileCountY; y++)
            {
                tiles[x, y] = GenerateSingleTile(tileSize, x, y);
            }
        }
    }
    private GameObject GenerateSingleTile (float tileSize, int x, int y)
    {
        GameObject tileObject = new GameObject(string.Format("X:{0}. Y:{1}", x, y ));
        tileObject.transform.parent = transform;

        Mesh mesh = new Mesh();
        tileObject.AddComponent<MeshFilter>().mesh = mesh;
        tileObject.AddComponent<MeshRenderer>().material = tileMateral;

        Vector3[] vertices = new Vector3[4];
        vertices[0] = new Vector3(x * tileSize,yOffset, y * tileSize) - bounds;
        vertices[1] = new Vector3(x * tileSize, yOffset, (y+1) * tileSize) - bounds;
        vertices[2] = new Vector3((x+1) * tileSize, yOffset, y * tileSize) - bounds;
        vertices[3] = new Vector3((x + 1) * tileSize, yOffset, (y+1) * tileSize) - bounds;

        int[] tris = new int[] {0,1,2,1,3,2};

        mesh.vertices = vertices;
        mesh.triangles = tris;
        mesh.RecalculateNormals();

        tileObject.layer = LayerMask.NameToLayer("Tile");
        tileObject.AddComponent<BoxCollider>();

        return tileObject;
    }

    /// <summary>
    /// Spawnear piezas
    /// </summary>
    private void SpawnAllPieces()
    {
        ////Inicializamos la variable de chessPieces
        chessPieces = new PiezasAjedrez[TileCountX, TileCountY];

        int whiteTeam = 0, blackTeam = 1;

        //Equipo Blanco
        chessPieces[0, 0] = SpawnSinglePiece(ChessPieceType.Torre, whiteTeam);
        chessPieces[1, 0] = SpawnSinglePiece(ChessPieceType.Caballo, whiteTeam);
        chessPieces[2, 0] = SpawnSinglePiece(ChessPieceType.Alfil, whiteTeam);
        chessPieces[3, 0] = SpawnSinglePiece(ChessPieceType.Rey, whiteTeam);
        chessPieces[4, 0] = SpawnSinglePiece(ChessPieceType.Reina, whiteTeam);
        chessPieces[5, 0] = SpawnSinglePiece(ChessPieceType.Alfil, whiteTeam);
        chessPieces[6, 0] = SpawnSinglePiece(ChessPieceType.Caballo, whiteTeam);
        chessPieces[7, 0] = SpawnSinglePiece(ChessPieceType.Torre, whiteTeam);
        for (int i = 0; i < TileCountX; i++)
        {
            chessPieces[i, 1] = SpawnSinglePiece(ChessPieceType.Peon, whiteTeam);
        }

        //Equipo negro
        chessPieces[0, 7] = SpawnSinglePiece(ChessPieceType.Torre, blackTeam);
        chessPieces[1, 7] = SpawnSinglePiece(ChessPieceType.Caballo, blackTeam);
        chessPieces[2, 7] = SpawnSinglePiece(ChessPieceType.Alfil, blackTeam);
        chessPieces[3, 7] = SpawnSinglePiece(ChessPieceType.Rey, blackTeam);
        chessPieces[4, 7] = SpawnSinglePiece(ChessPieceType.Reina, blackTeam);
        chessPieces[5, 7] = SpawnSinglePiece(ChessPieceType.Alfil, blackTeam);
        chessPieces[6, 7] = SpawnSinglePiece(ChessPieceType.Caballo, blackTeam);
        chessPieces[7, 7] = SpawnSinglePiece(ChessPieceType.Torre, blackTeam);
        for (int i = 0; i < TileCountX; i++)
        {
            chessPieces[i, 6] = SpawnSinglePiece(ChessPieceType.Peon, blackTeam);
        }
    }
    private PiezasAjedrez SpawnSinglePiece(ChessPieceType type, int team)
    {
        PiezasAjedrez cp = Instantiate(prefabs[(int)type - 1], transform).GetComponent<PiezasAjedrez>();

        cp.type = type;        
        cp.team = team;
        cp.GetComponent<MeshRenderer>().material = teamMaterials[team];

        return cp;
    }
    /// <summary>
    /// Posicionar Piezas
    /// </summary>
    private void PositionAllPieces()
    {
        for (int x = 0; x < TileCountX; x++)
            for (int y = 0; y < TileCountY; y++)
                if (chessPieces[x, y] != null)
                {
                    PositionSinglePiece(x, y, true);
                }                 
    }
    private void PositionSinglePiece(int x, int y, bool force = false)
    {
        chessPieces[x, y].currentX = x;
        chessPieces[x, y].currentX = y;
        chessPieces[x, y].SetPosition(GetTileCenter(x, y),force);
    }

    private Vector3 GetTileCenter(int x, int y)
    {
        return new Vector3(x * tileSize, yOffset, y * tileSize) - bounds + new Vector3(tileSize / 2 , 0, tileSize / 2);
    }

    private bool MoveTo(PiezasAjedrez cp, int x, int y)
    {
        Vector2Int previousPosition = new Vector2Int(cp.currentX, cp.currentY);

        // Hay otra pieza en el target position?
        if(chessPieces[x, y] != null)
        {
            PiezasAjedrez ocp = chessPieces[x, y];

            if(cp.team == ocp.team)            
                return false;
            
        }

        chessPieces[x, y] = cp;
        chessPieces[previousPosition.x, previousPosition.y] = null;

        PositionSinglePiece(x, y);

        return true;
    }

    /// <summary>
    /// Operaciones
    /// </summary>
    /// <param name="hitInfo"></param>
    /// <returns></returns>
    private Vector2Int LookupTileIndex(GameObject hitInfo)
    {
        // Recorrer todos los cuadros en la matriz
        for (int x = 0; x <TileCountX ; x++)
        {
            for (int y = 0; y < TileCountY ; y++)
            {
                if (tiles[x,y] == hitInfo)
                {
                    return new Vector2Int(x,y);
                }
            }
        }
        return -Vector2Int.one;
    }

    private void OnTriggerEnter(Collider other)
    {
        insideChessPuzzle = true;
    }
    private void OnTriggerExit(Collider other)
    {
        insideChessPuzzle = false;
    }
}
