using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using KModkit;

public class ScratchOffScript : MonoBehaviour
{

    public KMBombInfo Bomb;
    public KMAudio Audio;
    public KMSelectable submit;
    public KMSelectable[] buttons;
    public SpriteRenderer[] icons;
    public Sprite[] iconChoices;
    public MeshRenderer[] leds;
    public MeshRenderer[] stageLeds;
    public Material[] stageColors;
    public Material[] colors;

    static int moduleIdCounter = 1;
    int moduleId;
    private bool moduleSolved;
    private bool isAnimating;
    string[] coordinates = new string[] { "A1", "B1", "C1", "A2", "B2", "C2", "A3", "B3", "C3" };

    int chosenGrid;
    int[][] cashPlacements = new int[][] { new[] { 3, 5, 6 }, new[] { 2, 3, 8 }, new[] { 0, 6, 7 }, new[] { 0, 1, 4 }, new[] { 2, 5, 6 }, new[] { 0, 4, 7 }, new[] { 1, 4, 5 }, new[] { 0, 3, 5 }, new[] { 0, 2, 6 } };
    int[] cash;
    int finalSkull;

    int[] board = new int[9].Select(x => x = -1).ToArray();
    int[] operations = new int[3];
    bool[] revealed = new bool[9];
    int revealedCount = 0;
    int stage = 0;

    void Awake() 
    {
        moduleId = moduleIdCounter++;
        foreach (KMSelectable button in buttons) 
            button.OnInteract += delegate () { RevealIcon(Array.IndexOf(buttons, button)); return false; };
        submit.OnInteract += delegate () { Submit(); return false; };
    }

    void Start() 
    {
        GetInitials();
        GenerateStage();
        PlaceRemainder();
    }


    void GetInitials()
    {
        chosenGrid = (Bomb.GetSerialNumberNumbers().Sum() - 1) % 9;
        if (chosenGrid == -1) chosenGrid = 0;
        Debug.LogFormat("[Scratch-Off #{0}] The starting grid is grid #{1}.", moduleId, chosenGrid + 1);
    }

    void GenerateStage()
    {
        Debug.LogFormat("[Scratch-Off #{0}] ::STAGE #{1}::", moduleId, stage + 1);

        operations = Enumerable.Range(0, 6).ToList().Shuffle().Take(3).ToArray();
        for (int i = 0; i < 3; i++)
            leds[i].material = colors[operations[i]];
        Debug.LogFormat("[Scratch-Off #{0}] The three colors displayed are {1}.", moduleId, operations.Select(x => colors[x].name).Join(", "));
        

        finalSkull = chosenGrid;
        cash = cashPlacements[chosenGrid].ToArray(); //Not having a .ToArray() here basically causes the module to shit itself and have stuff overlap.
        for (int i = 0; i < 3; i++)
        {
            finalSkull = ApplyMovement(finalSkull, operations[i]);
            for (int j = 0; j < 3; j++)
                cash[j] = ApplyMovement(cash[j], operations[i]);
            Debug.LogFormat("[Scratch-Off #{0}] After the {1} operation, the skull is at position {2} and the cash are at positions {3}.",
                moduleId, colors[operations[i]].name, coordinates[finalSkull], cash.Select(x => coordinates[x]).Join(", ")); 
        }
         board[finalSkull] = 1;
        for (int i = 0; i < 3; i++)
            board[cash[i]] = 0;

        for (int i = 0; i < 3; i++)
            finalSkull = ApplyMovement(finalSkull, operations[i]);
    }
    void PlaceRemainder()
    {
        int[] remaining = new int[] { 0, 0, 0, 0, 1 }.Shuffle();
        int[] empties = Enumerable.Range(0, 9).Where(x => board[x] == -1).ToArray();
        for (int i = 0; i < empties.Length; i++)
            board[empties[i]] = remaining[i];
        Debug.LogFormat("[Scratch-Off #{0}] The grid is as follows:", moduleId);
        LogGrid(board, 3, 3, "$☠");
    }

