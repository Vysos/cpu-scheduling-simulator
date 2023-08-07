using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleFileBrowser;
using System.IO;
using System.Text;
using UnityEngine.SceneManagement;

public class FileBrowserOption : MonoBehaviour
{
    
    public void OpenLoadWindow()
    {
        FileBrowser.SetFilters( false, new FileBrowser.Filter( "Text Files", ".txt", ".csv" ) );
        FileBrowser.SetDefaultFilter( ".csv" );
        
         FileBrowser.ShowLoadDialog( ( paths ) => { Debug.Log( "Selected: " + paths[0] ); },
        						   () => { Debug.Log( "Canceled" ); },
        			   FileBrowser.PickMode.Folders, false, null, null, "Select Folder", "Select" );

        StartCoroutine( ShowLoadDialogCoroutine() );
    }
    
    IEnumerator ShowLoadDialogCoroutine()
    {
        // Show a load file dialog and wait for a response from user
        // Load file/folder: both, Allow multiple selection: true
        // Initial path: default (Documents), Initial filename: empty
        // Title: "Load File", Submit button text: "Load"
        yield return FileBrowser.WaitForLoadDialog( FileBrowser.PickMode.Files, false, null, null, "Load a Source File", "Load" );

        // Dialog is closed
        // Print whether the user has selected some files/folders or cancelled the operation (FileBrowser.Success)
        Debug.Log( FileBrowser.Success );

        if( FileBrowser.Success )
        {
            // Print paths of the selected files (FileBrowser.Result) (null, if FileBrowser.Success is false)
            for( int i = 0; i < FileBrowser.Result.Length; i++ )
                Debug.Log( FileBrowser.Result[i] );

            // Or, copy the first file to persistentDataPath
            string destinationPath = Path.Combine( Application.persistentDataPath, FileBrowserHelpers.GetFilename( FileBrowser.Result[0] ) );
            FileBrowserHelpers.CopyFile( FileBrowser.Result[0], destinationPath );
            
            using (StreamReader sr = new StreamReader(destinationPath))
            {
                string headerLine = sr.ReadLine();
                string line;
                while ((line = sr.ReadLine()) != null)
                { 
                    var values = line.Split(',');

                    ProcessGenerator.Instance.RequestGeneratorCSV(Int32.Parse(values[0]),
                        Int32.Parse(values[1]), Int32.Parse(values[2]),
                        Int32.Parse(values[3]));
                }
            }

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    public void OpenSaveWindow()
    {
        FileBrowser.SetFilters( false, new FileBrowser.Filter( "Text Files", ".txt", ".csv" ) );
        FileBrowser.SetDefaultFilter( ".csv" );
        
        FileBrowser.ShowSaveDialog( null, null, FileBrowser.PickMode.Files, false, "C:\\", "data.csv", "Save As", "Save" );

        StartCoroutine( ShowSaveDialogCoroutine() );
    }
    
    IEnumerator ShowSaveDialogCoroutine()
    {

        yield return FileBrowser.WaitForSaveDialog( FileBrowser.PickMode.Files, false, null, null, "Save processes", "Save" );

        Debug.Log( FileBrowser.Success );

        if( FileBrowser.Success )
        {
            for( int i = 0; i < FileBrowser.Result.Length; i++ )
                Debug.Log( FileBrowser.Result[i] );
            

            var csv = new StringBuilder();
            var header = "id,arrivalTime,priority,burstTime";
            csv.AppendLine(header); 
            List<Process> processes = WritingResults.Instance.processes;


            for (int i = 0; i < processes.Count; i++)
            {
                var id = processes[i].ID.ToString();
                var arrival = processes[i].ArrivalTime.ToString();
                var priority = processes[i].Priority.ToString();
                var burstTime = processes[i].BurstTime.ToString();
                var newLine = $"{id},{arrival},{priority},{burstTime}";
                csv.AppendLine(newLine); 
            }
            File.WriteAllText(FileBrowser.Result[0], csv.ToString());

        }
    }

    
    
}
