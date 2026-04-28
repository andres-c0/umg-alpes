Option Strict On
Option Explicit On

Imports System
Imports System.Collections.Generic
Imports System.Data
Imports Oracle.ManagedDataAccess.Client
Imports Alpes.Datos.Conexion
Imports Alpes.Entidades.Ventas

Namespace Repositorios
    Public Class TarjetaClienteDatos

        Private ReadOnly _conexionOracle As ConexionOracle

        Public Sub New()
            _conexionOracle = New ConexionOracle()
        End Sub

        Public Sub Insertar(ByVal entidad As TarjetaCliente)
            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                If cn.State = ConnectionState.Closed Then
                    cn.Open()
                End If

                Using cmd As New OracleCommand("PKG_TARJETA_CLIENTE.SP_INSERTAR_TARJETA_CLIENTE", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_CLI_ID", OracleDbType.Int32).Value = entidad.CliId
                    cmd.Parameters.Add("P_TITULAR", OracleDbType.Varchar2).Value = entidad.Titular
                    cmd.Parameters.Add("P_ULTIMOS_4", OracleDbType.Varchar2).Value = entidad.Ultimos4
                    cmd.Parameters.Add("P_MARCA", OracleDbType.Varchar2).Value = entidad.Marca
                    cmd.Parameters.Add("P_MES_VENCIMIENTO", OracleDbType.Int32).Value = entidad.MesVencimiento
                    cmd.Parameters.Add("P_ANIO_VENCIMIENTO", OracleDbType.Int32).Value = entidad.AnioVencimiento

                    If String.IsNullOrWhiteSpace(entidad.AliasTarjeta) Then
                        cmd.Parameters.Add("P_ALIAS_TARJETA", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("P_ALIAS_TARJETA", OracleDbType.Varchar2).Value = entidad.AliasTarjeta
                    End If

                    cmd.Parameters.Add("P_PREDETERMINADA", OracleDbType.Int32).Value = entidad.Predeterminada

                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Function ObtenerPorCliente(ByVal cliId As Integer) As List(Of TarjetaCliente)
            Dim lista As New List(Of TarjetaCliente)()

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                If cn.State = ConnectionState.Closed Then
                    cn.Open()
                End If

                Using cmd As New OracleCommand("PKG_TARJETA_CLIENTE.SP_OBTENER_TARJETAS_CLIENTE", cn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_CLI_ID", OracleDbType.Int32).Value = cliId
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

        Public Function ObtenerPorId(ByVal tarjetaClienteId As Integer) As TarjetaCliente
            Dim entidad As TarjetaCliente = Nothing

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                If cn.State = ConnectionState.Closed Then
                    cn.Open()
                End If

                Using cmd As New OracleCommand("PKG_TARJETA_CLIENTE.SP_OBTENER_TARJETA_POR_ID", cn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_TARJETA_CLIENTE_ID", OracleDbType.Int32).Value = tarjetaClienteId
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

        Public Sub MarcarPredeterminada(ByVal tarjetaClienteId As Integer, ByVal cliId As Integer)
            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                If cn.State = ConnectionState.Closed Then
                    cn.Open()
                End If

                Using cmd As New OracleCommand("PKG_TARJETA_CLIENTE.SP_MARCAR_PREDETERMINADA", cn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_TARJETA_CLIENTE_ID", OracleDbType.Int32).Value = tarjetaClienteId
                    cmd.Parameters.Add("P_CLI_ID", OracleDbType.Int32).Value = cliId
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Sub Desactivar(ByVal tarjetaClienteId As Integer)
            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                If cn.State = ConnectionState.Closed Then
                    cn.Open()
                End If

                Using cmd As New OracleCommand("PKG_TARJETA_CLIENTE.SP_DESACTIVAR_TARJETA_CLIENTE", cn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_TARJETA_CLIENTE_ID", OracleDbType.Int32).Value = tarjetaClienteId
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Private Function Mapear(ByVal dr As OracleDataReader) As TarjetaCliente
            Dim entidad As New TarjetaCliente()

            entidad.TarjetaClienteId = Convert.ToInt32(dr("TARJETA_CLIENTE_ID"))
            entidad.CliId = Convert.ToInt32(dr("CLI_ID"))
            entidad.Titular = If(IsDBNull(dr("TITULAR")), String.Empty, dr("TITULAR").ToString())
            entidad.Ultimos4 = If(IsDBNull(dr("ULTIMOS_4")), String.Empty, dr("ULTIMOS_4").ToString())
            entidad.Marca = If(IsDBNull(dr("MARCA")), String.Empty, dr("MARCA").ToString())
            entidad.MesVencimiento = If(IsDBNull(dr("MES_VENCIMIENTO")), 0, Convert.ToInt32(dr("MES_VENCIMIENTO")))
            entidad.AnioVencimiento = If(IsDBNull(dr("ANIO_VENCIMIENTO")), 0, Convert.ToInt32(dr("ANIO_VENCIMIENTO")))
            entidad.AliasTarjeta = If(IsDBNull(dr("ALIAS_TARJETA")), String.Empty, dr("ALIAS_TARJETA").ToString())
            entidad.Predeterminada = If(IsDBNull(dr("PREDETERMINADA")), 0, Convert.ToInt32(dr("PREDETERMINADA")))
            entidad.CreatedAt = If(IsDBNull(dr("CREATED_AT")), CType(Nothing, DateTime?), Convert.ToDateTime(dr("CREATED_AT")))
            entidad.UpdatedAt = If(IsDBNull(dr("UPDATED_AT")), CType(Nothing, DateTime?), Convert.ToDateTime(dr("UPDATED_AT")))
            entidad.Estado = If(IsDBNull(dr("ESTADO")), String.Empty, dr("ESTADO").ToString())

            Return entidad
        End Function

    End Class
End Namespace