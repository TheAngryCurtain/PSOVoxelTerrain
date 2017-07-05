using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

public static class Serialization
{
    public static string saveFolderName = "voxelGameSaves";

    public static string SaveLocation(string worldName)
    {
        string saveLocation = saveFolderName + "/" + worldName + "/";

        if (!Directory.Exists(saveLocation))
        {
            Directory.CreateDirectory(saveLocation);
        }

        return saveLocation;
    }

    public static string FileName(Vector3 chunkLocation)
    {
        string fileName = chunkLocation.x + "," + chunkLocation.y + "," + chunkLocation.z + ".bin";
        return fileName;
    }

    public static void SaveChunk(Chunk chunk)
    {
        Save save = new Save(chunk);
        if (save.blocks.Count == 0)
        {
            return;
        }

        string saveFile = SaveLocation(chunk.world.worldName);
        saveFile += FileName(chunk.worldPos);

        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(saveFile, FileMode.Create, FileAccess.Write, FileShare.None);
        formatter.Serialize(stream, chunk.blocks);
        stream.Close();
    }

    public static bool Load(Chunk chunk)
    {
        string saveFile = SaveLocation(chunk.world.worldName);
        saveFile += FileName(chunk.worldPos);

        if (!File.Exists(saveFile))
            return false;

        IFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(saveFile, FileMode.Open);

        Save save = (Save)formatter.Deserialize(stream);
        foreach (var block in save.blocks)
        {
            chunk.blocks[(int)block.Key.x, (int)block.Key.y, (int)block.Key.z] = block.Value;
        }
        stream.Close();

        return true;
    }
}
