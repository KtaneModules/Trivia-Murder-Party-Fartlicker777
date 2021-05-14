using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using KModkit;
using System.IO;

public class FileCreator : MonoBehaviour {

    //public KMBombInfo Bomb;
    //public KMAudio Audio;

    //Logging
    static int moduleIdCounter = 1;
    int moduleId;
    private bool moduleSolved;

    void Awake () {
        moduleId = moduleIdCounter++;
        /*
        foreach (KMSelectable object in keypad) {
            object.OnInteract += delegate () { keypadPress(object); return false; };
        }
        */

        //button.OnInteract += delegate () { buttonPress(); return false; };

    }

    // Use this for initialization
    void CreateText () {
      string Path = Application.dataPath + "/Log.txt";
      if (!File.Exists(Path)) {
         File.WriteAllText(Path, "I am locked.");
      }
   }

    void Start () {
      CreateText();
   }

    // Update is called once per frame
    void Update () {

    }
}
