using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using KModkit;

public class TheArena : MonoBehaviour {

    public KMBombInfo Bomb;
    public KMAudio Audio;
    public KMSelectable[] Weed;
    public TextMesh Aids;

    static int moduleIdCounter = 1;
    int moduleId;
    private bool moduleSolved;
    int Wonk = 0;
    string fuck = "";
    int SwordCheck = 0;
    int DefendCheck = 0;
    int CheckCheck = 0;
    int Sword = 0;
    int Defend = 0;
    int Check = 0;
    int kmjuyhghmgjhmgjuy = 0;

    void Awake () {
        moduleId = moduleIdCounter++;

        foreach (KMSelectable Button in Weed) {
            Button.OnInteract += delegate () { ButtonPress(Button); return false; };
        }
    }

    void Start () {
      StartCoroutine(Fuck());
    }

    void ButtonPress(KMSelectable Button) {
      for (int i = 0; i < 3; i++) {
        if (Button == Weed[i]) {
          if (Button == Weed[(kmjuyhghmgjhmgjuy + 1) % 3]) {
            GetComponent<KMBombModule>().HandlePass();
          }
          else {
            GetComponent<KMBombModule>().HandleStrike();
            StartCoroutine(Fuck());
          }
        }
      }
    }
    IEnumerator Fuck(){
      Wonk = UnityEngine.Random.Range(0,10);
      Aids.text = Wonk.ToString();
      fuck = Bomb.GetSerialNumber();
      for (int i = 0; i < 6; i++) {
        if (fuck[i] == 'S' || fuck[i] == 'W' || fuck[i] == '0' || fuck[i] == 'R' || fuck[i] == 'D') {
          SwordCheck += 1;
        }
      }
      for (int i = 0; i < 6; i++) {
        if (fuck[i] == 'S' || fuck[i] == 'H' || fuck[i] == '1' || fuck[i] == 'E' || fuck[i] == 'L' || fuck[i] == 'D') {
          DefendCheck += 1;
        }
      }
      for (int i = 0; i < 6; i++) {
        if (fuck[i] == 'S' || fuck[i] == 'W' || fuck[i] == '0' || fuck[i] == 'R' || fuck[i] == 'D') {
          CheckCheck += 1;
        }
      }
      if (SwordCheck == 1) {
        Sword += 1;
        Debug.LogFormat("[Mental Math #{0}] There is one character in SW0RD. Attack now has {1} point(s).", moduleId, Sword);
      }
      if (DefendCheck >= 2) {
        Defend += 1;
        Debug.LogFormat("[Mental Math #{0}] There are two characters in SH13LD. Defence now has {1} point(s).", moduleId, Defend);
      }
      if (CheckCheck == 1) {
        Check += 1;
        Debug.LogFormat("[Mental Math #{0}] There is one character in CA5H. Grab now has {1} point(s).", moduleId, Check);
      }
      if (Bomb.IsIndicatorOn("BOB")) {
        Sword += 1;
        Debug.LogFormat("[Mental Math #{0}] There is a lit BOB. Attack now has {1} point(s).", moduleId, Sword);
      }
      if (Bomb.IsIndicatorOn("TRN")) {
        Defend += 1;
        Debug.LogFormat("[Mental Math #{0}] There is a lit TRN. Defence now has {1} point(s).", moduleId, Defend);
      }
      if (Bomb.IsIndicatorOn("CAR")) {
        Check += 1;
        Debug.LogFormat("[Mental Math #{0}] There is a lit CAR. Grab now has {1} point(s).", moduleId, Check);
      }
      if (Bomb.GetBatteryHolderCount() != 0 && Bomb.GetBatteryCount() != 0) {
        if (Bomb.GetBatteryCount() / Bomb.GetBatteryHolderCount() == 2) {
          Sword += 1;
          Debug.LogFormat("[Mental Math #{0}] All batteries are double A. Attack now has {1} point(s).", moduleId, Sword);
        }
      }
      if (Bomb.GetBatteryCount() % 2 == 0) {
        Defend += 1;
        Debug.LogFormat("[Mental Math #{0}] There is an even amount of batteries. Defence now has {1} point(s).", moduleId, Defend);
      }
      if (Bomb.GetSolvableModuleNames().Contains("Splitting The Loot")) {
        Check += 1;
        Debug.LogFormat("[Mental Math #{0}] Splitting the Loot is on the bomb. Grab now has {1} point(s).", moduleId, Check);
      }
      if (Bomb.IsPortPresent(Port.StereoRCA)) {
        Sword += 1;
        Debug.LogFormat("[Mental Math #{0}] There is a Stereo RCA port. Attack now has {1} point(s).", moduleId, Sword);
      }
      if (Bomb.IsPortPresent(Port.Parallel)) {
        Defend += 1;
        Debug.LogFormat("[Mental Math #{0}] There is a parallel port. Defence now has {1} point(s).", moduleId, Defend);
      }
      if (Bomb.IsPortPresent(Port.RJ45)) {
        Check += 1;
        Debug.LogFormat("[Mental Math #{0}] There is an RJ-45 port present. Grab now has {1} point(s).", moduleId, Check);
      }
      if (Wonk == 2 || Wonk == 3 || Wonk == 5 || Wonk == 7) {
        Sword += 1;
        Debug.LogFormat("[Mental Math #{0}] The number is prime. Attack now has {1} point(s).", moduleId, Sword);
      }
      if (Wonk == 1 || Wonk == 4 || Wonk == 9) {
        Defend += 1;
        Debug.LogFormat("[Mental Math #{0}] The number is square. Defence now has {1} point(s).", moduleId, Defend);
      }
      if (Wonk == 6 || Wonk == 8 || Wonk == 0) {
        Check += 1;
        Debug.LogFormat("[Mental Math #{0}] The number is neither square nor prime. Grab now has {1} point(s).", moduleId, Check);
      }
      if (Sword > Defend && Sword > Check) { //Sword beats all
        kmjuyhghmgjhmgjuy = 0;
        Debug.LogFormat("[Mental Math #{0}] The enemy is attacking.", moduleId);
      }
      else if (Defend > Sword && Defend > Check) { //Defend beats all
        kmjuyhghmgjhmgjuy = 1;
        Debug.LogFormat("[Mental Math #{0}] The enemy is defending.", moduleId);
      }
      else if (Check > Defend && Check > Sword) { //Check beats all
        kmjuyhghmgjhmgjuy = 2;
        Debug.LogFormat("[Mental Math #{0}] The enemy is grabbing loot.", moduleId);
      }
      else if (Sword >= Defend && Sword != Check) { //Sword is tied with defend
        kmjuyhghmgjhmgjuy = 2;
        Debug.LogFormat("[Mental Math #{0}] The enemy is grabbing loot.", moduleId);
      }
      else if (Sword >= Check && Defend != Check) { //Sword is tied with check
        kmjuyhghmgjhmgjuy = 1;
        Debug.LogFormat("[Mental Math #{0}] The enemy is defending.", moduleId);
      }
      else if (Defend >= Check && Sword != Check) { //Defend is tied with check
        kmjuyhghmgjhmgjuy = 0;
        Debug.LogFormat("[Mental Math #{0}] The enemy is attacking.", moduleId);
      }
      else {
        kmjuyhghmgjhmgjuy = 1;
        Debug.LogFormat("[Mental Math #{0}] There was a tie, you should grab the money.", moduleId);
      }
      yield return null;
    }
}
