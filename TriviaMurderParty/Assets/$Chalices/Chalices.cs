using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using KModkit;

public class Chalices : MonoBehaviour {

    public KMBombInfo Bomb;
    public KMAudio Audio;
    public GameObject[] TopLeftAsshole;
    public GameObject[] TopMiddleAsshole;
    public GameObject[] MiddleLeftAsshole;
    public GameObject[] TheMainAss;
    public GameObject[] MiddleRightAsshole;
    public GameObject[] BottomLeftAsshole;
    public GameObject[] BottomMiddleAsshole;
    public GameObject[] BottomRightAsshole;
    public GameObject[] AllTheAssholes;
    public KMSelectable[] LittleHerobrineIAmCummingInYourMom;

    static int moduleIdCounter = 1;
    int moduleId;
    private bool moduleSolved;
    string[] Cups = {"Party Cup", "Mug", "Tall Glass", "Skull Glass", "Exotic Cup", "Wine Glass", "Shot Glass", "Metal Chalice"};
    bool[] Conditions = {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false};
    bool[] BlackConditions = {false, false, false, false, false, false, false, false};
    bool[] AnswerToIsAmazing = {false, false, false, false, false, false, false, false};
    string FuckYou = "";
    string ChalicesButString = "CHALICES";
    bool Active = false;

    void Awake () {
        moduleId = moduleIdCounter++;
        foreach (KMSelectable Slenderman in LittleHerobrineIAmCummingInYourMom) {
            Slenderman.OnInteract += delegate () { SlendermanPress(Slenderman); return false; };
        }
    }

