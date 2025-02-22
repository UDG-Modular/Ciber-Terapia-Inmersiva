using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using MongoDB.Driver;
using MongoDB.Bson;

public class RecordPositionToMongo : MonoBehaviour
{
    private string userName = UserData.UserName;
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
            string sceneName = SceneManager.GetActiveScene().name;
            string dateSuffix = DateTime.UtcNow.ToString("yyyy-MM-dd");
            string collectionName = $"{userName}-{sceneName}-{dateSuffix}";
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
        Vector3 position = transform.position;
        float currentTime = Time.time;

        var document = new BsonDocument
        {
            { "tiempo", currentTime },
            { "x", position.x },
            { "y", position.y },
            { "z", position.z }
        };

        await Task.Run(() => collection.InsertOne(document));
    }
}