    void RevealIcon(int position)
    {

        if (isAnimating || revealed[position]) return;
        Audio.PlaySoundAtTransform("scratch", buttons[position].transform);
        revealed[position] = true;
        icons[position].sprite = iconChoices[board[position]];
        if (moduleSolved) return;

        if (board[position] == 1)
        {
            Debug.LogFormat("[Scratch-Off #{0}] Scratched off a skull. Strike incurred and grid reset.", moduleId);
            StartCoroutine(Strike());
        }
        else
        {
            revealedCount++;
            Debug.LogFormat("[Scratch-Off #{0}] Scratched off a cash. There are now {1} revealed cash.", moduleId, revealedCount);
        }
    }
    
    void LogGrid(int[] grid, int height, int length, string charSet, int shift = 0)
    {
        string logger = string.Empty;
        for (int i = 0; i < height*length; i++)
        {
            logger += charSet[grid[i] + shift];
            if (i % length == length - 1)
            {
                Debug.LogFormat("[Scratch-Off #{0}] {1}", moduleId, logger);
                logger = string.Empty;
            }
        }
    }

    void IncrementStage()
    {
        stageLeds[stage % 3].material = stageColors[1];
        stage++;
        if (stage == 3)
        {
            moduleSolved = true;
            GetComponent<KMBombModule>().HandlePass();
            for (int i = 0; i < 3; i++)
                leds[i].material = colors[3];
            Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.CorrectChime, transform);
            return;
        }

    }

    void Submit()
    {
        submit.AddInteractionPunch(1);
        Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, submit.transform);
        if (moduleSolved) return;

        if (revealedCount < 3)
        {
            GetComponent<KMBombModule>().HandleStrike();
            Debug.LogFormat("[Scratch-Off #{0}] Attempted to submit when fewer than 3 cash were revealed. Strike incurred.", moduleId);
        }
        else
        {
            for (int i = 0; i < revealedCount - 2; i++)
                IncrementStage();
            if (!moduleSolved)
            {
                ClearBoard();
                GenerateStage();
                PlaceRemainder();
            }
        }
    }
    void ClearBoard()
    {
        revealedCount = 0;
        for (int i = 0; i < 9; i++)
        {
            board[i] = -1;
            revealed[i] = false;
            icons[i].sprite = null;
        }
    }

    int ApplyMovement(int start, int rule)
    {
        int row = start / 3;
        int col = start % 3;
        int row2 = row;
        int col2 = col;

        switch (rule)
        {
            case 0: row = col2; col = 2 - row2; break;
            case 1: row = 2 - col2; col = row2; break;
            case 2: row = 2 - row; break;
            case 3: col = 2 - col; break;
            case 4: row++; row %= 3; break;
            case 5: col++; col %= 3; break;
        }
        return 3 * row + col;
    }

    IEnumerator Strike()
    {
        isAnimating = true;
        yield return new WaitForSecondsRealtime(1);
        GetComponent<KMBombModule>().HandleStrike();
        yield return new WaitForSecondsRealtime(0.5f);
        ClearBoard();
        GenerateStage();
        PlaceRemainder();
        isAnimating = false;
    }


#pragma warning disable 414
    private readonly string TwitchHelpMessage = @"Use [!{0} scratch A1 B2 C3] to scratch off those spots. Use [!{0} submit] to press the submit button.";
#pragma warning restore 414

    IEnumerator ProcessTwitchCommand(string input)
    {
        string command = input.Trim().ToUpperInvariant();
        List<string> parameters = command.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
        if (command == "SUBMIT")
        {
            submit.OnInteract();
            yield return new WaitForSeconds(0.1f);
            yield break;
        }
        if (parameters[0] == "SCRATCH" || parameters[0] == "PRESS")
            parameters.RemoveAt(0);
        if (parameters.All(x => coordinates.Contains(x)))
        {
            yield return null;
            foreach (string coord in parameters)
            {
                buttons[Array.IndexOf(coordinates, coord)].OnInteract();
                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    IEnumerator TwitchHandleForcedSolve()
    {
        int i = 0;
        while (revealedCount < 6 - stage)
        {
            buttons[Enumerable.Range(0, 9).Where(x => board[x] == 0).ToArray()[i]].OnInteract();
            i++;
            yield return new WaitForSeconds(0.1f);
        }
        submit.OnInteract();
        yield return new WaitForSeconds(0.1f);
    }
}