using System;
using System.Threading.Tasks;
using UnityEngine;
using MongoDB.Driver;
using MongoDB.Bson;

public class RecordPositionToMongo : MonoBehaviour
{
    public string defaultString = "NombreUsuario"; // Identificador del usuario
    public float recordInterval = 1.0f;
    private float timer;
    private IMongoCollection<BsonDocument> collection;
    private IMongoDatabase database;

    private void Start()
    {
        ConnectToMongo();
    }

    private void ConnectToMongo()
    {
        try
        {
            string connectionString = MongoConfig.GetConnectionString();
            if (string.IsNullOrEmpty(connectionString))
            {
                Debug.LogError("La cadena de conexión está vacía. No se puede conectar a MongoDB.");
                return;
            }

            var client = new MongoClient(connectionString);
            database = client.GetDatabase("Coordenadas_Jugador");
            string dateSuffix = DateTime.UtcNow.ToString("yyyy-MM-dd");
            string collectionName = $"{defaultString}-{dateSuffix}";
            collection = database.GetCollection<BsonDocument>(collectionName);
            Debug.Log("Conexión exitosa a MongoDB");
        }
        catch (Exception ex)
        {
            Debug.LogError("Error de conexión a MongoDB: " + ex.Message);
        }
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= recordInterval)
        {
            RecordPositionAsync();
            timer = 0f;
        }
    }

    private async void RecordPositionAsync()
    {
        float currentTime = Time.time;
        Vector3 position = transform.position;

        var document = new BsonDocument
        {
            { "usuario", defaultString },
            { "tiempo", currentTime },
            { "x", position.x },
            { "y", position.y },
            { "z", position.z },
            { "timestamp", DateTime.UtcNow }
        };

        await Task.Run(() => collection.InsertOne(document));
        //Debug.Log("Posición guardada en MongoDB: " + document.ToJson());
    }
}