    void Start () {//party mug tall skull exotic wine shot metal
      FuckYou = Bomb.GetSerialNumber();
      Cups.Shuffle();
      Debug.LogFormat("[Chalices #{0}] The order of the chalices are: {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}.", moduleId, Cups[0], Cups[1], Cups[2], Cups[3], Cups[4], Cups[5], Cups[6], Cups[7]);
      switch (Cups[0]) {
        case "Party Cup": TopLeftAsshole[0].gameObject.SetActive(true); break;
        case "Mug": TopLeftAsshole[1].gameObject.SetActive(true); break;
        case "Tall Glass": TopLeftAsshole[2].gameObject.SetActive(true); break;
        case "Skull Glass": TopLeftAsshole[3].gameObject.SetActive(true); break;
        case "Exotic Cup": TopLeftAsshole[4].gameObject.SetActive(true); break;
        case "Wine Glass": TopLeftAsshole[5].gameObject.SetActive(true); break;
        case "Shot Glass": TopLeftAsshole[6].gameObject.SetActive(true); break;
        case "Metal Chalice": TopLeftAsshole[7].gameObject.SetActive(true); break;
      }
      switch (Cups[1]) {
        case "Party Cup": TopMiddleAsshole[0].gameObject.SetActive(true); break;
        case "Mug": TopMiddleAsshole[1].gameObject.SetActive(true); break;
        case "Tall Glass": TopMiddleAsshole[2].gameObject.SetActive(true); break;
        case "Skull Glass": TopMiddleAsshole[3].gameObject.SetActive(true); break;
        case "Exotic Cup": TopMiddleAsshole[4].gameObject.SetActive(true); break;
        case "Wine Glass": TopMiddleAsshole[5].gameObject.SetActive(true); break;
        case "Shot Glass": TopMiddleAsshole[6].gameObject.SetActive(true); break;
        case "Metal Chalice": TopMiddleAsshole[7].gameObject.SetActive(true); break;
      }
      switch (Cups[2]) {
        case "Party Cup": MiddleLeftAsshole[0].gameObject.SetActive(true); break;
        case "Mug": MiddleLeftAsshole[1].gameObject.SetActive(true); break;
        case "Tall Glass": MiddleLeftAsshole[2].gameObject.SetActive(true); break;
        case "Skull Glass": MiddleLeftAsshole[3].gameObject.SetActive(true); break;
        case "Exotic Cup": MiddleLeftAsshole[4].gameObject.SetActive(true); break;
        case "Wine Glass": MiddleLeftAsshole[5].gameObject.SetActive(true); break;
        case "Shot Glass": MiddleLeftAsshole[6].gameObject.SetActive(true); break;
        case "Metal Chalice": MiddleLeftAsshole[7].gameObject.SetActive(true); break;
      }
      switch (Cups[3]) {
        case "Party Cup": TheMainAss[0].gameObject.SetActive(true); break;
        case "Mug": TheMainAss[1].gameObject.SetActive(true); break;
        case "Tall Glass": TheMainAss[2].gameObject.SetActive(true); break;
        case "Skull Glass": TheMainAss[3].gameObject.SetActive(true); break;
        case "Exotic Cup": TheMainAss[4].gameObject.SetActive(true); break;
        case "Wine Glass": TheMainAss[5].gameObject.SetActive(true); break;
        case "Shot Glass": TheMainAss[6].gameObject.SetActive(true); break;
        case "Metal Chalice": TheMainAss[7].gameObject.SetActive(true); break;
      }
      switch (Cups[4]) {
        case "Party Cup": MiddleRightAsshole[0].gameObject.SetActive(true); break;
        case "Mug": MiddleRightAsshole[1].gameObject.SetActive(true); break;
        case "Tall Glass": MiddleRightAsshole[2].gameObject.SetActive(true); break;
        case "Skull Glass": MiddleRightAsshole[3].gameObject.SetActive(true); break;
        case "Exotic Cup": MiddleRightAsshole[4].gameObject.SetActive(true); break;
        case "Wine Glass": MiddleRightAsshole[5].gameObject.SetActive(true); break;
        case "Shot Glass": MiddleRightAsshole[6].gameObject.SetActive(true); break;
        case "Metal Chalice": MiddleRightAsshole[7].gameObject.SetActive(true); break;
      }
      switch (Cups[5]) {
        case "Party Cup": BottomLeftAsshole[0].gameObject.SetActive(true); break;
        case "Mug": BottomLeftAsshole[1].gameObject.SetActive(true); break;
        case "Tall Glass": BottomLeftAsshole[2].gameObject.SetActive(true); break;
        case "Skull Glass": BottomLeftAsshole[3].gameObject.SetActive(true); break;
        case "Exotic Cup": BottomLeftAsshole[4].gameObject.SetActive(true); break;
        case "Wine Glass": BottomLeftAsshole[5].gameObject.SetActive(true); break;
        case "Shot Glass": BottomLeftAsshole[6].gameObject.SetActive(true); break;
        case "Metal Chalice": BottomLeftAsshole[7].gameObject.SetActive(true); break;
      }
      switch (Cups[6]) {
        case "Party Cup": BottomMiddleAsshole[0].gameObject.SetActive(true); break;
        case "Mug": BottomMiddleAsshole[1].gameObject.SetActive(true); break;
        case "Tall Glass": BottomMiddleAsshole[2].gameObject.SetActive(true); break;
        case "Skull Glass": BottomMiddleAsshole[3].gameObject.SetActive(true); break;
        case "Exotic Cup": BottomMiddleAsshole[4].gameObject.SetActive(true); break;
        case "Wine Glass": BottomMiddleAsshole[5].gameObject.SetActive(true); break;
        case "Shot Glass": BottomMiddleAsshole[6].gameObject.SetActive(true); break;
        case "Metal Chalice": BottomMiddleAsshole[7].gameObject.SetActive(true); break;
      }
      switch (Cups[7]) {
        case "Party Cup": BottomRightAsshole[0].gameObject.SetActive(true); break;
        case "Mug": BottomRightAsshole[1].gameObject.SetActive(true); break;
        case "Tall Glass": BottomRightAsshole[2].gameObject.SetActive(true); break;
        case "Skull Glass": BottomRightAsshole[3].gameObject.SetActive(true); break;
        case "Exotic Cup": BottomRightAsshole[4].gameObject.SetActive(true); break;
        case "Wine Glass": BottomRightAsshole[5].gameObject.SetActive(true); break;
        case "Shot Glass": BottomRightAsshole[6].gameObject.SetActive(true); break;
        case "Metal Chalice": BottomRightAsshole[7].gameObject.SetActive(true); break;
      }
      //a to h
      if (Array.IndexOf(Cups, "Skull Glass") < Array.IndexOf(Cups, "Wine Glass")) {//a
        Conditions[0] = true;
        Debug.LogFormat("[Chalices #{0}] A's red condition is true.", moduleId);
      }
      else if (Array.IndexOf(Cups, "Party Cup") > Array.IndexOf(Cups, "Skull Glass")) {
        BlackConditions[0] = true;
        Debug.LogFormat("[Chalices #{0}] A's black condition is true.", moduleId);
      }
      if ((Array.IndexOf(Cups, "Tall Glass") + 1) % 2 == 0) {//b
        Conditions[1] = true;
        Debug.LogFormat("[Chalices #{0}] B's red condition is true.", moduleId);
      }
      else if ((Array.IndexOf(Cups, "Metal Chalice") + 1) % 2 == 1) {
        BlackConditions[1] = true;
        Debug.LogFormat("[Chalices #{0}] B's black condition is true.", moduleId);
      }
      if (Array.IndexOf(Cups, "Exotic Cup") - 1 != Array.IndexOf(Cups, "Tall Glass") && Array.IndexOf(Cups, "Exotic Cup") + 1 != Array.IndexOf(Cups, "Tall Glass")) {//c
        Conditions[2] = true;
        Debug.LogFormat("[Chalices #{0}] C's red condition is true.", moduleId);
      }
      else if (Array.IndexOf(Cups, "Shot Glass") - 1 == Array.IndexOf(Cups, "Wine Glass") || Array.IndexOf(Cups, "Shot Glass") + 1 == Array.IndexOf(Cups, "Wine Glass")) {
        BlackConditions[2] = true;
        Debug.LogFormat("[Chalices #{0}] C's black condition is true.", moduleId);
      }
      if (Array.IndexOf(Cups, "Mug") == 1 || Array.IndexOf(Cups, "Mug") == 2 || Array.IndexOf(Cups, "Mug") == 4 || Array.IndexOf(Cups, "Mug") == 6) {//d
        Conditions[3] = true;
        Debug.LogFormat("[Chalices #{0}] D's red condition is true.", moduleId);
      }
      else if ((Array.IndexOf(Cups, "Skull Cup") + 1).ToString() == FuckYou[5].ToString()) {
        BlackConditions[3] = true;
        Debug.LogFormat("[Chalices #{0}] D's black condition is true.", moduleId);
      }
      if (Array.IndexOf(Cups, "Party Cup") + 1 == Array.IndexOf(Cups, "Skull Glass") && Array.IndexOf(Cups, "Skull Glass") + 1 == Array.IndexOf(Cups, "Mug")) {//e
        Conditions[4] = true;
        Debug.LogFormat("[Chalices #{0}] E's red condition is true.", moduleId);
      }
      else if (Array.IndexOf(Cups, "Exotic Cup") + 1 == Array.IndexOf(Cups, "Wine Glass") && Array.IndexOf(Cups, "Wine Glass") + 1 == Array.IndexOf(Cups, "Tall Glass")) {
        BlackConditions[4] = true;
        Debug.LogFormat("[Chalices #{0}] E's black condition is true.", moduleId);
      }
      if (Array.IndexOf(Cups, "Metal Chalice") + 2 == Array.IndexOf(Cups, "Tall Glass") || Array.IndexOf(Cups, "Metal Chalice") - 2 == Array.IndexOf(Cups, "Tall Glass")) {//f
        Conditions[5] = true;
        Debug.LogFormat("[Chalices #{0}] F's red condition is true.", moduleId);
      }
      else if (Array.IndexOf(Cups, "Shot Glass") < 4) {
        BlackConditions[5] = true;
        Debug.LogFormat("[Chalices #{0}] F's black condition is true.", moduleId);
      }
      if (Array.IndexOf(Cups, "Skull Glass") > Array.IndexOf(Cups, "Metal Chalice")) {//g
        Conditions[6] = true;
        Debug.LogFormat("[Chalices #{0}] G's red condition is true.", moduleId);
      }
      else if (Array.IndexOf(Cups, "Tall Glass") > 3) {
        BlackConditions[6] = true;
        Debug.LogFormat("[Chalices #{0}] G's black condition is true.", moduleId);
      }
      if (Array.IndexOf(Cups, "Party Cup") == 0 || Array.IndexOf(Cups, "Party Cup") == 5 || Array.IndexOf(Cups, "Party Cup") == 6 || Array.IndexOf(Cups, "Party Cup") == 7) {//h
        Conditions[7] = true;
        Debug.LogFormat("[Chalices #{0}] H's red condition is true.", moduleId);
      }
      else if (Array.IndexOf(Cups, "Mug") == 1 || Array.IndexOf(Cups, "Mug") == 2 || Array.IndexOf(Cups, "Mug") == 3 || Array.IndexOf(Cups, "Mug") == 4) {
        BlackConditions[7] = true;
        Debug.LogFormat("[Chalices #{0}] H's black condition is true.", moduleId);
      }
      //i to p
      if (Bomb.IsIndicatorOn("BOB")) { //i
        Conditions[8] = true;
        Debug.LogFormat("[Chalices #{0}] I's condition is true.", moduleId);
      }
      if (Bomb.GetBatteryCount() == 5) { //j
        Conditions[9] = true;
        Debug.LogFormat("[Chalices #{0}] J's condition is true.", moduleId);
      }
      for (int i = 0; i < 6; i++) {//k
        if (FuckYou[i].ToString() == "A" || FuckYou[i].ToString() == "E" || FuckYou[i].ToString() == "I" || FuckYou[i].ToString() == "O" || FuckYou[i].ToString() == "U") {
          Conditions[10] = true;
          Debug.LogFormat("[Chalices #{0}] K's condition is true.", moduleId);
        }
      }
      if (Bomb.GetBatteryHolderCount() % 2 == 0) { //l
        Conditions[11] = true;
        Debug.LogFormat("[Chalices #{0}] L's condition is true.", moduleId);
      }
      for (int i = 0; i < 6; i++) {//m
        if (FuckYou[i].ToString() == "T" || FuckYou[i].ToString() == "M" || FuckYou[i].ToString() == "P") {
          Conditions[12] = true;
          Debug.LogFormat("[Chalices #{0}] M's condition is true.", moduleId);
        }
      }
      if (Bomb.IsIndicatorPresent("CAR") || Bomb.IsIndicatorPresent("CLR") || Bomb.IsIndicatorPresent("MSA") || Bomb.IsIndicatorPresent("NSA") || Bomb.IsIndicatorPresent("SIG") || Bomb.IsIndicatorPresent("SND")) { //n
        Conditions[13] = true;
        Debug.LogFormat("[Chalices #{0}] N's condition is true.", moduleId);
      }
      if (Bomb.IsPortPresent(Port.Parallel) && Bomb.IsPortPresent(Port.Serial)) { //o
        Conditions[14] = true;
        Debug.LogFormat("[Chalices #{0}] O's condition is true.", moduleId);
      }
      foreach (object[] plate in Bomb.GetPortPlates()) {//p
        if (plate.Length == 0) {
          Conditions[15] = true;
          Debug.LogFormat("[Chalices #{0}] P's condition is true.", moduleId);
        }
      }
      //w to z
      if (!(Array.IndexOf(Cups, "Mug") + 1 == Array.IndexOf(Cups, "Skull Glass") && Array.IndexOf(Cups, "Skull Glass") + 1 == Array.IndexOf(Cups, "Metal Chalice"))) { //w
        Conditions[16] = true;
        Debug.LogFormat("[Chalices #{0}] W's condition is true.", moduleId);
      }
      if (Array.IndexOf(Cups, "Party Cup") + 1 == Array.IndexOf(Cups, "Tall Glass") && Array.IndexOf(Cups, "Tall Glass") + 1 == Array.IndexOf(Cups, "Exotic Cup")) { //x
        Conditions[17] = true;
        Debug.LogFormat("[Chalices #{0}] X's condition is true.", moduleId);
      }
      if ((Array.IndexOf(Cups, "Wine Glasses") == 0 || Array.IndexOf(Cups, "Wine Glasses") == 7) && (Array.IndexOf(Cups, "Shot Glass") == 0 || Array.IndexOf(Cups, "Shot Glass") == 7)) { //y
        Conditions[18] = true;
        Debug.LogFormat("[Chalices #{0}] Y's condition is true.", moduleId);
      }
      if (Array.IndexOf(Cups, "Exotic Cup") == 0 && Array.IndexOf(Cups, "Metal Chalice") == 1
      && Array.IndexOf(Cups, "Mug") == 2
      && Array.IndexOf(Cups, "Party Cup") == 3
      && Array.IndexOf(Cups, "Shot Glass") == 4
      && Array.IndexOf(Cups, "Skull Glass") == 5
      && Array.IndexOf(Cups, "Tall Glass") == 6
      && Array.IndexOf(Cups, "Wine Glass") == 7) {//z
        Conditions[19] = true;
        Debug.LogFormat("[Chalices #{0}] Z's condition is true.", moduleId);
      }
      if (Conditions[0] && Conditions[1] && Conditions[2] &&
      Conditions[3] && Conditions[4] && Conditions[5] && Conditions[6] && Conditions[7] && Conditions[8] && Conditions[9]) {
        AnswerToIsAmazing[0] = true;
        Debug.LogFormat("[Chalices #{0}] There has been a gridlock, select the first cup.", moduleId);
        return;
      }
      switch (Bomb.GetPortCount() % 8) {
        case 0:
        InnerC();
        Debug.LogFormat("[Chalices #{0}] 8n + 0 ports goes to inner C.", moduleId);
        break;
        case 1:
        InnerH();
        Debug.LogFormat("[Chalices #{0}] 8n + 1 ports goes to inner H.", moduleId);
        break;
        case 2:
        InnerE();
        Debug.LogFormat("[Chalices #{0}] 8n + 2 ports goes to inner E.", moduleId);
        break;
        case 3:
        InnerF();
        Debug.LogFormat("[Chalices #{0}] 8n + 3 ports goes to inner F.", moduleId);
        break;
        case 4:
        InnerD();
        Debug.LogFormat("[Chalices #{0}] 8n + 4 ports goes to inner d.", moduleId);
        break;
        case 5:
        InnerG();
        Debug.LogFormat("[Chalices #{0}] 8n + 5 ports goes to inner G.", moduleId);
        break;
        case 6:
        InnerB();
        Debug.LogFormat("[Chalices #{0}] 8n + 6 ports goes to inner B.", moduleId);
        break;
        case 7:
        InnerA();
        Debug.LogFormat("[Chalices #{0}] 8n + 7 ports goes to inner A.", moduleId);
        break;
      }
    }

