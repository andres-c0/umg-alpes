Option Strict On
Option Explicit On

Imports System
Imports System.Collections.Generic
Imports System.Data
Imports Oracle.ManagedDataAccess.Client
Imports Alpes.Datos.Conexion
Imports Alpes.Entidades.Seguridad

Namespace Repositorios
    Public Class SesionDatos

        Private ReadOnly _conexionOracle As ConexionOracle

        Public Sub New()
            _conexionOracle = New ConexionOracle()
        End Sub

        Public Function Insertar(ByVal entidad As Sesion) As Integer
            Dim idGenerado As Integer = 0

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_SESION.SP_INSERTAR", cn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.BindByName = True

                    cmd.Parameters.Add("P_USU_ID", OracleDbType.Int32).Value = entidad.UsuId
                    cmd.Parameters.Add("P_TOKEN_HASH", OracleDbType.Varchar2).Value = entidad.TokenHash

                    If String.IsNullOrWhiteSpace(entidad.Ip) Then
                        cmd.Parameters.Add("P_IP", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("P_IP", OracleDbType.Varchar2).Value = entidad.Ip
                    End If

                    If String.IsNullOrWhiteSpace(entidad.UserAgent) Then
                        cmd.Parameters.Add("P_USER_AGENT", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("P_USER_AGENT", OracleDbType.Varchar2).Value = entidad.UserAgent
                    End If

                    cmd.Parameters.Add("P_INICIO_AT", OracleDbType.TimeStamp).Value = entidad.InicioAt

                    If entidad.FinAt.HasValue Then
                        cmd.Parameters.Add("P_FIN_AT", OracleDbType.TimeStamp).Value = entidad.FinAt.Value
                    Else
                        cmd.Parameters.Add("P_FIN_AT", OracleDbType.TimeStamp).Value = DBNull.Value
                    End If

                    cmd.Parameters.Add("P_ID", OracleDbType.Int32).Direction = ParameterDirection.Output

                    cmd.ExecuteNonQuery()
                    idGenerado = Convert.ToInt32(cmd.Parameters("P_ID").Value.ToString())
                End Using
            End Using

            Return idGenerado
        End Function

        Public Sub Actualizar(ByVal entidad As Sesion)
            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_SESION.SP_ACTUALIZAR", cn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.BindByName = True

                    cmd.Parameters.Add("P_SESION_ID", OracleDbType.Int32).Value = entidad.SesionId
                    cmd.Parameters.Add("P_USU_ID", OracleDbType.Int32).Value = entidad.UsuId
                    cmd.Parameters.Add("P_TOKEN_HASH", OracleDbType.Varchar2).Value = entidad.TokenHash

                    If String.IsNullOrWhiteSpace(entidad.Ip) Then
                        cmd.Parameters.Add("P_IP", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("P_IP", OracleDbType.Varchar2).Value = entidad.Ip
                    End If

                    If String.IsNullOrWhiteSpace(entidad.UserAgent) Then
                        cmd.Parameters.Add("P_USER_AGENT", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("P_USER_AGENT", OracleDbType.Varchar2).Value = entidad.UserAgent
                    End If

                    cmd.Parameters.Add("P_INICIO_AT", OracleDbType.TimeStamp).Value = entidad.InicioAt

                    If entidad.FinAt.HasValue Then
                        cmd.Parameters.Add("P_FIN_AT", OracleDbType.TimeStamp).Value = entidad.FinAt.Value
                    Else
                        cmd.Parameters.Add("P_FIN_AT", OracleDbType.TimeStamp).Value = DBNull.Value
                    End If

                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Sub Eliminar(ByVal id As Integer)
            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_SESION.SP_ELIMINAR", cn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.BindByName = True

                    cmd.Parameters.Add("P_ID", OracleDbType.Int32).Value = id
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Function ObtenerPorId(ByVal id As Integer) As Sesion
            Dim entidad As Sesion = Nothing

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_SESION.SP_OBTENER", cn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.BindByName = True

                    cmd.Parameters.Add("P_ID", OracleDbType.Int32).Value = id
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        If dr.Read() Then
                            entidad = MapearSesion(dr)
                        End If
                    End Using
                End Using
            End Using

            Return entidad
        End Function

        Public Function Listar() As List(Of Sesion)
            Dim lista As New List(Of Sesion)()

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_SESION.SP_LISTAR", cn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.BindByName = True

                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        While dr.Read()
                            lista.Add(MapearSesion(dr))
                        End While
                    End Using
                End Using
            End Using

            Return lista
        End Function

        Public Function Buscar(ByVal valor As String) As List(Of Sesion)
            Dim lista As New List(Of Sesion)()

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_SESION.SP_BUSCAR", cn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.BindByName = True

                    cmd.Parameters.Add("P_VALOR", OracleDbType.Varchar2).Value = valor
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        While dr.Read()
                            lista.Add(MapearSesion(dr))
                        End While
                    End Using
                End Using
            End Using

            Return lista
        End Function

        Private Function MapearSesion(ByVal dr As OracleDataReader) As Sesion
            Dim entidad As New Sesion()

            If Not IsDBNull(dr("SESION_ID")) Then
                entidad.SesionId = Convert.ToInt32(dr("SESION_ID"))
            End If

            If Not IsDBNull(dr("USU_ID")) Then
                entidad.UsuId = Convert.ToInt32(dr("USU_ID"))
            End If

            entidad.TokenHash = If(IsDBNull(dr("TOKEN_HASH")), String.Empty, dr("TOKEN_HASH").ToString())
            entidad.Ip = If(IsDBNull(dr("IP")), String.Empty, dr("IP").ToString())
            entidad.UserAgent = If(IsDBNull(dr("USER_AGENT")), String.Empty, dr("USER_AGENT").ToString())

            If Not IsDBNull(dr("INICIO_AT")) Then
                entidad.InicioAt = Convert.ToDateTime(dr("INICIO_AT"))
            End If

            If Not IsDBNull(dr("FIN_AT")) Then
                entidad.FinAt = Convert.ToDateTime(dr("FIN_AT"))
            Else
                entidad.FinAt = Nothing
            End If

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