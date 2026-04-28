Option Strict On
Option Explicit On

Imports System.Collections.Generic
Imports System.Data
Imports Oracle.ManagedDataAccess.Client
Imports Alpes.Datos.Conexion
Imports Alpes.Entidades.Ventas

Namespace Repositorios
    Public Class ListaDeseosDatos

        Private ReadOnly _conexionOracle As ConexionOracle

        Public Sub New()
            _conexionOracle = New ConexionOracle()
        End Sub

        Public Function Insertar(ByVal entidad As ListaDeseos) As Integer
            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using cmd As New OracleCommand("PKG_LISTA_DESEOS.SP_INSERTAR_LISTA_DESEOS", conexion)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_CLI_ID", OracleDbType.Int32).Value = entidad.CliId
                    cmd.Parameters.Add("P_PRODUCTO_ID", OracleDbType.Int32).Value = entidad.ProductoId

                    Dim pNota As New OracleParameter("P_NOTA", OracleDbType.Varchar2)
                    If String.IsNullOrWhiteSpace(entidad.Nota) Then
                        pNota.Value = DBNull.Value
                    Else
                        pNota.Value = entidad.Nota
                    End If
                    cmd.Parameters.Add(pNota)

                    Dim pListaDeseosId As New OracleParameter("P_LISTA_DESEOS_ID", OracleDbType.Int32)
                    pListaDeseosId.Direction = ParameterDirection.Output
                    cmd.Parameters.Add(pListaDeseosId)

                    cmd.ExecuteNonQuery()
                    Return Convert.ToInt32(pListaDeseosId.Value.ToString())
                End Using
            End Using
        End Function

        Public Function Actualizar(ByVal entidad As ListaDeseos) As Boolean
            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using cmd As New OracleCommand("PKG_LISTA_DESEOS.SP_ACTUALIZAR_LISTA_DESEOS", conexion)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_LISTA_DESEOS_ID", OracleDbType.Int32).Value = entidad.ListaDeseosId
                    cmd.Parameters.Add("P_CLI_ID", OracleDbType.Int32).Value = entidad.CliId
                    cmd.Parameters.Add("P_PRODUCTO_ID", OracleDbType.Int32).Value = entidad.ProductoId

                    Dim pNota As New OracleParameter("P_NOTA", OracleDbType.Varchar2)
                    If String.IsNullOrWhiteSpace(entidad.Nota) Then
                        pNota.Value = DBNull.Value
                    Else
                        pNota.Value = entidad.Nota
                    End If
                    cmd.Parameters.Add(pNota)

                    cmd.ExecuteNonQuery()
                    Return True
                End Using
            End Using
        End Function

        Public Function Eliminar(ByVal listaDeseosId As Integer) As Boolean
            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using cmd As New OracleCommand("PKG_LISTA_DESEOS.SP_ELIMINAR_LISTA_DESEOS", conexion)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_LISTA_DESEOS_ID", OracleDbType.Int32).Value = listaDeseosId
                    cmd.ExecuteNonQuery()
                    Return True
                End Using
            End Using
        End Function

        Public Function ObtenerPorId(ByVal listaDeseosId As Integer) As ListaDeseos
            Dim entidad As ListaDeseos = Nothing

            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using cmd As New OracleCommand("PKG_LISTA_DESEOS.SP_OBTENER_LISTA_DESEOS", conexion)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_LISTA_DESEOS_ID", OracleDbType.Int32).Value = listaDeseosId
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        If dr.Read() Then
                            entidad = Mapear(dr)
                        End If
                    End Using
                End Using
            End Using

            Return entidad
        End Function

        Public Function Listar() As List(Of ListaDeseos)
            Dim lista As New List(Of ListaDeseos)()

            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using cmd As New OracleCommand("PKG_LISTA_DESEOS.SP_LISTAR_LISTA_DESEOS", conexion)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        While dr.Read()
                            lista.Add(Mapear(dr))
                        End While
                    End Using
                End Using
            End Using

            Return lista
        End Function

        Public Function Buscar(ByVal criterio As String, ByVal valor As String) As List(Of ListaDeseos)
            Dim lista As New List(Of ListaDeseos)()

            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using cmd As New OracleCommand("PKG_LISTA_DESEOS.SP_BUSCAR_LISTA_DESEOS", conexion)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_CRITERIO", OracleDbType.Varchar2).Value = criterio
                    cmd.Parameters.Add("P_VALOR", OracleDbType.Varchar2).Value = valor
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        While dr.Read()
                            lista.Add(Mapear(dr))
                        End While
                    End Using
                End Using
            End Using

            Return lista
        End Function

        Private Function Mapear(ByVal dr As OracleDataReader) As ListaDeseos
            Dim entidad As New ListaDeseos()

            entidad.ListaDeseosId = Convert.ToInt32(dr("LISTA_DESEOS_ID"))
            entidad.CliId = Convert.ToInt32(dr("CLI_ID"))
            entidad.ProductoId = Convert.ToInt32(dr("PRODUCTO_ID"))
            entidad.Nota = If(dr("NOTA") Is DBNull.Value, String.Empty, dr("NOTA").ToString())
            entidad.Estado = If(dr("ESTADO") Is DBNull.Value, String.Empty, dr("ESTADO").ToString())

            Return entidad
        End Function

    End Class
End Namespace