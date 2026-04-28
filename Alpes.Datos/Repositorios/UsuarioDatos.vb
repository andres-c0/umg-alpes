Option Strict On
Option Explicit On

Imports System.Data
Imports Oracle.ManagedDataAccess.Client
Imports Alpes.Datos.Conexion
Imports Alpes.Entidades.Seguridad

Namespace Repositorios
    Public Class UsuarioDatos

        Private ReadOnly _conexionOracle As ConexionOracle

        Public Sub New()
            _conexionOracle = New ConexionOracle()
        End Sub

        Public Function Insertar(ByVal entidad As Usuario) As Integer
            Dim idGenerado As Integer = 0

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_USUARIO.SP_INSERTAR", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_USERNAME", OracleDbType.Varchar2).Value = entidad.Username
                    cmd.Parameters.Add("P_PASSWORD_HASH", OracleDbType.Varchar2).Value = entidad.PasswordHash
                    cmd.Parameters.Add("P_EMAIL", OracleDbType.Varchar2).Value = entidad.Email

                    If String.IsNullOrWhiteSpace(entidad.Telefono) Then
                        cmd.Parameters.Add("P_TELEFONO", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("P_TELEFONO", OracleDbType.Varchar2).Value = entidad.Telefono
                    End If

                    cmd.Parameters.Add("P_ROL_ID", OracleDbType.Int32).Value = entidad.RolId

                    If entidad.CliId.HasValue Then
                        cmd.Parameters.Add("P_CLI_ID", OracleDbType.Int32).Value = entidad.CliId.Value
                    Else
                        cmd.Parameters.Add("P_CLI_ID", OracleDbType.Int32).Value = DBNull.Value
                    End If

                    If entidad.EmpId.HasValue Then
                        cmd.Parameters.Add("P_EMP_ID", OracleDbType.Int32).Value = entidad.EmpId.Value
                    Else
                        cmd.Parameters.Add("P_EMP_ID", OracleDbType.Int32).Value = DBNull.Value
                    End If

                    If entidad.UltimoLoginAt.HasValue Then
                        cmd.Parameters.Add("P_ULTIMO_LOGIN_AT", OracleDbType.TimeStamp).Value = entidad.UltimoLoginAt.Value
                    Else
                        cmd.Parameters.Add("P_ULTIMO_LOGIN_AT", OracleDbType.TimeStamp).Value = DBNull.Value
                    End If

                    If entidad.BloqueadoHasta.HasValue Then
                        cmd.Parameters.Add("P_BLOQUEADO_HASTA", OracleDbType.TimeStamp).Value = entidad.BloqueadoHasta.Value
                    Else
                        cmd.Parameters.Add("P_BLOQUEADO_HASTA", OracleDbType.TimeStamp).Value = DBNull.Value
                    End If

                    If String.IsNullOrWhiteSpace(entidad.Estado) Then
                        cmd.Parameters.Add("P_ESTADO", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("P_ESTADO", OracleDbType.Varchar2).Value = entidad.Estado
                    End If

                    cmd.Parameters.Add("P_ID", OracleDbType.Int32).Direction = ParameterDirection.Output

                    cmd.ExecuteNonQuery()

                    idGenerado = Convert.ToInt32(cmd.Parameters("P_ID").Value.ToString())
                End Using
            End Using

            Return idGenerado
        End Function

        Public Sub Actualizar(ByVal entidad As Usuario)
            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_USUARIO.SP_ACTUALIZAR", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_USU_ID", OracleDbType.Int32).Value = entidad.UsuId
                    cmd.Parameters.Add("P_USERNAME", OracleDbType.Varchar2).Value = entidad.Username
                    cmd.Parameters.Add("P_PASSWORD_HASH", OracleDbType.Varchar2).Value = entidad.PasswordHash
                    cmd.Parameters.Add("P_EMAIL", OracleDbType.Varchar2).Value = entidad.Email

                    If String.IsNullOrWhiteSpace(entidad.Telefono) Then
                        cmd.Parameters.Add("P_TELEFONO", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("P_TELEFONO", OracleDbType.Varchar2).Value = entidad.Telefono
                    End If

                    cmd.Parameters.Add("P_ROL_ID", OracleDbType.Int32).Value = entidad.RolId

                    If entidad.CliId.HasValue Then
                        cmd.Parameters.Add("P_CLI_ID", OracleDbType.Int32).Value = entidad.CliId.Value
                    Else
                        cmd.Parameters.Add("P_CLI_ID", OracleDbType.Int32).Value = DBNull.Value
                    End If

                    If entidad.EmpId.HasValue Then
                        cmd.Parameters.Add("P_EMP_ID", OracleDbType.Int32).Value = entidad.EmpId.Value
                    Else
                        cmd.Parameters.Add("P_EMP_ID", OracleDbType.Int32).Value = DBNull.Value
                    End If

                    If entidad.UltimoLoginAt.HasValue Then
                        cmd.Parameters.Add("P_ULTIMO_LOGIN_AT", OracleDbType.TimeStamp).Value = entidad.UltimoLoginAt.Value
                    Else
                        cmd.Parameters.Add("P_ULTIMO_LOGIN_AT", OracleDbType.TimeStamp).Value = DBNull.Value
                    End If

                    If entidad.BloqueadoHasta.HasValue Then
                        cmd.Parameters.Add("P_BLOQUEADO_HASTA", OracleDbType.TimeStamp).Value = entidad.BloqueadoHasta.Value
                    Else
                        cmd.Parameters.Add("P_BLOQUEADO_HASTA", OracleDbType.TimeStamp).Value = DBNull.Value
                    End If

                    cmd.Parameters.Add("P_ESTADO", OracleDbType.Varchar2).Value = entidad.Estado

                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Sub Eliminar(ByVal id As Integer)
            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_USUARIO.SP_ELIMINAR", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_ID", OracleDbType.Int32).Value = id

                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Function ObtenerPorId(ByVal id As Integer) As Usuario
            Dim entidad As Usuario = Nothing

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_USUARIO.SP_OBTENER", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_ID", OracleDbType.Int32).Value = id
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        If dr.Read() Then
                            entidad = MapearUsuario(dr)
                        End If
                    End Using
                End Using
            End Using

            Return entidad
        End Function

        Public Function Listar() As List(Of Usuario)
            Dim lista As New List(Of Usuario)()

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_USUARIO.SP_LISTAR", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        While dr.Read()
                            lista.Add(MapearUsuario(dr))
                        End While
                    End Using
                End Using
            End Using

            Return lista
        End Function

        Public Function Buscar(ByVal valor As String) As List(Of Usuario)
            Dim lista As New List(Of Usuario)()

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_USUARIO.SP_BUSCAR", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_VALOR", OracleDbType.Varchar2).Value = valor
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        While dr.Read()
                            lista.Add(MapearUsuario(dr))
                        End While
                    End Using
                End Using
            End Using

            Return lista
        End Function

        Private Function MapearUsuario(ByVal dr As OracleDataReader) As Usuario
            Dim entidad As New Usuario()

            entidad.UsuId = Convert.ToInt32(dr("USU_ID"))
            entidad.Username = dr("USERNAME").ToString()
            entidad.PasswordHash = dr("PASSWORD_HASH").ToString()
            entidad.Email = dr("EMAIL").ToString()
            entidad.Telefono = If(IsDBNull(dr("TELEFONO")), Nothing, dr("TELEFONO").ToString())
            entidad.RolId = Convert.ToInt32(dr("ROL_ID"))
            entidad.CliId = If(IsDBNull(dr("CLI_ID")), CType(Nothing, Integer?), Convert.ToInt32(dr("CLI_ID")))
            entidad.EmpId = If(IsDBNull(dr("EMP_ID")), CType(Nothing, Integer?), Convert.ToInt32(dr("EMP_ID")))
            entidad.UltimoLoginAt = If(IsDBNull(dr("ULTIMO_LOGIN_AT")), CType(Nothing, DateTime?), Convert.ToDateTime(dr("ULTIMO_LOGIN_AT")))
            entidad.BloqueadoHasta = If(IsDBNull(dr("BLOQUEADO_HASTA")), CType(Nothing, DateTime?), Convert.ToDateTime(dr("BLOQUEADO_HASTA")))
            entidad.CreatedAt = If(IsDBNull(dr("CREATED_AT")), CType(Nothing, DateTime?), Convert.ToDateTime(dr("CREATED_AT")))
            entidad.UpdatedAt = If(IsDBNull(dr("UPDATED_AT")), CType(Nothing, DateTime?), Convert.ToDateTime(dr("UPDATED_AT")))
            entidad.Estado = dr("ESTADO").ToString()

            Return entidad
        End Function

    End Class
End Namespace