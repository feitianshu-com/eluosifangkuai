using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetromino : MonoBehaviour
{
    public GameObject blockPrefab;
    private int shapeIndex;
    private Transform[,] grid;
    private float fallInterval = 1f;
    private float lastFallTime;
    private bool isFalling = true;

    public bool isGameOver = false; // 标记游戏是否结束

    // 初始化形状
    public void Initialize(int shapeIndex, Transform[,] grid)
    {
        this.shapeIndex = shapeIndex;
        this.grid = grid;
        lastFallTime = Time.time;

        // 根据形状数据生成方块
        GenerateShape();

        // 检查初始位置是否合法
        if (!IsValidPosition())
        {
            isGameOver = true;
            Debug.Log("游戏结束！无法生成新方块");

            // 通知游戏管理器
            FindObjectOfType<GameManager>().GameOver();
        }
    }

    // 生成形状
    private void GenerateShape()
    {
        int[,] shapeData = GetShapeData(shapeIndex);

        for (int y = 0; y < 4; y++)
        {
            for (int x = 0; x < 4; x++)
            {
                if (shapeData[y, x] == 1)
                {
                    // 实例化方块并设置位置
                    GameObject block = Instantiate(blockPrefab,
                        new Vector3(x, y, 0),
                        Quaternion.identity,
                        transform);

                    // 设置方块颜色
                    Renderer renderer = block.GetComponent<Renderer>();
                    if (renderer != null)
                    {
                        renderer.material.color = TetrominoShapes.Colors[shapeIndex];
                    }
                    else
                    {
                        Debug.LogError("方块预制体缺少Renderer组件!");
                    }
                }
            }
        }

        // 将形状移到网格顶部中央 - 调整Y坐标从19降低到15
        transform.position = new Vector3(grid.GetLength(0) / 2 - 1.5f, grid.GetLength(1) - 0.5f, 0);
    }

    // 获取特定形状的数据
    private int[,] GetShapeData(int index)
    {
        int[,] shape = new int[4, 4];

        for (int y = 0; y < 4; y++)
        {
            for (int x = 0; x < 4; x++)
            {
                shape[y, x] = TetrominoShapes.Shapes[index, y, x];
            }
        }

        return shape;
    }

    void Update()
    {
        if (isFalling)
        {
            if (GameManager.instance != null && GameManager.instance.isGameStop)
            {
                Debug.Log("游戏已暂停，请点击恢复");
                return;
            }
            // 自动下落
            if (Time.time - lastFallTime >= fallInterval)
            {
                MoveDown();
                lastFallTime = Time.time;
            }

            // 玩家控制
            HandleInput();
        }
    }

    // 处理玩家输入
    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveLeft();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveRight();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            MoveDown();
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Rotate();
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            HardDrop();
        }
    }

    // 移动方法实现（需添加碰撞检测）
    private void MoveLeft()
    {
        transform.position += Vector3.left;
        if (!IsValidPosition())
            transform.position -= Vector3.left;
    }

    private void MoveRight()
    {
        transform.position += Vector3.right;
        if (!IsValidPosition())
            transform.position -= Vector3.right;
    }

    private void MoveDown()
    {
        transform.position += Vector3.down;
        if (!IsValidPosition())
        {
            transform.position -= Vector3.down;
            isFalling = false;
            LockToGrid();
            FindObjectOfType<TetrominoSpawner>().SpawnRandomTetromino();
        }
    }

    private void HardDrop()
    {
        while (IsValidPosition())
        {
            transform.position += Vector3.down;
        }
        transform.position -= Vector3.down;
        isFalling = false;
        LockToGrid();
        FindObjectOfType<TetrominoSpawner>().SpawnRandomTetromino();
    }

    private void Rotate()
    {
        transform.Rotate(0, 0, -90);
        if (!IsValidPosition())
            transform.Rotate(0, 0, 90);
    }

    // 检查位置是否有效
    private bool IsValidPosition()
    {
        foreach (Transform child in transform)
        {
            Debug.Log(child.position.y);
            // 向下取整（Floor），确保小数部分被舍去
            int gridX = Mathf.FloorToInt(child.position.x);
            int gridY = Mathf.FloorToInt(child.position.y);

            Vector2Int pos = new Vector2Int(gridX, gridY);

            // Vector2Int pos = Vector2Int.RoundToInt(child.position);

            Debug.Log(pos.y);
            // 检查左、右、下边界（下边界 y < 0 时无效）
            if (pos.x < 0 || pos.x >= grid.GetLength(0) || pos.y < 0)
            {
                Debug.Log("xxxxxxxxxxxxxx");
                return false;
            }
        
            // 检查是否与已有方块重叠（注意：y 可能超出网格高度，需判断）
            if (pos.y < grid.GetLength(1)) // 仅当 y 在网格范围内时检查重叠
            {
                if (grid[pos.x, pos.y] != null)
                {
                    return false;
                }
            }
        }
        return true;
    }

    // 将形状锁定到网格
    private void LockToGrid()
    {
        foreach (Transform child in transform)
        {
            // 向下取整（Floor），确保小数部分被舍去
            int gridX = Mathf.FloorToInt(child.position.x);
            int gridY = Mathf.FloorToInt(child.position.y);

            Vector2Int pos = new Vector2Int(gridX, gridY);

            // Vector2Int pos = Vector2Int.RoundToInt(child.position);

            if (pos.y < grid.GetLength(1))
            {
                grid[pos.x, pos.y] = child;
            }
        }

        // 检查并消除已满的行
        FindObjectOfType<TetrisBackgroundGrid>().CheckLines();
    }
}
