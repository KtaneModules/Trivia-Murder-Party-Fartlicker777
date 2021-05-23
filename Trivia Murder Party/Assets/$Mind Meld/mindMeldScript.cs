using System.Collections;
using UnityEngine;
using System.Linq;
using System.Text;
using System;
using Random = UnityEngine.Random;

public class mindMeldScript : MonoBehaviour {

   public KMBombModule Module;
   public KMBombInfo Info;
   public KMAudio Audio;
   public KMSelectable[] cardSelectables;
   public KMSelectable buttonSelectable;
   public Material[] cardMats, buttonMats;
   public Material blankMat;
   public MeshRenderer[] cardRenderers;
   public MeshRenderer buttonRenderer;
   public Transform[] spriteTransforms;
   public Transform moduleTransform;

   private static int _moduleIdCounter = 1;
   private int _moduleId;
   private bool solved;
   private bool inSubmissionMode = false;
   private bool redealAnimationPlaying, strikeAnimationPlaying, solveAnimationPlaying = false;
   private bool revealed = false;

   private int[] cardPositions = { 0, 1, 2, 3, 4 }; // 0 = circle, 1 = cross, 2 = waves, 3 = square, 4 = star
   private int categoryNumber;
   private string[] cards = { "", "", "", "", "" };
   private int answerCard = 0;

   private int[] playerPositions = { 8, 8, 8, 8 }; // 0 = redherring, 1 = alpha, 2 = screamer, 3 = sheriff, 4 = believer, 5 = jester, 6, lovers, 7 = nerd
   private Vector3[] originalPositions;
   private int[] playerRules = { 0, 1, 2, 3 }; // 0 = alphabetical, 1 = given order, 2 = length, 3 = vowels
   private bool playerOrder = false; // false = descending, true = ascending
   private int[] playerCards = { 0, 0, 0, 0 };

   private readonly string[] cardNames = { "circle", "cross", "waves", "square", "star" };
   private readonly string[] categoryNames = { "---", "Five-Man Band trope parts", "RPSLS moves", "Scooby-Doo characters", "Incredibles", "Ultracube directions", "---", "Zener cards", "Battleship ships", "Security Council P5", "stages of grief", "LGBTQ", "---", "NYC boroughs", "fingers", "US Army branches", "golf scores below par", "Great Lakes", "---", "angles", "senses", "tastes", "Simpsons", "Bop It Extreme", "---" };
   private readonly string[,] categories =
   {
        { "PLAINS", "ISLAND", "SWAMP", "MOUNTAIN", "FOREST" }, // unused
        { "LEADER", "LANCER", "SMARTGUY", "BIGGUY", "CHICK" },
        { "ROCK", "PAPER", "SCISSORS", "LIZARD", "SPOCK" },
        { "FRED", "DAPHNE", "SCOOBY", "SHAGGY", "VELMA" },
        { "MRINCREDIBLE", "ELASTIGIRL", "VIOLET", "DASH", "JACKJACK" },
        { "FRONT", "BOTTOM", "LEFT", "ZIG", "PING" },
        { "HUNT", "HUSTLE", "HUMP", "PARCEL", "PLETHORA" }, // unsued
        { "CIRCLE", "CROSS", "WAVES", "SQUARE", "STAR" },
        { "CARRIER", "BATTLESHIP", "SUBMARINE", "DESTROYER", "PATROLBOAT" },
        { "UNITEDSTATES", "UNITEDKINGDOM", "RUSSIA", "FRANCE", "CHINA" },
        { "DENIAL", "ANGER", "BARGAINING", "DEPRESSION", "ACCEPTANCE" },
        { "LESBIAN", "GAY", "BI", "TRANS", "QUEER" },
        { "SCARY", "SPORTY", "BABY", "GINGER", "POSH" }, // unused
        { "BROOKLYN", "BRONX", "QUEENS", "MANHATTAN", "STATENISLAND" },
        { "THUMB", "INDEX", "MIDDLE", "RING", "PINKY" },
        { "AIRFORCE", "COASTGUARD", "ARMY", "NAVY", "MARINECORPS" },
        { "BIRDIE", "EAGLE", "ALBATROSS", "CONDOR", "OSTRICH" },
        { "HURON", "ONTARIO", "MICHIGAN", "ERIE", "SUPERIOR" },
        { "WHO", "WHAT", "WHERE", "WHEN", "WHY" }, // me me when me when me when me when me when me when me when me when me when when unused
        { "ACUTE", "RIGHT", "OBTUSE", "STRAIGHT", "REFLEX" },
        { "SIGHT", "HEARING", "TASTE", "SMELL", "TOUCH" },
        { "SWEET", "SALTY", "SOUR", "BITTER", "UMAMI" },
        { "HOMER", "MARGE", "BART", "LISA", "MAGGIE" },
        { "BOPIT", "PULLIT", "TWISTIT", "FLICKIT", "SPINIT" },
        { "TROPOSPHERE", "STRATOSPHERE", "MESOSPHERE", "THERMOSPHERE", "EXOSPHERE" }, // unused
    };
   private readonly string[] playerNames = { "red herring", "alpha", "screamer", "sheriff", "believer", "jester", "lovers", "nerd", "" };
   private readonly string[] ruleNames = { "alphabetical order", "given order", "length", "number of vowels" };

