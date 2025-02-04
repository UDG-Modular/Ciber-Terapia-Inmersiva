using System;
using System.IO;
using UnityEngine;

public class RecordPositionToCSV : MonoBehaviour
{
    // Hardcoded string for the filename
    private string defaultString = "NombreUsuario"; // Replace "Example" with your desired string

    // Time interval for recording position (in seconds)
    public float recordInterval = 1.0f;
    private float timer;

    // File path for the current session
    private string filePath;

    private void Start()
    {
        // Generate the file name based on the current date, time, and default string
        string timestamp = DateTime.Now.ToString("dd-MM-yy-HH-mm");
        string fileName = $"{timestamp}-{defaultString}.csv";

        // Relative path within the Unity project
        string relativePath = Path.Combine("Assets/PlayerData", fileName);

        // Ensure the directory exists
        string directory = Path.GetDirectoryName(relativePath);
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        // Set the file path for this session
        filePath = Path.GetFullPath(relativePath);

        // Create or overwrite the file and write the header
        using (StreamWriter writer = new StreamWriter(filePath, false))
        {
            writer.WriteLine("Time,X,Y,Z");
        }

        Debug.Log("Saving positions to: " + filePath);
    }

    private void Update()
    {
        // Update the timer
        timer += Time.deltaTime;

        // Record the position at regular intervals
        if (timer >= recordInterval)
        {
            RecordPosition();
            timer = 0f; // Reset the timer
        }
    }

    private void RecordPosition()
    {
        // Get the current time and position
        float currentTime = Time.time;
        Vector3 position = transform.position;

        // Append the position to the CSV file
        using (StreamWriter writer = new StreamWriter(filePath, true))
        {
            writer.WriteLine($"{currentTime},{position.x},{position.y},{position.z}");
        }
    }
}
