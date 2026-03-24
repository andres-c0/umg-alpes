Imports Oracle.ManagedDataAccess.Client
Imports System.Data
Imports alpes.Entidades

Namespace Datos

    Public Class PreferenciaClienteDatos

        Private ReadOnly _conexion As alpes.Datos.ConexionOracle

        Public Sub New()
            _conexion = New alpes.Datos.ConexionOracle()
        End Sub

        Public Function Insertar(preferenciaCliente As PreferenciaCliente) As Integer
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_PREFERENCIA_CLIENTE.SP_INSERTAR_PREFERENCIA_CLIENTE", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_CLI_ID", OracleDbType.Int32).Value = preferenciaCliente.CliId
                    cmd.Parameters.Add("P_CATEGORIA_ID", OracleDbType.Int32).Value = preferenciaCliente.CategoriaId
                    cmd.Parameters.Add("P_PESO_PREFERENCIA", OracleDbType.Decimal).Value = preferenciaCliente.PesoPreferencia

                    Dim pOut As New OracleParameter("P_PREF_ID", OracleDbType.Int32)
                    pOut.Direction = ParameterDirection.Output
                    cmd.Parameters.Add(pOut)

                    conn.Open()
                    cmd.ExecuteNonQuery()
                    Return Convert.ToInt32(pOut.Value.ToString())
                End Using
            End Using
        End Function

        Public Sub Actualizar(preferenciaCliente As PreferenciaCliente)
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_PREFERENCIA_CLIENTE.SP_ACTUALIZAR_PREFERENCIA_CLIENTE", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_PREF_ID", OracleDbType.Int32).Value = preferenciaCliente.PrefId
                    cmd.Parameters.Add("P_CLI_ID", OracleDbType.Int32).Value = preferenciaCliente.CliId
                    cmd.Parameters.Add("P_CATEGORIA_ID", OracleDbType.Int32).Value = preferenciaCliente.CategoriaId
                    cmd.Parameters.Add("P_PESO_PREFERENCIA", OracleDbType.Decimal).Value = preferenciaCliente.PesoPreferencia

                    conn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Sub Eliminar(preferenciaClienteId As Integer)
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_PREFERENCIA_CLIENTE.SP_ELIMINAR_PREFERENCIA_CLIENTE", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_PREF_ID", OracleDbType.Int32).Value = preferenciaClienteId
                    conn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Function Listar() As DataTable
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_PREFERENCIA_CLIENTE.SP_LISTAR_PREFERENCIA_CLIENTE", conn)
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

        Public Function Buscar(criterio As String, valor As String) As DataTable
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_PREFERENCIA_CLIENTE.SP_BUSCAR_PREFERENCIA_CLIENTE", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_CRITERIO", OracleDbType.Varchar2).Value = criterio
                    cmd.Parameters.Add("P_VALOR", OracleDbType.Varchar2).Value = valor
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
