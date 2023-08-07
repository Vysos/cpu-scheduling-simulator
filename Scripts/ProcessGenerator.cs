using System.Collections.Generic;
using UnityEngine;

public class ProcessGenerator : MonoBehaviour
{
    public static ProcessGenerator Instance;
    int numberOfProcesses = 10000;
    public List<Process> processes = new List<Process>();

    
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

    
    public void RequestGenerator()
    {
        for (var i = 0; i < numberOfProcesses; i++) processes.Add(new Process());
        ProcessGenerator.Instance.processes = processes;
    }
    
    public void RequestGeneratorCSV(int id, int arrivalTime, int priority, int burstTime)
    {
        processes.Add(new Process(id, arrivalTime, priority, burstTime));
            
        ProcessGenerator.Instance.processes = processes;
    }

}
