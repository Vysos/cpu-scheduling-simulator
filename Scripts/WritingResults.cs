using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class WritingResults : MonoBehaviour
{
    public static WritingResults Instance;
        
    private Text tempText;
    private GameObject tempObj;

    public long averageResponseTime;
    public long averageTurnaroundTime;
    public long averageWaitingTime;
    public double CPUUtilization;
    public long elapsedTime;
    public double throughput;
    public int numberOfProcesses;
    public List<Process> processes;

    private bool isPrinted = false;

    // Start is called before the first frame update

    private int currentSceneIndex;
    
    void Awake ()   
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy (gameObject);
        }
    }
    
    private void Writer(String name, long metric)
    {
        tempObj = GameObject.Find(name);
        tempText = tempObj.GetComponent<Text>();
        tempText.text += metric;
    }
    
    private void Writer(String name, double metric)
    {
        tempObj = GameObject.Find(name);
        tempText = tempObj.GetComponent<Text>();
        tempText.text += String.Format("{0:F4}", metric);
    }

    public void PrintingResults()
    {
        Writer("Number of Processes Text", numberOfProcesses);
        Writer("Elapsed Time Text", elapsedTime);
        Writer("Throughput Text", throughput);
        Writer("CPU utilization Text",CPUUtilization);
        Writer("Average waiting time Text", averageWaitingTime);
        Writer("Average turnaround time Text", averageTurnaroundTime);
        Writer("Average response time Text", averageResponseTime);
    }
    

    // Update is called once per frame
    void Update()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (currentSceneIndex == 2 && isPrinted == false)
        {
            PrintingResults();
            isPrinted = true;
        }
    }
}
