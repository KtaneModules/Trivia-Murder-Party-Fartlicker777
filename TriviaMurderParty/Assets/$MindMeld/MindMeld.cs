using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using KModkit;

public class MindMeld : MonoBehaviour {

    public KMBombInfo Bomb;
    public KMAudio Audio;
    public KMSelectable Display;
    public TextMesh Weed;
    public Material[] Colores;
    public GameObject[] FuckYou;

    static int moduleIdCounter = 1;
    int moduleId;
    private bool moduleSolved;

    private string[][] Options = new string[9][] {
      new string[10] {"Hydrogen", "Helium", "Lithium", "Beryllium", "Boron", "Carbon", "Nitrogen", "Oxygen", "Fluorine", "Neon"},
      new string[10] {"Washington", "Adams", "Jefferson", "Madison", "Monroe", "Quincy Adams", "Jackson", "van Buren", "Harrison", "Tyler"},
      new string[10] {"The College Dropout", "Late Registration", "Graduation", "808s and Heartbreak", "My Beautiful Dark Twisted Fantasy", "Yeezus", "The Life of Pablo", "ye", "Jesus is King", "Donda"},
      new string[10] {"Tic Tac Toe", "Follow the Leader", "Friendship", "The Bulb", "Blind Alley", "Rock-Paper-Scissors-Lizard-Spock", "Hexamaze", "Bitmaps", "Colored Squares", "Adjacent Letters"},
      //Can't have the two next to each other
      new string[10] {"Symbolic Coordinates", "Poker", "Sonic the Hedgehog", "Algebra", "The Jukebox", "Identity Parade", "Maintenance", "Mortal Kombat", "LED Grid", "The iPhone"},
      new string[10] {"Child", "Style", "Shake", "Alive", "Axion", "Wreck", "Cause", "Pupil", "Cheat", "Watch"},
      new string[10] {"Sun", "Moon", "Mercury", "Venus", "Mars", "Jupiter", "Saturn", "Uranus", "Neptune", "Pluto"},
      new string[10] {"Ansuz", "Berkana", "Kenaz", "Dagaz", "Ehwaz", "Fehu", "Gebo", "Hagalaz", "Isa", "Jera"},
      new string[10] {"Flags", "Foreign Exchange Rates", "Mortal Kombat", "Street Fighter", "Monsplode, Fight!", "Tax Returns", "Passport Control", "Dungeon", "Dreamcipher", "Garfield Kart"}
    };
    private int[][] GodDamnMulticolored = new int[9][] {
      new int[3] {0, 0, 0},
      new int[3] {0, 0, 0},
      new int[3] {0, 0, 0},
      new int[4] {0, 0, 0, 0},
      new int[4] {0, 0, 0, 0},
      new int[4] {0, 0, 0, 0},
      new int[5] {0, 0, 0, 0, 0},
      new int[5] {0, 0, 0, 0, 0},
      new int[5] {0, 0, 0, 0, 0}
    };
    string[] Titles = {"One of the first ten\nelements on the\nperiodic table", "One of the first ten\npresidents of the\nUnited States", "One of the first ten\nstudio albums by\nKanye West", "One of the first ten\nmodules by Timwi", "One of the first ten\nmodules by\nRoyal_Flu$h", "One of the ten words\nin the top row of\nthe Tap Code chart", "One of the ten planet\nsymbols in Astrology", "One of the first ten\nrunes in\nElder Futhark", "One of Mrs. Kwan's\ntop ten favorite\nmodules"};
    string[] MorseCode = {"-----", ".----", "..---", "...--", "....-", ".....", "-....", "--...", "---..", "----.", ".-", "-...", "-.-.", "-..", ".", "..-.", "--.", "....", "..", ".---", "-.-", ".-..", "--", "-.", "---", ".--.", "--.-", ".-.", "...", "-", "..-", "...-", ".--", "-..-", "-.--", "--.."};
    string[] TapCode = {"65", "16", "26", "36", "46", "56", "61", "62", "63", "64", "11", "12", "13", "14", "15", "21", "22", "23", "24", "25", "66", "31", "32", "33", "34", "35", "41", "42", "43", "44", "45", "51", "52", "53", "54", "55"};
    string[] MorePain = {".-.,.","=","=,,","=,-",".,.-=","-.-",".",",..","=--=,","=.,",",","=-,=",",,.","--.=","-----","-","==.",",--=","-,==","=.,,","=====","=,",",=","=.","-,",",=-"};
    string AlphabetLol = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    string AlphabetLolButWithNumbers = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    string[] Colors = {"Grey", "Green", "White", "Yellow", "Cyan", "Magenta", "Red", "Black", "Brown", "Blue"};

