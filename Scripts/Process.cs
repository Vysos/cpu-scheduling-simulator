using UnityEngine;

public class Process
{
    private static int idCounter = 0;
    private int id;
    private int arrivalTime;
    private int priority;
    private int burstTime;
    private int remainingTime;  // used for preemptive algorithms
    private int firstApproved;

    public Process()
    {
        this.arrivalTime = Random.Range(0, 50000);
        this.priority = Random.Range(0, 10);
        this.id = idCounter++;
        this.burstTime = Random.Range(1, 10);
        this.remainingTime = burstTime;
    }

    public Process(int id, int arrivalTime, int priority, int burstTime)
    {
        this.id = id;
        this.arrivalTime = arrivalTime;
        this.priority = priority;
        this.burstTime = burstTime;
        this.remainingTime = burstTime;

    }


    public int ID => id;

    public int ArrivalTime => arrivalTime;

    public int Priority => priority;

    public int BurstTime => burstTime;

    public void SetRemainingTime(int newValue)
    {
        this.remainingTime = newValue;
    }

    public int RemainingTime => remainingTime;

    public void SetFirstApproved(int newValue)
    {
        this.firstApproved = newValue;
    }

    public int FirstApproved => firstApproved;
}

