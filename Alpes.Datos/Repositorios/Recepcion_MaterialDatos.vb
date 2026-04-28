Option Strict On
Option Explicit On

Imports System
Imports System.Collections.Generic
Imports System.Data
Imports Oracle.ManagedDataAccess.Client
Imports Alpes.Datos.Conexion
Imports Alpes.Entidades.Compras

Namespace Repositorios
    Public Class Recepcion_MaterialDatos

        Private ReadOnly _conexionOracle As ConexionOracle

        Public Sub New()
            _conexionOracle = New ConexionOracle()
        End Sub

        Public Function Insertar(ByVal entidad As Recepcion_Material) As Integer
            Dim idGenerado As Integer = 0

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_RECEPCION_MATERIAL.SP_INSERTAR_RECEPCION_MATERIAL", cn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.BindByName = True

                    cmd.Parameters.Add("P_ORDEN_COMPRA_ID", OracleDbType.Int32).Value = entidad.OrdenCompraId
                    cmd.Parameters.Add("P_FECHA_RECEPCION", OracleDbType.Date).Value = entidad.FechaRecepcion

                    If entidad.EmpIdRecibe.HasValue Then
                        cmd.Parameters.Add("P_EMP_ID_RECIBE", OracleDbType.Int32).Value = entidad.EmpIdRecibe.Value
                    Else
                        cmd.Parameters.Add("P_EMP_ID_RECIBE", OracleDbType.Int32).Value = DBNull.Value
                    End If

                    If String.IsNullOrWhiteSpace(entidad.Observaciones) Then
                        cmd.Parameters.Add("P_OBSERVACIONES", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("P_OBSERVACIONES", OracleDbType.Varchar2).Value = entidad.Observaciones
                    End If

                    cmd.Parameters.Add("P_RECEPCION_MATERIAL_ID", OracleDbType.Int32).Direction = ParameterDirection.Output

                    cmd.ExecuteNonQuery()
                    idGenerado = Convert.ToInt32(cmd.Parameters("P_RECEPCION_MATERIAL_ID").Value.ToString())
                End Using
            End Using

            Return idGenerado
        End Function

        Public Sub Actualizar(ByVal entidad As Recepcion_Material)
            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_RECEPCION_MATERIAL.SP_ACTUALIZAR_RECEPCION_MATERIAL", cn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.BindByName = True

                    cmd.Parameters.Add("P_RECEPCION_MATERIAL_ID", OracleDbType.Int32).Value = entidad.RecepcionMaterialId
                    cmd.Parameters.Add("P_ORDEN_COMPRA_ID", OracleDbType.Int32).Value = entidad.OrdenCompraId
                    cmd.Parameters.Add("P_FECHA_RECEPCION", OracleDbType.Date).Value = entidad.FechaRecepcion

                    If entidad.EmpIdRecibe.HasValue Then
                        cmd.Parameters.Add("P_EMP_ID_RECIBE", OracleDbType.Int32).Value = entidad.EmpIdRecibe.Value
                    Else
                        cmd.Parameters.Add("P_EMP_ID_RECIBE", OracleDbType.Int32).Value = DBNull.Value
                    End If

                    If String.IsNullOrWhiteSpace(entidad.Observaciones) Then
                        cmd.Parameters.Add("P_OBSERVACIONES", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("P_OBSERVACIONES", OracleDbType.Varchar2).Value = entidad.Observaciones
                    End If

                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Sub Eliminar(ByVal id As Integer)
            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_RECEPCION_MATERIAL.SP_ELIMINAR_RECEPCION_MATERIAL", cn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.BindByName = True

                    cmd.Parameters.Add("P_RECEPCION_MATERIAL_ID", OracleDbType.Int32).Value = id
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Function ObtenerPorId(ByVal id As Integer) As Recepcion_Material
            Dim entidad As Recepcion_Material = Nothing

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_RECEPCION_MATERIAL.SP_OBTENER_RECEPCION_MATERIAL", cn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.BindByName = True

                    cmd.Parameters.Add("P_RECEPCION_MATERIAL_ID", OracleDbType.Int32).Value = id
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        If dr.Read() Then
                            entidad = MapearRecepcionMaterialDetalle(dr)
                        End If
                    End Using
                End Using
            End Using

            Return entidad
        End Function

        Public Function Listar() As List(Of Recepcion_Material)
            Dim lista As New List(Of Recepcion_Material)()

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_RECEPCION_MATERIAL.SP_LISTAR_RECEPCIONES_MATERIAL", cn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.BindByName = True

                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        While dr.Read()
                            Dim itemBase As Recepcion_Material = MapearRecepcionMaterialLista(dr)
                            Dim itemDetalle As Recepcion_Material = ObtenerPorId(itemBase.RecepcionMaterialId)

                            If itemDetalle IsNot Nothing Then
                                If itemDetalle.OrdenCompraId <= 0 Then
                                    itemDetalle.OrdenCompraId = itemBase.OrdenCompraId
                                End If

                                If String.IsNullOrWhiteSpace(itemDetalle.NumOc) Then
                                    itemDetalle.NumOc = itemBase.NumOc
                                End If

                                If Not itemDetalle.EmpIdRecibe.HasValue AndAlso itemBase.EmpIdRecibe.HasValue Then
                                    itemDetalle.EmpIdRecibe = itemBase.EmpIdRecibe
                                End If

                                If String.IsNullOrWhiteSpace(itemDetalle.EmpleadoRecibe) Then
                                    itemDetalle.EmpleadoRecibe = itemBase.EmpleadoRecibe
                                End If

                                If itemDetalle.FechaRecepcion = DateTime.MinValue Then
                                    itemDetalle.FechaRecepcion = itemBase.FechaRecepcion
                                End If

                                If String.IsNullOrWhiteSpace(itemDetalle.Estado) Then
                                    itemDetalle.Estado = itemBase.Estado
                                End If

                                lista.Add(itemDetalle)
                            Else
                                lista.Add(itemBase)
                            End If
                        End While
                    End Using
                End Using
            End Using

            Return lista
        End Function

        Public Function Buscar(ByVal criterio As String, ByVal valor As String) As List(Of Recepcion_Material)
            Dim lista As New List(Of Recepcion_Material)()

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_RECEPCION_MATERIAL.SP_BUSCAR_RECEPCIONES_MATERIAL", cn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.BindByName = True

                    cmd.Parameters.Add("P_CRITERIO", OracleDbType.Varchar2).Value = criterio
                    cmd.Parameters.Add("P_VALOR", OracleDbType.Varchar2).Value = valor
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        While dr.Read()
                            Dim itemBase As Recepcion_Material = MapearRecepcionMaterialLista(dr)
                            Dim itemDetalle As Recepcion_Material = ObtenerPorId(itemBase.RecepcionMaterialId)

                            If itemDetalle IsNot Nothing Then
                                If itemDetalle.OrdenCompraId <= 0 Then
                                    itemDetalle.OrdenCompraId = itemBase.OrdenCompraId
                                End If

                                If String.IsNullOrWhiteSpace(itemDetalle.NumOc) Then
                                    itemDetalle.NumOc = itemBase.NumOc
                                End If

                                If Not itemDetalle.EmpIdRecibe.HasValue AndAlso itemBase.EmpIdRecibe.HasValue Then
                                    itemDetalle.EmpIdRecibe = itemBase.EmpIdRecibe
                                End If

                                If String.IsNullOrWhiteSpace(itemDetalle.EmpleadoRecibe) Then
                                    itemDetalle.EmpleadoRecibe = itemBase.EmpleadoRecibe
                                End If

                                If itemDetalle.FechaRecepcion = DateTime.MinValue Then
                                    itemDetalle.FechaRecepcion = itemBase.FechaRecepcion
                                End If

                                If String.IsNullOrWhiteSpace(itemDetalle.Estado) Then
                                    itemDetalle.Estado = itemBase.Estado
                                End If

                                lista.Add(itemDetalle)
                            Else
                                lista.Add(itemBase)
                            End If
                        End While
                    End Using
                End Using
            End Using

            Return lista
        End Function

        Private Function MapearRecepcionMaterialLista(ByVal dr As OracleDataReader) As Recepcion_Material
            Dim entidad As New Recepcion_Material()

            If TieneColumna(dr, "RECEPCION_MATERIAL_ID") AndAlso Not IsDBNull(dr("RECEPCION_MATERIAL_ID")) Then
                entidad.RecepcionMaterialId = Convert.ToInt32(dr("RECEPCION_MATERIAL_ID"))
            End If

            If TieneColumna(dr, "ORDEN_COMPRA_ID") AndAlso Not IsDBNull(dr("ORDEN_COMPRA_ID")) Then
                entidad.OrdenCompraId = Convert.ToInt32(dr("ORDEN_COMPRA_ID"))
            End If

            entidad.NumOc = If(TieneColumna(dr, "NUM_OC") AndAlso Not IsDBNull(dr("NUM_OC")), dr("NUM_OC").ToString(), String.Empty)

            If TieneColumna(dr, "FECHA_RECEPCION") AndAlso Not IsDBNull(dr("FECHA_RECEPCION")) Then
                entidad.FechaRecepcion = Convert.ToDateTime(dr("FECHA_RECEPCION"))
            End If

            If TieneColumna(dr, "EMP_ID_RECIBE") AndAlso Not IsDBNull(dr("EMP_ID_RECIBE")) Then
                entidad.EmpIdRecibe = Convert.ToInt32(dr("EMP_ID_RECIBE"))
            Else
                entidad.EmpIdRecibe = Nothing
            End If

            entidad.EmpleadoRecibe = If(TieneColumna(dr, "EMPLEADO_RECIBE") AndAlso Not IsDBNull(dr("EMPLEADO_RECIBE")), dr("EMPLEADO_RECIBE").ToString(), String.Empty)
            entidad.Observaciones = If(TieneColumna(dr, "OBSERVACIONES") AndAlso Not IsDBNull(dr("OBSERVACIONES")), dr("OBSERVACIONES").ToString(), String.Empty)
            entidad.Estado = If(TieneColumna(dr, "ESTADO") AndAlso Not IsDBNull(dr("ESTADO")), dr("ESTADO").ToString(), String.Empty)

            If TieneColumna(dr, "CREATED_AT") AndAlso Not IsDBNull(dr("CREATED_AT")) Then
                entidad.CreatedAt = Convert.ToDateTime(dr("CREATED_AT"))
            Else
                entidad.CreatedAt = Nothing
            End If

            If TieneColumna(dr, "UPDATED_AT") AndAlso Not IsDBNull(dr("UPDATED_AT")) Then
                entidad.UpdatedAt = Convert.ToDateTime(dr("UPDATED_AT"))
            Else
                entidad.UpdatedAt = Nothing
            End If

            Return entidad
        End Function

        Private Function MapearRecepcionMaterialDetalle(ByVal dr As OracleDataReader) As Recepcion_Material
            Dim entidad As New Recepcion_Material()

            If TieneColumna(dr, "RECEPCION_MATERIAL_ID") AndAlso Not IsDBNull(dr("RECEPCION_MATERIAL_ID")) Then
                entidad.RecepcionMaterialId = Convert.ToInt32(dr("RECEPCION_MATERIAL_ID"))
            End If

            If TieneColumna(dr, "ORDEN_COMPRA_ID") AndAlso Not IsDBNull(dr("ORDEN_COMPRA_ID")) Then
                entidad.OrdenCompraId = Convert.ToInt32(dr("ORDEN_COMPRA_ID"))
            End If

            entidad.NumOc = If(TieneColumna(dr, "NUM_OC") AndAlso Not IsDBNull(dr("NUM_OC")), dr("NUM_OC").ToString(), String.Empty)

            If TieneColumna(dr, "FECHA_RECEPCION") AndAlso Not IsDBNull(dr("FECHA_RECEPCION")) Then
                entidad.FechaRecepcion = Convert.ToDateTime(dr("FECHA_RECEPCION"))
            End If

            If TieneColumna(dr, "EMP_ID_RECIBE") AndAlso Not IsDBNull(dr("EMP_ID_RECIBE")) Then
                entidad.EmpIdRecibe = Convert.ToInt32(dr("EMP_ID_RECIBE"))
            Else
                entidad.EmpIdRecibe = Nothing
            End If

            entidad.EmpleadoRecibe = If(TieneColumna(dr, "EMPLEADO_RECIBE") AndAlso Not IsDBNull(dr("EMPLEADO_RECIBE")), dr("EMPLEADO_RECIBE").ToString(), String.Empty)
            entidad.Observaciones = If(TieneColumna(dr, "OBSERVACIONES") AndAlso Not IsDBNull(dr("OBSERVACIONES")), dr("OBSERVACIONES").ToString(), String.Empty)
            entidad.Estado = If(TieneColumna(dr, "ESTADO") AndAlso Not IsDBNull(dr("ESTADO")), dr("ESTADO").ToString(), String.Empty)

            If TieneColumna(dr, "CREATED_AT") AndAlso Not IsDBNull(dr("CREATED_AT")) Then
                entidad.CreatedAt = Convert.ToDateTime(dr("CREATED_AT"))
            Else
                entidad.CreatedAt = Nothing
            End If

            If TieneColumna(dr, "UPDATED_AT") AndAlso Not IsDBNull(dr("UPDATED_AT")) Then
                entidad.UpdatedAt = Convert.ToDateTime(dr("UPDATED_AT"))
            Else
                entidad.UpdatedAt = Nothing
            End If

            Return entidad
        End Function

        Private Function TieneColumna(ByVal dr As IDataRecord, ByVal nombreColumna As String) As Boolean
            For i As Integer = 0 To dr.FieldCount - 1
                If String.Equals(dr.GetName(i), nombreColumna, StringComparison.OrdinalIgnoreCase) Then
                    Return True
                End If
            Next

            Return False
        End Function

    End Class
End Namespace