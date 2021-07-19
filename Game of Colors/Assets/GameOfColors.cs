using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using KModkit;
using Rnd = UnityEngine.Random;

public class GameOfColors : MonoBehaviour {

   public KMBombInfo Bomb;
   public KMAudio Audio;
   public KMSelectable[] Buttons;
   public Material[] Colors;
   public GameObject Background;
   public Material WhiteNonLit;
   public GameObject ColorIndicator;
   public KMSelectable[] Arrows;
   public KMSelectable Submit;

   static int moduleIdCounter = 1;
   int moduleId;
   private bool moduleSolved;

   int[] Grid = new int[25];
   bool[][] GOLGrids = new bool[][] {
      new bool[25],
      new bool[25],
      new bool[25]
   };

   bool[][] Goal = new bool[][] {
      new bool[25],
      new bool[25],
      new bool[25]
   };
   int[] FinalAnswer = new int[25];
   int[] Submission = new int[25];
   int ColorIndex;
   //bool CMYK;
   bool Animating;

   void Awake () {
      moduleId = moduleIdCounter++;

      foreach (KMSelectable Button in Buttons) {
         Button.OnInteract += delegate () { ButtonPress(Button); return false; };
      }

      foreach (KMSelectable Arrow in Arrows) {
         Arrow.OnInteract += delegate () { ArrowPress(Arrow); return false; };
      }

      Submit.OnInteract += delegate () { SubmitPress(); return false; };

   }

   void SubmitPress () {
      //StartCoroutine(Solve());
      //return;
      for (int i = 0; i < 25; i++) {
         //Debug.Log(i.ToString() + " is ");
         //Debug.Log("submitted " + FinalAnswer[i].ToString());
         //Debug.Log("submitted " + Submission[i].ToString());
         if (FinalAnswer[i] != Submission[i]) {
            StartCoroutine(Strike());
            return;
         }
      }
      StartCoroutine(Solve());
   }

