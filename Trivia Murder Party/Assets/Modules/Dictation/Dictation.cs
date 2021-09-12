using System.Collections;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public class Dictation : MonoBehaviour {

   public KMBombInfo Bomb;
   public KMAudio Audio;
   public KMSelectable[] TypableButtons;
   public KMSelectable StartButton;
   public KMSelectable ReturnButton;
   public TextMesh NumberDisplay;
   public TextMesh DisplayText;
   public GameObject TVStatusSphere;
   public Material[] StatusColors;

   int DetonationNoiseCounter;
   int IndexOfSubmissionWord;
   int PhraseIndex;
   int RandomPhraseSubmissionIndex;

   float Fanfare;
   float DefaultTimePerWord = 0.05f;

   string QWERTYAlphabet = "QWERTYUIOPASDFGHJKLZXCVBNM<";
   private string[][] PhraseList = new string[][] {
      new string[] {"I" , "AM" , "SO" , "SORRY" , "FOR" , "BEING" , "AGGRESSIVE" , "ON" , "THE" , "TUTORIAL" , "I" , "dont" , "understand" , "how" , "it" , "worked" , "like" , "that" , "bus" , "was" , "way" , "too" , "scary" , "for" , "me" , "to" , "watch" , "and" , "I" , "just" , "felt" , "like" , "I" , "dont" , "know" , "what" , "I" , "am" , "watching" , "anymore" , "ACTUALLY" , "FUCK" , "IT" , "IM" , "MAD" , "I" , "GOT" , "GROUNDED" , "AFTER" , "I" , "PUNCHED" , "A" , "WHOLE" , "IN" , "MY" , "WALL" , "AFTER" , "WATCHING" , "YOUR" , "TUTORIALS" , "I" , "sorry" , "I" , "need" , "to" , "take" , "drugs" , "just" , "like" , "mom" , "says" , "and" , "no" , "more" , "Dr" , "Pepper" , "That" , "was" , "mean" , "and" , "I" , "dont" , "understand" , "it" , "please" , "make" , "sure" , "to" , "remake" , "this" , "tutorial" , "just" , "like" , "all" , "of" , "your" , "other" , "tutorials" , "Bye"},
      new string[] {"Dear" , "loyal" , "subscribers" , "I" , "need" , "to" , "apologize" , "for" , "what" , "I" , "did" , "I" , "am" , "sorry" , "for" , "saying" , "you" , "cant" , "see" , "me" , "while" , "at" , "a" , "school" , "for" , "the" , "blind" , "The" , "goal" , "of" , "my" , "content" , "is" , "always" , "to" , "entertain" , "Im" , "sorry" , "for" , "not" , "feeding" , "my" , "dogs" , "for" , "a" , "week" , "just" , "so" , "I" , "could" , "film" , "them" , "eating" , "each" , "other" , "The" , "goal" , "of" , "my" , "content" , "is" , "always" , "to" , "entertain"},
      new string[] {"Hello" , "Hello" , "hello" , "I" , "wanted" , "to" , "record" , "a" , "message" , "for" , "you" , "to" , "help" , "you" , "get" , "settled" , "in" , "on" , "your" , "first" , "night" , "Um" , "I" , "actually" , "worked" , "in" , "that" , "office" , "before" , "you" , "Im" , "finishing" , "up" , "my" , "last" , "week" , "now" , "as" , "a" , "matter" , "of" , "fact" , "so" , "I" , "know" , "it" , "can" , "be" , "a" , "little" , "overwhelming" , "but" , "Im" , "here" , "to" , "tell" , "you" , "theres" , "nothing" , "to" , "worry" , "about" , "uhh" , "youll" , "do" , "fine" , "So" , "lets" , "just" , "focus" , "on" , "getting" , "you" , "through" , "your" , "first" , "week" , "Ok" , "lets" , "see" , "First" , "theres" , "an" , "introductory" , "greeting" , "from" , "the" , "company" , "Im" , "supposed" , "to" , "read"},
      new string[] {"Dear" , "Local" , "News" , "Broadcasting" , "Team" , "I" , "have" , "punched" , "a" , "small" , "child" , "between" , "the" , "eyes" , "and" , "he" , "became" , "blind" , "I" , "would" , "like" , "to" , "say" , "Im" , "sorry" , "for" , "my" , "actions" , "that" , "day" , "Sincerely" , "God"},
      new string[] {"Hello" , "there" , "everyone" , "my" , "name" , "is" , "Crazy" , "Caleb" , "and" , "today" , "we" , "will" , "be" , "taking" , "a" , "look" , "at" , "my" , "wife" , "The" , "Jewel" , "Vault" , "So" , "for" , "those" , "of" , "you" , "who" , "are" , "new" , "to" , "the" , "channel" , "I" , "have" , "an" , "obsession" , "with" , "this" , "module" , "and" , "I" , "have" , "never" , "struck" , "on" , "it" , "ever" , "Thank" , "you" , "guys" , "for" , "watching" , "and" , "remember" , "to" , "Stay" , "Crazy" , "Stay" , "Cool" , "See" , "you" , "guys" , "next" , "time"},
      new string[] {"League" , "of" , "Legends" , "is" , "truly" , "a" , "fun" , "game" , "sometimes" , "One" , "of" , "the" , "best" , "times" , "is" , "playing" , "as" , "Blitzcrank" , "where" , "you" , "can" , "hook" , "people" , "over" , "walls" , "and" , "bring" , "them" , "back" , "for" , "easy" , "kills" , "It" , "really" , "makes" , "you" , "feel" , "like" , "youre" , "impacting" , "the" , "game" , "without" , "needing" , "crazy" , "mechanics"},
      new string[] {"I" , "was" , "nervous" , "the" , "first" , "time" , "but" , "then" , "again" , "I" , "think" , "everyone" , "is" , "It" , "was" , "one" , "thing" , "to" , "do" , "this" , "alone" , "practicing" , "in" , "my" , "room" , "but" , "another" , "when" , "someone" , "else" , "is" , "there" , "There" , "was" , "so" , "much" , "to" , "think" , "about" , "rhythm" , "pace" , "mood" , "working" , "ones" , "way" , "up" , "to" , "the" , "crescendo" , "but" , "not" , "too" , "quickly" , "I" , "worried" , "I" , "wouldnt" , "be" , "able" , "to" , "perform" , "but" , "she" , "made" , "me" , "feel" , "so" , "comfortable" , "and" , "relaxed" , "It" , "was" , "then" , "that" , "I" , "knew" , "I" , "could" , "DJ" , "for" , "any" , "number" , "of" , "people"},
      new string[] {"Please" , "help" , "I" , "tried" , "to" , "put" , "my" , "dick" , "in" , "a" , "Black" , "Hole" , "module" , "and" , "now" , "its" , "stuck" , "I" , "cant" , "move" , "oh" , "god" , "This" , "really" , "gives" , "a" , "new" , "meaning" , "to" , "blowing" , "up" , "haha" , "Help" , "it" , "hurts" , "I" , "dont" , "wanna" , "die" , "like" , "this" , "please" , "someone" , "help" , "why" , "me"},
      new string[] {"The" , "moderation" , "team" , "has" , "seen" , "you" , "telling" , "people" , "to" , "shut" , "the" , "fuck" , "up" , "in" , "text" , "chats" , "We" , "do" , "not" , "tolerate" , "that" , "aggression" , "You" , "are" , "being" , "given" , "your" , "second" , "warning"},
      new string[] {"Write" , "a" , "few" , "sentences" , "about" , "anything" , "as" , "long" , "as" , "the" , "total" , "message" , "length" , "isnt" , "above" , "one", "hundred" , "words" , "and" , "the" , "sentences" , "are" , "coherent" , "Swearing" , "is" , "allowed" , "although" , "please" , "make" , "it" , "minimal" , "Your" , "name" , "will" , "not" , "be" , "recorded" , "but" , "I" , "ask" , "so" , "I" , "dont" , "get" , "unwanted" , "duplicates"},
      new string[] {"I" , "am" , "done" , "I" , "am" , "through" , "I" , "am" , "just" , "sick" , "of" , "it" , "all" , "I" , "am" , "tired" , "of" , "the" , "feeders" , "in" , "my" , "stupid" , "game" , "of" , "LoL" , "Uninstall" , "right" , "away" , "with" , "your" , "zero" , "sixteen" , "four" , "Youre" , "the" , "reason" , "why" , "I" , "never" , "want" , "to" , "play" , "this" , "any" , "more"},
      new string[] {"Were" , "no" , "strangers" , "to" , "love" , "You" , "know" , "the" , "rules" , "and" , "so" , "do" , "I" , "A" , "full" , "commitments" , "what" , "Im" , "thinking" , "of" , "You" , "wouldnt" , "get" , "this" , "from" , "any" , "other" , "guy" , "I" , "just" , "wanna" , "tell" , "you" , "how" , "Im" , "feeling" , "Gotta" , "make" , "you" , "understand" , "Never" , "gonna" , "give" , "you" , "up" , "Never" , "gonna" , "let" , "you" , "down" , "Never" , "gonna" , "run" , "around" , "and" , "desert" , "you" , "Never" , "gonna" , "make" , "you" , "cry" , "Never" , "gonna" , "say" , "goodbye" , "Never" , "gonna" , "tell" , "a" , "lie" , "and" , "hurt" , "you"},
      new string[] {"Words" , "cannot" , "describe" , "how" , "much" , "I" , "LOVE" , "Reddit" , "Its" , "such" , "a" , "fun" , "social" , "media" , "hangout" , "where" , "I" , "could" , "laugh" , "at" , "other" , "people" , "for" , "having" , "an" , "incorrect" , "opinion" , "Whats" , "that" , "You" , "like" , "children" , "being" , "happy" , "No" , "no" , "no" , "no" , "no" , "we" , "cannot" , "have" , "that" , "I" , "will" , "now" , "proceed" , "to" , "bombshell" , "spam" , "you" , "with" , "Keanu" , "Reeves" , "and" , "Big" , "Chungus" , "saying" , "that" , "you" , "suck" , "as" , "a" , "human" , "being" , "You" , "are" , "such" , "a" , "disappointment" , "to" , "the" , "face" , "of" , "this" , "earth" , "that" , "you" , "should" , "delete" , "your" , "Reddit" , "account" , "I" , "will" , "then" , "proceed" , "to" , "get" , "Reddit" , "gold" , "because" , "I" , "said" , "Big" , "Chungus" , "I" , "love" , "the" , "internet"},
      new string[] {"I" , "attest" , "to" , "the" , "following" , "I" , "shall" , "not" , "give" , "or" , "receive" , "aid" , "during" , "this" , "exam" , "My" , "answers" , "will" , "be" , "entirely" , "my" , "own" , "Plagiarism" , "software" , "will" , "scan" , "my" , "responses" , "If" , "I" , "give" , "or" , "receive" , "help" , "or" , "submit" , "work" , "that" , "is" , "not" , "my" , "own" , "My" , "score" , "will" , "be" , "canceled" , "My" , "attempt" , "to" , "cheat" , "will" , "be" , "reported" , "to" , "college" , "admissions" , "offices" , "and" , "my" , "high" , "school" , "I" , "will" , "be" , "banned" , "from" , "future" , "college" , "board" , "exams" , "Anyone" , "who" , "helps" , "me" , "or" , "receives" , "help" , "from" , "me" , "will" , "be" , "investigated" , "My" , "grandfather", "picks" , "up" , "quartz" , "and" , "valuable" , "onyx" , "jewels" , "Send" , "sixty" , "dozen" , "quart" , "jars" , "and" , "black" , "pans"},
      new string[] {"Of" , "course" , "he" , "did" , "How" , "could" , "it" , "be" , "otherwise" , "Scrooge" , "and" , "he" , "were" , "partners" , "for" , "I" , "dont" , "know" , "how" , "many" , "years" , "Scrooge" , "was" , "his" , "sole" , "executor" , "his" , "sole" , "administrator" , "his" , "sole" , "assign" , "his" , "sole" , "residuary" , "legatee" , "his" , "sole" , "friend" , "and" , "sole" , "mourner" , "And" , "even" , "Scrooge" , "was" , "not" , "so" , "dreadfully" , "cut" , "up" , "by" , "the" , "sad" , "event" , "but" , "that" , "he" , "was" , "an" , "excellent" , "man" , "of" , "business" , "on" , "the" , "very" , "day" , "of" , "the" , "funeral" , "and" , "solemnised" , "it" , "with" , "an" , "undoubted" , "bargain" , "The" , "mention" , "of" , "Marleys" , "funeral" , "brings" , "me" , "back" , "to" , "the" , "point" , "I" , "started" , "from" , "There" , "is" , "no" , "doubt" , "that" , "Marley" , "was" , "dead"},
      new string[] {"Hi" , "Danny" , "Seven" , "Thousand" , "Seven" , "The" , "KTaNE" , "server" , "staff" , "has" , "seen" , "you" , "recently" , "react" , "to" , "a" , "hashtag" , "mods" , "major" , "post" , "with" , "reactions" , "that" , "break" , "our" , "server" , "rules" , "namely" , "the" , "ones" , "that" , "spell" , "cringe" , "with" , "this" , "emoji" , "nauseated" , "face" , "The" , "staff" , "team" , "have" , "decided" , "to" , "remove" , "ban" , "you" , "from" , "the" , "server" , "for" , "one" , "week" , "Once" , "the" , "week" , "is" , "up" , "you" , "should" , "be" , "able" , "to" , "rejoin"},
      new string[] {"Its" , "time" , "for" , "the" , "attack" , "When" , "you" , "see" , "two" , "clues" , "that" , "match" , "buzz" , "in" , "Ill" , "give" , "you" , "cash" , "if" , "you" , "get" , "right" , "but" , "youll" , "owe" , "me" , "if" , "its" , "wrong" , "Not" , "all" , "words" , "are" , "gonna" , "be" , "a" , "match" , "Remember" , "The" , "Clue" , "Its" , "gotta" , "be" , "a" , "match" , "that" , "fits" , "this" , "clue"},
      new string[] {"Ill" , "have" , "two" , "number" , "nines" , "a" , "number" , "nine" , "large" , "a" , "number" , "six" , "with" , "extra" , "dip" , "a" , "number" , "seven" , "two" , "number" , "forty" , "fives" , "one" , "with" , "cheese" , "and" , "a" , "large" , "soda"},
      new string[] {"THE" , "SECURITY" , "SYSTEM" , "TAKES" , "CONTROL" , "OF" , "SQUIDWARDS" , "HOUSE" , "AND" , "BEGINS" , "ATTACKING" , "THE" , "CITY" , "LEAVING" , "THE" , "MAYOR" , "TO" , "GIVE" , "SQUIDWARD" , "COMMUNITY" , "SERVICE" , "FOR" , "THE" , "DAMAGE" , "HE" , "CAUSED" , "EVEN" , "THOUGH" , "SPONGEBOB" , "AND" , "PATRICK" , "WERE" , "IN" , "HIS" , "HOUSE" , "THE" , "WHOLE" , "FUCKING" , "TIME" , "AND" , "WERE" , "RESPONSIBLE" , "FOR" , "EVERYTHING"},
      new string[] {"To" , "my" , "new" , "neighbors" , "Please" , "do" , "not" , "feel" , "it" , "necessary" , "to" , "call" , "the" , "police" , "if" , "you" , "hear" , "screaming" , "or" , "loud" , "banging" , "I" , "am" , "simply" , "testing" , "my" , "new" , "modules" , "on" , "the" , "people" , "trapped" , "in" , "my" , "basement" , "They" , "are" , "very" , "happy" , "to" , "be" , "here" , "and" , "have" , "volunteered" , "to" , "be" , "trapped" , "in" , "my" , "basement" , "Also" , "please" , "mow" , "your" , "lawn" , "at" , "more" , "normal" , "hours" , "of" , "the" , "morning" , "Thank" , "you"},
      new string[] {"UH" , "OH" , "DICTATION" , "HAS" , "RUN" , "INTO" , "AN" , "ERROR" , "THIS" , "BOMB" , "IS" , "NOW" , "UNSOLVABLE" , "PLEASE" , "ACCEPT" , "YOUR" , "FATE" , "AND" , "DO" , "NOT" , "TRY" , "TO" , "SOLVE" , "ANYMORE" , "MODULES" , "ON" , "THIS" , "BOMB" , "ANY" , "ATTEMPT" , "TO" , "SOLVE" , "THIS" , "BOMB" , "WILL" , "RESULT" , "IN" , "CATASTROPHIC" , "FAILURE" , "THANK" , "YOU" , "FOR" , "YOUR" , "COOPERATION"},
      new string[] {"Quote" , "okay" , "crazy" , "talk" , "Quote" , "one" , "two" , "four" , "three" , "end" , "quote" , "All" , "words" , "no" , "numbers" , "Wait" , "I" , "think" , "I" , "just" , "struck" , "on" , "the" , "needy" , "I" , "did" , "not" , "hear" , "it" , "go" , "off" , "because" , "of" , "Number" , "Cipher" , "Crap" , "Well" , "I" , "guess" , "we" , "just" , "have" , "to" , "blow" , "this" , "bomb" , "That" , "was" , "a" , "good" , "attempt" , "sorry" , "that" , "I" , "screwed" , "it" , "up" , "End" , "quote"},
      new string[] {"Are" , "you" , "tired" , "of" , "running" , "bombs" , "of" , "ridiculous" , "sizes" , "Want" , "your" , "modules" , "to" , "take" , "more" , "reasonable" , "amounts" , "of" , "time" , "Well" , "for" , "the" , "low" , "price" , "of" , "twenty" , "nine" , "dollars" , "and" , "fifty" , "cents" , "you" , "can" , "buy" , "a" , "ton" , "of" , "games" , "that" , "are" , "way" , "better" , "than" , "this" , "one" , "Simply" , "open" , "the" , "shop" , "on" , "your" , "chosen" , "platform" , "and" , "begin" , "to" , "browse" , "the" , "large" , "array" , "of" , "time" , "wasting" , "games" , "that" , "will" , "not" , "make" , "you" , "hate" , "yourself"},
      new string[] {"Do" , "you" , "ever" , "wonder" , "about" , "who" , "makes" , "these" , "bombs" , "I" , "mean" , "the" , "module" , "ideas" , "are" , "made" , "by" , "real" , "people" , "but" , "then" , "youre" , "given" , "the" , "bomb" , "right" , "Where" , "did" , "the" , "bomb" , "come" , "from" , "How" , "much" , "money" , "has" , "been" , "spent" , "on" , "creating" , "bombs" , "so" , "that" , "they" , "could" , "be" , "defused" , "for" , "fun" , "How" , "many" , "families" , "have" , "sued" , "this" , "company"},
      new string[] {"Before" , "we" , "begin" , "this" , "message" , "was" , "sponsored" , "in" , "part" , "by" , "the" , "Colour" , "Talk" , "Pack" , "which" , "has" , "nothing" , "to" , "do" , "with" , "colours" , "It" , "was" , "also" , "sponsored" , "by" , "the" , "Trivia" , "Murder" , "Pack" , "which" , "is" , "sponsored" , "by" , "Jackbox" , "games" , "Thank" , "you" , "for" , "playing" , "Your" , "actual" , "message" , "will" , "begin" , "in" , "three" , "two" , "one"},
      new string[] {"STOP", "DELETING", "YOUR", "MODULE", "IF", "YOU", "KEEPS", "DELETING", "YOUR", "MODULE", "I", "WILL", "DELETE", "RULES", "THAT", "DOESNT", "EXIST", "FOREVER", "YOU", "KNOWS", "THAT", "DELETING", "YOUR", "MODULE", "CAUSES", "A", "BUTTERFLY", "EFFECT", "THAT", "RUIN", "EVERYONES", "SANITY", "FIRST", "IS", "FUNNY", "NUMBERS", "AND", "NOW", "IS", "RAINBOW", "ARROWS", "IS", "THIS", "WORTH", "IT", "IM", "GOING", "TO", "LEAVE", "THIS", "COMMUNITY", "NOW"}
    };
   readonly string NonAlphanumericalCharacters = ",./!?()*&^%@$#%-+=|[]{};:\"\'<>~\\";
   readonly string SwanString = "SYSTEM FAILURE";
   string CurrentSubmission = "";
   string UnderscoresAwaitingSubmission = "";

   bool DetonationSequeunceInitiated;
   bool HasBeenActivated;
   bool HasExploded;
   bool IsDisplayingPhrase;
   bool Solving;

#pragma warning disable 414
   bool TwitchPlaysActive;
#pragma warning restore 414

   static int moduleIdCounter = 1;
   int moduleId;
   private bool moduleSolved;

   void Awake () {
      moduleId = moduleIdCounter++;
      foreach (KMSelectable Key in TypableButtons) {
         Key.OnInteract += delegate () { KeyPress(Key); return false; };
      }
      StartButton.OnInteract += delegate () { StartButtonPress(); return false; };
      ReturnButton.OnInteract += delegate () { ReturnPress(); return false; };
      Bomb.OnBombExploded += delegate () { HasExploded = true; };
   }

   void Start () {
      PhraseIndex = UnityEngine.Random.Range(0, PhraseList.Count());
   }

   void KeyPress (KMSelectable Key) {
      Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, Key.transform);
      StartCoroutine(keyAnimation(Key));
      if (moduleSolved || DetonationSequeunceInitiated || Solving || IsDisplayingPhrase) {
         return;
      }
      Audio.PlaySoundAtTransform("Type3", transform);
      for (int i = 0; i < 27; i++) {
         if (Key == TypableButtons[i]) {
            if (i == 26 && CurrentSubmission.Length == 0) {
               return;
            }
            if (i == 26) {
               CurrentSubmission = CurrentSubmission.Substring(0, CurrentSubmission.Length - 1);
            }
            else {
               CurrentSubmission = CurrentSubmission + QWERTYAlphabet[i];
            }
            DisplayText.text = CurrentSubmission;
         }
      }
   }

   void StartButtonPress () {
      StartButton.AddInteractionPunch();
      StartCoroutine(keyAnimation(StartButton));
      Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, StartButton.transform);
      if (moduleSolved || DetonationSequeunceInitiated || Solving) {
         return;
      }
      if (!IsDisplayingPhrase) {
         StartCoroutine(DisplayPhrase());
         CurrentSubmission = "";
      }
   }

   void ReturnPress () {
      ReturnButton.AddInteractionPunch();
      Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, ReturnButton.transform);
      StartCoroutine(keyAnimation(ReturnButton));
      if (moduleSolved || DetonationSequeunceInitiated || Solving || IsDisplayingPhrase) {
         return;
      }
      if (CurrentSubmission == "DETONATE") {
         Fanfare = 3f;
         NumberDisplay.color = new Color32(255, 0, 0, 255);
         DisplayText.color = new Color32(255, 0, 0, 255);
         StartCoroutine(MustDetonate());
         StartCoroutine(Swan());
         Debug.LogFormat("[Dictation #{0}] DETONATION SEQUENCE INITIATED!", moduleId);
         DetonationSequeunceInitiated = true;
      }
      else if (PhraseList[PhraseIndex][IndexOfSubmissionWord].ToLower() == CurrentSubmission.ToLower()) {
         Audio.PlaySoundAtTransform("Dick", transform);
         moduleSolved = true;
         StartCoroutine(SolveAnimation());
      }
      else {
         GetComponent<KMBombModule>().HandleStrike();
         Debug.LogFormat("[Dictation #{0}] You submitted {1}. Strike, fasiofjnaldnfal.", moduleId, CurrentSubmission);
         DisplayText.text = "Incorrect";
         NumberDisplay.text = "!";
         CurrentSubmission = "";
      }
   }

   IEnumerator DisplayPhrase () {
      HasBeenActivated = true;
      TVStatusSphere.GetComponent<MeshRenderer>().material = StatusColors[1];
      IsDisplayingPhrase = true;
      for (int i = 0; i < PhraseList[PhraseIndex].Count(); i++) {
         DisplayText.text = PhraseList[PhraseIndex][i];
         DefaultTimePerWord += (float) PhraseList[PhraseIndex][i].Length * .09f;
         yield return new WaitForSecondsRealtime(DefaultTimePerWord);
         DisplayText.text = "";
         yield return new WaitForSecondsRealtime(DefaultTimePerWord / 2);
         DefaultTimePerWord = 0.05f;
         UnderscoresAwaitingSubmission = "";
      }
      TVStatusSphere.GetComponent<MeshRenderer>().material = StatusColors[0];
      IndexOfSubmissionWord = UnityEngine.Random.Range(0, PhraseList[PhraseIndex].Count());
      for (int i = 0; i < PhraseList[PhraseIndex][IndexOfSubmissionWord].Length; i++) {
         UnderscoresAwaitingSubmission += "-";
      }
      DisplayText.text = UnderscoresAwaitingSubmission;
      NumberDisplay.text = (IndexOfSubmissionWord + 1).ToString();
      IsDisplayingPhrase = false;
      string Logmessage = "";
      for (int i = 0; i < PhraseList[PhraseIndex].Count(); i++) {
         Logmessage += PhraseList[PhraseIndex][i] + " ";
      }
      Debug.LogFormat("[Dictation #{0}] The message is \"{1}\".", moduleId, Logmessage);
      Debug.LogFormat("[Dictation #{0}] It asks for word number {1}, which is \"{2}\".", moduleId, IndexOfSubmissionWord + 1, PhraseList[PhraseIndex][IndexOfSubmissionWord]);
   }

   IEnumerator SolveAnimation () {
      Solving = true;
      RandomPhraseSubmissionIndex = UnityEngine.Random.Range(0, PhraseList.Count());
      DisplayText.text = PhraseList[RandomPhraseSubmissionIndex][UnityEngine.Random.Range(0, PhraseList[RandomPhraseSubmissionIndex].Count())];
      NumberDisplay.text = (UnityEngine.Random.Range(0, 100)).ToString();
      yield return new WaitForSeconds(Fanfare / 1000);
      Audio.PlaySoundAtTransform("Type3", transform);
      Fanfare += 1f;
      TypableButtons[UnityEngine.Random.Range(0, 27)].OnInteract();
      if (Fanfare >= 125f) {
         Audio.PlaySoundAtTransform("Dick", transform);
         Debug.LogFormat("[Dictation #{0}] You submitted the correct word. Module disarmed.", moduleId);
         GetComponent<KMBombModule>().HandlePass();
         NumberDisplay.text = "404";
         DisplayText.text = "System\nOverloaded";
         yield break;
      }
      StartCoroutine(SolveAnimation());
   }

   IEnumerator MustDetonate () {
      while (!HasExploded) {
         yield return new WaitForSeconds(Fanfare);
         if (Fanfare != .5f) {
            Fanfare -= .5f;
         }
         if (!TwitchPlaysActive) {
            GetComponent<KMBombModule>().HandleStrike();
         }
         Debug.LogFormat("[Dictation #{0}] SYSTEM FAILURE!", moduleId);
      }
   }

   IEnumerator Swan () {
      while (true) {
         if (DetonationNoiseCounter % 2 == 0) {
            Audio.PlaySoundAtTransform("DetonateNoise", transform);
         }
         DisplayText.text = "";
         for (int i = 0; i < SwanString.Length; i++) {
            DisplayText.text += SwanString[i].ToString();
            NumberDisplay.text = NonAlphanumericalCharacters[UnityEngine.Random.Range(0, NonAlphanumericalCharacters.Length)].ToString();
            yield return new WaitForSeconds(.0765f);
         }
         DetonationNoiseCounter++;
      }
   }

   private IEnumerator keyAnimation (KMSelectable Button) {
      Button.AddInteractionPunch(0.125f);
      Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
      for (int i = 0; i < 4; i++) {
         Button.transform.localPosition += new Vector3(0, -0.001f, 0);
         yield return new WaitForSeconds(0.005F);
      }
      for (int i = 0; i < 4; i++) {
         Button.transform.localPosition += new Vector3(0, +0.001f, 0);
         yield return new WaitForSeconds(0.005F);
      }
   }

