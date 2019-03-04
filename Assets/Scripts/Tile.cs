using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public SpriteRenderer m_TileRenderer;
    public int m_iRow;
    public int m_iCol;

    public Vector2 m_Position;

    public enum TILE_STATE
    {
        EMPTY,
        BLOCK,
        IN_TRANSITION,
        OCCUPIED,
        OUT_TRANSITION,
        FALL

    };

    public TILE_STATE m_TileState;

    public void SetBlock()
    {
        m_TileState = TILE_STATE.BLOCK;
        m_bBlockType = true;
        m_TileRenderer.sprite = m_TileSprite;
    }

    public void Initialize(Sprite sprite, int col, int row, float fSize, float fScale)
    {
        m_TileSprite = sprite;
        m_TileRenderer = gameObject.AddComponent<SpriteRenderer>();
        if (m_TileState == TILE_STATE.BLOCK) m_bBlockType = true;
        transform.localScale = new Vector3(fScale, fScale, 0);
        m_iRow = row;
        m_iCol = col;
        transform.localPosition = new Vector3((m_iCol + 1) * fSize, (m_iRow + 1) * fSize, 0);
        m_Position = new Vector2(transform.localPosition.x, transform.localPosition.y);

        //ADDING COLLIDER FOR DEBUG PURPOSE
        BoxCollider2D bollide  = gameObject.AddComponent<BoxCollider2D>();
        bollide.size = new Vector2(256, 256);
       
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public bool m_bBlockType;
    public Sprite m_TileSprite;

    public void TileClicked()
    {
        if(m_bBlockType)
        {
            if(m_TileState == TILE_STATE.BLOCK)
            {
                m_TileRenderer.sprite = null;
                m_TileState = TILE_STATE.EMPTY;
            }
            else
            {
                m_TileRenderer.sprite = m_TileSprite;
                m_TileState = TILE_STATE.BLOCK;
            }
        }
    }

    public void ReleaseTile()
    {
        m_TileRenderer.sprite = null;
        m_TileState = TILE_STATE.EMPTY;
    }

    public void BlockTile()
    {
        m_TileRenderer.sprite = m_TileSprite;
        m_TileState = TILE_STATE.BLOCK;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
