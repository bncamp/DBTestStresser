using DBTestStresser.Util;
using DiagnosticsUtils;
using Npgsql;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DBTestStresser.Model {
    public class Test {
        public int Id { get; set; }
        public TestSeries Serie { get; set; }

        public int ConcurrencyAmount { get; set; }

        public string[] Queries { get; set; }
        public void Execute() {
            List<Task> tasks = new List<Task>();
            Task t;
            string query = "";
            ConcurrentBag<long> threadTimes = new ConcurrentBag<long>();
            ConcurrentBag<double> queriesTimes = new ConcurrentBag<double>();
            ConcurrentBag<string> outputs = new ConcurrentBag<string>();
            
            Stopwatch chrono = new Stopwatch();
            GUI.Log("Running " + ConcurrencyAmount + " concurrents jobs...");
            chrono.Start();
            for (int i = 0; i < ConcurrencyAmount; i++) {
                query = Queries[i];
                t = Task.Run(
                    () => ExecuteThread(this.Serie.DBMS,threadTimes, queriesTimes,outputs, query)
                );
                tasks.Add(t);
                
            }
            try {
                Task.WaitAll(tasks.ToArray());
                chrono.Stop();
                
                // Mean query execution time
                string debug = "COLLECTION SIZE: " + threadTimes.Count + " : {";
                double threadMean = 0.0;
                double queriesMean = 0.0;
                double totalMean = 0.0;

                foreach (long ms in threadTimes) {
                    debug += ms + ',';
                    threadMean += (double)ms;
                }
                foreach (double ms in queriesTimes) {
                    debug += ms + ',';
                    queriesMean += ms;
                }
                if (threadTimes.Count > 0) {
                    threadMean = threadMean / threadTimes.Count;
                    queriesMean = queriesMean / queriesTimes.Count;
                    totalMean = chrono.ElapsedMilliseconds / queriesTimes.Count;
                }
                debug += "}";
                //Console.WriteLine(debug);
                GUI.Log("All jobs returned : ");
                GUI.Log("average thread execution time : " + threadMean + " ms");
                //GUI.Log("average query execution time : " + queriesMean + " ms");
                //GUI.Log("total execution time / thread nb : " + totalMean + " ms");
                GUI.Log("");
                this.Serie.CSV += ConcurrencyAmount + ";" + threadMean + ";" + queriesMean + ";" 
                    + totalMean + "\n";
            } catch (Exception e) {
                chrono.Stop();
                GUI.Log("Threads join failed : " + e.Message + "\n" + e.StackTrace);
                GUI.Log("Outputs =============");
                foreach (var err in outputs) {
                    if (!String.IsNullOrEmpty(err)) {
                        GUI.Log(err);
                    }
                }
            }
        }

        public void ExecuteThread(EntityDBMS dbms,ConcurrentBag<long> threadTimes,
            ConcurrentBag<double> queryTimes,
            ConcurrentBag<string> outputs, string query) {

            Stopwatch threadChrono = new Stopwatch();
            ExecutionStopwatch queryChrono = new ExecutionStopwatch();
            var cnx = dbms.GetConnection();

            try {
                cnx.Open();
                threadChrono.Start();
                queryChrono.Start();
                if (this.Serie.OperationType == "Read") {
                    dbms.ReadQuery(cnx, query);
                } else {
                    dbms.WriteQuery(cnx, query);
                }
                threadChrono.Stop();
                queryChrono.Stop();
                cnx.Close();
            } catch (Exception e) {
                outputs.Add("ERROR : " + e.Message + "\n" + e.StackTrace);
                if (cnx.State == System.Data.ConnectionState.Open) {
                    cnx.Close();
                }
                if (threadChrono.IsRunning) {
                    threadChrono.Stop();
                }
                if (queryChrono.IsRunning) {
                    queryChrono.Stop();
                }
            }

            // Thread end
            threadTimes.Add(threadChrono.ElapsedMilliseconds);
            queryTimes.Add(queryChrono.Elapsed.TotalMilliseconds);

            Console.WriteLine("thread : " + threadChrono.ElapsedMilliseconds + "; query : "
                + queryChrono.Elapsed.ToString());
        }
    }
}
