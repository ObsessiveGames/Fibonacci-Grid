using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class GridManager : MonoBehaviour
{
    public static GridManager instance;

    public SpawnedTile[,] Grid;
    private int Vertical, Horizontal;

    [Header("Grid Size")]
    [SerializeField] private int Columns;
    [SerializeField] private int Rows;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        // If the columns and rows has value, re-adjust the camera orthographicSize.
        // This will also cause a bug, if there rows is uneven which causes a float value, need to make sure this doesn't happen.
        Camera.main.orthographicSize = Rows / 2;

        // Needs to be a orthographic camera and this will be the size which is set once.
        Vertical = (int)Camera.main.orthographicSize;
        // Uses the vertical size and then the screen resolution size.
        Horizontal = Vertical * (Screen.width / Screen.height);

        // Give grid a value of how many columns and rows it will have.
        Grid = new SpawnedTile[Columns, Rows];

        // Generate the grid.
        for (int i = 0; i < Columns; i++)
        {
            for (int j = 0; j < Rows; j++)
            {
                SpawnTile(i, j);
            }
        }
    }

    private void SpawnTile(int x, int y)
    {
        GameObject g = Instantiate(Resources.Load("Square")) as GameObject;
        SpawnedTile s = g.GetComponent<SpawnedTile>();
        g.transform.parent = transform;
        g.name = "x: " + x + " y: " + y;
        g.transform.position = new Vector3(x - (Horizontal - 0.5f), y - (Vertical - 0.5f));
        s.x = x;
        s.y = y;
        s.FibonacciCheck();
        Grid[x, y] = s;
    }

    public void ColRowChange(int col, int row)
    {
        // Change row values.
        for (int i = 0; i < Columns; i++)
        {
            if (i == col) continue;
            Grid[i, row].IncrementValue();
        }

        // Change col values.
        for (int i = 0; i < Rows; i++)
        {
            if (i == row) continue;
            Grid[col, i].IncrementValue();
        }

        // col and row
        Grid[col, row].IncrementValue();

        for (int i = 0; i < Columns; i++)
        {
            for (int j = 0; j < Rows; j++)
            {
                Grid[i, j].UpdateValueText();
            }
        }
    }

    public void FibonacciGridCheck()
    {
        // Go through grid for FibonacciCheck
        for (int i = 0; i < Columns; i++)
        {
            for (int j = 0; j < Rows; j++)
            {
                if (!FibonacciCheck(Grid[i,j].value)) continue;
                FibonacciConsecutiveCheck(i, j);
            }
        }
    }

    // Checks the individual sprite with 5 consecutive numbers
    public void FibonacciConsecutiveCheck(int col, int row)
    {
        int i = 0;
        int leftOneSeperator = 1, rightOneSeperator = 1, upOneSeperator = 1, downOneSeperator = 1;
        bool leftInSequence = true, rightInSequence = true, upInSequence = true, downInSequence = true ;

        // These checks will not make sure that the value is the next term, but it will make sure that it is a higher consecutive number.

        while (i < 4)
        {
            // Left check
            if (col > 4 && leftInSequence && Grid[col - i, row].FibonacciCheck())
            {
                if (Grid[col - i, row].value == 1 && Grid[col - (i + 1), row].value == 1 && leftOneSeperator == 1 && i < 2)
                {
                    leftOneSeperator++;
                }
                else if (Grid[col - i, row].value < Grid[col - (i + 1), row].value && FibonacciCheck(Grid[col - (i + 1), row].value + Grid[col - i, row].value))
                {
                    leftInSequence = true;
                }
                else
                {
                    leftInSequence = false;
                }
            }
            else
            {
                leftInSequence = false;
            }

            // Right check
            if (col < Columns - 4 && rightInSequence && Grid[col + i, row].FibonacciCheck())
            {
                if (Grid[col + i, row].value == 1 && Grid[col + (i + 1), row].value == 1 && rightOneSeperator == 1 && i < 2)
                {
                    rightOneSeperator++;
                }
                else if (Grid[col + i, row].value < Grid[col + (i + 1), row].value && FibonacciCheck(Grid[col + (i + 1), row].value + Grid[col + i, row].value))
                {
                    rightInSequence = true;
                }
                else
                {
                    rightInSequence = false;
                }
            }
            else
            {
                rightInSequence = false;
            }

            // Up check
            if (row < Rows - 4 && upInSequence && Grid[col, row + i].FibonacciCheck())
            {
                if (Grid[col, row + i].value == 1 && Grid[col, row + (i + 1)].value == 1 && upOneSeperator == 1 && i < 2)
                {
                    upOneSeperator++;
                }
                else if (Grid[col, row + i].value < Grid[col, row + (i + 1)].value && FibonacciCheck(Grid[col, row + (i + 1)].value + Grid[col, row + i].value))
                {
                    upInSequence = true;
                }
                else
                {
                    upInSequence = false;
                }
            }
            else
            {
                upInSequence = false;
            }

            // Down check
            if (row > 4 && downInSequence && Grid[col, row - i].FibonacciCheck())
            {
                if (Grid[col, row - i].value == 1 && Grid[col, row - (i + 1)].value == 1 && downOneSeperator == 1 && i < 2)
                {
                    downOneSeperator++;
                }
                else if (Grid[col, row - i].value < Grid[col, row - (i + 1)].value && FibonacciCheck(Grid[col, row - (i + 1)].value + Grid[col, row - i].value))
                {
                    downInSequence = true;
                }
                else
                {
                    downInSequence = false;
                }
            }
            else
            {
                downInSequence = false;
            }

            if (!leftInSequence && !rightInSequence && !upInSequence && !downInSequence) break;

            i++;
        }

        // When the while loop is done with the sequence check it will use any sequences that remained true and ChangeColor and reset the value.
        if (i == 4)
        {
            if (leftInSequence)
            {
                for (int j = 0; j < 5; j++)
                {
                    StartCoroutine(Grid[col - j, row].ChangeColorGreen());
                }
            }

            if (rightInSequence)
            {
                for (int j = 0; j < 5; j++)
                {
                    StartCoroutine(Grid[col + j, row].ChangeColorGreen());
                }
            }

            if (upInSequence)
            {
                for (int j = 0; j < 5; j++)
                {
                    StartCoroutine(Grid[col, row + j].ChangeColorGreen());
                }
            }

            if (downInSequence)
            {
                for (int j = 0; j < 5; j++)
                {
                    StartCoroutine(Grid[col, row - j].ChangeColorGreen());
                }
            }
        }
    }

    private bool IsPerfectSquare(long number)
    {
        double result = Math.Sqrt(number);
        bool isSquare = result % 1 == 0;
        return isSquare;
    }

    // Checks if the value is of the Fibonacci Sequence.
    public bool FibonacciCheck(int check)
    {
        long intValueSquared = check * check;
        if (IsPerfectSquare(5 * intValueSquared + 4) || IsPerfectSquare(5 * intValueSquared - 4))
        {
            return true;
        }

        return false;
    }
}
