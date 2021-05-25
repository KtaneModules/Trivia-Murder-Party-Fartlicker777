using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using KModkit;
using System.Text.RegularExpressions;
using rnd = UnityEngine.Random;

public class mirror : MonoBehaviour
{
    public new KMAudio audio;
    public KMBombInfo bomb;
    public KMBombModule module;

    public KMSelectable mainSelectable;
    public TextMesh[] mirrorTexts;
    public KMSelectable[] textButtons;

    public Font[] boringFonts;
    public Font[] fancyFonts;
    public Font[] handwritingFonts;
    public Font[] standardFonts;
    public Font[] techFonts;

    public Material[] boringMats;
    public Material[] fancyMats;
    public Material[] handwritingMats;
    public Material[] standardMats;
    public Material[] techMats;

    private Font[] chosenFonts;
    private Material[] chosenMats;

    public bool TwitchPlaysActive;

    private int fontCategory;
    private int fontIndex;
    private int fontPosition;
    private int wordTable;
    private int solution;

    private bool moduleReady;
    private bool moduleSelected;
    private bool[] textsAnimating = new bool[3];
    private static readonly string[] table1 = new string[26] { "ALPACA", "BUBBLE", "COWBOY", "DIESEL", "EULOGY", "FUSION", "GASKET", "HOODIE", "ICEBOX", "JOYPOP", "KLEPTO", "LAMBDA", "MARBLE", "NUGGET", "OCELOT", "PIRATE", "QUOKKA", "RHYTHM", "SAPPHO", "TIMBRE", "UTOPIA", "VIXENS", "WOBBLE", "XYLOSE", "YOGURT", "ZIGZAG" };
    private static readonly string[] table2 = new string[26] { "ARCADE", "BAMBOO", "COPPER", "DOMAIN", "EMBLEM", "FUNGUS", "GAZEBO", "HAMLET", "IMPISH", "JESUIT", "KARATE", "LEGACY", "MOSQUE", "NOODLE", "OOLONG", "PIGEON", "QUINOA", "ROCKET", "SUNSET", "TOMBOY", "UNCORK",  "VESTRY", "WARDEN", "XOANON", "YONDER", "ZEALOT" };
    private static readonly string[] table3 = new string[26] { "AVENUE", "BISHOP", "CAMPUS", "DOCTOR", "EQUINE", "FUTURE", "GUITAR", "HOCKEY", "IONIZE", "JALOPY", "KITTEN", "LARYNX", "MONKEY", "NAPALM", "OXFORD", "PLASMA", "QUICHE", "REFLEX", "STRIKE", "TUXEDO", "URINAL", "VOYAGE", "WEASEL", "XENONS", "YODLER", "ZYGOTE" };

    private static int moduleIdCounter = 1;
    private int moduleId;
    private bool moduleSolved;

    private void Awake()
    {
        moduleId = moduleIdCounter++;
        module.OnActivate += delegate () { audio.PlaySoundAtTransform("m-start", transform); CheckForTP(); };
        foreach (KMSelectable word in textButtons)
            word.OnInteract += delegate () { PressWord(word); return false; };
    }

    private void Start()
    {
        fontCategory = rnd.Range(0, 5);
        switch (fontCategory)
        {
            case 0:
                chosenFonts = boringFonts.ToArray();
                chosenMats = boringMats.ToArray();
                break;
            case 1:
                chosenFonts = fancyFonts.ToArray();
                chosenMats = fancyMats.ToArray();
                break;
            case 2:
                chosenFonts = handwritingFonts.ToArray();
                chosenMats = handwritingMats.ToArray();
                break;
            case 3:
                chosenFonts = standardFonts.ToArray();
                chosenMats = standardMats.ToArray();
                break;
            case 4:
                chosenFonts = techFonts.ToArray();
                chosenMats = techMats.ToArray();
                break;
        }
        fontIndex = rnd.Range(0, 3);
        fontPosition = rnd.Range(0, 3);
        wordTable = rnd.Range(0, 3);
        foreach (TextMesh t in mirrorTexts)
            t.text = "";
        StartCoroutine(DisableThings()); // Unity doesn't like it if I don't let the other words exist for an extra frame
        mirrorTexts[1].font = chosenFonts[fontIndex];
        mirrorTexts[1].GetComponent<Renderer>().material = chosenMats[fontIndex];
        if (false)
//        if (Application.isEditor) // OnFocus doesn't get called in the TestHarness
        {
            moduleSelected = true;
            StartCoroutine(BeginModule());
        }
    }

