using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class SpawnedTile : MonoBehaviour
{
    // x = col
    // y = row
    public int x, y;
    public int value;

    private Collider2D collider;
    private TextMeshProUGUI valueText;
    // This sprite needs to change the color when values gets changed.
    private SpriteRenderer spriteColor;

    private void Start()
    {
        collider = GetComponent<Collider2D>();
        valueText = GetComponentInChildren<TextMeshProUGUI>();
        spriteColor = GetComponent<SpriteRenderer>();

        if (value == 0)
            valueText.enabled = false;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Check if it is this sprite.
            Vector3 wp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 clickPosition = new Vector2(wp.x, wp.y);
            if (collider == Physics2D.OverlapPoint(clickPosition))
            {
                // Increase all values of of Tiles in the same row and column by 1.
                GridManager.instance.ColRowChange(x, y);
            }
        }
    }

    // called from GridManager.
    public void IncrementValue()
    {
        value++;
        StartCoroutine(ChangeColorYellow());
    }

    public void UpdateValueText()
    {
        FibonacciCheck();

        if (value > 0)
        {
            valueText.enabled = true;
            valueText.text = value.ToString();
            ConsecutiveFibonacciCheck();
        }
        else
        {
            valueText.enabled = false;
        }
    }
    
    public bool FibonacciCheck()
    {
        return GridManager.instance.FibonacciCheck(value);
    }

    // Fibonacci Grid check
    private void ConsecutiveFibonacciCheck()
    {
        // Check if there is 4 or more spawned tiles next to you including yourself for 5.
        GridManager.instance.FibonacciGridCheck();
    }

  
    private IEnumerator ChangeColorYellow()
    {
        spriteColor.color = Color.yellow;
        yield return new WaitForSeconds(1f);
        spriteColor.color = Color.white;
    }

    public IEnumerator ChangeColorGreen()
    {
        yield return new WaitForSeconds(2f);
        spriteColor.color = Color.green;
        yield return new WaitForSeconds(2f);
        value = 0;
        UpdateValueText();
        spriteColor.color = Color.white;
    }
}
