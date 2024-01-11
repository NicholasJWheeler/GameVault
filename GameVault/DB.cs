using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using SQLite;
using System.Reflection;

namespace GameVault;
class DB {
    private static string DBName = "log.db";
    public static SQLiteConnection backlogConn;
    public static SQLiteConnection vaultConn;
    public static void OpenConnection() {
        string libFolder = FileSystem.AppDataDirectory;
        string fname = System.IO.Path.Combine(libFolder, DBName);
        // File.Delete(fname);--> Temporarily delete the database file if you would like to start from scratch
        backlogConn = new SQLiteConnection(fname);
        vaultConn = new SQLiteConnection(fname);
        backlogConn.CreateTable<BacklogGame>();
        vaultConn.CreateTable<VaultGame>();
        /*try
        {
            string libFolder = FileSystem.AppDataDirectory;
            string fname = System.IO.Path.Combine(libFolder, DBName);
            backlogConn = new SQLiteConnection(fname);
            vaultConn = new SQLiteConnection(fname);
            backlogConn.CreateTable<BacklogGame>();
            vaultConn.CreateTable<VaultGame>();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error in OpenConnection: {ex.Message}");
            // Handle the exception as needed
        }*/
    }

}