   IEnumerator Strike () {
      StartCoroutine(Clear());
      while (Animating) {
         yield return null;
      }
      yield return new WaitForSeconds(1f);
      Buttons[12].GetComponent<MeshRenderer>().material = Colors[1];
      yield return new WaitForSeconds(.1f);
      Buttons[6].GetComponent<MeshRenderer>().material = Colors[1];
      Buttons[8].GetComponent<MeshRenderer>().material = Colors[1];
      Buttons[16].GetComponent<MeshRenderer>().material = Colors[1];
      Buttons[18].GetComponent<MeshRenderer>().material = Colors[1];
      yield return new WaitForSeconds(.1f);
      Buttons[0].GetComponent<MeshRenderer>().material = Colors[1];
      Buttons[4].GetComponent<MeshRenderer>().material = Colors[1];
      Buttons[7].GetComponent<MeshRenderer>().material = Colors[1];
      Buttons[11].GetComponent<MeshRenderer>().material = Colors[1];
      Buttons[13].GetComponent<MeshRenderer>().material = Colors[1];
      Buttons[17].GetComponent<MeshRenderer>().material = Colors[1];
      Buttons[20].GetComponent<MeshRenderer>().material = Colors[1];
      Buttons[24].GetComponent<MeshRenderer>().material = Colors[1];
      yield return new WaitForSeconds(.1f);
      Buttons[1].GetComponent<MeshRenderer>().material = Colors[1];
      Buttons[3].GetComponent<MeshRenderer>().material = Colors[1];
      Buttons[5].GetComponent<MeshRenderer>().material = Colors[1];
      Buttons[9].GetComponent<MeshRenderer>().material = Colors[1];
      Buttons[15].GetComponent<MeshRenderer>().material = Colors[1];
      Buttons[19].GetComponent<MeshRenderer>().material = Colors[1];
      Buttons[21].GetComponent<MeshRenderer>().material = Colors[1];
      Buttons[23].GetComponent<MeshRenderer>().material = Colors[1];
      yield return new WaitForSeconds(.3f);
      for (int j = 0; j < 3; j++) {
         for (int i = 0; i < 25; i++) {
            Buttons[i].GetComponent<MeshRenderer>().material = Colors[0];
         }
         yield return new WaitForSeconds(.3f);
         Buttons[0].GetComponent<MeshRenderer>().material = Colors[1];
         Buttons[4].GetComponent<MeshRenderer>().material = Colors[1];
         Buttons[7].GetComponent<MeshRenderer>().material = Colors[1];
         Buttons[11].GetComponent<MeshRenderer>().material = Colors[1];
         Buttons[13].GetComponent<MeshRenderer>().material = Colors[1];
         Buttons[17].GetComponent<MeshRenderer>().material = Colors[1];
         Buttons[20].GetComponent<MeshRenderer>().material = Colors[1];
         Buttons[24].GetComponent<MeshRenderer>().material = Colors[1];
         Buttons[1].GetComponent<MeshRenderer>().material = Colors[1];
         Buttons[3].GetComponent<MeshRenderer>().material = Colors[1];
         Buttons[5].GetComponent<MeshRenderer>().material = Colors[1];
         Buttons[9].GetComponent<MeshRenderer>().material = Colors[1];
         Buttons[15].GetComponent<MeshRenderer>().material = Colors[1];
         Buttons[19].GetComponent<MeshRenderer>().material = Colors[1];
         Buttons[21].GetComponent<MeshRenderer>().material = Colors[1];
         Buttons[23].GetComponent<MeshRenderer>().material = Colors[1];
         Buttons[12].GetComponent<MeshRenderer>().material = Colors[1];
         Buttons[6].GetComponent<MeshRenderer>().material = Colors[1];
         Buttons[8].GetComponent<MeshRenderer>().material = Colors[1];
         Buttons[16].GetComponent<MeshRenderer>().material = Colors[1];
         Buttons[18].GetComponent<MeshRenderer>().material = Colors[1];
         yield return new WaitForSeconds(.3f);
      }
      GetComponent<KMBombModule>().HandleStrike();
      Buttons[0].GetComponent<MeshRenderer>().material = Colors[0];
      yield return new WaitForSeconds(.1f);
      Buttons[1].GetComponent<MeshRenderer>().material = Colors[0];
      Buttons[5].GetComponent<MeshRenderer>().material = Colors[0];
      yield return new WaitForSeconds(.1f);
      Buttons[2].GetComponent<MeshRenderer>().material = Colors[0];
      Buttons[6].GetComponent<MeshRenderer>().material = Colors[0];
      Buttons[10].GetComponent<MeshRenderer>().material = Colors[0];
      yield return new WaitForSeconds(.1f);
      Buttons[3].GetComponent<MeshRenderer>().material = Colors[0];
      Buttons[7].GetComponent<MeshRenderer>().material = Colors[0];
      Buttons[11].GetComponent<MeshRenderer>().material = Colors[0];
      Buttons[15].GetComponent<MeshRenderer>().material = Colors[0];
      yield return new WaitForSeconds(.1f);
      Buttons[4].GetComponent<MeshRenderer>().material = Colors[0];
      Buttons[8].GetComponent<MeshRenderer>().material = Colors[0];
      Buttons[12].GetComponent<MeshRenderer>().material = Colors[0];
      Buttons[16].GetComponent<MeshRenderer>().material = Colors[0];
      Buttons[20].GetComponent<MeshRenderer>().material = Colors[0];
      yield return new WaitForSeconds(.1f);
      Buttons[9].GetComponent<MeshRenderer>().material = Colors[0];
      Buttons[13].GetComponent<MeshRenderer>().material = Colors[0];
      Buttons[17].GetComponent<MeshRenderer>().material = Colors[0];
      Buttons[21].GetComponent<MeshRenderer>().material = Colors[0];
      yield return new WaitForSeconds(.1f);
      Buttons[14].GetComponent<MeshRenderer>().material = Colors[0];
      Buttons[18].GetComponent<MeshRenderer>().material = Colors[0];
      Buttons[22].GetComponent<MeshRenderer>().material = Colors[0];
      yield return new WaitForSeconds(.1f);
      Buttons[19].GetComponent<MeshRenderer>().material = Colors[0];
      Buttons[23].GetComponent<MeshRenderer>().material = Colors[0];
      yield return new WaitForSeconds(.1f);
      Buttons[24].GetComponent<MeshRenderer>().material = Colors[0];
      yield return new WaitForSeconds(.1f);
      Buttons[0].GetComponent<MeshRenderer>().material = Colors[7 - Grid[0]];
      yield return new WaitForSeconds(.1f);
      Buttons[1].GetComponent<MeshRenderer>().material = Colors[7 - Grid[1]];
      Buttons[5].GetComponent<MeshRenderer>().material = Colors[7 - Grid[5]];
      yield return new WaitForSeconds(.1f);
      Buttons[2].GetComponent<MeshRenderer>().material = Colors[7 - Grid[2]];
      Buttons[6].GetComponent<MeshRenderer>().material = Colors[7 - Grid[6]];
      Buttons[10].GetComponent<MeshRenderer>().material = Colors[7 - Grid[10]];
      yield return new WaitForSeconds(.1f);
      Buttons[3].GetComponent<MeshRenderer>().material = Colors[7 - Grid[3]];
      Buttons[7].GetComponent<MeshRenderer>().material = Colors[7 - Grid[7]];
      Buttons[11].GetComponent<MeshRenderer>().material = Colors[7 - Grid[11]];
      Buttons[15].GetComponent<MeshRenderer>().material = Colors[7 - Grid[15]];
      yield return new WaitForSeconds(.1f);
      Buttons[4].GetComponent<MeshRenderer>().material = Colors[7 - Grid[4]];
      Buttons[8].GetComponent<MeshRenderer>().material = Colors[7 - Grid[8]];
      Buttons[12].GetComponent<MeshRenderer>().material = Colors[7 - Grid[12]];
      Buttons[16].GetComponent<MeshRenderer>().material = Colors[7 - Grid[16]];
      Buttons[20].GetComponent<MeshRenderer>().material = Colors[7 - Grid[20]];
      yield return new WaitForSeconds(.1f);
      Buttons[9].GetComponent<MeshRenderer>().material = Colors[7 - Grid[9]];
      Buttons[13].GetComponent<MeshRenderer>().material = Colors[7 - Grid[13]];
      Buttons[17].GetComponent<MeshRenderer>().material = Colors[7 - Grid[17]];
      Buttons[21].GetComponent<MeshRenderer>().material = Colors[7 - Grid[21]];
      yield return new WaitForSeconds(.1f);
      Buttons[14].GetComponent<MeshRenderer>().material = Colors[7 - Grid[14]];
      Buttons[18].GetComponent<MeshRenderer>().material = Colors[7 - Grid[18]];
      Buttons[22].GetComponent<MeshRenderer>().material = Colors[7 - Grid[22]];
      yield return new WaitForSeconds(.1f);
      Buttons[19].GetComponent<MeshRenderer>().material = Colors[7 - Grid[19]];
      Buttons[23].GetComponent<MeshRenderer>().material = Colors[7 - Grid[23]];
      yield return new WaitForSeconds(.1f);
      Buttons[24].GetComponent<MeshRenderer>().material = Colors[7 - Grid[24]];
   }

