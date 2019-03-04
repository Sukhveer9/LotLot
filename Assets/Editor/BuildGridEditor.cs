using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Grid))]
public class BuildGridEditor : Editor
{
    private int m_iNumOfRows = 19;
    private int m_iNumOfCols = 26;
    private float m_fTileSize = 43.8f;
    private float m_fTileScale = 0.17f;

    public Sprite m_TileSprite;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        Grid gridObject = (Grid)target;
        m_iNumOfRows = EditorGUILayout.IntField("Num of Rows", m_iNumOfRows);
        m_iNumOfCols = EditorGUILayout.IntField("Num of Columns", m_iNumOfCols);
        m_fTileSize = EditorGUILayout.FloatField("Tile Size", m_fTileSize);
        m_fTileScale = EditorGUILayout.FloatField("Tile Scale", m_fTileScale);

        if(GUILayout.Button("Build Grid"))
        {
            gridObject.BuildGrid(m_iNumOfRows, m_iNumOfCols, m_fTileSize, m_fTileScale);
            UnityEditor.SceneManagement.EditorSceneManager.MarkAllScenesDirty();EditorUtility.SetDirty(gridObject.gameObject);
        }

        if(GUILayout.Button("Erase Grid"))
        {
            gridObject.DeleteGrid();
            UnityEditor.SceneManagement.EditorSceneManager.MarkAllScenesDirty();
        }
    }
}
