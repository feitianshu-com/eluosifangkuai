using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrominoShapes : MonoBehaviour
{
    // 每个形状用二维数组表示（0=空, 1=方块）
    public static readonly int[,,] Shapes = new int[7, 4, 4]
    {
        // I形状 - 青色
        {
            {0,0,0,0},
            {1,1,1,1},
            {0,0,0,0},
            {0,0,0,0}
        },
        // O形状 - 黄色
        {
            {0,1,1,0},
            {0,1,1,0},
            {0,0,0,0},
            {0,0,0,0}
        },
        // T形状 - 紫色
        {
            {0,1,0,0},
            {1,1,1,0},
            {0,0,0,0},
            {0,0,0,0}
        },
        // L形状 - 橙色
        {
            {0,0,1,0},
            {1,1,1,0},
            {0,0,0,0},
            {0,0,0,0}
        },
        // J形状 - 蓝色
        {
            {1,0,0,0},
            {1,1,1,0},
            {0,0,0,0},
            {0,0,0,0}
        },
        // S形状 - 绿色
        {
            {0,1,1,0},
            {1,1,0,0},
            {0,0,0,0},
            {0,0,0,0}
        },
        // Z形状 - 红色
        {
            {1,1,0,0},
            {0,1,1,0},
            {0,0,0,0},
            {0,0,0,0}
        }
    };

    // 每种形状的颜色
    public static readonly Color[] Colors = new Color[]
    {
        Color.cyan,      // I
        Color.yellow,    // O
        Color.magenta,   // T
        new Color(1, 0.5f, 0), // 橙色
        Color.blue,      // J
        Color.green,     // S
        Color.red        // Z
    };
}