    void SlendermanPress(KMSelectable Slenderman) {
      if (Active) {
        return;
      }
      for (int i = 0; i < 8; i++) {
        if (Slenderman == LittleHerobrineIAmCummingInYourMom[i]) {
          if (AnswerToIsAmazing[i]) {
            Active = true;
            StartCoroutine(Good());
            StartCoroutine(JustLikeDad(i));
          }
          else {
            Active = true;
            StartCoroutine(Bad());
            Debug.LogFormat("[Chalices #{0}] You selected chalice {1}. Strike...................................................................................................................................................................................................................................................................................................................................................................................................................................................", moduleId, i + 1);
          }
        }
      }
    }

    IEnumerator Good() {
      Audio.PlaySoundAtTransform("Coffee", transform);
      yield return new WaitForSeconds(4f);
      Audio.PlaySoundAtTransform("jnoneeofnw", transform);
      GetComponent<KMBombModule>().HandlePass();
      Debug.LogFormat("[Chalices #{0}] You selected the right chalice again.", moduleId);
    }

    IEnumerator Bad() {
      Audio.PlaySoundAtTransform("Coffee", transform);
      yield return new WaitForSeconds(4f);
      Audio.PlaySoundAtTransform("OH MY GOD!!! WAAAAAAH!!!!", transform);
      GetComponent<KMBombModule>().HandleStrike();
      Active = false;
    }