    private IEnumerator BeginModule()
    {
        if (Application.isEditor)
            yield return new WaitForSeconds(7f);
        var startingWord = table1.Concat(table2).Concat(table3).PickRandom();
        Debug.LogFormat("[Mirror #{0}] Word initially written by the ghost: {1}", moduleId, startingWord);
        StartCoroutine(WriteWord(startingWord, 1));
        yield return new WaitForSeconds(5f);
        var defaultColor = mirrorTexts[1].color;
        StartCoroutine(FadeWord(defaultColor, 1));
        yield return new WaitUntil(() => textsAnimating.All(b => !b));
        mirrorTexts[1].text = "";
        mirrorTexts[1].color = defaultColor;
        mirrorTexts[0].gameObject.SetActive(true);
        mirrorTexts[2].gameObject.SetActive(true);
        var dummyCount = 0;
        var unusedFonts = chosenFonts.Where((x, i) => i != fontIndex).ToArray();
        var unusedMats = chosenMats.Where((x, i) => i != fontIndex).ToArray();
        for (int i = 0; i < 3; i++)
        {
            if (i == fontPosition)
            {
                mirrorTexts[i].font = chosenFonts[fontIndex];
                mirrorTexts[i].GetComponent<Renderer>().material = chosenMats[fontIndex];
            }
            else
            {
                mirrorTexts[i].font = unusedFonts[dummyCount];
                mirrorTexts[i].GetComponent<Renderer>().material = unusedMats[dummyCount];
                dummyCount++;
            }
        }
        var orderToShow = Enumerable.Range(0, 3).ToList().Shuffle().ToArray();
        var chosenTable = wordTable == 0 ? table1.ToArray() : wordTable == 1 ? table2.ToArray() : table3.ToArray();
        for (int i = 0; i < 3; i++)
        {
            StartCoroutine(WriteWord(orderToShow[i] == fontPosition ? chosenTable.PickRandom() : table1.Concat(table2).Concat(table3).PickRandom(), orderToShow[i]));
            yield return new WaitForSeconds(rnd.Range(.5f, .8f));
        }
        yield return new WaitUntil(() => textsAnimating.All(b => !b));
        var ordinals = new string[] { "1st", "2nd", "3rd" };
        Debug.LogFormat("[Mirror #{0}] Words shown: {1}", moduleId, mirrorTexts.Select(x => x.text).Join(", "));
        Debug.LogFormat("[Mirror #{0}] The original ghost wrote the {1} word. It's in the {2} table.", moduleId, ordinals[fontPosition], ordinals[wordTable]);
        var offsets = new int[] { 0, 2, 1 };
        solution = (fontPosition + offsets[wordTable]) % 3;
        Debug.LogFormat("[Mirror #{0}] Select the {1} word.", moduleId, ordinals[solution]);
        moduleReady = true;
    }

    private IEnumerator WriteWord(string word, int ix)
    {
        textsAnimating[ix] = true;
        audio.PlaySoundAtTransform("marker" + rnd.Range(1, 5), mirrorTexts[ix].transform);
        for(int i = 0; i < 6; i++)
        {
            mirrorTexts[ix].text += word[i].ToString();
            yield return new WaitForSeconds(.16f);
        }
        textsAnimating[ix] = false;
    }

    private IEnumerator FadeWord(Color startingColor, int ix)
    {
        var elapsed = 0f;
        var duration = 1.25f;
        textsAnimating[ix] = true;
        while (elapsed < duration)
        {
            mirrorTexts[ix].color = Color.Lerp(startingColor, Color.clear, elapsed / duration);
            yield return null;
            elapsed += Time.deltaTime;
        }
        mirrorTexts[ix].color = Color.clear;
        textsAnimating[ix] = false;
        if (moduleSolved)
            mirrorTexts[ix].gameObject.SetActive(false);
    }

    private void PressWord(KMSelectable word)
    {
        word.AddInteractionPunch(.25f);
        if (!moduleReady || moduleSolved)
            return;
        Debug.LogFormat("[Mirror #{0}] You selected {1}.", moduleId, word.GetComponent<TextMesh>().text);
        if (Array.IndexOf(textButtons, word) == solution)
        {
            module.HandlePass();
            moduleSolved = true;
            Debug.LogFormat("[Mirror #{0}] That was correct. Module solved!", moduleId);
            audio.PlaySoundAtTransform("m-solve", transform);
            StartCoroutine(FadeAll());
        }
        else
        {
            module.HandleStrike();
            Debug.LogFormat("[Mirror #{0}] That was incorrect. Strike!", moduleId);
        }
    }

    private void CheckForTP()
    {
        if (!TwitchPlaysActive)
            mainSelectable.OnFocus += delegate () { if (moduleSelected) { return; } moduleSelected = true; StartCoroutine(BeginModule()); };
    }

    private IEnumerator FadeAll()
    {
        var order = Enumerable.Range(0, 3).ToList().Shuffle().ToArray();
        var defaultColor = mirrorTexts[0].color;
        for (int i = 0; i < 3; i++)
        {
            StartCoroutine(FadeWord(defaultColor, order[i]));
            yield return new WaitForSeconds(rnd.Range(.5f, .8f));
        }
    }

    private IEnumerator DisableThings()
    {
        yield return null; // See Start()
        mirrorTexts[0].gameObject.SetActive(false);
        mirrorTexts[2].gameObject.SetActive(false);
    }

#pragma warning disable 414
    private readonly string TwitchHelpMessage = @"Use <!{0} select> to select the module and cause the ghost to write a message. Use <!{0} word 2> To press that word from top to bottom.";
#pragma warning restore 414


    IEnumerator ProcessTwitchCommand(string command)
    {
        command = command.Trim().ToUpper();
        if (command == "SELECT")
        {
            if (moduleSelected)
            {
                yield return "sendtochaterror Module has already been selected";
                yield break;
            }
            else
            {
                yield return null;
                moduleSelected = true;
                StartCoroutine(BeginModule());
            }
        }
        else if (Regex.IsMatch(command, @"^(SUBMIT)|(WORD)|(PRESS)\s+[1-3]$"))
        {
            if (!moduleReady)
            {
                yield return "sendtochaterror What words? There are no words here!";
                yield break;
            }
            else
            {
                yield return null;
                textButtons[command.Last() - '0' - 1].OnInteract();
            }
        }
    }

    IEnumerator TwitchHandleForcedSolve()
    {
        if (!moduleSelected)
        {
            yield return null;
            moduleSelected = true;
            StartCoroutine(BeginModule());
        }
        while (!moduleReady)
            yield return true;
        textButtons[solution].OnInteract();
    }

}
