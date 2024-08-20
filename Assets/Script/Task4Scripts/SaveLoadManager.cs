using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveLoadManager
{
    public static void Save(NeuralNetwork neuralNetwork, string fileName)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        using (FileStream stream = new FileStream(fileName, FileMode.Create))
        {
            formatter.Serialize(stream, neuralNetwork);
        }
    }

    public static NeuralNetwork Load(string fileName)
    {
        if (File.Exists(fileName))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream(fileName, FileMode.Open))
            {
                return (NeuralNetwork)formatter.Deserialize(stream);
            }
        }

        return null;
    }
}
