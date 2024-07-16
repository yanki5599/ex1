using ex1;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sqlClientPractic
{
    internal static class DBContext
    {
        private static string _connectionString = GetConnString();

        public static DataTable? MakeQuery(string queryStr)
        {
            DataTable output = new DataTable();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(queryStr, conn))
                {
                    try
                    {
                        conn.Open();
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            adapter.Fill(output);
                        }

                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine("An error occured: " + ex.Message);
                        return null;
                    }
                }
            }

            return output;
        }

        public static DataTable? MakeQuery(string queryStr, params SqlParameter[] sqlParameters)
        {
            DataTable output = new DataTable();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(queryStr, conn))
                {
                    // Add parameters to the command if they exist
                    if (sqlParameters != null)
                    {
                        cmd.Parameters.AddRange(sqlParameters);
                    }

                    try
                    {
                        conn.Open();
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            adapter.Fill(output);
                        }
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine("SQL Error occurred: " + ex.Message);
                        return null; // Return null or throw exception as per your application's error handling strategy
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("An error occurred: " + ex.Message);
                        return null; // Return null or throw exception as per your application's error handling strategy
                    }
                }
            }

            return output;
        }

        private static string GetConnString()
        {
            var config = new ConfigurationBuilder()
                        .AddUserSecrets<Program>()
                        .Build();
            string? connectionString = config["connectionString"];

            if (connectionString == null)
                throw new Exception("Cannot read conn striong from secrets");

            return connectionString;
        }

        public static int ExecuteNonQuery(string queryStr)
        {
            int affectedRows = 0;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(queryStr, conn))
                {
                    try
                    {
                        conn.Open();
                        affectedRows = cmd.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine("An error occured: " + ex.Message);
                        return -1;
                    }
                }
            }
            return affectedRows;
        }

        public static int ExecuteNonQuery(string queryStr, params SqlParameter[] sqlParameters)
        {
            int affectedRows = 0;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(queryStr, conn))
                {
                    try
                    {
                        conn.Open();
                        cmd.Parameters.AddRange(sqlParameters);
                        affectedRows = cmd.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine("An error occured: " + ex.Message);
                        return -1;
                    }
                }
            }
            return affectedRows;
        }
    }
}