    IEnumerator JustLikeDad(int weed) {
      yield return new WaitForSeconds(4f);
      AllTheAssholes[weed].gameObject.SetActive(false);
      switch (weed) {
        case 0:
        for (int i = 0; i < 8; i++) {
          TopLeftAsshole[i].gameObject.SetActive(false);
        }
        break;
        case 1:
        for (int i = 0; i < 8; i++) {
          TopMiddleAsshole[i].gameObject.SetActive(false);
        }
        break;
        case 2:
        for (int i = 0; i < 8; i++) {
          MiddleLeftAsshole[i].gameObject.SetActive(false);
        }
        break;
        case 3:
        for (int i = 0; i < 8; i++) {
          TheMainAss[i].gameObject.SetActive(false);
        }
        break;
        case 4:
        for (int i = 0; i < 8; i++) {
          MiddleRightAsshole[i].gameObject.SetActive(false);
        }
        break;
        case 5:
        for (int i = 0; i < 8; i++) {
          BottomLeftAsshole[i].gameObject.SetActive(false);
        }
        break;
        case 6:
        for (int i = 0; i < 8; i++) {
          BottomMiddleAsshole[i].gameObject.SetActive(false);
        }
        break;
        case 7:
        for (int i = 0; i < 8; i++) {
          BottomRightAsshole[i].gameObject.SetActive(false);
        }
        break;
      }
    }

