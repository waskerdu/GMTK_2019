using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

/// <summary>
/// Static class used to serialize a class or struct to a file on the drive.
/// <para>
/// Set Folder, directory, and fileType from DataController class file.
/// </para>
/// </summary>
public static class DataController
{
    public static string subFolders = "Saves/";
    public static string directory = subFolders;
    public static string fileType = ".sav";

    /// <summary>
    /// Save function for saving to a file.
    /// </summary>
    /// <typeparam name="T">Class or Struct type to be saved.</typeparam>
    /// <param name="dat">Type instance with data to be saved.</param>
    /// <param name="fileName">Name of file to save to. Note: do not include filetype. Filetype is set in DataController file.</param>
    public static void Save<T>(T dat, string fileName)
    {
        string path = fileName + fileType;
        if (!Directory.Exists(directory))
            Directory.CreateDirectory(directory);

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(directory + path);

        bf.Serialize(file, dat);
        file.Close();
    }

    /// <summary>
    /// Load function for loading from a file.
    /// </summary>
    /// <typeparam name="T">Class or Struct type to be loaded.</typeparam>
    /// <param name="fileName">Name of file to load from. Note: do not include filetype. Filetype is set in DataController file.</param>
    /// <returns>Class or Struct instance.</returns>
    public static T Load<T>(string fileName)
    {
        string path = fileName + fileType;
        if (!Directory.Exists(directory))
            Directory.CreateDirectory(directory);

        if (File.Exists(directory + path))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(directory + path, FileMode.Open);

            T dat = (T)bf.Deserialize(file);
            file.Close();
            return dat;
        }

        return default(T);
    }

    /// <summary>
    /// Function for retrieving all save files from the save directory.
    /// </summary>
    /// <returns>List of file names including file types.</returns>
    public static string[] GetAvailableSaveFiles()
    {
        if (!Directory.Exists(directory))
            Directory.CreateDirectory(directory);

        DirectoryInfo d = new DirectoryInfo(directory);
        FileInfo[] files = d.GetFiles("*" + fileType);

        List<string> list = new List<string>();
        foreach (FileInfo file in files)
            list.Add(file.Name.Split('.')[0]);
        return list.ToArray();
    }
}