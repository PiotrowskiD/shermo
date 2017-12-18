using System.Collections.Generic;
using ServerMonitor.Model;
using System.Data.SqlClient;
using System.Configuration;
using log4net;
using System.Reflection;

namespace ServerReporterService
{
    class SqlPerformance
    {
        private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public List<SqlPerformanceInfo> CheckParameters()
        {
            List<SqlPerformanceInfo> sqlPerfInfos = new List<SqlPerformanceInfo>();

            sqlPerfInfos.Clear();
            var connectionString = ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString;

            using (var con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(
                    @"SELECT TOP 10 SUBSTRING(qt.TEXT, (qs.statement_start_offset/2)+1,
                    ((CASE qs.statement_end_offset
                    WHEN - 1 THEN DATALENGTH(qt.TEXT)
                    ELSE qs.statement_end_offset
                    END - qs.statement_start_offset) / 2) + 1),
                    qs.execution_count,
                    qs.total_logical_reads, qs.last_logical_reads,
                    qs.total_logical_writes, qs.last_logical_writes,
                    qs.total_worker_time,
                    qs.last_worker_time,
                    qs.total_elapsed_time / 1000000 total_elapsed_time_in_S,
                    qs.last_elapsed_time / 1000000 last_elapsed_time_in_S,
                    qs.last_execution_time,
                    qp.query_plan
                    FROM sys.dm_exec_query_stats qs
                    CROSS APPLY sys.dm_exec_sql_text(qs.sql_handle) qt
                    CROSS APPLY sys.dm_exec_query_plan(qs.plan_handle) qp
                    ORDER BY qs.total_logical_reads DESC --logical reads
                    -- ORDER BY qs.total_logical_writes DESC --logical writes
                    -- ORDER BY qs.total_worker_time DESC --CPU time", con);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                        try
                        {
                            sqlPerfInfos.Add(new SqlPerformanceInfo
                            {
                                Message = reader.GetString(0),
                                ExecutionCount = reader.GetInt64(1).ToString(),
                                TotalLogicalReads = reader.GetInt64(2).ToString(),
                                LastLogicalReads = reader.GetInt64(3).ToString(),
                                TotalLogicalWrites = reader.GetInt64(4).ToString(),
                                LastLogicalWrites = reader.GetInt64(5).ToString(),
                                TotalWorkerTime = reader.GetInt64(6).ToString(),
                                LastWorkerTime = reader.GetInt64(7).ToString(),
                                TotalElapsedTime = reader.GetInt64(8).ToString(),
                                LastElapsedTime = reader.GetInt64(9).ToString(),
                                LastExecutionTime = reader.GetDateTime(10).ToString(),
                                QueryPlan = reader.GetString(11)
                            });
                        }
                        catch { }
                }

            }
            logger.Info("Sql Perfoemance checked succesfully");
            return sqlPerfInfos;
        }
    }
}
