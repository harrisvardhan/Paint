﻿using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Leap.Unity.LeapPaint_v3 {

  public class FileManager : MonoBehaviour {

    public string localSaveDir = "./Paintings/";

    public string paintingsDirTextPrefix = "";
    public Text paintingsDirText;

    public Action OnShouldRefreshFiles = () => { };

    void Start() {
      if (!Directory.Exists(localSaveDir)) {
        Directory.CreateDirectory(localSaveDir);
      }

      if (paintingsDirText != null) {
        paintingsDirText.text = paintingsDirTextPrefix + Path.GetFullPath(localSaveDir);
      }
    }

    public string[] GetFiles() {
      string[] files = Directory.GetFiles(localSaveDir);
      List<string> jsonFiles = new List<string>(files.Length);
      for (int i = 0; i < files.Length; i++) {
        if (Path.GetExtension(files[i]).Equals(".json")) {
          jsonFiles.Add(files[i]);
        }
      }
      return jsonFiles.ToArray();
    }

    public string NameFromPath(string path) {
      return Path.GetFileName(path);
    }

    public void Save(string fileName, string fileContents) {
      using (StreamWriter writer = new StreamWriter(Path.Combine(localSaveDir, fileName), false)) {
        writer.Write(fileContents);
      }
    
      OnShouldRefreshFiles();
    }

    public string Load(string fileName) {
      string json = "";
      using (StreamReader reader = new StreamReader(Path.Combine(localSaveDir, fileName))) {
        json = reader.ReadToEnd();
      }
      return json;
    }

  }


}