    int ChosenStringNumber = 0;
    int[] Possibilities = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
    string[] InitialOrder = {"", "", "", "", "", "", "", "", "", "", ""};
    string Word = "";
    int FuckINeedAThing = 0;
    int AnswerTo = 0;

    void Awake () {
        moduleId = moduleIdCounter++;
        Display.OnInteract += delegate () { DisplayPress(); return false; };
    }

    void Start () {
      ChosenStringNumber = UnityEngine.Random.Range(0, 9);
      Weed.text = Titles[ChosenStringNumber];
      for (int i = 0; i < 10; i++) {
        InitialOrder[i] = Options[ChosenStringNumber][i];
      }
      GodDamnMulticolored.Shuffle();
      Options[ChosenStringNumber].Shuffle();
      Word = Options[ChosenStringNumber][9];
      GodDamnMulticolored.Shuffle();
      Debug.LogFormat("[Mind Meld #{0}] The chosen category is {1}.", moduleId, (Titles[ChosenStringNumber]).Replace('\n', ' '));
      StartCoroutine(FuckNuts());
      Debug.LogFormat("[Mind Meld #{0}] In reading order, the answers on the cards are {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}.", moduleId, Options[ChosenStringNumber][0], Options[ChosenStringNumber][1], Options[ChosenStringNumber][2], Options[ChosenStringNumber][3],
      Options[ChosenStringNumber][4], Options[ChosenStringNumber][5], Options[ChosenStringNumber][6], Options[ChosenStringNumber][7], Options[ChosenStringNumber][8], Options[ChosenStringNumber][9]);
      Debug.LogFormat("[Mind Meld #{0}] They are encrypted in {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}.", moduleId, Possibilities[0].ToString().Replace("0", "Morse Code").Replace("1", "Tap Code").Replace("2", "More Code").Replace("3", "Reading Order").Replace("4", "Reverse Reading Order").Replace("5", "Multicolored Flashing"), Possibilities[1].ToString().Replace("0", "Morse Code").Replace("1", "Tap Code").Replace("2", "More Code").Replace("3", "Reading Order").Replace("4", "Reverse Reading Order").Replace("5", "Multicolored Flashing"), Possibilities[2].ToString().Replace("0", "Morse Code").Replace("1", "Tap Code").Replace("2", "More Code").Replace("3", "Reading Order").Replace("4", "Reverse Reading Order").Replace("5", "Multicolored Flashing"), Possibilities[3].ToString().Replace("0", "Morse Code").Replace("1", "Tap Code").Replace("2", "More Code").Replace("3", "Reading Order").Replace("4", "Reverse Reading Order").Replace("5", "Multicolored Flashing"), Possibilities[4].ToString().Replace("0", "Morse Code").Replace("1", "Tap Code").Replace("2", "More Code").Replace("3", "Reading Order").Replace("4", "Reverse Reading Order").Replace("5", "Multicolored Flashing"), Possibilities[5].ToString().Replace("0", "Morse Code").Replace("1", "Tap Code").Replace("2", "More Code").Replace("3", "Reading Order").Replace("4", "Reverse Reading Order").Replace("5", "Multicolored Flashing"), Possibilities[6].ToString().Replace("0", "Morse Code").Replace("1", "Tap Code").Replace("2", "More Code").Replace("3", "Reading Order").Replace("4", "Reverse Reading Order").Replace("5", "Multicolored Flashing"), Possibilities[7].ToString().Replace("0", "Morse Code").Replace("1", "Tap Code").Replace("2", "More Code").Replace("3", "Reading Order").Replace("4", "Reverse Reading Order").Replace("5", "Multicolored Flashing"), Possibilities[8].ToString().Replace("0", "Morse Code").Replace("1", "Tap Code").Replace("2", "More Code").Replace("3", "Reading Order").Replace("4", "Reverse Reading Order").Replace("5", "Multicolored Flashing"));
      Debug.LogFormat("[Mind Meld #{0}] The chosen word was {1}.", moduleId, Word);
      for (int i = 0; i < 10; i++) {
        if (Word == InitialOrder[i]) {
          AnswerTo = (i + 1) % 10;
        }
      }
      Debug.LogFormat("[Mind Meld #{0}] Press on a {1}.", moduleId, AnswerTo);
    }

    IEnumerator FuckNuts () {
      for (int i = 0; i < 9; i++) {
        Encryptor(i);
        yield return new WaitForSeconds(1f);
      }
    }