   IEnumerator Clear () {
      Buttons[0].GetComponent<MeshRenderer>().material = Colors[0];
      yield return new WaitForSeconds(.1f);
      Buttons[1].GetComponent<MeshRenderer>().material = Colors[0];
      Buttons[5].GetComponent<MeshRenderer>().material = Colors[0];
      yield return new WaitForSeconds(.1f);
      Buttons[2].GetComponent<MeshRenderer>().material = Colors[0];
      Buttons[6].GetComponent<MeshRenderer>().material = Colors[0];
      Buttons[10].GetComponent<MeshRenderer>().material = Colors[0];
      yield return new WaitForSeconds(.1f);
      Buttons[3].GetComponent<MeshRenderer>().material = Colors[0];
      Buttons[7].GetComponent<MeshRenderer>().material = Colors[0];
      Buttons[11].GetComponent<MeshRenderer>().material = Colors[0];
      Buttons[15].GetComponent<MeshRenderer>().material = Colors[0];
      yield return new WaitForSeconds(.1f);
      Buttons[4].GetComponent<MeshRenderer>().material = Colors[0];
      Buttons[8].GetComponent<MeshRenderer>().material = Colors[0];
      Buttons[12].GetComponent<MeshRenderer>().material = Colors[0];
      Buttons[16].GetComponent<MeshRenderer>().material = Colors[0];
      Buttons[20].GetComponent<MeshRenderer>().material = Colors[0];
      yield return new WaitForSeconds(.1f);
      Buttons[9].GetComponent<MeshRenderer>().material = Colors[0];
      Buttons[13].GetComponent<MeshRenderer>().material = Colors[0];
      Buttons[17].GetComponent<MeshRenderer>().material = Colors[0];
      Buttons[21].GetComponent<MeshRenderer>().material = Colors[0];
      yield return new WaitForSeconds(.1f);
      Buttons[14].GetComponent<MeshRenderer>().material = Colors[0];
      Buttons[18].GetComponent<MeshRenderer>().material = Colors[0];
      Buttons[22].GetComponent<MeshRenderer>().material = Colors[0];
      yield return new WaitForSeconds(.1f);
      Buttons[19].GetComponent<MeshRenderer>().material = Colors[0];
      Buttons[23].GetComponent<MeshRenderer>().material = Colors[0];
      yield return new WaitForSeconds(.1f);
      Buttons[24].GetComponent<MeshRenderer>().material = Colors[0];
      yield return new WaitForSeconds(.1f);
   }

