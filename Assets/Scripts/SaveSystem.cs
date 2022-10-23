using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem
{
    public static void Save(PlayerFrames[] frames) {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/progress.my";
        FileStream fileStream = new FileStream(path, FileMode.Create);
        Debug.Log(path);
        binaryFormatter.Serialize(fileStream, frames);
        fileStream.Close();
    }

    public static PlayerFrames[] Load() {
        string path = Application.persistentDataPath + "/progress.my";

        if (File.Exists(path)) {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(path, FileMode.Open);
            PlayerFrames[] frames = binaryFormatter.Deserialize(fileStream) as PlayerFrames[];
            fileStream.Close();
            return frames;
        }
        else {
            Debug.Log("No File");
            return null;
        }

    }

    public static void DeleteFile() {
        string path = Application.persistentDataPath + "/progress.my";
        if (File.Exists(path)) {
            File.Delete(path);
        }
        else {
            Debug.Log("No File to delete");
        }
    }
}
