using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using KModkit;

public class PixelArt : MonoBehaviour {

    public KMBombInfo Bomb;
    public KMAudio Audio;
    public KMSelectable[] FastFoodChains;
    public GameObject[] Weed;
    public Material[] Colores;
    public KMSelectable Chungus;

    bool[] eXishsTwoTruthsAndALieWillNeverBeFinished = {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false};
    bool[] ButtonTrueThing = {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false};
    bool[] Active = {false, false, true};

    static int moduleIdCounter = 1;
    int moduleId;
    private bool moduleSolved;

    void Awake () {
        moduleId = moduleIdCounter++;

        foreach (KMSelectable Burger in FastFoodChains) {
            Burger.OnInteract += delegate () { BurgerPress(Burger); return false; };
        }
        Chungus.OnInteract += delegate () { ChungusPress(); return false; };
    }

    void BurgerPress(KMSelectable Burger) {
      Burger.AddInteractionPunch();
      Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, Burger.transform);
      if (!Active[1])
        return;
      for (int i = 0; i < eXishsTwoTruthsAndALieWillNeverBeFinished.Count(); i++)
        if (Burger == FastFoodChains[i]) {
          ButtonTrueThing[i] = !ButtonTrueThing[i];
          if (ButtonTrueThing[i])
            Weed[i].GetComponent<MeshRenderer>().material = Colores[1];
          else
            Weed[i].GetComponent<MeshRenderer>().material = Colores[0];
      }
    }

    void ChungusPress() {
      if (Active[1])
        StartCoroutine(FatmanInbound());
      else if (!Active[0]) {
        Active[0] = true;
        StartCoroutine(ChungusGenerator());
      }
    }

    IEnumerator ChungusGenerator() {
      Debug.LogFormat("[Pixel Art #{0}] In reading order the order is:", moduleId);
      for (int i = 0; i < eXishsTwoTruthsAndALieWillNeverBeFinished.Count(); i++) {
        if (UnityEngine.Random.Range(0, 2) == 1) {
          eXishsTwoTruthsAndALieWillNeverBeFinished[i] = true;
          Weed[i].GetComponent<MeshRenderer>().material = Colores[1];
          Debug.LogFormat("[Pixel Art #{0}] Red.", moduleId);
        }
        else {
          Weed[i].GetComponent<MeshRenderer>().material = Colores[0];
          Debug.LogFormat("[Pixel Art #{0}] White.", moduleId);
        }
      }
      yield return new WaitForSeconds(7f);
      for (int i = 0; i < eXishsTwoTruthsAndALieWillNeverBeFinished.Count(); i++)
        Weed[i].GetComponent<MeshRenderer>().material = Colores[0];
      Active[1] = true;
    }

    IEnumerator FatmanInbound() {
      Active[1] = false;
      for (int i = 0; i < eXishsTwoTruthsAndALieWillNeverBeFinished.Count(); i++) {
        if (!ButtonTrueThing[i])
          Debug.LogFormat("[Pixel Art #{0}] White", moduleId);
        else
          Debug.LogFormat("[Pixel Art #{0}] Red", moduleId);
      }
      for (int i = 0; i < eXishsTwoTruthsAndALieWillNeverBeFinished.Count(); i++) {
        if (eXishsTwoTruthsAndALieWillNeverBeFinished[i] == ButtonTrueThing[i])
          Weed[i].GetComponent<MeshRenderer>().material = Colores[2];
        else {
          Weed[i].GetComponent<MeshRenderer>().material = Colores[1];
          Active[2] = false;
        }
        yield return new WaitForSeconds(.1f);
      }
      if (Active[2])
        GetComponent<KMBombModule>().HandlePass();
      else {
        GetComponent<KMBombModule>().HandleStrike();
        for (int i = 0; i < eXishsTwoTruthsAndALieWillNeverBeFinished.Count(); i++) {
          eXishsTwoTruthsAndALieWillNeverBeFinished[i] = false;
          ButtonTrueThing[i] = false;
          Weed[i].GetComponent<MeshRenderer>().material = Colores[3];
        }
        Active[0] = false;
        Active[2] = true;
      }
      yield return null;
    }

    #pragma warning disable 414
    private readonly string TwitchHelpMessage = @"Use !{0} start A1 to start. Use !{0} toggle A1 to toggle the corresponding square. Letters are columns and numbers are rows. Use !{0} submit to submit.";
    #pragma warning restore 414

    IEnumerator ProcessTwitchCommand(string Command) {
      yield return null;
      int Index = 0;
      Command = Command.Trim();
      string[] Parameters = Command.Split(' ');
      if (Parameters[0].ToString().ToLower() != "toggle" && Parameters[0].ToString().ToLower() != "submit" && Parameters[0].ToString().ToLower() != "start") {
        yield return "sendtochaterror Invalid command!";
        yield break;
      }
      if (Parameters[0].ToString().ToLower() == "start" || Parameters[0].ToString().ToLower() == "submit")
        Chungus.OnInteract();
      else if (Parameters[0].ToString().ToLower() == "toggle") {
        for (int i = 1; i < Parameters.Length; i++) {
          if (Parameters[i].Length != 2) {
            yield return "sendtochaterror Invalid command!";
            yield break;
          }
          if (Parameters[i].ToString().ToLower() == "e1") {
            yield return "sendtochaterror That does not exist!";
            yield break;
          }
          else if (Parameters[i][1].ToString() == "1")
            Index += 0;
          else if (Parameters[i][1].ToString() == "2")
            Index += 4;
          else if (Parameters[i][1].ToString() == "3")
            Index += 9;
          else if (Parameters[i][1].ToString() == "4")
            Index += 14;
          else if (Parameters[i][1].ToString() == "5")
            Index += 19;
          else {
            yield return "sendtochaterror Invalid command!";
            yield break;
          }
          if (Parameters[i][0].ToString().ToLower() == "a")
            Index += 1;
          else if (Parameters[i][0].ToString().ToLower() == "b")
            Index += 2;
          else if (Parameters[i][0].ToString().ToLower() == "c")
            Index += 3;
          else if (Parameters[i][0].ToString().ToLower() == "d")
            Index += 4;
          else if (Parameters[i][0].ToString().ToLower() == "e")
            Index += 5;
          Index -= 1;
          FastFoodChains[Index].OnInteract();
          Index &= 0;
        }
      }
    }

    IEnumerator TwitchHandleForcedSolve () {
      if (!Active[0]) {
        Chungus.OnInteract();
        yield return null;
      }
      for (int i = 0; i < 24; i++)
        while (ButtonTrueThing[i] != eXishsTwoTruthsAndALieWillNeverBeFinished[i]) {
          FastFoodChains[i].OnInteract();
          yield return new WaitForSecondsRealtime(.1f);
        }
      Chungus.OnInteract();
    }
}
