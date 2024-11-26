using System.Data.SqlClient;

public static class ConexaoBD
{
    public static SqlConnection GetConexao()
    {
        string strCon = "Server=DESKTOP-UOHE1VE\\SQLEXPRESS;Database=PBL;Trusted_Connection=True;";
        SqlConnection conexao = new SqlConnection(strCon);
        conexao.Open();
        return conexao;
    }
}