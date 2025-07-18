using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrominoSpawner : MonoBehaviour
{
    public GameObject tetrominoPrefab;
    public int gridWidth = 10;
    public int gridHeight = 20;


    public TetrisBackgroundGrid gridManager; // 引用网格管理器

    // 生成随机形状
    public void SpawnRandomTetromino()
    {
        // 游戏结束时直接返回，不生成新方块
        if (GameManager.instance != null && GameManager.instance.isGameOver)
        {
            Debug.Log("游戏已结束，停止生成方块");
            return;
        }
        if (GameManager.instance != null && GameManager.instance.isGameStop)
        {
            Debug.Log("游戏已暂停，请点击恢复");
            return;
        }
        
        int randomShape = Random.Range(0, 7);
        GameObject tetromino = Instantiate(
            tetrominoPrefab,
            Vector3.zero,
            Quaternion.identity,
            transform);

        // 确保tetromino预制体上有Tetromino组件
        Tetromino tetrominoComponent = tetromino.GetComponent<Tetromino>();
        if (tetrominoComponent != null)
        {
            tetrominoComponent.Initialize(randomShape, gridManager.grid);
        }
        else
        {
            Debug.LogError("Tetromino预制体缺少Tetromino组件!");
        }
    }

    // 启动游戏时生成第一个形状
    void Start()
    {
        Debug.Log("TetrominoSpawner.Start() 已执行");

        gridManager = FindObjectOfType<TetrisBackgroundGrid>();

        // 生成第一个方块
        // SpawnRandomTetromino();
    }
}