    void InnerA(){
      if (Conditions[0]) {
        InnerB();
        Debug.LogFormat("[Chalices #{0}] Going to Inner B.", moduleId);
      }
      else if (BlackConditions[0]) {
        OuterA();
        Debug.LogFormat("[Chalices #{0}] Going to Outer A.", moduleId);
      }
      else {
        OuterE();
        Debug.LogFormat("[Chalices #{0}] Going to Outer E.", moduleId);
      }
    }

    void InnerB(){
      if (Conditions[1]) {
        InnerG();
        Debug.LogFormat("[Chalices #{0}] Going to Inner G.", moduleId);
      }
      else if (BlackConditions[1]) {
        OuterB();
        Debug.LogFormat("[Chalices #{0}] Going to Outer B.", moduleId);
      }
      else {
        OuterA();
        Debug.LogFormat("[Chalices #{0}] Going to Outer A.", moduleId);
      }
    }

    void InnerC(){
      if (Conditions[2]) {
        InnerA();
        Debug.LogFormat("[Chalices #{0}] Going to Inner A.", moduleId);
      }
      else if (BlackConditions[2]) {
        OuterE();
        Debug.LogFormat("[Chalices #{0}] Going to Outer E.", moduleId);
      }
      else {
        OuterC();
        Debug.LogFormat("[Chalices #{0}] Going to Outer C.", moduleId);
      }
    }