    void DisplayPress () {
      if ((Bomb.GetTime() % 10) - (Bomb.GetTime() % 1) == AnswerTo) {
        GetComponent<KMBombModule>().HandlePass();
      }
      else {
        GetComponent<KMBombModule>().HandleStrike();
      }
    }

    void Encryptor (int BigBigChungus) {
      if (Options[ChosenStringNumber][BigBigChungus] == InitialOrder[BigBigChungus]) {
        Possibilities[BigBigChungus] = 3;
      }
      else if (11 - Array.IndexOf(Options[ChosenStringNumber], Options[ChosenStringNumber][BigBigChungus]) == Array.IndexOf(InitialOrder, InitialOrder[BigBigChungus])) {
        Possibilities[BigBigChungus] = 4;
      }
      if (Options[ChosenStringNumber][BigBigChungus] == "808s and Heartbreak") {
        Possibilities[BigBigChungus] = UnityEngine.Random.Range(0,6);
        while (Possibilities[BigBigChungus] == 2 || Possibilities[BigBigChungus] == 3 || Possibilities[BigBigChungus] == 4) {
          Possibilities[BigBigChungus] = UnityEngine.Random.Range(0,6);
        }
      }
      if ((Options[ChosenStringNumber][BigBigChungus][0] == 'T' && Options[ChosenStringNumber][BigBigChungus][1] == 'h') || (Options[ChosenStringNumber][BigBigChungus][0] == 'y' && Options[ChosenStringNumber][BigBigChungus][1] == 'e')) {
        Possibilities[BigBigChungus] = 5;
      }
      else {
        Possibilities[BigBigChungus] = UnityEngine.Random.Range(0,6);
        while (Possibilities[BigBigChungus] == 3 || Possibilities[BigBigChungus] == 4) {
          Possibilities[BigBigChungus] = UnityEngine.Random.Range(0,6);
        }
      }
      StartCoroutine(Flashing(BigBigChungus));
    }

