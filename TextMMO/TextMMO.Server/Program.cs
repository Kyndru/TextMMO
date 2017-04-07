using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TextMMO.Server
{
    class Program
    {
		/*
		
		General ToDo:
			
			- Accept Client connections
			- generally retrieve and send data from/to Clients
			- send world data to clients (all informations around the players location)
			- send data from other players to clients (show other players positions)
			
		Database:
			- Store data in local database (MySql?)
				- players states, positions, enemy states
			- retreive data
			
		*/
        static void Main(string[] args)
        {
            Thread tickThread = new Thread(tick);
            tickThread.Start(4500);
            Thread.Sleep(1000);
            Console.WriteLine("Main thread ({0}) exiting...",
                        Thread.CurrentThread.ManagedThreadId);
        }

        static void tick(object obj)
        {
            int interval;
            try
            {
                interval = (int)obj;
            }
            catch (InvalidCastException)
            {
                interval = 5000;
            }
            DateTime start = DateTime.Now;
            var sw = Stopwatch.StartNew();
            Console.WriteLine("Thread {0}: {1}, Priority {2}",
                              Thread.CurrentThread.ManagedThreadId,
                              Thread.CurrentThread.ThreadState,
                              Thread.CurrentThread.Priority);
            do
            {
                Console.WriteLine("Thread {0}: Elapsed {1} ms",
                                  Thread.CurrentThread.ManagedThreadId,
                                  sw.ElapsedMilliseconds);
                //Thread.Sleep(10);
            } while (sw.ElapsedMilliseconds <= interval);
            sw.Stop();
        }

        void connectDB()
        {
            var dbCon = DBConnection.Instance();
            dbCon.DatabaseName = "YourDatabase";
            if (dbCon.IsConnect())
            {
                //suppose col0 and col1 are defined as VARCHAR in the DB
                string query = "SELECT col0,col1 FROM YourTable";
                var cmd = new MySqlCommand(query, dbCon.Connection);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string someStringFromColumnZero = reader.GetString(0);
                    string someStringFromColumnOne = reader.GetString(1);
                    Console.WriteLine(someStringFromColumnZero + "," + someStringFromColumnOne);
                }
            }
        }
    }
}
