using System;
using System.Data;
using System.Data.SqlClient;

namespace PBL_LP.DAO
{
    public static class HelperDAO
    {
        public static void ExecutaSQL(string sql, SqlParameter[] parametros)
        {
            using (SqlConnection conexao = ConexaoBD.GetConexao())
            {
                try
                {
                    using (SqlCommand comando = new SqlCommand(sql, conexao))
                    {
                        if (parametros != null)
                            comando.Parameters.AddRange(parametros);
                        comando.ExecuteNonQuery();
                    }
                }
                catch (SqlException ex)
                {
                    throw new Exception("Erro ao executar SQL: " + ex.Message);
                }
                finally
                {
                    conexao.Close();
                }
            }
        }

        public static DataTable ExecutaSelect(string sql, SqlParameter[] parametros)
        {
            using (SqlConnection conexao = ConexaoBD.GetConexao())
            {
                try
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(sql, conexao))
                    {
                        if (parametros != null)
                            adapter.SelectCommand.Parameters.AddRange(parametros);
                        DataTable tabela = new DataTable();
                        adapter.Fill(tabela);
                        return tabela;
                    }
                }
                catch (SqlException ex)
                {
                    throw new Exception("Erro ao executar SELECT: " + ex.Message);
                }
                finally
                {
                    conexao.Close();
                }
            }
        }

        public static object ExecutaScalar(string sql, SqlParameter[] parametros)
        {
            using (SqlConnection conexao = ConexaoBD.GetConexao())
            {
                try
                {
                    using (SqlCommand comando = new SqlCommand(sql, conexao))
                    {
                        if (parametros != null)
                            comando.Parameters.AddRange(parametros);
                        return comando.ExecuteScalar();
                    }
                }
                catch (SqlException ex)
                {
                    throw new Exception("Erro ao executar Scalar: " + ex.Message);
                }
                finally
                {
                    conexao.Close();
                }
            }
        }
        public static void ExecutaProc(string nomeProc, SqlParameter[] parametros)
        {
            using (SqlConnection conexao = ConexaoBD.GetConexao())
            {
                using (SqlCommand comando = new SqlCommand(nomeProc, conexao))
                {
                    comando.CommandType = CommandType.StoredProcedure;
                    if (parametros != null)
                        comando.Parameters.AddRange(parametros);
                    comando.ExecuteNonQuery();
                }
            }
        }

        public static DataTable ExecutaProcSelect(string nomeProc, SqlParameter[] parametros)
        {
            using (SqlConnection conexao = ConexaoBD.GetConexao())
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter(nomeProc, conexao))
                {
                    if (parametros != null)
                        adapter.SelectCommand.Parameters.AddRange(parametros);

                    adapter.SelectCommand.CommandType = CommandType.StoredProcedure;

                    DataTable tabela = new DataTable();
                    adapter.Fill(tabela);
                    return tabela;
                }
            }
        }
    }
}