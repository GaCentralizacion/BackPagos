using System;

using System.Data.Entity;
using System.Data.SqlClient;
using System.Data;


namespace DataAccess.Model
{
    public class InlineInserts : DbContext
    {
        public InlineInserts() : base("name=PagosEntities")
        {
        }

        public string ActualizaCartera(int idEmpresa)
        {
            string cnx = this.Database.Connection.ConnectionString;
            using (SqlConnection con = new SqlConnection(cnx))
            {
                using (SqlCommand cmd = new SqlCommand("PROC_ACTUALIZA_CARTERA_SP", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 10000;
                    cmd.Parameters.Add("@idEmpresa", SqlDbType.Int).Value = idEmpresa;
                    con.Open();
                    return cmd.ExecuteNonQuery().ToString();
                }
            }
        }

        public string LiberaDocument(int idlote, string documento)
        {
            string cnx = this.Database.Connection.ConnectionString;
            using (SqlConnection con = new SqlConnection(cnx))
            {
                using (SqlCommand cmd = new SqlCommand("PROC_LIBERA_DOCUMENTO_SP", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 10000;
                    cmd.Parameters.Add("@idLote", SqlDbType.Int).Value = idlote;
                    cmd.Parameters.Add("@documento", SqlDbType.VarChar).Value = documento;
                    con.Open();
                    return cmd.ExecuteNonQuery().ToString();
                }
            }
        }

        public string aplicaLoteDirectosql(int idEmpresa, int idLote, int idUsuario)
        {
            string cnx = this.Database.Connection.ConnectionString;
            using (SqlConnection con = new SqlConnection(cnx))
            {
                using (SqlCommand cmd = new SqlCommand("INS_APLICA_LOTEDIRECTO_SP", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 50;
                    cmd.Parameters.Add("@emp_idempresa", SqlDbType.Int).Value = idEmpresa;
                    cmd.Parameters.Add("@idLote", SqlDbType.Int).Value = idLote;
                    cmd.Parameters.Add("@idUsuario", SqlDbType.Int).Value = idUsuario;
                    con.Open();
                    return cmd.ExecuteNonQuery().ToString();
                }
            }
        }

        public string borraLoteSQLbusqueda(int idLote)
        {
            string cnx = this.Database.Connection.ConnectionString;
            using (SqlConnection con = new SqlConnection(cnx))
            {
                using (SqlCommand cmd = new SqlCommand("DEL_BORRA_LOTE_SP", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 50;
                    cmd.Parameters.Add("@idLote", SqlDbType.Int).Value = idLote;
                    con.Open();
                    return cmd.ExecuteNonQuery().ToString();
                }
            }
        }
        

    }
}
