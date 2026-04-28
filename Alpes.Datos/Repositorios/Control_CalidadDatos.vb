Option Strict On
Option Explicit On

Imports System
Imports System.Collections.Generic
Imports System.Data
Imports Oracle.ManagedDataAccess.Client
Imports Alpes.Datos.Conexion
Imports Alpes.Entidades.Compras

Namespace Repositorios
    Public Class Control_CalidadDatos

        Private ReadOnly _conexionOracle As ConexionOracle

        Public Sub New()
            _conexionOracle = New ConexionOracle()
        End Sub

        Public Function Insertar(ByVal entidad As Control_Calidad) As Integer
            Dim idGenerado As Integer = 0

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_CONTROL_CALIDAD.SP_INSERTAR", cn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.BindByName = True

                    cmd.Parameters.Add("P_ORIGEN", OracleDbType.Varchar2).Value = entidad.Origen

                    If entidad.OrdenProduccionId.HasValue Then
                        cmd.Parameters.Add("P_ORDEN_PRODUCCION_ID", OracleDbType.Int32).Value = entidad.OrdenProduccionId.Value
                    Else
                        cmd.Parameters.Add("P_ORDEN_PRODUCCION_ID", OracleDbType.Int32).Value = DBNull.Value
                    End If

                    If entidad.RecepcionMaterialId.HasValue Then
                        cmd.Parameters.Add("P_RECEPCION_MATERIAL_ID", OracleDbType.Int32).Value = entidad.RecepcionMaterialId.Value
                    Else
                        cmd.Parameters.Add("P_RECEPCION_MATERIAL_ID", OracleDbType.Int32).Value = DBNull.Value
                    End If

                    cmd.Parameters.Add("P_RESULTADO", OracleDbType.Varchar2).Value = entidad.Resultado

                    If String.IsNullOrWhiteSpace(entidad.Observacion) Then
                        cmd.Parameters.Add("P_OBSERVACION", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("P_OBSERVACION", OracleDbType.Varchar2).Value = entidad.Observacion
                    End If

                    cmd.Parameters.Add("P_INSPECCION_AT", OracleDbType.TimeStamp).Value = entidad.InspeccionAt

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

        Public Sub Actualizar(ByVal entidad As Control_Calidad)
            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_CONTROL_CALIDAD.SP_ACTUALIZAR", cn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.BindByName = True

                    cmd.Parameters.Add("P_CONTROL_CALIDAD_ID", OracleDbType.Int32).Value = entidad.ControlCalidadId
                    cmd.Parameters.Add("P_ORIGEN", OracleDbType.Varchar2).Value = entidad.Origen

                    If entidad.OrdenProduccionId.HasValue Then
                        cmd.Parameters.Add("P_ORDEN_PRODUCCION_ID", OracleDbType.Int32).Value = entidad.OrdenProduccionId.Value
                    Else
                        cmd.Parameters.Add("P_ORDEN_PRODUCCION_ID", OracleDbType.Int32).Value = DBNull.Value
                    End If

                    If entidad.RecepcionMaterialId.HasValue Then
                        cmd.Parameters.Add("P_RECEPCION_MATERIAL_ID", OracleDbType.Int32).Value = entidad.RecepcionMaterialId.Value
                    Else
                        cmd.Parameters.Add("P_RECEPCION_MATERIAL_ID", OracleDbType.Int32).Value = DBNull.Value
                    End If

                    cmd.Parameters.Add("P_RESULTADO", OracleDbType.Varchar2).Value = entidad.Resultado

                    If String.IsNullOrWhiteSpace(entidad.Observacion) Then
                        cmd.Parameters.Add("P_OBSERVACION", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("P_OBSERVACION", OracleDbType.Varchar2).Value = entidad.Observacion
                    End If

                    cmd.Parameters.Add("P_INSPECCION_AT", OracleDbType.TimeStamp).Value = entidad.InspeccionAt

                    If String.IsNullOrWhiteSpace(entidad.Estado) Then
                        cmd.Parameters.Add("P_ESTADO", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("P_ESTADO", OracleDbType.Varchar2).Value = entidad.Estado
                    End If

                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Sub Eliminar(ByVal id As Integer)
            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_CONTROL_CALIDAD.SP_ELIMINAR", cn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.BindByName = True

                    cmd.Parameters.Add("P_ID", OracleDbType.Int32).Value = id
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Function ObtenerPorId(ByVal id As Integer) As Control_Calidad
            Dim entidad As Control_Calidad = Nothing

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_CONTROL_CALIDAD.SP_OBTENER", cn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.BindByName = True

                    cmd.Parameters.Add("P_ID", OracleDbType.Int32).Value = id
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        If dr.Read() Then
                            entidad = MapearControlCalidad(dr)
                        End If
                    End Using
                End Using
            End Using

            Return entidad
        End Function

        Public Function Listar() As List(Of Control_Calidad)
            Dim lista As New List(Of Control_Calidad)()

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_CONTROL_CALIDAD.SP_LISTAR", cn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.BindByName = True

                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        While dr.Read()
                            lista.Add(MapearControlCalidad(dr))
                        End While
                    End Using
                End Using
            End Using

            Return lista
        End Function

        Public Function Buscar(ByVal valor As String) As List(Of Control_Calidad)
            Dim lista As New List(Of Control_Calidad)()

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_CONTROL_CALIDAD.SP_BUSCAR", cn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.BindByName = True

                    cmd.Parameters.Add("P_VALOR", OracleDbType.Varchar2).Value = valor
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        While dr.Read()
                            lista.Add(MapearControlCalidad(dr))
                        End While
                    End Using
                End Using
            End Using

            Return lista
        End Function

        Private Function MapearControlCalidad(ByVal dr As OracleDataReader) As Control_Calidad
            Dim entidad As New Control_Calidad()

            If Not IsDBNull(dr("CONTROL_CALIDAD_ID")) Then
                entidad.ControlCalidadId = Convert.ToInt32(dr("CONTROL_CALIDAD_ID"))
            End If

            entidad.Origen = If(IsDBNull(dr("ORIGEN")), String.Empty, dr("ORIGEN").ToString())

            If Not IsDBNull(dr("ORDEN_PRODUCCION_ID")) Then
                entidad.OrdenProduccionId = Convert.ToInt32(dr("ORDEN_PRODUCCION_ID"))
            Else
                entidad.OrdenProduccionId = Nothing
            End If

            If Not IsDBNull(dr("RECEPCION_MATERIAL_ID")) Then
                entidad.RecepcionMaterialId = Convert.ToInt32(dr("RECEPCION_MATERIAL_ID"))
            Else
                entidad.RecepcionMaterialId = Nothing
            End If

            entidad.Resultado = If(IsDBNull(dr("RESULTADO")), String.Empty, dr("RESULTADO").ToString())
            entidad.Observacion = If(IsDBNull(dr("OBSERVACION")), String.Empty, dr("OBSERVACION").ToString())

            If Not IsDBNull(dr("INSPECCION_AT")) Then
                entidad.InspeccionAt = Convert.ToDateTime(dr("INSPECCION_AT"))
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

            entidad.Estado = If(IsDBNull(dr("ESTADO")), String.Empty, dr("ESTADO").ToString())

            Return entidad
        End Function

    End Class
End Namespace