using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{

    public Grid m_Grid;

    private Queue<int> m_UnusedBallIndexQueue;
    private Ball[] m_BallsList;
    public GameObject BallsObjects;
    public Sprite m_BallSprite;

    // Start is called before the first frame update
    void Start()
    {
        m_BallsList = new Ball[300];
        for(int i = 0; i < 300; i++)
        {
            GameObject gObject = new GameObject("Ball+" + i);
            Ball ball = gObject.AddComponent<Ball>();
            SpriteRenderer ballRenderer = gObject.AddComponent<SpriteRenderer>();
            ballRenderer.sprite = m_BallSprite;
          //  ball.Initialize(m_Grid);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
