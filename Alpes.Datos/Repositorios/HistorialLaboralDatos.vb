Option Strict On
Option Explicit On

Imports System.Data
Imports Oracle.ManagedDataAccess.Client
Imports Alpes.Datos.Conexion
Imports Alpes.Entidades.RRHH

Namespace Repositorios
    Public Class HistorialLaboralDatos

        Private ReadOnly _conexionOracle As ConexionOracle

        Public Sub New()
            _conexionOracle = New ConexionOracle()
        End Sub

        Public Function Insertar(ByVal entidad As HistorialLaboral) As Integer
            Dim idGenerado As Integer = 0

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_HISTORIAL_LABORAL.INSERTAR", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_EMP_ID", OracleDbType.Int32).Value = entidad.EmpId
                    cmd.Parameters.Add("P_FECHA_CAMBIO", OracleDbType.Date).Value = entidad.FechaCambio
                    cmd.Parameters.Add("P_TIPO_CAMBIO", OracleDbType.Varchar2).Value = entidad.TipoCambio

                    If String.IsNullOrWhiteSpace(entidad.Detalle) Then
                        cmd.Parameters.Add("P_DETALLE", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("P_DETALLE", OracleDbType.Varchar2).Value = entidad.Detalle
                    End If

                    cmd.Parameters.Add("P_ESTADO", OracleDbType.Varchar2).Value = entidad.Estado
                    cmd.Parameters.Add("P_HISTORIAL_LABORAL_ID", OracleDbType.Int32).Direction = ParameterDirection.Output

                    cmd.ExecuteNonQuery()

                    idGenerado = Convert.ToInt32(cmd.Parameters("P_HISTORIAL_LABORAL_ID").Value.ToString())
                End Using
            End Using

            Return idGenerado
        End Function

        Public Sub Actualizar(ByVal entidad As HistorialLaboral)
            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_HISTORIAL_LABORAL.ACTUALIZAR", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_HISTORIAL_LABORAL_ID", OracleDbType.Int32).Value = entidad.HistorialLaboralId
                    cmd.Parameters.Add("P_EMP_ID", OracleDbType.Int32).Value = entidad.EmpId
                    cmd.Parameters.Add("P_FECHA_CAMBIO", OracleDbType.Date).Value = entidad.FechaCambio
                    cmd.Parameters.Add("P_TIPO_CAMBIO", OracleDbType.Varchar2).Value = entidad.TipoCambio

                    If String.IsNullOrWhiteSpace(entidad.Detalle) Then
                        cmd.Parameters.Add("P_DETALLE", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("P_DETALLE", OracleDbType.Varchar2).Value = entidad.Detalle
                    End If

                    cmd.Parameters.Add("P_ESTADO", OracleDbType.Varchar2).Value = entidad.Estado

                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Sub Eliminar(ByVal id As Integer)
            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_HISTORIAL_LABORAL.ELIMINAR", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_HISTORIAL_LABORAL_ID", OracleDbType.Int32).Value = id

                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Function ObtenerPorId(ByVal id As Integer) As HistorialLaboral
            Dim entidad As HistorialLaboral = Nothing

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_HISTORIAL_LABORAL.OBTENER_POR_ID", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_HISTORIAL_LABORAL_ID", OracleDbType.Int32).Value = id
                    cmd.Parameters.Add("P_RESULTADO", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        If dr.Read() Then
                            entidad = MapearHistorialLaboral(dr)
                        End If
                    End Using
                End Using
            End Using

            Return entidad
        End Function

        Public Function Listar() As List(Of HistorialLaboral)
            Dim lista As New List(Of HistorialLaboral)()

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_HISTORIAL_LABORAL.LISTAR", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_RESULTADO", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        While dr.Read()
                            lista.Add(MapearHistorialLaboral(dr))
                        End While
                    End Using
                End Using
            End Using

            Return lista
        End Function

        Private Function MapearHistorialLaboral(ByVal dr As OracleDataReader) As HistorialLaboral
            Dim entidad As New HistorialLaboral()

            entidad.HistorialLaboralId = Convert.ToInt32(dr("HISTORIAL_LABORAL_ID"))
            entidad.EmpId = Convert.ToInt32(dr("EMP_ID"))
            entidad.FechaCambio = Convert.ToDateTime(dr("FECHA_CAMBIO"))
            entidad.TipoCambio = dr("TIPO_CAMBIO").ToString()
            entidad.Detalle = If(IsDBNull(dr("DETALLE")), Nothing, dr("DETALLE").ToString())
            entidad.Estado = If(IsDBNull(dr("ESTADO")), Nothing, dr("ESTADO").ToString())
            entidad.CreatedAt = If(IsDBNull(dr("CREATED_AT")), CType(Nothing, DateTime?), Convert.ToDateTime(dr("CREATED_AT")))
            entidad.UpdatedAt = If(IsDBNull(dr("UPDATED_AT")), CType(Nothing, DateTime?), Convert.ToDateTime(dr("UPDATED_AT")))

            Return entidad
        End Function

    End Class
End Namespace