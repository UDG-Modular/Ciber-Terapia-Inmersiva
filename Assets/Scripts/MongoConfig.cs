using System.IO;
using UnityEngine;

public static class MongoConfig
{
    private static string configFilePath = Path.Combine(Application.streamingAssetsPath, "mongo_config.txt");

    public static string GetConnectionString()
    {
        if (File.Exists(configFilePath))
        {
            return File.ReadAllText(configFilePath).Trim();
        }
        else
        {
            Debug.LogError("No se encontró el archivo de configuración: " + configFilePath);
            return string.Empty;
        }
    }
}
