Option Strict On
Option Explicit On

Imports System
Imports System.Collections.Generic
Imports System.Data
Imports Oracle.ManagedDataAccess.Client
Imports Alpes.Datos.Conexion
Imports Alpes.Entidades.Seguridad

Namespace Repositorios
    Public Class Historial_Cambio_ContrasenaDatos

        Private ReadOnly _conexionOracle As ConexionOracle

        Public Sub New()
            _conexionOracle = New ConexionOracle()
        End Sub

        Public Function Insertar(ByVal entidad As Historial_Cambio_Contrasena) As Integer
            Dim idGenerado As Integer = 0

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_HISTORIAL_CAMBIO_CONTRASENA.SP_INSERTAR", cn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.BindByName = True

                    cmd.Parameters.Add("P_USU_ID", OracleDbType.Int32).Value = entidad.UsuId
                    cmd.Parameters.Add("P_CAMBIO_AT", OracleDbType.TimeStamp).Value = entidad.CambioAt

                    If String.IsNullOrWhiteSpace(entidad.Motivo) Then
                        cmd.Parameters.Add("P_MOTIVO", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("P_MOTIVO", OracleDbType.Varchar2).Value = entidad.Motivo
                    End If

                    If String.IsNullOrWhiteSpace(entidad.Ip) Then
                        cmd.Parameters.Add("P_IP", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("P_IP", OracleDbType.Varchar2).Value = entidad.Ip
                    End If

                    cmd.Parameters.Add("P_ID", OracleDbType.Int32).Direction = ParameterDirection.Output

                    cmd.ExecuteNonQuery()
                    idGenerado = Convert.ToInt32(cmd.Parameters("P_ID").Value.ToString())
                End Using
            End Using

            Return idGenerado
        End Function

        Public Sub Actualizar(ByVal entidad As Historial_Cambio_Contrasena)
            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_HISTORIAL_CAMBIO_CONTRASENA.SP_ACTUALIZAR", cn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.BindByName = True

                    cmd.Parameters.Add("P_HC_ID", OracleDbType.Int32).Value = entidad.HcId
                    cmd.Parameters.Add("P_USU_ID", OracleDbType.Int32).Value = entidad.UsuId
                    cmd.Parameters.Add("P_CAMBIO_AT", OracleDbType.TimeStamp).Value = entidad.CambioAt

                    If String.IsNullOrWhiteSpace(entidad.Motivo) Then
                        cmd.Parameters.Add("P_MOTIVO", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("P_MOTIVO", OracleDbType.Varchar2).Value = entidad.Motivo
                    End If

                    If String.IsNullOrWhiteSpace(entidad.Ip) Then
                        cmd.Parameters.Add("P_IP", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("P_IP", OracleDbType.Varchar2).Value = entidad.Ip
                    End If

                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Sub Eliminar(ByVal id As Integer)
            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_HISTORIAL_CAMBIO_CONTRASENA.SP_ELIMINAR", cn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.BindByName = True

                    cmd.Parameters.Add("P_ID", OracleDbType.Int32).Value = id
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Function ObtenerPorId(ByVal id As Integer) As Historial_Cambio_Contrasena
            Dim entidad As Historial_Cambio_Contrasena = Nothing

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_HISTORIAL_CAMBIO_CONTRASENA.SP_OBTENER", cn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.BindByName = True

                    cmd.Parameters.Add("P_ID", OracleDbType.Int32).Value = id
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        If dr.Read() Then
                            entidad = MapearHistorialCambioContrasena(dr)
                        End If
                    End Using
                End Using
            End Using

            Return entidad
        End Function

        Public Function Listar() As List(Of Historial_Cambio_Contrasena)
            Dim lista As New List(Of Historial_Cambio_Contrasena)()

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_HISTORIAL_CAMBIO_CONTRASENA.SP_LISTAR", cn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.BindByName = True

                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        While dr.Read()
                            lista.Add(MapearHistorialCambioContrasena(dr))
                        End While
                    End Using
                End Using
            End Using

            Return lista
        End Function

        Public Function Buscar(ByVal valor As String) As List(Of Historial_Cambio_Contrasena)
            Dim lista As New List(Of Historial_Cambio_Contrasena)()

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_HISTORIAL_CAMBIO_CONTRASENA.SP_BUSCAR", cn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.BindByName = True

                    cmd.Parameters.Add("P_VALOR", OracleDbType.TimeStamp).Value = Convert.ToDateTime(valor)
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        While dr.Read()
                            lista.Add(MapearHistorialCambioContrasena(dr))
                        End While
                    End Using
                End Using
            End Using

            Return lista
        End Function

        Private Function MapearHistorialCambioContrasena(ByVal dr As OracleDataReader) As Historial_Cambio_Contrasena
            Dim entidad As New Historial_Cambio_Contrasena()

            If Not IsDBNull(dr("HC_ID")) Then
                entidad.HcId = Convert.ToInt32(dr("HC_ID"))
            End If

            If Not IsDBNull(dr("USU_ID")) Then
                entidad.UsuId = Convert.ToInt32(dr("USU_ID"))
            End If

            If Not IsDBNull(dr("CAMBIO_AT")) Then
                entidad.CambioAt = Convert.ToDateTime(dr("CAMBIO_AT"))
            End If

            entidad.Motivo = If(IsDBNull(dr("MOTIVO")), String.Empty, dr("MOTIVO").ToString())
            entidad.Ip = If(IsDBNull(dr("IP")), String.Empty, dr("IP").ToString())

            If Not IsDBNull(dr("CREATED_AT")) Then
                entidad.CreatedAt = Convert.ToDateTime(dr("CREATED_AT"))
            Else
                entidad.CreatedAt = Nothing
            End If

            If Not IsDBNull(dr("UPDATED_AT")) Then
                entidad.UpdatedAt = Convert.ToDateTime(dr("UPDATED_AT"))
            Else
                entidad.UpdatedAt = Nothing
            End If

            Return entidad
        End Function

    End Class
End Namespace