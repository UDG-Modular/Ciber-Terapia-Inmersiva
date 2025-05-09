using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using MongoDB.Driver;
using MongoDB.Bson;

public class RecordPositionToMongo : MonoBehaviour
{
    private string userName = UserData.UserName;
    private float recordInterval = 1.0f;
    private float timer;
    private IMongoCollection<BsonDocument> collection;
    private IMongoDatabase database;
    private string sceneName;
    private string sessionId;
    private string fecha;

    private async void Start()
    {
        Debug.Log(userName);
        await ConnectToMongo();
    }

    private async Task ConnectToMongo()
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

            sceneName = SceneManager.GetActiveScene().name;
            fecha = DateTime.UtcNow.ToString("yyyy-MM-dd");
            sessionId = UserData.SessionID;

            string collectionName = "SesionesCompletas";
            collection = database.GetCollection<BsonDocument>(collectionName);

            await CrearDocumentoSesionSiNoExiste();
        }
        catch (Exception ex)
        {
            Debug.LogError("Error de conexión a MongoDB: " + ex.Message);
        }
    }

    private async Task CrearDocumentoSesionSiNoExiste()
    {
        var filtro = Builders<BsonDocument>.Filter.Eq("datosSesion.sessionId", sessionId);
        bool existe = await collection.Find(filtro).AnyAsync();

        if (!existe)
        {
            var documento = new BsonDocument
            {
                { "datosSesion", new BsonDocument
                    {
                        { "userName", userName },
                        { "sessionId", sessionId },
                        { "fecha", fecha },
                        { "completada", false },
                        { "escenas", new BsonDocument() },
                        { "horaInicio", DateTime.UtcNow },
                        { "horaFin", BsonNull.Value }
                    }
                },
                { "coordenadas", new BsonDocument
                    {
                        { "escena", new BsonDocument() }
                    }
                }
            };
            await collection.InsertOneAsync(documento);
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

        var startTime = DateTime.UtcNow;

        try
        {
            // Aquí asumo que tienes una referencia a tu documento raíz ya cargado
            var filter = Builders<BsonDocument>.Filter.Eq("datosSesion.sessionId", sessionId);
            var update = Builders<BsonDocument>.Update.Push($"coordenadas.escena.{sceneName}", document);
            await database.GetCollection<BsonDocument>("SesionesCompletas").UpdateOneAsync(filter, update);

            var endTime = DateTime.UtcNow;
            var latencyMs = (endTime - startTime).TotalMilliseconds;
            Debug.Log($"Registro insertado. Latencia: {latencyMs} ms");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error insertando coordenadas: {ex.Message}");
        }
    }


    private async void OnApplicationQuit()
    {
        var filtro = Builders<BsonDocument>.Filter.Eq("datosSesion.sessionId", sessionId);

        var update = Builders<BsonDocument>.Update
            .Set("datosSesion.completada", true)
            .Set("datosSesion.horaFin", DateTime.UtcNow);

        await collection.UpdateOneAsync(filtro, update);
    }
}