   // Use this for initialization
   void Start () {
      _moduleId = _moduleIdCounter++;
      for (int i = 0; i < spriteTransforms.Length; i++) {
         spriteTransforms[i].gameObject.SetActive(false);
      }

      originalPositions = spriteTransforms.Select(tx => tx.transform.localPosition).ToArray();
      Module.OnActivate += Activate;
   }

   void Activate () {
      for (int i = 0; i < cardSelectables.Length; i++) {
         int j = i;

         cardSelectables[j].OnInteract += delegate () {
            if (!solved && !redealAnimationPlaying && !strikeAnimationPlaying && !solveAnimationPlaying)
               PressCard(j);
            cardSelectables[j].AddInteractionPunch();
            // add an audio file
            return false;
         };
      }

      buttonSelectable.OnInteract += delegate () {
         if (!solved && !redealAnimationPlaying && !strikeAnimationPlaying && !solveAnimationPlaying)
            PressButton();
         buttonSelectable.AddInteractionPunch();
         // add an audio file
         return false;
      };

      GenerateModule();
   }

   void GenerateModule () {
      playerRules = playerRules.Shuffle().ToArray(); // generate rules for the players to follow
      if (Random.Range(0, 2) == 0)
         playerOrder = true;
      for (int i = 0; i < 4; i++) {
         var placeholder = Random.Range(0, 8);
         while (playerPositions.Contains(placeholder)) {
            placeholder = Random.Range(0, 8);
         }

         playerPositions[i] = placeholder;
         if (playerOrder)
            DebugMsg("The " + playerNames[playerPositions[i]] + " picks in " + ruleNames[playerRules[i]] + " (ascending).");
         else
            DebugMsg("The " + playerNames[playerPositions[i]] + " picks in " + ruleNames[playerRules[i]] + " (descending).");

      }


      while (!((playerPositions[0] < playerPositions[1]) && (playerPositions[1] < playerPositions[2]) && (playerPositions[2] < playerPositions[3]))) { // bogo sort
         playerPositions = playerPositions.Shuffle().ToArray();
      }

      GenerateCards(); // generate cards
   }

   void GenerateCards () {
      cards = new string[5] { "", "", "", "", "" };
      cardPositions = cardPositions.Shuffle().ToArray();
      DebugMsg("The cards, in reading order, are " + cardNames[cardPositions[0]] + ", " + cardNames[cardPositions[1]] + ", " + cardNames[cardPositions[2]] + ", " + cardNames[cardPositions[3]] + ", and " + cardNames[cardPositions[0]] + ".");
      categoryNumber = cardPositions[4] * 5 + cardPositions[0];
      DebugMsg("The category is " + categoryNames[categoryNumber] + ".");
      StartCoroutine(Redeal());
   }