   IEnumerator Solve () {
      StartCoroutine(Clear());
      while (Animating) {
         yield return null;
      }
      GetComponent<KMBombModule>().HandlePass();
   }

   void ArrowPress (KMSelectable Arrow) {
      if (Arrow == Arrows[0]) {
         ColorIndex++;
         ColorIndex %= 8;
      }
      else {
         ColorIndex--;
         if (ColorIndex < 0) {
            ColorIndex += 8;
         }
      }
      ColorIndicator.GetComponent<MeshRenderer>().material = Colors[7 - ColorIndex];
      /*if (CMYK) {
         
      }
      else {
         ColorIndicator.GetComponent<MeshRenderer>().material = Colors[ColorIndex];
      }*/

   }

   void Start () { //K R G Y B M C W
      for (int i = 0; i < 25; i++) {
         if (Rnd.Range(0, 5) < 2) { // Red/Cyan
            GOLGrids[0][i] = true;
            Grid[i]++;
         }
         if (Rnd.Range(0, 5) < 2) { // Green/Magenta
            GOLGrids[1][i] = true;
            Grid[i] += 2;
         }
         if (Rnd.Range(0, 5) < 2) { // Blue/Yellow
            GOLGrids[2][i] = true;
            Grid[i] += 4;
         }
         Submission[i] = Grid[i];
      }
      /*if (Rnd.Range(0, 2) == 0) {
         for (int i = 0; i < 25; i++) {
            Buttons[i].GetComponent<MeshRenderer>().material = Colors[Grid[i]];
         }
         ColorIndicator.GetComponent<MeshRenderer>().material = Colors[0];
      }*/
      //else {
      //CMYK = true;
      ColorIndicator.GetComponent<MeshRenderer>().material = Colors[7];
      for (int i = 0; i < 25; i++) {
         Buttons[i].GetComponent<MeshRenderer>().material = Colors[7 - Grid[i]];
      }
      //}
      for (int i = 0; i < 3; i++) {
         Debug.LogFormat("[Game of Color #{0}] The grid for {1} is:", moduleId, true ? new string[] { "red", "green", "blue" }[i] : new string[] { "cyan", "magenta", "yellow" }[i]);
         Debug.LogFormat("[Game of Color #{0}] {1}{2}{3}{4}{5}", moduleId, GOLGrids[i][0] ? "*" : ".", GOLGrids[i][1] ? "*" : ".", GOLGrids[i][2] ? "*" : ".", GOLGrids[i][3] ? "*" : ".", GOLGrids[i][4] ? "*" : ".");
         Debug.LogFormat("[Game of Color #{0}] {1}{2}{3}{4}{5}", moduleId, GOLGrids[i][5] ? "*" : ".", GOLGrids[i][6] ? "*" : ".", GOLGrids[i][7] ? "*" : ".", GOLGrids[i][8] ? "*" : ".", GOLGrids[i][9] ? "*" : ".");
         Debug.LogFormat("[Game of Color #{0}] {1}{2}{3}{4}{5}", moduleId, GOLGrids[i][10] ? "*" : ".", GOLGrids[i][11] ? "*" : ".", GOLGrids[i][12] ? "*" : ".", GOLGrids[i][13] ? "*" : ".", GOLGrids[i][14] ? "*" : ".");
         Debug.LogFormat("[Game of Color #{0}] {1}{2}{3}{4}{5}", moduleId, GOLGrids[i][15] ? "*" : ".", GOLGrids[i][16] ? "*" : ".", GOLGrids[i][17] ? "*" : ".", GOLGrids[i][18] ? "*" : ".", GOLGrids[i][19] ? "*" : ".");
         Debug.LogFormat("[Game of Color #{0}] {1}{2}{3}{4}{5}", moduleId, GOLGrids[i][20] ? "*" : ".", GOLGrids[i][21] ? "*" : ".", GOLGrids[i][22] ? "*" : ".", GOLGrids[i][23] ? "*" : ".", GOLGrids[i][24] ? "*" : ".");
      }
      for (int i = 0; i < 3; i++) {
         GOLIteration(5, 5, i);
      }
      for (int i = 0; i < 3; i++) {
         Debug.LogFormat("[Game of Color #{0}] The goal grid for {1} is:", moduleId, true ? new string[] { "red", "green", "blue" }[i] : new string[] { "cyan", "magenta", "yellow" }[i]);
         Debug.LogFormat("[Game of Color #{0}] {1}{2}{3}{4}{5}", moduleId, Goal[i][0] ? "*" : ".", Goal[i][1] ? "*" : ".", Goal[i][2] ? "*" : ".", Goal[i][3] ? "*" : ".", Goal[i][4] ? "*" : ".");
         Debug.LogFormat("[Game of Color #{0}] {1}{2}{3}{4}{5}", moduleId, Goal[i][5] ? "*" : ".", Goal[i][6] ? "*" : ".", Goal[i][7] ? "*" : ".", Goal[i][8] ? "*" : ".", Goal[i][9] ? "*" : ".");
         Debug.LogFormat("[Game of Color #{0}] {1}{2}{3}{4}{5}", moduleId, Goal[i][10] ? "*" : ".", Goal[i][11] ? "*" : ".", Goal[i][12] ? "*" : ".", Goal[i][13] ? "*" : ".", Goal[i][14] ? "*" : ".");
         Debug.LogFormat("[Game of Color #{0}] {1}{2}{3}{4}{5}", moduleId, Goal[i][15] ? "*" : ".", Goal[i][16] ? "*" : ".", Goal[i][17] ? "*" : ".", Goal[i][18] ? "*" : ".", Goal[i][19] ? "*" : ".");
         Debug.LogFormat("[Game of Color #{0}] {1}{2}{3}{4}{5}", moduleId, Goal[i][20] ? "*" : ".", Goal[i][21] ? "*" : ".", Goal[i][22] ? "*" : ".", Goal[i][23] ? "*" : ".", Goal[i][24] ? "*" : ".");
      }
      Debug.LogFormat("[Game of Color #{0}] The final answer is:", moduleId);
      for (int i = 0; i < 25; i++) {
         for (int j = 0; j < 3; j++) {
            if (Goal[j][i]) {
               FinalAnswer[i] += (int) Math.Pow(2, j);
            }
         }
      }
      //if (true) {
      Debug.LogFormat("[Game of Color #{0}] {1}{2}{3}{4}{5}", moduleId, "KRGYBMCW"[7 - FinalAnswer[0]], "KRGYBMCW"[7 - FinalAnswer[1]], "KRGYBMCW"[7 - FinalAnswer[2]], "KRGYBMCW"[7 - FinalAnswer[3]], "KRGYBMCW"[7 - FinalAnswer[4]]);
      Debug.LogFormat("[Game of Color #{0}] {1}{2}{3}{4}{5}", moduleId, "KRGYBMCW"[7 - FinalAnswer[5]], "KRGYBMCW"[7 - FinalAnswer[6]], "KRGYBMCW"[7 - FinalAnswer[7]], "KRGYBMCW"[7 - FinalAnswer[8]], "KRGYBMCW"[7 - FinalAnswer[9]]);
      Debug.LogFormat("[Game of Color #{0}] {1}{2}{3}{4}{5}", moduleId, "KRGYBMCW"[7 - FinalAnswer[10]], "KRGYBMCW"[7 - FinalAnswer[11]], "KRGYBMCW"[7 - FinalAnswer[12]], "KRGYBMCW"[7 - FinalAnswer[13]], "KRGYBMCW"[7 - FinalAnswer[14]]);
      Debug.LogFormat("[Game of Color #{0}] {1}{2}{3}{4}{5}", moduleId, "KRGYBMCW"[7 - FinalAnswer[15]], "KRGYBMCW"[7 - FinalAnswer[16]], "KRGYBMCW"[7 - FinalAnswer[17]], "KRGYBMCW"[7 - FinalAnswer[18]], "KRGYBMCW"[7 - FinalAnswer[19]]);
      Debug.LogFormat("[Game of Color #{0}] {1}{2}{3}{4}{5}", moduleId, "KRGYBMCW"[7 - FinalAnswer[20]], "KRGYBMCW"[7 - FinalAnswer[21]], "KRGYBMCW"[7 - FinalAnswer[22]], "KRGYBMCW"[7 - FinalAnswer[23]], "KRGYBMCW"[7 - FinalAnswer[24]]);
      /*}
      else {
         Debug.LogFormat("[Game of Color #{0}] {1}{2}{3}{4}{5}", moduleId, "KRGYBMCW"[FinalAnswer[0]], "KRGYBMCW"[FinalAnswer[1]], "KRGYBMCW"[FinalAnswer[2]], "KRGYBMCW"[FinalAnswer[3]], "KRGYBMCW"[FinalAnswer[4]]);
         Debug.LogFormat("[Game of Color #{0}] {1}{2}{3}{4}{5}", moduleId, "KRGYBMCW"[FinalAnswer[5]], "KRGYBMCW"[FinalAnswer[6]], "KRGYBMCW"[FinalAnswer[7]], "KRGYBMCW"[FinalAnswer[8]], "KRGYBMCW"[FinalAnswer[9]]);
         Debug.LogFormat("[Game of Color #{0}] {1}{2}{3}{4}{5}", moduleId, "KRGYBMCW"[FinalAnswer[10]], "KRGYBMCW"[FinalAnswer[11]], "KRGYBMCW"[FinalAnswer[12]], "KRGYBMCW"[FinalAnswer[13]], "KRGYBMCW"[FinalAnswer[14]]);
         Debug.LogFormat("[Game of Color #{0}] {1}{2}{3}{4}{5}", moduleId, "KRGYBMCW"[FinalAnswer[15]], "KRGYBMCW"[FinalAnswer[16]], "KRGYBMCW"[FinalAnswer[17]], "KRGYBMCW"[FinalAnswer[18]], "KRGYBMCW"[FinalAnswer[19]]);
         Debug.LogFormat("[Game of Color #{0}] {1}{2}{3}{4}{5}", moduleId, "KRGYBMCW"[FinalAnswer[20]], "KRGYBMCW"[FinalAnswer[21]], "KRGYBMCW"[FinalAnswer[22]], "KRGYBMCW"[FinalAnswer[23]], "KRGYBMCW"[FinalAnswer[24]]);
      }*/
   }

