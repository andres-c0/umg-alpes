Option Strict On
Option Explicit On

Imports System.Collections.Generic
Imports System.Data
Imports Oracle.ManagedDataAccess.Client
Imports Alpes.Datos.Conexion
Imports Alpes.Entidades.Ventas

Namespace Repositorios
    Public Class CarritoDatos

        Private ReadOnly _conexionOracle As ConexionOracle

        Public Sub New()
            _conexionOracle = New ConexionOracle()
        End Sub

        Public Function Insertar(ByVal entidad As Carrito) As Integer
            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using cmd As New OracleCommand("PKG_CARRITO.SP_INSERTAR_CARRITO", conexion)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_CLI_ID", OracleDbType.Int32).Value = entidad.CliId

                    Dim pEstadoCarrito As New OracleParameter("P_ESTADO_CARRITO", OracleDbType.Varchar2)
                    If String.IsNullOrWhiteSpace(entidad.EstadoCarrito) Then
                        pEstadoCarrito.Value = DBNull.Value
                    Else
                        pEstadoCarrito.Value = entidad.EstadoCarrito
                    End If
                    cmd.Parameters.Add(pEstadoCarrito)

                    Dim pUltimoCalculoAt As New OracleParameter("P_ULTIMO_CALCULO_AT", OracleDbType.TimeStamp)
                    If entidad.UltimoCalculoAt.HasValue Then
                        pUltimoCalculoAt.Value = entidad.UltimoCalculoAt.Value
                    Else
                        pUltimoCalculoAt.Value = DBNull.Value
                    End If
                    cmd.Parameters.Add(pUltimoCalculoAt)

                    Dim pCarritoId As New OracleParameter("P_CARRITO_ID", OracleDbType.Int32)
                    pCarritoId.Direction = ParameterDirection.Output
                    cmd.Parameters.Add(pCarritoId)

                    cmd.ExecuteNonQuery()
                    Return Convert.ToInt32(pCarritoId.Value.ToString())
                End Using
            End Using
        End Function

        Public Function Actualizar(ByVal entidad As Carrito) As Boolean
            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using cmd As New OracleCommand("PKG_CARRITO.SP_ACTUALIZAR_CARRITO", conexion)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_CARRITO_ID", OracleDbType.Int32).Value = entidad.CarritoId
                    cmd.Parameters.Add("P_CLI_ID", OracleDbType.Int32).Value = entidad.CliId

                    Dim pEstadoCarrito As New OracleParameter("P_ESTADO_CARRITO", OracleDbType.Varchar2)
                    If String.IsNullOrWhiteSpace(entidad.EstadoCarrito) Then
                        pEstadoCarrito.Value = DBNull.Value
                    Else
                        pEstadoCarrito.Value = entidad.EstadoCarrito
                    End If
                    cmd.Parameters.Add(pEstadoCarrito)

                    Dim pUltimoCalculoAt As New OracleParameter("P_ULTIMO_CALCULO_AT", OracleDbType.TimeStamp)
                    If entidad.UltimoCalculoAt.HasValue Then
                        pUltimoCalculoAt.Value = entidad.UltimoCalculoAt.Value
                    Else
                        pUltimoCalculoAt.Value = DBNull.Value
                    End If
                    cmd.Parameters.Add(pUltimoCalculoAt)

                    cmd.ExecuteNonQuery()
                    Return True
                End Using
            End Using
        End Function

        Public Function Eliminar(ByVal carritoId As Integer) As Boolean
            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using cmd As New OracleCommand("PKG_CARRITO.SP_ELIMINAR_CARRITO", conexion)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_CARRITO_ID", OracleDbType.Int32).Value = carritoId
                    cmd.ExecuteNonQuery()
                    Return True
                End Using
            End Using
        End Function

        Public Function ObtenerPorId(ByVal carritoId As Integer) As Carrito
            Dim entidad As Carrito = Nothing

            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using cmd As New OracleCommand("PKG_CARRITO.SP_OBTENER_CARRITO", conexion)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_CARRITO_ID", OracleDbType.Int32).Value = carritoId
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

        Public Function Listar() As List(Of Carrito)
            Dim lista As New List(Of Carrito)()

            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using cmd As New OracleCommand("PKG_CARRITO.SP_LISTAR_CARRITO", conexion)
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

        Public Function Buscar(ByVal criterio As String, ByVal valor As String) As List(Of Carrito)
            Dim lista As New List(Of Carrito)()

            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using cmd As New OracleCommand("PKG_CARRITO.SP_BUSCAR_CARRITO", conexion)
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

        Private Function Mapear(ByVal dr As OracleDataReader) As Carrito
            Dim entidad As New Carrito()

            entidad.CarritoId = Convert.ToInt32(dr("CARRITO_ID"))
            entidad.CliId = Convert.ToInt32(dr("CLI_ID"))
            entidad.EstadoCarrito = If(dr("ESTADO_CARRITO") Is DBNull.Value, String.Empty, dr("ESTADO_CARRITO").ToString())

            If dr("ULTIMO_CALCULO_AT") Is DBNull.Value Then
                entidad.UltimoCalculoAt = Nothing
            Else
                entidad.UltimoCalculoAt = Convert.ToDateTime(dr("ULTIMO_CALCULO_AT"))
            End If

            entidad.Estado = If(dr("ESTADO") Is DBNull.Value, String.Empty, dr("ESTADO").ToString())

            Return entidad
        End Function

    End Class
End Namespace