   int PickACard (int rule) {
      var chosenCard = 0;
      while (cards[chosenCard] == "") {
         chosenCard++;
         if (chosenCard >= 5)
            break;
      }
      switch (rule) {
         case 0:
            for (int i = 1; i < 5; i++) {
               while (cards[i] == "" || cards[i] == cards[chosenCard]) {
                  i++;

                  if (i >= 5)
                     break;
               }

               if (i >= 5)
                  break;

               var offset1 = 0;
               var offset2 = 0;
               var letter1 = cards[i][0];
               var letter2 = cards[chosenCard][0];

               while (letter1 == letter2) {
                  offset1++;
                  offset2++;
                  letter1 = cards[i][offset1];
                  letter2 = cards[chosenCard][offset2];
               }

               if ((Encoding.ASCII.GetBytes(letter1.ToString()).First() < Encoding.ASCII.GetBytes(letter2.ToString()).First() && playerOrder) || (Encoding.ASCII.GetBytes(letter1.ToString()).First() > Encoding.ASCII.GetBytes(letter2.ToString()).First() && !playerOrder))
                  chosenCard = i;
            }
            break;
         case 1:
            for (int i = 0; i < 5; i++) {
               if (cards.Contains(categories[categoryNumber, i]) && playerOrder) {
                  chosenCard = Array.IndexOf(cards, categories[categoryNumber, i]);
                  break;
               }
               else if (cards.Contains(categories[categoryNumber, 4 - i]) && !playerOrder) {
                  chosenCard = Array.IndexOf(cards, categories[categoryNumber, 4 - i]);
                  break;
               }
            }
            break;
         case 2:
            for (int i = 1; i < 5; i++) {
               while (cards[i] == "" || cards[i] == cards[chosenCard]) {
                  i++;

                  if (i >= 5)
                     break;
               }

               if (i >= 5)
                  break;

               if ((cards[i].Length < cards[chosenCard].Length && playerOrder) || (cards[i].Length > cards[chosenCard].Length && !playerOrder)) {
                  chosenCard = i;
               }

               else if (cards[i].Length == cards[chosenCard].Length) {
                  var offset1 = 0;
                  var offset2 = 0;
                  var letter1 = cards[i][offset1];
                  var letter2 = cards[chosenCard][0];

                  while (letter1 == letter2) {
                     offset1++;
                     offset2++;
                     letter1 = cards[i][offset1];
                     letter2 = cards[chosenCard][offset2];
                  }

                  if (Encoding.ASCII.GetBytes(letter1.ToString()).First() < Encoding.ASCII.GetBytes(letter2.ToString()).First())
                     chosenCard = i;
               }
            }
            break;
         case 3:
            for (int i = 1; i < 5; i++) {
               while (cards[i] == "" || cards[i] == cards[chosenCard]) {
                  i++;

                  if (i >= 5)
                     break;
               }

               if (i >= 5)
                  break;

               if ((cards[i].Count(x => x.EqualsAny('A', 'E', 'I', 'O', 'U')) < cards[chosenCard].Count(x => x.EqualsAny('A', 'E', 'I', 'O', 'U')) && playerOrder) || (cards[i].Count(x => x.EqualsAny('A', 'E', 'I', 'O', 'U')) > cards[chosenCard].Count(x => x.EqualsAny('A', 'E', 'I', 'O', 'U')) && !playerOrder))
                  chosenCard = i;
               else if (cards[i].Count(x => x.EqualsAny('A', 'E', 'I', 'O', 'U')) == cards[chosenCard].Count(x => x.EqualsAny('A', 'E', 'I', 'O', 'U'))) {
                  var offset1 = 0;
                  var offset2 = 0;
                  var letter1 = cards[i][offset1];
                  var letter2 = cards[chosenCard][0];

                  while (letter1 == letter2) {
                     offset1++;
                     offset2++;
                     letter1 = cards[i][offset1];
                     letter2 = cards[chosenCard][offset2];
                  }

                  if (Encoding.ASCII.GetBytes(letter1.ToString()).First() < Encoding.ASCII.GetBytes(letter2.ToString()).First())
                     chosenCard = i;
               }
            }
            break;
         default:
            for (int i = 0; i < 5; i++) {
               if (cards[i] != "") {
                  chosenCard = i;
                  break;
               }
            }
            break;
      }

      return chosenCard;
   }

   void PressCard (int cardNumber) {
      if (inSubmissionMode) {
         DebugMsg("Submitting...");
         if (cardNumber == answerCard) {
            StartCoroutine(Solve());
            solved = true;
         }

         else
            StartCoroutine(Strike());

         inSubmissionMode = false;
      }

      else if (revealed) {
         DebugMsg("Resetting...");
         revealed = false;

         for (int i = 0; i < 4; i++)
            spriteTransforms[playerPositions[i]].gameObject.SetActive(false);

         GenerateCards();
      }

      else {
         DebugMsg("Revealing...");
         revealed = true;
         Audio.PlaySoundAtTransform("click", Module.transform);
         for (int i = 0; i < 5; i++)
            cardRenderers[i].material = blankMat;

         for (int i = 0; i < 4; i++) {
            spriteTransforms[playerPositions[i]].gameObject.SetActive(true);
            spriteTransforms[playerPositions[i]].localPosition = originalPositions[playerCards[i]];
         }
      }
   }

   void PressButton () {
      if (inSubmissionMode) {
         buttonRenderer.material = buttonMats[0];
         inSubmissionMode = false;
         DebugMsg("Exiting submission mode...");
      }

      else {
         buttonRenderer.material = buttonMats[1];
         inSubmissionMode = true;
         DebugMsg("Entering submission mode...");

         revealed = false;

      }

      for (int i = 0; i < 4; i++) {
         spriteTransforms[playerPositions[i]].gameObject.SetActive(false);
      }

      GenerateCards();
   }