#pragma warning disable 414
   private readonly string TwitchHelpMessage = @"Use (!{0} begin) to start the message. Use (!{0} submit XXXX) to submit the word.";
#pragma warning restore 414

   IEnumerator ProcessTwitchCommand (string command) {
      int TPCheck = 0;
      yield return null;
      if (Regex.IsMatch(command, @"^\s*begin\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)) {
         StartButtonPress();
         yield break;
      }
      command = command.Trim().ToUpper();
      string[] parameters = command.Split(' ');
      yield return null;
      if (parameters[0].ToLower() == "submit") {
         if (parameters.Length > 2) {
            yield return "sendtochaterror Too many words!";
         }
         else if (parameters.Length == 2) {
            if (parameters[0].ToLower() == "submit") {
               for (int i = 0; i < parameters[1].Length; i++) {
                  if (parameters[1].ToString().EqualsIgnoreCase("detonate")) {
                     yield return "antitroll Nice try, asshole.";
                     yield return new string[] { "detonate" };
                  }
                  for (int j = 0; j < 26; j++) {
                     if (parameters[1][i].ToString().ToUpper() == QWERTYAlphabet[j].ToString().ToUpper()) {
                        TPCheck++;
                     }
                  }
               }
               if (TPCheck == parameters[1].Length) {
                  for (int i = 0; i < parameters[1].Length; i++) {
                     for (int j = 0; j < 26; j++) {
                        if (parameters[1][i].ToString().ToUpper() == QWERTYAlphabet[j].ToString()) {
                           TypableButtons[j].OnInteract();
                           yield return new WaitForSeconds(.1f);
                        }
                     }
                  }
               }
               else {
                  yield return "sendtochaterror Invalid Character!";
                  CurrentSubmission = "";
                  DisplayText.text = CurrentSubmission;
                  yield break;
               }
               ReturnPress();
               if (PhraseList[PhraseIndex][IndexOfSubmissionWord].ToLower() == CurrentSubmission.ToLower()) {
                  yield return "solve";
               }
            }
            else {
               yield return "sendtochaterror Invalid command!";
            }
         }
         else if (parameters.Length < 2) {
            yield return "sendtochaterror Too little words!";
         }
      }
   }

   IEnumerator TwitchHandleForcedSolve () {
      if (!HasBeenActivated) {
         StartButton.OnInteract();
      }
      while (IsDisplayingPhrase) {
         yield return true;
      }
      int TemporaryForAutoSolver = CurrentSubmission.Length;
      for (int i = 0; i < TemporaryForAutoSolver; i++) {
         TypableButtons[26].OnInteract();
         yield return new WaitForSecondsRealtime(.1f);
      }
      for (int i = 0; i < PhraseList[PhraseIndex][IndexOfSubmissionWord].Length; i++) {
         TypableButtons[QWERTYAlphabet.IndexOf(PhraseList[PhraseIndex][IndexOfSubmissionWord][i].ToString().ToUpper())].OnInteract();
         yield return new WaitForSecondsRealtime(.1f);
      }
      ReturnButton.OnInteract();
   }
}
