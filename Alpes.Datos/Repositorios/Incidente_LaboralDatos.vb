Option Strict On
Option Explicit On

Imports System
Imports System.Collections.Generic
Imports System.Data
Imports Oracle.ManagedDataAccess.Client
Imports Alpes.Datos.Conexion
Imports Alpes.Entidades.RRHH

Namespace Repositorios
    Public Class IncidenteLaboralDatos

        Private ReadOnly _conexionOracle As ConexionOracle

        Public Sub New()
            _conexionOracle = New ConexionOracle()
        End Sub

        Public Function Insertar(ByVal entidad As IncidenteLaboral) As Integer
            Dim idGenerado As Integer = 0

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_INCIDENTE_LABORAL.INSERTAR", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    If entidad.EmpId.HasValue Then
                        cmd.Parameters.Add("p_emp_id", OracleDbType.Int32).Value = entidad.EmpId.Value
                    Else
                        cmd.Parameters.Add("p_emp_id", OracleDbType.Int32).Value = DBNull.Value
                    End If

                    If entidad.FechaIncidente.HasValue Then
                        cmd.Parameters.Add("p_fecha_incidente", OracleDbType.Date).Value = entidad.FechaIncidente.Value
                    Else
                        cmd.Parameters.Add("p_fecha_incidente", OracleDbType.Date).Value = DBNull.Value
                    End If

                    cmd.Parameters.Add("p_descripcion", OracleDbType.Varchar2).Value = entidad.Descripcion

                    If String.IsNullOrWhiteSpace(entidad.Gravedad) Then
                        cmd.Parameters.Add("p_gravedad", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("p_gravedad", OracleDbType.Varchar2).Value = entidad.Gravedad
                    End If

                    If String.IsNullOrWhiteSpace(entidad.AccionesTomadas) Then
                        cmd.Parameters.Add("p_acciones_tomadas", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("p_acciones_tomadas", OracleDbType.Varchar2).Value = entidad.AccionesTomadas
                    End If

                    If String.IsNullOrWhiteSpace(entidad.Estado) Then
                        cmd.Parameters.Add("p_estado", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("p_estado", OracleDbType.Varchar2).Value = entidad.Estado
                    End If

                    cmd.Parameters.Add("p_incidente_id", OracleDbType.Int32).Direction = ParameterDirection.Output

                    cmd.ExecuteNonQuery()

                    idGenerado = Convert.ToInt32(cmd.Parameters("p_incidente_id").Value.ToString())
                End Using
            End Using

            Return idGenerado
        End Function

        Public Sub Actualizar(ByVal entidad As IncidenteLaboral)
            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_INCIDENTE_LABORAL.ACTUALIZAR", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("p_incidente_id", OracleDbType.Int32).Value = entidad.IncidenteId

                    If entidad.EmpId.HasValue Then
                        cmd.Parameters.Add("p_emp_id", OracleDbType.Int32).Value = entidad.EmpId.Value
                    Else
                        cmd.Parameters.Add("p_emp_id", OracleDbType.Int32).Value = DBNull.Value
                    End If

                    If entidad.FechaIncidente.HasValue Then
                        cmd.Parameters.Add("p_fecha_incidente", OracleDbType.Date).Value = entidad.FechaIncidente.Value
                    Else
                        cmd.Parameters.Add("p_fecha_incidente", OracleDbType.Date).Value = DBNull.Value
                    End If

                    cmd.Parameters.Add("p_descripcion", OracleDbType.Varchar2).Value = entidad.Descripcion

                    If String.IsNullOrWhiteSpace(entidad.Gravedad) Then
                        cmd.Parameters.Add("p_gravedad", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("p_gravedad", OracleDbType.Varchar2).Value = entidad.Gravedad
                    End If

                    If String.IsNullOrWhiteSpace(entidad.AccionesTomadas) Then
                        cmd.Parameters.Add("p_acciones_tomadas", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("p_acciones_tomadas", OracleDbType.Varchar2).Value = entidad.AccionesTomadas
                    End If

                    cmd.Parameters.Add("p_estado", OracleDbType.Varchar2).Value = entidad.Estado

                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Sub Eliminar(ByVal id As Integer)
            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_INCIDENTE_LABORAL.ELIMINAR", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("p_incidente_id", OracleDbType.Int32).Value = id

                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Function ObtenerPorId(ByVal id As Integer) As IncidenteLaboral
            Dim entidad As IncidenteLaboral = Nothing

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_INCIDENTE_LABORAL.OBTENER_POR_ID", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("p_incidente_id", OracleDbType.Int32).Value = id
                    cmd.Parameters.Add("p_resultado", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        If dr.Read() Then
                            entidad = MapearIncidenteLaboral(dr)
                        End If
                    End Using
                End Using
            End Using

            Return entidad
        End Function

        Public Function Listar() As List(Of IncidenteLaboral)
            Dim lista As New List(Of IncidenteLaboral)()

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_INCIDENTE_LABORAL.LISTAR", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("p_resultado", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        While dr.Read()
                            lista.Add(MapearIncidenteLaboral(dr))
                        End While
                    End Using
                End Using
            End Using

            Return lista
        End Function

        Private Function MapearIncidenteLaboral(ByVal dr As OracleDataReader) As IncidenteLaboral
            Dim entidad As New IncidenteLaboral()

            entidad.IncidenteId = LeerEntero(dr, "INCIDENTE_ID")
            entidad.EmpId = LeerEnteroNullable(dr, "EMP_ID")
            entidad.FechaIncidente = LeerFechaNullable(dr, "FECHA_INCIDENTE")
            entidad.Descripcion = LeerTexto(dr, "DESCRIPCION")
            entidad.Gravedad = LeerTexto(dr, "GRAVEDAD")
            entidad.AccionesTomadas = LeerTexto(dr, "ACCIONES_TOMADAS")
            entidad.CreatedAt = LeerFechaNullable(dr, "CREATED_AT")
            entidad.UpdatedAt = LeerFechaNullable(dr, "UPDATED_AT")
            entidad.Estado = LeerTexto(dr, "ESTADO")

            Return entidad
        End Function

        Private Function LeerTexto(ByVal dr As OracleDataReader, ByVal columna As String) As String
            If TieneColumna(dr, columna) AndAlso Not dr.IsDBNull(dr.GetOrdinal(columna)) Then
                Return dr(columna).ToString()
            End If

            Return String.Empty
        End Function

        Private Function LeerEntero(ByVal dr As OracleDataReader, ByVal columna As String) As Integer
            If TieneColumna(dr, columna) AndAlso Not dr.IsDBNull(dr.GetOrdinal(columna)) Then
                Return Convert.ToInt32(dr(columna))
            End If

            Return 0
        End Function

        Private Function LeerEnteroNullable(ByVal dr As OracleDataReader, ByVal columna As String) As Integer?
            If TieneColumna(dr, columna) AndAlso Not dr.IsDBNull(dr.GetOrdinal(columna)) Then
                Return Convert.ToInt32(dr(columna))
            End If

            Return Nothing
        End Function

        Private Function LeerFechaNullable(ByVal dr As OracleDataReader, ByVal columna As String) As DateTime?
            If TieneColumna(dr, columna) AndAlso Not dr.IsDBNull(dr.GetOrdinal(columna)) Then
                Return Convert.ToDateTime(dr(columna))
            End If

            Return Nothing
        End Function

        Private Function TieneColumna(ByVal dr As OracleDataReader, ByVal nombreColumna As String) As Boolean
            For i As Integer = 0 To dr.FieldCount - 1
                If dr.GetName(i).ToUpper() = nombreColumna.ToUpper() Then
                    Return True
                End If
            Next

            Return False
        End Function

    End Class
End Namespace