   void ButtonPress (KMSelectable Button) {
      for (int i = 0; i < 25; i++) {
         if (Button == Buttons[i]) {
            Submission[i] = ColorIndex;
            //if (CMYK) {
            Button.GetComponent<MeshRenderer>().material = Colors[7 - ColorIndex];
            //}
            //else {
            //   Button.GetComponent<MeshRenderer>().material = Colors[ColorIndex];
            //}
         }
      }
   }

   void GOLIteration (int Width, int Height, int Color) {
      int WhiteSquares = 0;
      for (int i = 0; i < Width * Height; i++) {
         //Debug.Log(i);
         if (i == 0) { //TL
            WhiteSquares += (GOLGrids[Color][i + 1] ? 1 : 0) + (GOLGrids[Color][i + 1 + Width] ? 1 : 0) + (GOLGrids[Color][Width] ? 1 : 0);
         }
         else if (i / Width == 0 && i % Height == Height - 1) { //TR
            WhiteSquares += (GOLGrids[Color][i - 1] ? 1 : 0) + (GOLGrids[Color][i - 1 + Width] ? 1 : 0) + (GOLGrids[Color][i + Width] ? 1 : 0);
         }
         else if (i == Width * Height - 1) {//BR
            WhiteSquares += (GOLGrids[Color][i - 1] ? 1 : 0) + (GOLGrids[Color][i - 1 - Width] ? 1 : 0) + (GOLGrids[Color][i - Width] ? 1 : 0);
         }
         else if (i == Width * (Height - 1)) {//BL
            WhiteSquares += (GOLGrids[Color][i + 1] ? 1 : 0) + (GOLGrids[Color][i + 1 - Width] ? 1 : 0) + (GOLGrids[Color][i - Width] ? 1 : 0);
         }
         else if (i / Width == 0) {//Top row
            WhiteSquares += (GOLGrids[Color][i - 1] ? 1 : 0) + (GOLGrids[Color][i - 1 + Width] ? 1 : 0) + (GOLGrids[Color][i + Width] ? 1 : 0) + (GOLGrids[Color][i + Width + 1] ? 1 : 0) + (GOLGrids[Color][i + 1] ? 1 : 0);
         }
         else if (i % Height == 0) {//Left column
            WhiteSquares += (GOLGrids[Color][i - Width] ? 1 : 0) + (GOLGrids[Color][i + 1 - Width] ? 1 : 0) + (GOLGrids[Color][i + 1] ? 1 : 0) + (GOLGrids[Color][i + 1 + Width] ? 1 : 0) + (GOLGrids[Color][i + Width] ? 1 : 0);
         }
         else if (i % Height == Height - 1) {//Right Column
            WhiteSquares += (GOLGrids[Color][i - Width] ? 1 : 0) + (GOLGrids[Color][i - 1 - Width] ? 1 : 0) + (GOLGrids[Color][i - 1] ? 1 : 0) + (GOLGrids[Color][i - 1 + Width] ? 1 : 0) + (GOLGrids[Color][i + Width] ? 1 : 0);
         }
         else if (i / Width == Width - 1) { //Bottom row
            WhiteSquares += (GOLGrids[Color][i - 1] ? 1 : 0) + (GOLGrids[Color][i - 1 - Width] ? 1 : 0) + (GOLGrids[Color][i - Width] ? 1 : 0) + (GOLGrids[Color][i + 1 - Width] ? 1 : 0) + (GOLGrids[Color][i + 1] ? 1 : 0);
         }
         else {//Middle
            WhiteSquares += (GOLGrids[Color][i - 1 - Width] ? 1 : 0) + (GOLGrids[Color][i - Width] ? 1 : 0) + (GOLGrids[Color][i + 1 - Width] ? 1 : 0) + (GOLGrids[Color][i + 1] ? 1 : 0) + (GOLGrids[Color][i + 1 + Width] ? 1 : 0) + (GOLGrids[Color][i + Width] ? 1 : 0) + (GOLGrids[Color][i - 1 + Width] ? 1 : 0) + (GOLGrids[Color][i - 1] ? 1 : 0);
         }
         if (GOLGrids[Color][i]) {
            if (WhiteSquares == 2 || WhiteSquares == 3) {
               Goal[Color][i] = true;
            }
            else {
               Goal[Color][i] = false;
            }
         }
         else if (WhiteSquares == 3) {
            Goal[Color][i] = true;
         }
         else {
            Goal[Color][i] = false;
         }
         WhiteSquares = 0;
      }
   }

#pragma warning disable 414
   private readonly string TwitchHelpMessage = @"Use !{0} ABCDE/12345;KRGBCMYW to set that coordinate to that color. Chain coordinates with semicolons. Spaces are not permitted. Use !{0} Submit to submit";
#pragma warning restore 414

   IEnumerator ProcessTwitchCommand (string Command) {
      Command = Command.Trim().ToUpper();
      if (Command == "SUBMIT") {
         Submit.OnInteract();
         yield break;
      }
      string[] Parameters = Command.Split(';');
      List<int> Valid = new List<int> { };
      for (int i = 0; i < Parameters.Length - 1; i++) {
         if ("KRGBCMY".Contains(Parameters[i])) {
            for (int j = 0; j < Valid.Count(); j++) {
               Buttons[j].OnInteract();
            }
         }
         else if (!"ABCDE".Contains(Parameters)) {

         }

         Valid.Add();
      }
      yield return null;
   }

   IEnumerator TwitchHandleForcedSolve () {
      yield return null;
   }
}