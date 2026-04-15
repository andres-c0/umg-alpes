Option Strict On
Option Explicit On

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

                Using transaccion As OracleTransaction = conexion.BeginTransaction()
                    Using cmd As New OracleCommand("PKG_PREFERENCIA_CLIENTE.SP_INSERTAR_PREFERENCIA_CLIENTE", conexion)
                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Transaction = transaccion

                        cmd.Parameters.Add("P_CLI_ID", OracleDbType.Int32).Value = entidad.CliId
                        cmd.Parameters.Add("P_CATEGORIA_ID", OracleDbType.Int32).Value = entidad.CategoriaId

                        Dim pEstilo As New OracleParameter("P_ESTILO", OracleDbType.Varchar2)
                        If String.IsNullOrWhiteSpace(entidad.Estilo) Then
                            pEstilo.Value = DBNull.Value
                        Else
                            pEstilo.Value = entidad.Estilo
                        End If
                        cmd.Parameters.Add(pEstilo)

                        Dim pMaterialPreferido As New OracleParameter("P_MATERIAL_PREFERIDO", OracleDbType.Varchar2)
                        If String.IsNullOrWhiteSpace(entidad.MaterialPreferido) Then
                            pMaterialPreferido.Value = DBNull.Value
                        Else
                            pMaterialPreferido.Value = entidad.MaterialPreferido
                        End If
                        cmd.Parameters.Add(pMaterialPreferido)

                        Dim pPreferenciaId As New OracleParameter("P_PREFERENCIA_ID", OracleDbType.Int32)
                        pPreferenciaId.Direction = ParameterDirection.Output
                        cmd.Parameters.Add(pPreferenciaId)

                        cmd.ExecuteNonQuery()
                        transaccion.Commit()

                        Return Convert.ToInt32(pPreferenciaId.Value.ToString())
                    End Using
                End Using
            End Using
        End Function

        Public Function Actualizar(ByVal entidad As PreferenciaCliente) As Boolean
            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using transaccion As OracleTransaction = conexion.BeginTransaction()
                    Using cmd As New OracleCommand("PKG_PREFERENCIA_CLIENTE.SP_ACTUALIZAR_PREFERENCIA_CLIENTE", conexion)
                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Transaction = transaccion

                        cmd.Parameters.Add("P_PREFERENCIA_ID", OracleDbType.Int32).Value = entidad.PreferenciaId
                        cmd.Parameters.Add("P_CLI_ID", OracleDbType.Int32).Value = entidad.CliId
                        cmd.Parameters.Add("P_CATEGORIA_ID", OracleDbType.Int32).Value = entidad.CategoriaId

                        Dim pEstilo As New OracleParameter("P_ESTILO", OracleDbType.Varchar2)
                        If String.IsNullOrWhiteSpace(entidad.Estilo) Then
                            pEstilo.Value = DBNull.Value
                        Else
                            pEstilo.Value = entidad.Estilo
                        End If
                        cmd.Parameters.Add(pEstilo)

                        Dim pMaterialPreferido As New OracleParameter("P_MATERIAL_PREFERIDO", OracleDbType.Varchar2)
                        If String.IsNullOrWhiteSpace(entidad.MaterialPreferido) Then
                            pMaterialPreferido.Value = DBNull.Value
                        Else
                            pMaterialPreferido.Value = entidad.MaterialPreferido
                        End If
                        cmd.Parameters.Add(pMaterialPreferido)

                        cmd.ExecuteNonQuery()
                        transaccion.Commit()

                        Return True
                    End Using
                End Using
            End Using
        End Function

        Public Function Eliminar(ByVal preferenciaId As Integer) As Boolean
            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using transaccion As OracleTransaction = conexion.BeginTransaction()
                    Using cmd As New OracleCommand("PKG_PREFERENCIA_CLIENTE.SP_ELIMINAR_PREFERENCIA_CLIENTE", conexion)
                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Transaction = transaccion

                        cmd.Parameters.Add("P_PREFERENCIA_ID", OracleDbType.Int32).Value = preferenciaId

                        cmd.ExecuteNonQuery()
                        transaccion.Commit()

                        Return True
                    End Using
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

                    cmd.Parameters.Add("P_PREFERENCIA_ID", OracleDbType.Int32).Value = preferenciaId
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

                Using cmd As New OracleCommand("PKG_PREFERENCIA_CLIENTE.SP_LISTAR_PREFERENCIAS_CLIENTE", conexion)
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

                Using cmd As New OracleCommand("PKG_PREFERENCIA_CLIENTE.SP_BUSCAR_PREFERENCIAS_CLIENTE", conexion)
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

            entidad.PreferenciaId = Convert.ToInt32(dr("PREFERENCIA_ID"))
            entidad.CliId = Convert.ToInt32(dr("CLI_ID"))
            entidad.CategoriaId = Convert.ToInt32(dr("CATEGORIA_ID"))
            entidad.Estilo = If(dr("ESTILO") Is DBNull.Value, String.Empty, dr("ESTILO").ToString())
            entidad.MaterialPreferido = If(dr("MATERIAL_PREFERIDO") Is DBNull.Value, String.Empty, dr("MATERIAL_PREFERIDO").ToString())
            entidad.Estado = If(dr("ESTADO") Is DBNull.Value, String.Empty, dr("ESTADO").ToString())

            Return entidad
        End Function

    End Class
End Namespace