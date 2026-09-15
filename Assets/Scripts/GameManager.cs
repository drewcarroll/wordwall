using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public List<LetterBlock> blocks = new List<LetterBlock>();
    public SpriteRenderer exitRenderer;

    private bool wordFormed = false;
    private bool gameOver = false;
    private string message = "";

    void Awake()
    {
        Instance = this;
    }

    public void CheckWord()
    {
        if (wordFormed) return;

        foreach (LetterBlock block in blocks)
        {
            if (block.letter != MazeData.Word[0]) continue;

            bool match = true;
            for (int i = 1; i < MazeData.Word.Length; i++)
            {
                LetterBlock next = BlockAt(block.Cell + new Vector2Int(i, 0));
                if (next == null || next.letter != MazeData.Word[i])
                {
                    match = false;
                    break;
                }
            }

            if (match)
            {
                wordFormed = true;
                exitRenderer.color = Color.green;
                return;
            }
        }
    }

    LetterBlock BlockAt(Vector2Int cell)
    {
        foreach (LetterBlock block in blocks)
        {
            if (block.Cell == cell) return block;
        }
        return null;
    }

    public void Win()
    {
        if (gameOver || !wordFormed) return;
        gameOver = true;
        message = "You escaped! You win!";
        Time.timeScale = 0f;
    }

    public void Lose()
    {
        if (gameOver) return;
        gameOver = true;
        message = "The cat caught you! You lose!";
        Time.timeScale = 0f;
    }

    void Update()
    {
        if (gameOver && Input.GetKeyDown(KeyCode.R))
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    void OnGUI()
    {
        GUIStyle style = new GUIStyle();
        style.fontSize = 28;
        style.normal.textColor = Color.white;
        style.alignment = TextAnchor.MiddleCenter;

        string hint = wordFormed
            ? "You spelled " + MazeData.Word + "! The exit is open."
            : "Push the letters together to spell " + MazeData.Word;

        GUI.Label(new Rect(0, 10, Screen.width, 40), hint, style);

        if (!gameOver) return;

        style.fontSize = 32;
        GUI.Label(new Rect(0, Screen.height / 2 - 60, Screen.width, 40), message, style);
        GUI.Label(new Rect(0, Screen.height / 2, Screen.width, 40), "Press R to restart", style);
    }
}
