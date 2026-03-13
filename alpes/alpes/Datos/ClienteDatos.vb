Imports Oracle.ManagedDataAccess.Client
Imports System.Data
Imports alpes.Entidades

Namespace Datos
    Public Class ClienteDatos

        Private ReadOnly _conexion As ConexionOracle

        Public Sub New()
            _conexion = New ConexionOracle()
        End Sub

        Public Function Insertar(cliente As Cliente) As Integer
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_CLIENTE.SP_INSERTAR_CLIENTE", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_TIPO_DOCUMENTO", OracleDbType.Varchar2).Value = cliente.TipoDocumento
                    cmd.Parameters.Add("P_NUM_DOCUMENTO", OracleDbType.Varchar2).Value = cliente.NumDocumento
                    cmd.Parameters.Add("P_NIT", OracleDbType.Varchar2).Value = If(String.IsNullOrWhiteSpace(cliente.Nit), CType(DBNull.Value, Object), cliente.Nit)
                    cmd.Parameters.Add("P_NOMBRES", OracleDbType.Varchar2).Value = cliente.Nombres
                    cmd.Parameters.Add("P_APELLIDOS", OracleDbType.Varchar2).Value = cliente.Apellidos
                    cmd.Parameters.Add("P_EMAIL", OracleDbType.Varchar2).Value = cliente.Email
                    cmd.Parameters.Add("P_TEL_RESIDENCIA", OracleDbType.Varchar2).Value = cliente.TelResidencia
                    cmd.Parameters.Add("P_TEL_CELULAR", OracleDbType.Varchar2).Value = If(String.IsNullOrWhiteSpace(cliente.TelCelular), CType(DBNull.Value, Object), cliente.TelCelular)
                    cmd.Parameters.Add("P_DIRECCION", OracleDbType.Varchar2).Value = cliente.Direccion
                    cmd.Parameters.Add("P_CIUDAD", OracleDbType.Varchar2).Value = cliente.Ciudad
                    cmd.Parameters.Add("P_DEPARTAMENTO", OracleDbType.Varchar2).Value = cliente.Departamento
                    cmd.Parameters.Add("P_PAIS", OracleDbType.Varchar2).Value = cliente.Pais
                    cmd.Parameters.Add("P_PROFESION", OracleDbType.Varchar2).Value = If(String.IsNullOrWhiteSpace(cliente.Profesion), CType(DBNull.Value, Object), cliente.Profesion)

                    Dim pOut As New OracleParameter("P_CLI_ID", OracleDbType.Int32)
                    pOut.Direction = ParameterDirection.Output
                    cmd.Parameters.Add(pOut)

                    conn.Open()
                    cmd.ExecuteNonQuery()

                    Return Convert.ToInt32(pOut.Value.ToString())
                End Using
            End Using
        End Function

        Public Function Listar() As DataTable
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_CLIENTE.SP_LISTAR_CLIENTES", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using da As New OracleDataAdapter(cmd)
                        Dim dt As New DataTable()
                        da.Fill(dt)
                        Return dt
                    End Using
                End Using
            End Using
        End Function

    End Class
End Namespace