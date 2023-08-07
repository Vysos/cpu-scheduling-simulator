# CPU Process Scheduling Simulator

This is a CPU process scheduling simulator built with Unity. It allows simulating and comparing different CPU scheduling algorithms.

![Screenshot of the menu of the App](https://github.com/Vysos/cpu-scheduling-simulator/blob/main/Screenshots/Menu1.png)

#### Algorithms Implemented
- First Come First Serve (FCFS)
- Shortest Job First (SJF) - Preemptive and Nonpreemptive
- Priority Scheduling - Preemptive and Nonpreemptive
- Round Robin
  
#### Features
- Generate random processes or load processes from a CSV file
- Simulate scheduling algorithms
- View scheduling metrics like average turnaround time, waiting time etc.
- Save process data to CSV file

#### Code Structure
The key scripts are:
- ProcessGenerator.cs - Generates random processes
- ProcessManager.cs - Contains logic to simulate scheduling algorithms
- WritingResults.cs - Displays results on UI
- FileBrowserOptions.cs - File browser for loading and saving CSV
  
The Process, ProcessGenerator and ProcessManager classes handle the core simulation logic. WritingResults displays the results on screen. FileBrowserOptions provides save/load CSV capabilities.

#### Installation
No special installation steps required. Just install using the setup.exe which is in the **Releases** section.

#### More Screenshots

![Screenshot of the menu of the App](https://github.com/Vysos/cpu-scheduling-simulator/blob/main/Screenshots/Menu2.png)

![Screenshot of the menu of the App](https://github.com/Vysos/cpu-scheduling-simulator/blob/main/Screenshots/Unity-Environment1.jpg)

![Screenshot of the menu of the App](https://github.com/Vysos/cpu-scheduling-simulator/blob/main/Screenshots/Unity-environment2.jpg)