   IEnumerator Strike () {
      strikeAnimationPlaying = true;
      revealed = true;
      Audio.PlaySoundAtTransform("click", Module.transform);
      Audio.PlaySoundAtTransform("fuckyou", Module.transform);

      yield return new WaitForSeconds(.7f);
      buttonRenderer.material = buttonMats[2];
      yield return new WaitForSeconds(.4f);
      for (int i = 0; i < 5; i++)
         cardRenderers[i].material = blankMat;
      for (int i = 0; i < 4; i++) {
         spriteTransforms[playerPositions[i]].gameObject.SetActive(true);
         spriteTransforms[playerPositions[i]].localPosition = originalPositions[playerCards[i]];
      }

      yield return new WaitForSeconds(.9f);
      yield return new WaitForSeconds(.25f);
      for (int i = 0; i < 4; i++)
         spriteTransforms[playerPositions[i]].gameObject.SetActive(false);
      GenerateCards();
      yield return new WaitForSeconds(2f);
      if (Random.Range(0, 20) == 0)
         Audio.PlaySoundAtTransform("blan", Module.transform);
      else
         Audio.PlaySoundAtTransform("scream", Module.transform);
      Module.HandleStrike();

      buttonRenderer.material = buttonMats[0];
      inSubmissionMode = false;
      DebugMsg("Exiting submission mode...");
      revealed = false;
      strikeAnimationPlaying = false;

   }
   IEnumerator Solve () {
      solveAnimationPlaying = true;
      revealed = true;
      Audio.PlaySoundAtTransform("click", Module.transform);
      Audio.PlaySoundAtTransform("fuckyeah", Module.transform);

      yield return new WaitForSeconds(.6f);
      buttonRenderer.material = buttonMats[2];
      for (int i = 0; i < 5; i++)
         cardRenderers[i].material = blankMat;
      for (int i = 0; i < 4; i++) {
         spriteTransforms[playerPositions[i]].gameObject.SetActive(true);
         spriteTransforms[playerPositions[i]].localPosition = originalPositions[playerCards[i]];
      }
      yield return new WaitForSeconds(.6f);
      Module.HandlePass();
      yield return new WaitForSeconds(1f);
      for (int i = 0; i < 4; i++)
         spriteTransforms[playerPositions[i]].gameObject.SetActive(false);

      yield return new WaitForSeconds(.5f);
      buttonRenderer.material = buttonMats[1];
      inSubmissionMode = false;
      DebugMsg("Exiting submission mode...");
      revealed = false;
      solveAnimationPlaying = false;
   }
   IEnumerator Redeal () {
      redealAnimationPlaying = true;
      for (int i = 0; i < cardRenderers.Length; i++)
         cardRenderers[i].material = blankMat;

      for (int i = 0; i < cardRenderers.Length; i++) {
         yield return new WaitForSeconds(.1f);
         cardRenderers[i].material = cardMats[cardPositions[i]];
         cards[i] = categories[categoryNumber, cardPositions[i]];
         Audio.PlaySoundAtTransform("card" + Random.Range(1, 11).ToString(), Module.transform);
      }

      for (int i = 0; i < 4; i++) {
         playerCards[i] = PickACard(playerRules[i]);

         DebugMsg("The " + playerNames[playerPositions[i]] + " picked the " + cards[playerCards[i]] + " card");
         cards[playerCards[i]] = "";
      }

      answerCard = PickACard(3);
      DebugMsg("You should pick the " + cards[answerCard] + " card (which is #" + (answerCard + 1) + ").");

      redealAnimationPlaying = false;
   }

   void DebugMsg (string msg) {
      Debug.LogFormat("[Mind Meld #{0}] {1}", _moduleId, msg);
   }

#pragma warning disable 414
   private readonly string TwitchHelpMessage = @"Use !{0} 0/1/2/3/4 to press those cards. Use !{0} submit to toggle the submit button.";
#pragma warning restore 414

   IEnumerator ProcessTwitchCommand (string Command) {
      Command = Command.Trim().ToUpper();
      yield return null;
      if ("01234".Contains(Command)) {
         cardSelectables[int.Parse(Command)].OnInteract();
      }
      else if (Command == "SUBMIT") {
         buttonSelectable.OnInteract();
      }
      else if (Command == "BLAN") {
         Audio.PlaySoundAtTransform("blan", Module.transform);
      }
      else {
         yield return "sendtochaterror I DON'T UNDERSTAND I AM SORRY;";
      }
   }

   IEnumerator TwitchHandleForcedSolve () {
      if (!inSubmissionMode) {
         buttonSelectable.OnInteract();
      }
      while (redealAnimationPlaying) {
         yield return true;
      }
      yield return ProcessTwitchCommand(answerCard.ToString());
   }
}
