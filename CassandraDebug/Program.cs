using Cassandra;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace CassandraDebug {
    public class Program {
        static async Task Main(string[] args) {
            Console.WriteLine("DEBUG CASSANDRAAAAAAA");
            PoolingOptions poolingOptions = new PoolingOptions();
            poolingOptions
                .SetCoreConnectionsPerHost(HostDistance.Local, 1280)
                .SetMaxConnectionsPerHost(HostDistance.Local, 1280)
                .SetCoreConnectionsPerHost(HostDistance.Remote, 1280)
                .SetMaxConnectionsPerHost(HostDistance.Remote, 1280);

            poolingOptions
                .SetMaxSimultaneousRequestsPerConnectionTreshold(HostDistance.Local, 32768)
                .SetMinSimultaneousRequestsPerConnectionTreshold(HostDistance.Remote, 2000);

            var cluster = Cluster.Builder()
                .AddContactPoints("192.168.2.146")
                .WithPoolingOptions(poolingOptions)
                .WithQueryOptions(new QueryOptions().SetConsistencyLevel(ConsistencyLevel.One))
                .WithLoadBalancingPolicy(new RoundRobinPolicy())
                .Build();

            var options = new ParallelOptions();
            options.MaxDegreeOfParallelism = 50;

            using (var session = cluster.Connect("examplestore")) {
                var ps = session.Prepare("INSERT INTO ExampleStore.\"Order\"(order_id,order_date)" +
                    " VALUES (?,?)");

                var total = new Stopwatch();
                total.Start();
                //var r = Parallel.For(0, 1000, options, (x) =>
                //{
                //    {
                //        var s = new Stopwatch();
                //        s.Start();
                //        var statement = ps.Bind(156, DateTime.UtcNow.ToString());
                //        var t = session.ExecuteAsync(statement);
                //        t.Wait();
                //        s.Stop();
                //        Console.WriteLine("time : " + s.ElapsedMilliseconds + " ms");
                //    }
                //});
                //object p = await ConcurrentUtils.Times(1000000, 512, _ =>
                //    session.ExecuteAsync(ps.Bind(parameters)));
                object[] parameters= new object[]{ 156, DateTime.UtcNow.ToString()} ;
                await ConcurrentUtils.ConcurrentUtils.Times(10000, 512, _ =>
                        session.ExecuteAsync(ps.Bind(parameters)));
                total.Stop();
                Console.WriteLine("TOTAL -> 1000 queries in " + total.ElapsedMilliseconds + "ms");
                double mean = (double)total.ElapsedMilliseconds / 1000;
                Console.WriteLine("Mean : a query every " + mean + "ms");
            }
        }
    }
    
}