    void InnerD(){
      if (Conditions[3]) {
        InnerF();
        Debug.LogFormat("[Chalices #{0}] Going to Inner F.", moduleId);
      }
      else if (BlackConditions[3]) {
        OuterD();
        Debug.LogFormat("[Chalices #{0}] Going to Outer D.", moduleId);
      }
      else {
        OuterG();
        Debug.LogFormat("[Chalices #{0}] Going to Outer G.", moduleId);
      }
    }

    void InnerE(){
      if (Conditions[4]) {
        InnerH();
        Debug.LogFormat("[Chalices #{0}] Going to Inner H.", moduleId);
      }
      else if (BlackConditions[4]) {
        OuterH();
        Debug.LogFormat("[Chalices #{0}] Going to Outer H.", moduleId);
      }
      else {
        OuterF();
        Debug.LogFormat("[Chalices #{0}] Going to Outer F.", moduleId);
      }
    }

    void InnerF(){
      if (Conditions[5]) {
        InnerE();
        Debug.LogFormat("[Chalices #{0}] Going to Inner E.", moduleId);
      }
      else if (BlackConditions[5]) {
        OuterF();
        Debug.LogFormat("[Chalices #{0}] Going to Outer F.", moduleId);
      }
      else {
        OuterD();
        Debug.LogFormat("[Chalices #{0}] Going to Outer D.", moduleId);
      }
    }

    void InnerG(){
      if (Conditions[6]) {
        InnerD();
        Debug.LogFormat("[Chalices #{0}] Going to Inner D.", moduleId);
      }
      else if (BlackConditions[6]) {
        OuterG();
        Debug.LogFormat("[Chalices #{0}] Going to Outer G.", moduleId);
      }
      else {
        OuterB();
        Debug.LogFormat("[Chalices #{0}] Going to Outer B.", moduleId);
      }
    }

