using System.Collections;
using System.Linq;
using UnityEngine;
//using System.Diagnostics; I have no clue why you have this, it breaks Jailbreak in Unity

//oh

public class Jailbreak : MonoBehaviour {

   public KMBombInfo Bomb;
   public KMAudio Audio;
   private KMAudio.KMAudioRef ClockSoundForNoLooping;
   public KMSelectable[] Buttons;
   public TextMesh[] DisplayLetters;
   public TextMesh TimerDisplay;
   public TextMesh GuessDisplay;

   int Timer = 90;

   string[] TPProtectedWords = new string[5] { "HELP", "VIEW", "SHOW", "ZOOM", "TILT" };
   string GoalWord = "";
   string InputWord = "";
   string QWERTYAlphabet = "QWERTYUIOPASDFGHJKLZXCVBNM";

   bool CanType;
   bool Check;
   bool IsActive;
   bool Last30Seconds;

#pragma warning disable 0649
   bool TwitchPlaysActive;
#pragma warning restore 0649

   static int moduleIdCounter = 1;
   int moduleId;
   private bool moduleSolved;

   void Awake () {
      moduleId = moduleIdCounter++;

      foreach (KMSelectable Button in Buttons) {
         Button.OnInteract += delegate () { ButtonPress(Button); return false; };
      }
      GetComponent<KMBombModule>().OnActivate += delegate () { Activate(); };
   }

   void Activate () {
      for (int i = 0; i < 4; i++) {
         DisplayLetters[i].text = "_";
      }
      GuessDisplay.text = "Query";
      StartCoroutine(WordGeneration());

      char[] Vowels = { 'A', 'E', 'I', 'O', 'U', 'Y'};
      bool HasVowels = false;

      for (int i = 0; i < WordList.Phrases.Length; i++) { //It's midnight ok i want to sleep
         for (int j = 0; j < 4; j++) {
            for (int k = 0; k < 6; k++) {
               if (WordList.Phrases[i][j] == Vowels[k]) {
                  HasVowels = true;
               }
            }
         }
         if (!HasVowels) {
            Debug.Log(WordList.Phrases[i]);
            HasVowels = false;
         }
      }
   }

   void ButtonPress (KMSelectable Button) {
      Audio.PlaySoundAtTransform("Clack", Button.transform);
      Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, Button.transform);
      if (CanType || moduleSolved) {
         return;
      }
      for (int i = 0; i < 27; i++) {
         if (!IsActive) {
            StartCoroutine(AllTimerFunctions());
            StartCoroutine(TickingSound());
         }
         if (Button == Buttons[i] && i != 26) {
            if (InputWord.Length == 4) {
               return;
            }
            InputWord += QWERTYAlphabet[i].ToString();
            GuessDisplay.text = InputWord;
         }
         else if (InputWord.ToLower() == GoalWord && Button == Buttons[i]) {
            StopAllCoroutines();
            moduleSolved = true;
            StartCoroutine(SolveAnimation());
         }
         else if (Button == Buttons[i] && InputWord.Length < 4) {
            GetComponent<KMBombModule>().HandleStrike();
            Audio.PlaySoundAtTransform("DooDooDooDoo", transform);
            GuessDisplay.text = "Query";
            InputWord = "";
            GuessDisplay.text = InputWord;
         }
         else if (Button == Buttons[i] && i == 26 && InputWord.Length == 4) {
            for (int j = 0; j < WordList.Phrases.Count(); j++) {
               if (InputWord.ToLower() == WordList.Phrases[j]) {
                  Check = true;
                  break;
               }
            }
            for (int j = 0; j < 4; j++) {
               if (InputWord[j].ToString().ToLower() == GoalWord[j].ToString() && Check) {
                  DisplayLetters[j].text = InputWord[j].ToString();
               }
            }
            if (!Check) {
               Debug.LogFormat("[Jailbreak #{0}] You queried {1}, but that is not a word!", moduleId, InputWord);
            }
            else {
               Debug.LogFormat("[Jailbreak #{0}] You queried {1}. It shows {2} {3} {4} {5}.", moduleId, InputWord, DisplayLetters[0].text, DisplayLetters[1].text, DisplayLetters[2].text, DisplayLetters[3].text);
            }
            InputWord = "";
            GuessDisplay.text = "Query";
         }
         Check = false;
      }
   }

   IEnumerator WordGeneration () {
      GoalWord = WordList.Phrases[UnityEngine.Random.Range(0, WordList.Phrases.Count())];
      bool TPCheck = true;
      while (TwitchPlaysActive && TPCheck) {
         TPCheck = false;
         for (int myass = 0; myass < TPProtectedWords.Length; myass++) {
            if (GoalWord.ToUpper() == TPProtectedWords[myass]) {
               TPCheck = true;
            }
         }
         if (TPCheck) {
            GoalWord = WordList.Phrases[UnityEngine.Random.Range(0, WordList.Phrases.Count())];
         }
      }
      Debug.LogFormat("[Jailbreak #{0}] The generated word is {1}.", moduleId, GoalWord);
      yield return null;
   }

   IEnumerator AllTimerFunctions () {
      IsActive = true;
      yield return new WaitForSecondsRealtime(1f);
      Timer--;
      TimerDisplay.text = Timer.ToString();
      if (Timer == 30) {
         Last30Seconds = true;
         ClockSoundForNoLooping = Audio.PlaySoundAtTransformWithRef("clock", transform);
      }
      if (Timer == 0) {
         if (ClockSoundForNoLooping != null) {
            ClockSoundForNoLooping.StopSound();
            ClockSoundForNoLooping = null;
         }
         for (int i = 0; i < 4; i++) {
            DisplayLetters[i].text = GoalWord[i].ToString().ToUpper();
         }
         Debug.LogFormat("[Jailbreak #{0}] You took too long!", moduleId);
         Last30Seconds = false;
         CanType = true;
         yield return new WaitForSecondsRealtime(5f);
         CanType = false;
         //GetComponent<KMBombModule>().HandleStrike();
         GuessDisplay.text = "Query";
         Audio.PlaySoundAtTransform("DooDooDooDoo", transform);
         StartCoroutine(WordGeneration());
         IsActive = false;
         Timer = 90;
         for (int i = 0; i < 4; i++) {
            DisplayLetters[i].text = "_";
         }
      }
      else {
         StartCoroutine(AllTimerFunctions());
      }
   }

   IEnumerator TickingSound () {
      yield return new WaitForSeconds(0.2f);
      Audio.PlaySoundAtTransform("tick", transform);
      if (Last30Seconds) {
         yield return null;
      }
      else {
         StartCoroutine(TickingSound());
      }
   }

   IEnumerator SolveAnimation () {
      if (ClockSoundForNoLooping != null) {
         ClockSoundForNoLooping.StopSound();
         ClockSoundForNoLooping = null;
      }
      for (int j = 0; j < 4; j++) {
         DisplayLetters[j].text = GoalWord[j].ToString().ToUpper();
      }
      yield return new WaitForSeconds(3f);
      if (DisplayLetters[0].text.ToString() != "J") {
         DisplayLetters[0].text = "J";
         yield return new WaitForSeconds(1f);
      }
      if (DisplayLetters[1].text.ToString() != "A") {
         DisplayLetters[1].text = "A";
         yield return new WaitForSeconds(1f);
      }
      if (DisplayLetters[2].text.ToString() != "I") {
         DisplayLetters[2].text = "I";
         yield return new WaitForSeconds(1f);
      }
      if (DisplayLetters[3].text.ToString() != "L") {
         DisplayLetters[3].text = "L";
         yield return new WaitForSeconds(1f);
      }
      GuessDisplay.text = "";
      for (int i = 0; i < 5; i++) {
         GuessDisplay.text += "BROKE"[i].ToString();
         yield return new WaitForSeconds(.2f);
      }
      TimerDisplay.text = "";
      Debug.LogFormat("[Jailbreak #{0}] You guessed the word. Module disarmed.", moduleId);
      GetComponent<KMBombModule>().HandlePass();
      Audio.PlaySoundAtTransform("DooDOodoodoodooDOoooooo", transform);
      moduleSolved = true;
   }

