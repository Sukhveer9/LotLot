using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{

    public Tile[,] m_Grid;
    public float m_fTileSize;
    public float m_fTileScale;
    public Sprite m_TileSprite;
    public Sprite m_BallSprite;

    public int m_iColPerBox;
    public int m_iRowsPerBox;

    private  List<int> m_StartingRow;
    private  List<int> m_StartingCol;



    public Ball[] m_TestBalls;

    //size formula : pixel size * scale
    void Start()
    {
        m_StartingRow = new List<int>();
        m_StartingCol = new List<int>();
        Application.targetFrameRate = 60;
        m_Grid = new Tile[19,26];
        int iCounter = 0;
        for(int i = 0; i < 19; i++)
        {
            for (int j = 0; j < 26; j++)
            {
                //  GameObject gObject = new GameObject("Tile"+i+"-"+j);
                //  gObject.AddComponent<Tile>();
                //  gObject.transform.parent = transform;
                //m_Grid[i, j] = gObject.GetComponent<Tile>();
                // m_Grid[i, j].Initialize(m_TileSprite, j, i, m_fTileSize, m_fTileScale);
                m_Grid[i, j] = transform.GetChild(iCounter).GetComponent<Tile>();
                iCounter++;
            }
        }

        m_BallsList = new Ball[m_iNumOfBalls];
        m_UnusedBallIndexQueue = new Queue<int>();
        for (int i = 0; i < m_iNumOfBalls; i++)
        {
            GameObject gObject = new GameObject("Ball_" + i);
            gObject.transform.parent = BallsObjects.transform;
            Ball ball = gObject.AddComponent<Ball>();
            SpriteRenderer ballRenderer = gObject.AddComponent<SpriteRenderer>();
            m_BallsList[i] = ball;
            gObject.SetActive(false);
            ballRenderer.sprite = m_BallSprite;
            m_UnusedBallIndexQueue.Enqueue(i);
        }

        for(int i = 0; i < 16; i++)
        {
            bool bBlock = true;
            for(int j = 0; j < 26-1; j++)
            {
                if(m_Grid[i,j].m_TileState == Tile.TILE_STATE.EMPTY)
                {
                    // m_StartingRow = i;
                    // m_start
                    if(bBlock)
                    {
                        if (!m_StartingRow.Contains(i))
                        {
                            m_StartingRow.Add(i);
                        }
                        if (!m_StartingCol.Contains(j))
                        {
                            m_StartingCol.Add(j);
                        }
                        bBlock = false;
                    }
                }
                if(m_Grid[i,j].m_TileState == Tile.TILE_STATE.BLOCK)
                {
                    if (bBlock == false) bBlock = true;
                }
            }
        }

        m_bGameReady = true;
    }

    public void BuildGrid(int iNumOfRows, int iNumOfCols, float fTileSize, float fTileScale)
    {
        m_Grid = new Tile[iNumOfRows, iNumOfCols];
        for(int i = 0; i < iNumOfRows; i++)
        {
            for(int j = 0; j < iNumOfCols; j++)
            {
                GameObject gObject = new GameObject("Tile " + i + "-" + j);
                m_Grid[i, j] = gObject.AddComponent<Tile>();
                gObject.transform.parent = transform;
                gObject.GetComponent<Tile>();
                m_Grid[i, j].Initialize(m_TileSprite, j, i, m_fTileSize, m_fTileScale/*, m_BallSprite*/);
            }
        }

        //SETUP THE GRID BORDERS
        //set the outer perimiter
        for(int i = 0; i < iNumOfRows; i++)
        {
            for(int j = 0; j < iNumOfCols; j++)
            {
                if(j == 0 || j == iNumOfCols-2)
                {
                    m_Grid[i, j].SetBlock();
                }
                if (i == 0 && j < iNumOfCols - 2)
                    m_Grid[i, j].SetBlock();
            }
        }

        //BUILD THE BOX
        for (int i = 0; i < iNumOfRows; i++)
        {
            for (int j = 0; j < iNumOfCols; j++)
            {
                if (j%6 == 0 && j < iNumOfCols - 2 && i < iNumOfRows - 2)
                    m_Grid[i, j].SetBlock();

                if (i % 4 == 0 && j < iNumOfCols - 2 && i < iNumOfRows - 2)
                {
                    m_Grid[i, j].SetBlock();
                }
            }
        }
    }

    public void DeleteGrid()
    {
        int iNumOfChilds = gameObject.transform.childCount;
        for(int i = 0; i < iNumOfChilds; i++)
        {
            GameObject ob = gameObject.transform.GetChild(0).gameObject;
            DestroyImmediate(ob);
        }
        m_Grid = null;
    }

    private Queue<int> m_UnusedBallIndexQueue;
    private Ball[] m_BallsList;
    public GameObject BallsObjects;

    private int m_BallCount = 0;
    private bool m_bGameReady = false;

    public void LaunchBall()
    {
        int iBallIndex = m_UnusedBallIndexQueue.Dequeue();
        m_BallsList[iBallIndex].Initialize(m_Grid, m_fBallSpeed);
        m_BallsList[iBallIndex].gameObject.SetActive(true);
    }

    private int m_FrameCounter = 1;
    private int m_iTickCounter = 0;


    public void Ticked()
    {
        m_iTickCounter++;
        for(int i = 0; i < 19; i++)
        {
            for(int j = 0; j < 26; j++)
            {
                if (m_Grid[i, j].m_TileState == Tile.TILE_STATE.IN_TRANSITION)
                    m_Grid[i, j].m_TileState = Tile.TILE_STATE.OCCUPIED;
                else if (m_Grid[i, j].m_TileState == Tile.TILE_STATE.OUT_TRANSITION)
                    m_Grid[i, j].m_TileState = Tile.TILE_STATE.EMPTY;
            }
        }
        
        if (/*m_BallCount != m_iNumOfBalls && */m_iTickCounter >= 2)
        {
            m_iTickCounter = 0;
            if (m_Grid[18,1].m_TileState != Tile.TILE_STATE.OCCUPIED)
            {
               // m_iTickCounter = 0;
               // m_BallCount++;
                LaunchBall();
            }

        }

        for (int i = 0; i < m_BallsList.Length; i++)
        {
            if (m_BallsList[i] != null && m_BallsList[i].gameObject.activeInHierarchy)
            {
                m_BallsList[i].BallDownPriority();
            }
        }

        for (int i = 0; i < m_BallsList.Length; i++)
        {
            if (m_BallsList[i] != null && m_BallsList[i].gameObject.activeInHierarchy)
            {
                if(!m_BallsList[i].CanUse())
                    m_BallsList[i].NextTile();
            }
        }

    }

    public Ball GetBall(int iRow, int iCol)
    {
        int iBallsLength = m_BallsList.Length;
        for(int i = 0; i < iBallsLength; i++)
        {
            if(m_BallsList[i].GetRow() == iRow && m_BallsList[i].GetCol() == iCol)
            {
                return m_BallsList[i];
            }
        }
        return null;
    }

    public int m_FrameTick;
    public float m_fBallSpeed;
    public int m_iNumOfBalls;
    // Update is called once per frame
    void Update()
    {
        if (!m_bGameReady) return;
        if (m_FrameCounter >= m_FrameTick)
        {
            m_FrameCounter = 0;
            Ticked();
        }
        m_FrameCounter++;

        for (int i = 0; i < m_BallsList.Length; i++)
        {
            if (m_BallsList[i] != null && m_BallsList[i].gameObject.activeInHierarchy)
            {
                m_BallsList[i].UpdateBallMovement();
                Vector3 pos = m_BallsList[i].transform.localPosition;
                if(pos.y < -12)
                {
                    m_BallsList[i].gameObject.SetActive(false);
                    m_UnusedBallIndexQueue.Enqueue(i);
                }
            }
        }
        

        if (Input.GetMouseButtonUp(0))
        {
            Vector3 mousePosition = Input.mousePosition;
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null)
            {
                Tile tile = hit.collider.gameObject.GetComponent<Tile>();
                tile.TileClicked();
            }
        }
    }
}