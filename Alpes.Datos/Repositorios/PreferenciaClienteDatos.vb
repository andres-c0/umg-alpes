Option Strict On
Option Explicit On

Imports System
Imports System.Collections.Generic
Imports System.Data
Imports Oracle.ManagedDataAccess.Client
Imports Alpes.Datos.Conexion
Imports Alpes.Entidades.Ventas

Namespace Repositorios
    Public Class PreferenciaClienteDatos

        Private ReadOnly _conexionOracle As ConexionOracle

        Public Sub New()
            _conexionOracle = New ConexionOracle()
        End Sub

        Public Function Insertar(ByVal entidad As PreferenciaCliente) As Integer
            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using cmd As New OracleCommand("PKG_PREFERENCIA_CLIENTE.SP_INSERTAR_PREFERENCIA_CLIENTE", conexion)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_CLI_ID", OracleDbType.Int32).Value = entidad.CliId
                    cmd.Parameters.Add("P_CATEGORIA_ID", OracleDbType.Int32).Value = entidad.CategoriaId
                    cmd.Parameters.Add("P_PESO_PREFERENCIA", OracleDbType.Decimal).Value = entidad.PesoPreferencia

                    Dim pPrefId As New OracleParameter("P_PREF_ID", OracleDbType.Int32)
                    pPrefId.Direction = ParameterDirection.Output
                    cmd.Parameters.Add(pPrefId)

                    cmd.ExecuteNonQuery()

                    Return Convert.ToInt32(pPrefId.Value.ToString())
                End Using
            End Using
        End Function

        Public Function Actualizar(ByVal entidad As PreferenciaCliente) As Boolean
            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using cmd As New OracleCommand("PKG_PREFERENCIA_CLIENTE.SP_ACTUALIZAR_PREFERENCIA_CLIENTE", conexion)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_PREF_ID", OracleDbType.Int32).Value = entidad.PreferenciaId
                    cmd.Parameters.Add("P_CLI_ID", OracleDbType.Int32).Value = entidad.CliId
                    cmd.Parameters.Add("P_CATEGORIA_ID", OracleDbType.Int32).Value = entidad.CategoriaId
                    cmd.Parameters.Add("P_PESO_PREFERENCIA", OracleDbType.Decimal).Value = entidad.PesoPreferencia

                    cmd.ExecuteNonQuery()
                    Return True
                End Using
            End Using
        End Function

        Public Function Eliminar(ByVal preferenciaId As Integer) As Boolean
            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using cmd As New OracleCommand("PKG_PREFERENCIA_CLIENTE.SP_ELIMINAR_PREFERENCIA_CLIENTE", conexion)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_PREF_ID", OracleDbType.Int32).Value = preferenciaId

                    cmd.ExecuteNonQuery()
                    Return True
                End Using
            End Using
        End Function

        Public Function ObtenerPorId(ByVal preferenciaId As Integer) As PreferenciaCliente
            Dim entidad As PreferenciaCliente = Nothing

            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using cmd As New OracleCommand("PKG_PREFERENCIA_CLIENTE.SP_OBTENER_PREFERENCIA_CLIENTE", conexion)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_PREF_ID", OracleDbType.Int32).Value = preferenciaId
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

        Public Function Listar() As List(Of PreferenciaCliente)
            Dim lista As New List(Of PreferenciaCliente)()

            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using cmd As New OracleCommand("PKG_PREFERENCIA_CLIENTE.SP_LISTAR_PREFERENCIA_CLIENTE", conexion)
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

        Public Function Buscar(ByVal criterio As String, ByVal valor As String) As List(Of PreferenciaCliente)
            Dim lista As New List(Of PreferenciaCliente)()

            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using cmd As New OracleCommand("PKG_PREFERENCIA_CLIENTE.SP_BUSCAR_PREFERENCIA_CLIENTE", conexion)
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

        Private Function Mapear(ByVal dr As OracleDataReader) As PreferenciaCliente
            Dim entidad As New PreferenciaCliente()

            entidad.PreferenciaId = Convert.ToInt32(dr("PREF_ID"))
            entidad.CliId = Convert.ToInt32(dr("CLI_ID"))
            entidad.CategoriaId = Convert.ToInt32(dr("CATEGORIA_ID"))
            entidad.PesoPreferencia = If(dr("PESO_PREFERENCIA") Is DBNull.Value, 0D, Convert.ToDecimal(dr("PESO_PREFERENCIA")))
            entidad.Estado = If(dr("ESTADO") Is DBNull.Value, String.Empty, dr("ESTADO").ToString())

            Return entidad
        End Function

    End Class
End Namespace