#pragma warning disable 414
   private readonly string TwitchHelpMessage = @"Use !{0} XXXX to guess a word.";
#pragma warning restore 414

   IEnumerator ProcessTwitchCommand (string command) {
      bool[] TPWordValidity = { false, false, false, false };
      yield return null;
      if (command.Length > 4) {
         yield return "sendtochaterror Too big a word!";
      }
      else if (command.Length < 4) {
         GetComponent<KMBombModule>().HandleStrike();
         Audio.PlaySoundAtTransform("DooDooDooDoo", transform);
         yield break;
      }
      else {
         for (int i = 0; i < command.Length; i++) {
            for (int j = 0; j < 26; j++) {
               if (command[i].ToString().ToUpper() == QWERTYAlphabet[j].ToString().ToUpper()) {
                  TPWordValidity[i] = true;
               }
            }
         }
         if (TPWordValidity[0] && TPWordValidity[1] && TPWordValidity[2] && TPWordValidity[3]) {
            for (int i = 0; i < 4; i++) {
               for (int j = 0; j < 26; j++) {
                  if (command[i].ToString().ToUpper() == QWERTYAlphabet[j].ToString().ToUpper()) {
                     Buttons[j].OnInteract();
                     TPWordValidity[i] = false;
                     yield return new WaitForSeconds(.01f);
                  }
               }
            }
            Buttons[26].OnInteract();
            if (command.ToUpper() == GoalWord.ToUpper()) {
               yield return "solve";
            }
         }
         else {
            yield return "sendtochaterror Invalid Character!";
            yield break;
         }
      }
   }

   IEnumerator TwitchHandleForcedSolve () {

      while (!moduleSolved) {
         string AutoGuessWord = "";
         int AutoGuessCorrectLetters = 0;
         AutoGuessWord = WordList.Phrases[UnityEngine.Random.Range(0, WordList.Phrases.Count())];
         if (Timer <= 3) {
            AutoGuessWord = GoalWord;
         }
         for (int i = 0; i < 4; i++) {
            if (DisplayLetters[i].text != "_") {
               AutoGuessCorrectLetters++;
            }
         }
         if (AutoGuessCorrectLetters >= 3) {
            AutoGuessWord = GoalWord;
         }
         for (int i = 0; i < 4; i++) {
            Buttons[QWERTYAlphabet.IndexOf(AutoGuessWord[i].ToString().ToUpper())].OnInteract();
            yield return new WaitForSecondsRealtime(.1f);
         }
         Buttons[26].OnInteract();
      }

      //yield return ProcessTwitchCommand(GoalWord);
   }
}
