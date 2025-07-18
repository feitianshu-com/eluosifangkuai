using UnityEngine;

public class TetrisBackgroundGrid : MonoBehaviour
{
    [Header("网格设置")]
    [SerializeField] private int gridWidth = 10;
    [SerializeField] private int gridHeight = 20;
    [SerializeField] private float cellSize = 1f;
    [SerializeField] private Color gridColor = new Color(0.2f, 0.2f, 0.2f, 0.5f);

    [Header("引用")]
    [SerializeField] private Material lineMaterial;

    public Transform[,] grid;

    // 在Scene视图中绘制Gizmos
    private void OnDrawGizmos()
    {
        DrawGridLines();
    }

    private void OnRenderObject()
    {
        DrawGridLines();
    }

    private void DrawGridLines()
    {
        if (!lineMaterial)
        {
            Debug.LogError("缺少线条材质引用！");
            return;
        }

        // 设置材质
        lineMaterial.SetColor("_Color", gridColor);
        lineMaterial.SetPass(0);

        GL.PushMatrix();
        GL.MultMatrix(transform.localToWorldMatrix);

        // 绘制垂直线
        GL.Begin(GL.LINES);
        GL.Color(gridColor);

        for (int x = 0; x <= gridWidth; x++)
        {
            GL.Vertex3(x * cellSize, 0, 0);
            GL.Vertex3(x * cellSize, gridHeight * cellSize, 0);
        }

        // 绘制水平线
        for (int y = 0; y <= gridHeight; y++)
        {
            GL.Vertex3(0, y * cellSize, 0);
            GL.Vertex3(gridWidth * cellSize, y * cellSize, 0);
        }

        GL.End();
        GL.PopMatrix();
    }


    // 检查并消除已满的行
    public void CheckLines()
    {
        for (int y = 0; y < gridHeight; y++)
        {
            if (IsLineComplete(y))
            {
                ClearLine(y);
                MoveLinesDown(y + 1);
                y--; // 重新检查当前行（现在是上一行下移后的行）
            }
        }
    }

    // 检查指定行是否已满
    private bool IsLineComplete(int y)
    {
        // 检查 y 是否在有效范围内
        if (y < 0)
        {
            Debug.LogError($"检查行完成度时 y 越界: {y}");
            return false;
        }
        for (int x = 0; x < gridWidth; x++)
        {
            Debug.LogError($"{x}           {y}");
            if (grid[x, y] == null)
            {
                return false;
            }
        }
        return true;
    }

    // 清除指定行
    private void ClearLine(int y)
    {
        for (int x = 0; x < gridWidth; x++)
        {
            if (grid[x, y] != null)
            {
                Destroy(grid[x, y].gameObject);
                grid[x, y] = null;
                FindObjectOfType<GameManager>().AddScore(1);
            }
        }
    }

    // 将指定行及以上的所有行下移
    private void MoveLinesDown(int startY)
    {
        for (int y = startY; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                if (grid[x, y] != null)
                {
                    grid[x, y - 1] = grid[x, y];
                    grid[x, y] = null;
                    grid[x, y - 1].position += new Vector3(0, -1, 0);
                }
            }
        }
    }

    void Start()
    {
        // 初始化网格
        grid = new Transform[gridWidth, gridHeight];
    }

    // 提供公共访问方法（可选）
    public Transform[,] GetGrid()
    {
        return grid;
    }
}