    void InnerH(){
      if (Conditions[7]) {
        InnerC();
        Debug.LogFormat("[Chalices #{0}] Going to Inner C.", moduleId);
      }
      else if (BlackConditions[7]) {
        OuterC();
        Debug.LogFormat("[Chalices #{0}] Going to Outer C.", moduleId);
      }
      else {
        OuterH();
        Debug.LogFormat("[Chalices #{0}] Going to Outer H.", moduleId);
      }
    }

    void OuterA(){
      if (Conditions[0]) {
        OuterB();
        Debug.LogFormat("[Chalices #{0}] Going to Outer B.", moduleId);
      }
      else {
        LetterX();
        Debug.LogFormat("[Chalices #{0}] Going to X.", moduleId);
      }
    }

    void OuterB(){
      if (Conditions[1]) {
        OuterG();
        Debug.LogFormat("[Chalices #{0}] Going to Outer G.", moduleId);
      }
      else {
        LetterX();
        Debug.LogFormat("[Chalices #{0}] Going to Outer X.", moduleId);
      }
    }

    void OuterC(){
      if (Conditions[2]) {
        OuterE();
        Debug.LogFormat("[Chalices #{0}] Going to Outer E.", moduleId);
      }
      else {
        LetterW();
        Debug.LogFormat("[Chalices #{0}] Going to W.", moduleId);
      }
    }

    void OuterD(){
      if (Conditions[3]) {
        OuterF();
        Debug.LogFormat("[Chalices #{0}] Going to Outer F.", moduleId);
      }
      else {
        LetterY();
        Debug.LogFormat("[Chalices #{0}] Going to Y.", moduleId);
      }
    }

    void OuterE(){
      if (Conditions[4]) {
        OuterA();
        Debug.LogFormat("[Chalices #{0}] Going to Outer A.", moduleId);
      }
      else {
        LetterW();
        Debug.LogFormat("[Chalices #{0}] Going to W.", moduleId);
      }
    }

    void OuterF(){
      if (Conditions[5]) {
        OuterH();
        Debug.LogFormat("[Chalices #{0}] Going to Outer H.", moduleId);
      }
      else {
        LetterZ();
        Debug.LogFormat("[Chalices #{0}] Going to Z.", moduleId);
      }
    }

    void OuterG(){
      if (Conditions[6]) {
        OuterD();
        Debug.LogFormat("[Chalices #{0}] Going to Outer D.", moduleId);
      }
      else {
        LetterY();
        Debug.LogFormat("[Chalices #{0}] Going to Y.", moduleId);
      }
    }

    void OuterH(){
      if (Conditions[7]) {
        OuterC();
        Debug.LogFormat("[Chalices #{0}] Going to Outer C.", moduleId);
      }
      else {
        LetterZ();
        Debug.LogFormat("[Chalices #{0}] Going to Z.", moduleId);
      }
    }

    void LetterW(){
      if (Conditions[16]) {
        LetterN();
        Debug.LogFormat("[Chalices #{0}] Going to N.", moduleId);
      }
      else {
        LetterI();
        Debug.LogFormat("[Chalices #{0}] Going to I.", moduleId);
      }
    }

    void LetterX(){
      if (Conditions[17]) {
        LetterP();
        Debug.LogFormat("[Chalices #{0}] Going to P.", moduleId);
      }
      else {
        LetterJ();
        Debug.LogFormat("[Chalices #{0}] Going to J.", moduleId);
      }
    }

    void LetterY(){
      if (Conditions[18]) {
        LetterM();
        Debug.LogFormat("[Chalices #{0}] Going to M.", moduleId);
      }
      else {
        LetterL();
        Debug.LogFormat("[Chalices #{0}] Going to L.", moduleId);
      }
    }

    void LetterZ(){
      if (Conditions[19]) {
        LetterK();
        Debug.LogFormat("[Chalices #{0}] Going to K.", moduleId);
      }
      else {
        LetterO();
        Debug.LogFormat("[Chalices #{0}] Going to O.", moduleId);
      }
    }

    void LetterI(){
      if (Conditions[8]) {
        AnswerToIsAmazing[2] = true;
        Debug.LogFormat("[Chalices #{0}] You should drink from chalice 3.", moduleId);
      }
      else {
        AnswerToIsAmazing[1] = true;
        Debug.LogFormat("[Chalices #{0}] You should drink from chalice 2.", moduleId);
      }
    }