    IEnumerator Flashing (int BigBigChungus) {
      while (true) {
        switch (Possibilities[BigBigChungus]) {
          case 0://Morse
          StartOfMorse:
          for (int i = 0; i < 3; i++) {
            for (int j = 0; j < AlphabetLolButWithNumbers.Length; j++) {
              if (Options[ChosenStringNumber][BigBigChungus][i].ToString().ToUpper() == AlphabetLolButWithNumbers[j].ToString().ToUpper()) {
                for (int x = 0; x < MorseCode[j].Length; x++) {
                  if (MorseCode[j][x] == '.') {
                    FuckYou[BigBigChungus].GetComponent<MeshRenderer>().material = Colores[6];
                    yield return new WaitForSeconds(.2f);
                    FuckYou[BigBigChungus].GetComponent<MeshRenderer>().material = Colores[7];//black
                    yield return new WaitForSeconds(.2f);
                  }
                  else {
                    FuckYou[BigBigChungus].GetComponent<MeshRenderer>().material = Colores[6];
                    yield return new WaitForSeconds(.6f);
                    FuckYou[BigBigChungus].GetComponent<MeshRenderer>().material = Colores[7];//black
                    yield return new WaitForSeconds(.2f);
                  }
                }
              }
            }
            yield return new WaitForSeconds(.6f);
          }
          goto StartOfMorse;
          break;
          case 1://Tap
          StartOfTap:
          for (int i = 0; i < 3; i++) {
            for (int j = 0; j < AlphabetLolButWithNumbers.Length; j++) {
              if (Options[ChosenStringNumber][BigBigChungus][i].ToString().ToUpper() == AlphabetLolButWithNumbers[j].ToString().ToUpper()) {
                for (int x = 0; x < 2; x++) {
                  for (int p = 0; p < int.Parse(TapCode[j][x].ToString()); p++) {
                    FuckYou[BigBigChungus].GetComponent<MeshRenderer>().material = Colores[9];
                    yield return new WaitForSeconds(.2f);
                    FuckYou[BigBigChungus].GetComponent<MeshRenderer>().material = Colores[7];//black
                    yield return new WaitForSeconds(.2f);
                  }
                  yield return new WaitForSeconds(.4f);
                }
              }
            }
            yield return new WaitForSeconds(.4f);
          }
          yield return new WaitForSeconds(1f);
          goto StartOfTap;
          break;
          case 2://More
          StartOfMore:
          for (int i = 0; i < 3; i++) {
            for (int j = 0; j < AlphabetLol.Length; j++) {
              if (Options[ChosenStringNumber][BigBigChungus][i].ToString().ToUpper() == AlphabetLol[j].ToString().ToUpper()) {
                for (int x = 0; x < MorePain[j].Length; x++) {
                  if (MorePain[j][x] == ',') {
                    FuckYou[BigBigChungus].GetComponent<MeshRenderer>().material = Colores[1];
                    yield return new WaitForSeconds(.2f);
                    FuckYou[BigBigChungus].GetComponent<MeshRenderer>().material = Colores[7];//black
                    yield return new WaitForSeconds(1f);
                  }
                  else if (MorePain[j][x] == '.') {
                    FuckYou[BigBigChungus].GetComponent<MeshRenderer>().material = Colores[1];
                    yield return new WaitForSeconds(.6f);
                    FuckYou[BigBigChungus].GetComponent<MeshRenderer>().material = Colores[7];//black
                    yield return new WaitForSeconds(1f);
                  }
                  else if (MorePain[j][x] == '=') {
                    FuckYou[BigBigChungus].GetComponent<MeshRenderer>().material = Colores[1];
                    yield return new WaitForSeconds(1f);
                    FuckYou[BigBigChungus].GetComponent<MeshRenderer>().material = Colores[7];//black
                    yield return new WaitForSeconds(1f);
                  }
                  else if (MorePain[j][x] == '-') {
                    FuckYou[BigBigChungus].GetComponent<MeshRenderer>().material = Colores[1];
                    yield return new WaitForSeconds(3f);
                    FuckYou[BigBigChungus].GetComponent<MeshRenderer>().material = Colores[7];//black
                    yield return new WaitForSeconds(1f);
                  }
                }
              }
            }
            yield return new WaitForSeconds(1f);
          }
          goto StartOfMore;
          break;
          case 3://White
          FuckYou[BigBigChungus].GetComponent<MeshRenderer>().material = Colores[2];
          break;
          case 4://Black
          FuckYou[BigBigChungus].GetComponent<MeshRenderer>().material = Colores[7];
          break;
          case 5://Gamer
          for (int i = 0; i < GodDamnMulticolored[BigBigChungus].Length; i++) {
            GodDamnMulticolored[BigBigChungus][i] = UnityEngine.Random.Range(0, 10);
          }
          Debug.Log(Array.IndexOf(Options[ChosenStringNumber], Options[ChosenStringNumber][BigBigChungus]));
          for (int i = 0; i < InitialOrder.Length; i++) {
            if (Array.IndexOf(Options[ChosenStringNumber], Options[ChosenStringNumber][BigBigChungus]) == Array.IndexOf(InitialOrder, InitialOrder[i])) {
              FuckINeedAThing = i;
              break;
            }
          }
          for (int i = 0; i < GodDamnMulticolored[BigBigChungus].Length; i++) {
            if (i + 1 == GodDamnMulticolored[BigBigChungus].Length) {
              int shuthjfdnlkjafnalkjnf = 0;
              for (int j = 0; j < GodDamnMulticolored[BigBigChungus].Length; j++) {
                shuthjfdnlkjafnalkjnf += GodDamnMulticolored[BigBigChungus][j];
                shuthjfdnlkjafnalkjnf %= 10;
              }
              FuckINeedAThing -= shuthjfdnlkjafnalkjnf;
              if (FuckINeedAThing < 0) {
                FuckINeedAThing += 10;
              }
              GodDamnMulticolored[BigBigChungus][i] = FuckINeedAThing;
            }
            else {
              GodDamnMulticolored[BigBigChungus][i] = UnityEngine.Random.Range(0,10);
              if (i != 0) {
                while (GodDamnMulticolored[BigBigChungus][i] == GodDamnMulticolored[BigBigChungus][i - 1]) {
                  GodDamnMulticolored[BigBigChungus][i] = UnityEngine.Random.Range(0,10);
                }
              }
            }
          }
          Debug.LogFormat("[Mind Meld #{0}] Card #{1} is flashing:", moduleId, BigBigChungus + 1);
          for (int i = 0; i < GodDamnMulticolored[BigBigChungus].Length; i++) {
            Debug.LogFormat("[Mind Meld #{0}] {1}", moduleId, Colors[GodDamnMulticolored[BigBigChungus][i]]);
          }
          gamered:
          for (int i = 0; i < GodDamnMulticolored[BigBigChungus].Length; i++) {
            FuckYou[BigBigChungus].GetComponent<MeshRenderer>().material = Colores[GodDamnMulticolored[BigBigChungus][i]];
            yield return new WaitForSeconds(.4f);
          }
          FuckYou[BigBigChungus].GetComponent<MeshRenderer>().material = Colores[10];
          yield return new WaitForSeconds(1f);
          goto gamered;
          break;
        }
      }
    }
}
