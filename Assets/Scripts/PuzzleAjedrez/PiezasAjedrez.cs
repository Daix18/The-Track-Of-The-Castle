using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ChessPieceType
{
    None = 0,
    Peon = 1,
    Torre = 2,
    Caballo = 3,
    Alfil = 4,
    Reina = 5,
    Rey = 6,
}

public class PiezasAjedrez : MonoBehaviour
{
    public int team;
    public int currentX;
    public int currentY;
    public ChessPieceType type;

    private Vector3 desiredPosition;
    private Vector3 desiredScale = Vector3.one;

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * 10);
        transform.localScale = Vector3.Lerp(transform.localScale, desiredScale, Time.deltaTime * 10);
    }

    public virtual void SetPosition(Vector3 position, bool force = false)
    {
        desiredPosition = position;
        if(force)
            transform.position = desiredPosition;

    } 
    public virtual void SetScale(Vector3 scale, bool force = false)
    {
        desiredScale = scale;
        if (force)
            transform.localScale = desiredScale;
    }
    public virtual bool IsValidMove(int x, int y)
    {
       // Comprueba si la posición a la que se quiere mover está dentro del tablero
    if (x < 0 || x > 7 || y < 0 || y > 7)
    {
        return false;
    }

    // Si el peón está en su posición inicial, puede moverse dos casillas hacia adelante
    if (currentY == 1 && y == 3 && currentX == x)
    {
        return true;
    }

    // El peón solo puede moverse hacia adelante
    if (y <= currentY)
    {
        return false;
    }

    // El peón puede moverse una casilla hacia adelante
    if (y == currentY + 1 && Tablero.THIS.chessPieces[x, y] == null)
    {
        return true;
    }

    // El peón puede moverse diagonalmente hacia adelante si hay una pieza enemiga allí
    if (y == currentY + 1 && Mathf.Abs(x - currentX) == 1 && Tablero.THIS.chessPieces[x, y] != null && Tablero.THIS.chessPieces[x, y].team != team)
    {
        return true;
    }

    return false;
    }
}