    void LetterJ(){
      if (Conditions[9]) {
        AnswerToIsAmazing[0] = true;
        Debug.LogFormat("[Chalices #{0}] You should drink from chalice 1.", moduleId);
      }
      else {
        AnswerToIsAmazing[7] = true;
        Debug.LogFormat("[Chalices #{0}] You should drink from chalice 8.", moduleId);
      }
    }

    void LetterK(){
      if (Conditions[10]) {
        AnswerToIsAmazing[3] = true;
        Debug.LogFormat("[Chalices #{0}] You should drink from chalice 4.", moduleId);
      }
      else {
        AnswerToIsAmazing[2] = true;
        Debug.LogFormat("[Chalices #{0}] You should drink from chalice 3.", moduleId);
      }
    }

    void LetterL(){
      if (Conditions[11]) {
        AnswerToIsAmazing[6] = true;
        Debug.LogFormat("[Chalices #{0}] You should drink from chalice 7.", moduleId);
      }
      else {
        AnswerToIsAmazing[5] = true;
        Debug.LogFormat("[Chalices #{0}] You should drink from chalice 6.", moduleId);
      }
    }

    void LetterM(){
      if (Conditions[12]) {
        AnswerToIsAmazing[5] = true;
        Debug.LogFormat("[Chalices #{0}] You should drink from chalice 6.", moduleId);
      }
      else {
        AnswerToIsAmazing[4] = true;
        Debug.LogFormat("[Chalices #{0}] You should drink from chalice 5.", moduleId);
      }
    }

    void LetterN(){
      if (Conditions[13]) {
        AnswerToIsAmazing[1] = true;
        Debug.LogFormat("[Chalices #{0}] You should drink from chalice 2.", moduleId);
      }
      else {
        AnswerToIsAmazing[0] = true;
        Debug.LogFormat("[Chalices #{0}] You should drink from chalice 1.", moduleId);
      }
    }

    void LetterO(){
      if (Conditions[14]) {
        AnswerToIsAmazing[4] = true;
        Debug.LogFormat("[Chalices #{0}] You should drink from chalice 5.", moduleId);
      }
      else {
        AnswerToIsAmazing[3] = true;
        Debug.LogFormat("[Chalices #{0}] You should drink from chalice 4.", moduleId);
      }
    }

    void LetterP(){
      if (Conditions[14]) {
        AnswerToIsAmazing[7] = true;
        Debug.LogFormat("[Chalices #{0}] You should drink from chalice 8.", moduleId);
      }
      else {
        AnswerToIsAmazing[6] = true;
        Debug.LogFormat("[Chalices #{0}] You should drink from chalice 7.", moduleId);
      }
    }

    #pragma warning disable 414
    private readonly string TwitchHelpMessage = @"Use !{0} # to select the corresponding chalice.";
    #pragma warning restore 414

    IEnumerator ProcessTwitchCommand(string command) {
      yield return null;
      command = command.Trim();
      if (command == "1") {
        LittleHerobrineIAmCummingInYourMom[0].OnInteract();
        yield break;
      }
      else if (command == "2") {
        LittleHerobrineIAmCummingInYourMom[1].OnInteract();
        yield break;
      }
      else if (command == "3") {
        LittleHerobrineIAmCummingInYourMom[2].OnInteract();
        yield break;
      }
      else if (command == "4") {
        LittleHerobrineIAmCummingInYourMom[3].OnInteract();
        yield break;
      }
      else if (command == "5") {
        LittleHerobrineIAmCummingInYourMom[4].OnInteract();
        yield break;
      }
      else if (command == "6") {
        LittleHerobrineIAmCummingInYourMom[5].OnInteract();
        yield break;
      }
      else if (command == "7") {
        LittleHerobrineIAmCummingInYourMom[6].OnInteract();
        yield break;
      }
      else if (command == "8") {
        LittleHerobrineIAmCummingInYourMom[7].OnInteract();
        yield break;
      }
      else {
        yield return "sendtochaterror Invalid command!";
        yield break;
      }
    }
}
