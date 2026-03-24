Imports Oracle.ManagedDataAccess.Client
Imports System.Data
Imports alpes.Entidades

Namespace Datos

    Public Class ListaDeseosDatos

        Private ReadOnly _conexion As alpes.Datos.ConexionOracle

        Public Sub New()
            _conexion = New alpes.Datos.ConexionOracle()
        End Sub

        Public Function Insertar(listaDeseos As ListaDeseos) As Integer
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_LISTA_DESEOS.SP_INSERTAR_LISTA_DESEOS", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_CLI_ID", OracleDbType.Int32).Value = listaDeseos.CliId
                    cmd.Parameters.Add("P_PRODUCTO_ID", OracleDbType.Int32).Value = listaDeseos.ProductoId
                    cmd.Parameters.Add("P_NOTA", OracleDbType.Varchar2).Value =
                        If(String.IsNullOrWhiteSpace(listaDeseos.Nota), DBNull.Value, CType(listaDeseos.Nota, Object))

                    Dim pOut As New OracleParameter("P_LISTA_DESEOS_ID", OracleDbType.Int32)
                    pOut.Direction = ParameterDirection.Output
                    cmd.Parameters.Add(pOut)

                    conn.Open()
                    cmd.ExecuteNonQuery()
                    Return Convert.ToInt32(pOut.Value.ToString())
                End Using
            End Using
        End Function

        Public Sub Actualizar(listaDeseos As ListaDeseos)
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_LISTA_DESEOS.SP_ACTUALIZAR_LISTA_DESEOS", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_LISTA_DESEOS_ID", OracleDbType.Int32).Value = listaDeseos.ListaDeseosId
                    cmd.Parameters.Add("P_CLI_ID", OracleDbType.Int32).Value = listaDeseos.CliId
                    cmd.Parameters.Add("P_PRODUCTO_ID", OracleDbType.Int32).Value = listaDeseos.ProductoId
                    cmd.Parameters.Add("P_NOTA", OracleDbType.Varchar2).Value =
                        If(String.IsNullOrWhiteSpace(listaDeseos.Nota), DBNull.Value, CType(listaDeseos.Nota, Object))

                    conn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Sub Eliminar(listaDeseosId As Integer)
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_LISTA_DESEOS.SP_ELIMINAR_LISTA_DESEOS", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_LISTA_DESEOS_ID", OracleDbType.Int32).Value = listaDeseosId
                    conn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Function Listar() As DataTable
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_LISTA_DESEOS.SP_LISTAR_LISTA_DESEOS", conn)
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
                Using cmd As New OracleCommand("PKG_LISTA_DESEOS.SP_BUSCAR_LISTA_DESEOS", conn)
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
