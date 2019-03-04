using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public int m_iRow;
    public int m_iCol;

    public int m_iPrevCol;
    public int m_iPrevRow;

    public Vector2 m_Position;

    private Tile[,] m_Grid;

    public enum BALL_STATE
    {
        BALL_STOPPED,
        BALL_MOVE_LEFT,
        BALL_MOVE_RIGHT,
        BALL_MOVE_DOWN,
        FALL

    };

    public BALL_STATE m_BallState;

    private float m_fBallSpeed;

    public int GetRow()
    {
        return m_iRow;
    }

    public int GetCol()
    {
        return m_iCol;
    }

    public void Initialize(Tile[,] grid, float iBallSpeed)
    {
        m_Grid = grid;
        m_iRow = 18;
        m_iCol = 1;
        m_fBallSpeed = iBallSpeed;

        transform.localScale = new Vector3(0.17f, 0.17f, 0.17f);
        transform.localPosition = m_Grid[m_iRow, m_iCol].m_Position;

        
        m_BallState = BALL_STATE.BALL_MOVE_DOWN;
        m_iPrevCol = m_iCol;
        m_iPrevRow = m_iRow;
        m_Grid[18, 1].m_TileState = Tile.TILE_STATE.OCCUPIED;
    }


    public bool BallDownPriority()
    {
        int iRow = m_iRow - 1;
        if(iRow == 0)
        {
            m_Grid[m_iRow, m_iCol].m_TileState = Tile.TILE_STATE.OUT_TRANSITION;
            m_BallState = BALL_STATE.FALL;
            m_bCanUse = true;
            return true;
        }

        if(m_BallState == BALL_STATE.FALL)
        {
            m_bCanUse = true;
            return true;
        }

        if (iRow < 19 && (m_Grid[iRow, m_iCol].m_TileState == Tile.TILE_STATE.IN_TRANSITION || m_Grid[iRow, m_iCol].m_TileState == Tile.TILE_STATE.OCCUPIED || m_Grid[iRow, m_iCol].m_TileState == Tile.TILE_STATE.OUT_TRANSITION))
        {
            if (m_iCol + 1 > 25) return true;
            if ( m_Grid[iRow, m_iCol + 1].m_TileState == Tile.TILE_STATE.EMPTY || m_Grid[iRow, m_iCol - 1].m_TileState == Tile.TILE_STATE.EMPTY)
            {
                m_BallState = BALL_STATE.BALL_STOPPED;
                m_bCanUse = false;
                return false;
            }

        }
        if (m_Grid[iRow, m_iCol].m_TileState == Tile.TILE_STATE.EMPTY)
        {
            m_BallState = BALL_STATE.BALL_MOVE_DOWN;
            m_Grid[iRow, m_iCol].m_TileState = Tile.TILE_STATE.IN_TRANSITION;
            m_iPrevRow = m_iRow;
            m_iRow = iRow;
            if(m_Grid[m_iPrevRow, m_iCol].m_TileState != Tile.TILE_STATE.BLOCK)
                m_Grid[m_iPrevRow, m_iCol].m_TileState = Tile.TILE_STATE.OUT_TRANSITION;
            m_bCanUse = true;
            return true;
        }
        m_bCanUse = false;
        return false;
    }

    private bool m_bCanUse;

    public bool CanUse()
    {
        return m_bCanUse;
    }


    public bool CanMove()
    {
        //CHECK IF ANYTHING IS ABOVE
        int iRow = m_iRow - 1;
        if (m_iCol + 1 == 26) return false;
        if (iRow < 19 && (m_Grid[iRow, m_iCol].m_TileState == Tile.TILE_STATE.IN_TRANSITION || m_Grid[iRow, m_iCol].m_TileState == Tile.TILE_STATE.OCCUPIED || m_Grid[iRow, m_iCol].m_TileState == Tile.TILE_STATE.OUT_TRANSITION))
        {
            if(m_Grid[iRow,m_iCol+1].m_TileState == Tile.TILE_STATE.EMPTY || m_Grid[iRow,m_iCol+1].m_TileState == Tile.TILE_STATE.IN_TRANSITION || m_Grid[iRow, m_iCol + 1].m_TileState == Tile.TILE_STATE.OUT_TRANSITION)
            {
                m_BallState = BALL_STATE.BALL_STOPPED;
                return false;
            }
            if (m_Grid[iRow, m_iCol - 1].m_TileState == Tile.TILE_STATE.EMPTY || m_Grid[iRow, m_iCol - 1].m_TileState == Tile.TILE_STATE.IN_TRANSITION ||  m_Grid[iRow, m_iCol - 1].m_TileState == Tile.TILE_STATE.OUT_TRANSITION)
            {
                m_BallState = BALL_STATE.BALL_STOPPED;
                return false;
            }

        }
        int iTileState1 = (int)m_Grid[m_iRow, m_iCol + 1].m_TileState;
        int iTileState2 = (int)m_Grid[m_iRow, m_iCol - 1].m_TileState;
        bool bYes1 = true;
        bool bYes2 = true;
        if (iTileState1>0 && iTileState1 <= 4)
        {
            bYes1 = false;
        }

        if (iTileState2 > 0 && iTileState2 <= 4)
        {
            bYes2 = false;
        }
        iTileState1 = (int)m_Grid[m_iRow - 1, m_iCol].m_TileState;

        iTileState1 = (int)m_Grid[m_iRow, m_iCol + 1].m_TileState;
        if(!bYes1 && !bYes2)
        {
            m_BallState = BALL_STATE.BALL_STOPPED;
            gameObject.transform.localPosition = m_Grid[m_iRow, m_iCol].transform.localPosition;
            return false;
        }
        return true;
    }


    public void NextTile()
    {

        if (!CanMove()) return;

        {
            if(m_BallState == BALL_STATE.BALL_MOVE_DOWN || m_BallState == BALL_STATE.BALL_STOPPED)
                if (m_Grid[m_iRow, m_iCol - 1].m_TileState == Tile.TILE_STATE.EMPTY) m_BallState = BALL_STATE.BALL_MOVE_LEFT;
            else m_BallState = BALL_STATE.BALL_MOVE_RIGHT;
        }



        //HORIZONTAL!!!!!!!!!!!
        int iCol = m_iCol;
        iCol += m_BallState == BALL_STATE.BALL_MOVE_RIGHT ? 1 : -1;

        if (m_Grid[m_iRow, iCol].m_TileState != Tile.TILE_STATE.EMPTY)
        {
            m_BallState = m_BallState == BALL_STATE.BALL_MOVE_RIGHT ? BALL_STATE.BALL_MOVE_LEFT : BALL_STATE.BALL_MOVE_RIGHT;
        }
        else
        {
            m_iPrevCol = m_iCol;
            m_Grid[m_iRow, iCol].m_TileState = Tile.TILE_STATE.IN_TRANSITION;
            m_iCol = iCol;
            if(m_Grid[m_iRow, m_iPrevCol].m_TileState != Tile.TILE_STATE.BLOCK)
                m_Grid[m_iRow, m_iPrevCol].m_TileState = Tile.TILE_STATE.OUT_TRANSITION;
        }
    }

    public void UpdateBallMovement()
    {
        if (m_BallState == BALL_STATE.BALL_MOVE_RIGHT)
        {
            Vector3 localPos = transform.localPosition;
            if (localPos.x < m_Grid[m_iRow, m_iCol].m_Position.x)
            {

                transform.localPosition = new Vector3(localPos.x + m_fBallSpeed, localPos.y, localPos.z);
            }
        }
        else if (m_BallState == BALL_STATE.BALL_MOVE_LEFT)
        {
            Vector3 localPos = transform.localPosition;
            if (localPos.x > m_Grid[m_iRow, m_iCol].m_Position.x)
            {
                transform.localPosition = new Vector3(localPos.x - m_fBallSpeed, localPos.y, localPos.z);
            }
        }
        else if (m_BallState == BALL_STATE.BALL_MOVE_DOWN)
        {
            Vector3 localPos = transform.localPosition;
            if (localPos.y > m_Grid[m_iRow, m_iCol].m_Position.y)
            {
                transform.localPosition = new Vector3(localPos.x, localPos.y - m_fBallSpeed, localPos.z);
            }
        }
        else if (m_BallState == BALL_STATE.FALL)
        {
            Vector3 localPos = transform.localPosition;
            transform.localPosition = new Vector3(localPos.x, localPos.y - m_fBallSpeed, localPos.z);
        }
    }
}
