using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ProcessManager : MonoBehaviour
{
    // metrics
    private long averageResponseTime;
    private long averageTurnaroundTime;
    private long averageWaitingTime;
    private double CPUUtilization;
    private double throughput;
    private int elapsedTime;
    private int numberOfProcesses;


    // needed variables
    private List<Process> processes;
    private readonly Queue<Process> readyQueue = new Queue<Process>();
    private List<Process> sortedProcesses;
    private int currentTime;
    private int iterator;



    public void Start()
    {
        processes = ProcessGenerator.Instance.processes;
        SortProcesses();
        ReadyQueueReallocation();
    }


    private void TimeUnit()
    {
        for (int i = 0, temp = 0; i < 10000; i++)
            if (i / 2 == 0)
                temp = i / 2;
            else
                temp = 2 * i;

        currentTime++;
        ReadyQueueReallocation();
    }

    private void SortProcesses()
    {
        sortedProcesses = processes.OrderBy(o => o.ArrivalTime).ToList();
        numberOfProcesses = sortedProcesses.Count;
    }


    private void ReadyQueueReallocation()
    {
        if (iterator <= numberOfProcesses - 1)
            while (sortedProcesses[iterator].ArrivalTime <= currentTime
                   && readyQueue.Count < 100)
            {
                readyQueue.Enqueue(sortedProcesses[iterator]);
                iterator++;
                if (iterator == sortedProcesses.Count) break;
            }
    }

    private void CalculateMetrics()
    {
        throughput = (double) numberOfProcesses / currentTime;
        averageResponseTime /= numberOfProcesses;
        averageTurnaroundTime /= numberOfProcesses;
        averageWaitingTime /= numberOfProcesses;
        elapsedTime = currentTime;

        // CPU Utilization
        var burstTimeSummation = 0;
        for (var i = 0; i < numberOfProcesses; i++) burstTimeSummation += sortedProcesses[i].BurstTime;
        CPUUtilization = (double) burstTimeSummation / currentTime;
    }

    private void UpdateAverages(Process process)
    {
        var turnaroundTime = currentTime - process.ArrivalTime;
        var waitingTime = turnaroundTime - process.BurstTime;
        var responseTime = process.FirstApproved - process.ArrivalTime;

        averageResponseTime += responseTime;
        averageTurnaroundTime += turnaroundTime;
        averageWaitingTime += waitingTime;
    }

    public void FCFS()
    {
        while (iterator <= numberOfProcesses - 1 || readyQueue.Count != 0)
        {
            while (readyQueue.Count == 0 && iterator < numberOfProcesses) TimeUnit();

            var process = readyQueue.Dequeue();

            for (var i = 0; i < process.BurstTime; i++)
            {
                if (i == 0)
                    process.SetFirstApproved(currentTime);

                TimeUnit();
            }

            UpdateAverages(process);
            
            // context switch cost
            if (iterator < numberOfProcesses || readyQueue.Count != 0)
                TimeUnit();
        }

        CalculateMetrics();
        WriteToUI();
    }

    public void NonPreemptiveSJF()
    {
        var readyList = new List<Process>();

        while (iterator <= numberOfProcesses - 1 || readyQueue.Count != 0 || readyList.Count != 0)
        {
            while (readyList.Count == 0 && readyQueue.Count == 0 && iterator < numberOfProcesses) TimeUnit();
            while (readyQueue.Count != 0) readyList.Add(readyQueue.Dequeue());

            readyList = readyList.OrderBy(o => o.BurstTime).ToList();

            var process = readyList[0];

            for (var i = 0; i < process.BurstTime; i++)
            {
                if (i == 0)
                    process.SetFirstApproved(currentTime);
                TimeUnit();
            }

            UpdateAverages(process);

            // context switch cost
            if (iterator < numberOfProcesses || readyQueue.Count != 0)
                TimeUnit();

            readyList.RemoveAt(0);
        }

        CalculateMetrics();
        WriteToUI();
    }

    public void NonPreemptivePriority()
    {
        var readyList = new List<Process>();

        while (iterator <= numberOfProcesses - 1 || readyQueue.Count != 0 || readyList.Count != 0)
        {
            while (readyList.Count == 0 && readyQueue.Count == 0 && iterator < numberOfProcesses) TimeUnit();
            while (readyQueue.Count != 0) readyList.Add(readyQueue.Dequeue());

            readyList = readyList.OrderBy(o => o.Priority).ToList();

            var process = readyList[0];

            for (var i = 0; i < process.BurstTime; i++)
            {
                if (i == 0)
                    process.SetFirstApproved(currentTime);
                TimeUnit();
            }

            UpdateAverages(process);

            // context switch cost
            if (iterator < numberOfProcesses || readyQueue.Count != 0)
                TimeUnit();

            readyList.RemoveAt(0);
        }

        CalculateMetrics();
        WriteToUI();
    }

    public void PreemptiveSJF()
    {
        bool isFirstProcess = true; // Context switch doesn't happen at first entry
        var readyList = new List<Process>();
        var process = new Process();

        while (iterator <= numberOfProcesses - 1 || readyQueue.Count != 0 || readyList.Count != 0)
        {
            while (readyList.Count == 0 && readyQueue.Count == 0 && iterator < numberOfProcesses) TimeUnit();
            while (readyQueue.Count != 0) readyList.Add(readyQueue.Dequeue());

            readyList = readyList.OrderBy(o => o.RemainingTime).ToList();

            if (process != readyList[0])
            {
                // context switch cost
                if (readyList.Count != 0 || readyQueue.Count != 0 || iterator < numberOfProcesses)
                    if (!isFirstProcess)
                    {
                        TimeUnit();
                        while (readyQueue.Count != 0) readyList.Add(readyQueue.Dequeue());
                        readyList = readyList.OrderBy(o => o.RemainingTime).ToList();
                    }
                    else
                    {
                        isFirstProcess = false;
                    }

                process = readyList[0];
            }


            while (process.RemainingTime != 0)
            {
                if (process.BurstTime == process.RemainingTime)
                    process.SetFirstApproved(currentTime);
                TimeUnit();
                process.SetRemainingTime(process.RemainingTime - 1);

                while (readyQueue.Count != 0) readyList.Add(readyQueue.Dequeue());

                readyList = readyList.OrderBy(o => o.RemainingTime).ToList();

                if (process != readyList[0])
                {
                    // context switch cost
                    if (readyList.Count != 0 || readyQueue.Count != 0 || iterator < numberOfProcesses)
                    {
                        TimeUnit();
                        while (readyQueue.Count != 0) readyList.Add(readyQueue.Dequeue());
                        readyList = readyList.OrderBy(o => o.RemainingTime).ToList();
                    }

                    process = readyList[0];
                }
            }

            UpdateAverages(process);
            readyList.RemoveAt(0);
        }

        CalculateMetrics();
        WriteToUI();
    }

    public void PreemptivePriority()
    {
        var readyList = new List<Process>();
        var process = new Process();

        while (iterator <= numberOfProcesses - 1 || readyQueue.Count != 0 || readyList.Count != 0)
        {
            while (readyList.Count == 0 && readyQueue.Count == 0 && iterator < numberOfProcesses) TimeUnit();
            while (readyQueue.Count != 0) readyList.Add(readyQueue.Dequeue());

            readyList = readyList.OrderBy(o => o.Priority).ToList();

            if (process != readyList[0])
            {
                // context switch cost
                if (readyList.Count != 0 || readyQueue.Count != 0 || iterator < numberOfProcesses)
                    if (currentTime != 0 && currentTime != 1)
                    {
                        TimeUnit();
                        while (readyQueue.Count != 0) readyList.Add(readyQueue.Dequeue());
                        readyList = readyList.OrderBy(o => o.RemainingTime).ToList();
                    }

                process = readyList[0];
            }


            while (process.RemainingTime != 0)
            {
                if (process.BurstTime == process.RemainingTime)
                    process.SetFirstApproved(currentTime);
                TimeUnit();
                process.SetRemainingTime(process.RemainingTime - 1);

                while (readyQueue.Count != 0) readyList.Add(readyQueue.Dequeue());

                readyList = readyList.OrderBy(o => o.Priority).ToList();

                if (process != readyList[0])
                {
                    // context switch cost
                    if (readyList.Count != 0 || readyQueue.Count != 0 || iterator < numberOfProcesses)
                        if (currentTime != 0 && currentTime != 1)
                        {
                            TimeUnit();
                            while (readyQueue.Count != 0) readyList.Add(readyQueue.Dequeue());
                            readyList = readyList.OrderBy(o => o.Priority).ToList();
                        }

                    process = readyList[0];
                }
            }

            UpdateAverages(process);
            readyList.RemoveAt(0);
        }

        CalculateMetrics();
        WriteToUI();
    }

    public void RoundRobin()
    {
        const int timeQuantum = 4;

        while (iterator <= numberOfProcesses - 1 || readyQueue.Count != 0)
        {
            while (readyQueue.Count == 0 && iterator < numberOfProcesses) TimeUnit();

            var process = readyQueue.Dequeue();
            for (var i = 0; i < timeQuantum; i++)
            {
                if (process.RemainingTime == 0) break;

                if (process.BurstTime == process.RemainingTime)
                    process.SetFirstApproved(currentTime);
                TimeUnit();


                process.SetRemainingTime(process.RemainingTime - 1);
            }

            if (process.RemainingTime > 0)
                readyQueue.Enqueue(process);
            else if (process.RemainingTime == 0) UpdateAverages(process);

            // context switch cost
            if (iterator < numberOfProcesses || readyQueue.Count != 0)
                TimeUnit();
        }

        CalculateMetrics();
        WriteToUI();
    }

    private void WriteToUI()
    {
        WritingResults.Instance.averageResponseTime = averageResponseTime;
        WritingResults.Instance.averageTurnaroundTime = averageTurnaroundTime;
        WritingResults.Instance.averageWaitingTime = averageWaitingTime;
        WritingResults.Instance.CPUUtilization = CPUUtilization;
        WritingResults.Instance.elapsedTime = elapsedTime;
        WritingResults.Instance.throughput = throughput;
        WritingResults.Instance.numberOfProcesses = numberOfProcesses;
        WritingResults.Instance.processes = sortedProcesses;
    }
}