Option Strict On
Option Explicit On

Imports System
Imports System.Collections.Generic
Imports System.Data
Imports Oracle.ManagedDataAccess.Client
Imports Alpes.Datos.Conexion
Imports Alpes.Entidades.Inventario

Namespace Repositorios
    Public Class Movimiento_InventarioDatos

        Private ReadOnly _conexionOracle As ConexionOracle

        Public Sub New()
            _conexionOracle = New ConexionOracle()
        End Sub

        Public Function Insertar(ByVal entidad As Movimiento_Inventario) As Integer
            Dim idGenerado As Integer = 0

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_MOVIMIENTO_INVENTARIO.SP_INSERTAR_MOVIMIENTO_INVENTARIO", cn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.BindByName = True

                    cmd.Parameters.Add("P_INV_PROD_ID", OracleDbType.Int32).Value = entidad.InvProdId
                    cmd.Parameters.Add("P_TIPO_MOV", OracleDbType.Varchar2).Value = entidad.TipoMov
                    cmd.Parameters.Add("P_CANTIDAD", OracleDbType.Int32).Value = entidad.Cantidad

                    If String.IsNullOrWhiteSpace(entidad.Motivo) Then
                        cmd.Parameters.Add("P_MOTIVO", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("P_MOTIVO", OracleDbType.Varchar2).Value = entidad.Motivo
                    End If

                    If entidad.ReferenciaId.HasValue Then
                        cmd.Parameters.Add("P_REFERENCIA_ID", OracleDbType.Int32).Value = entidad.ReferenciaId.Value
                    Else
                        cmd.Parameters.Add("P_REFERENCIA_ID", OracleDbType.Int32).Value = DBNull.Value
                    End If

                    cmd.Parameters.Add("P_MOV_AT", OracleDbType.TimeStamp).Value = entidad.MovAt
                    cmd.Parameters.Add("P_MOV_INV_ID", OracleDbType.Int32).Direction = ParameterDirection.Output

                    cmd.ExecuteNonQuery()
                    idGenerado = Convert.ToInt32(cmd.Parameters("P_MOV_INV_ID").Value.ToString())
                End Using
            End Using

            Return idGenerado
        End Function

        Public Sub Actualizar(ByVal entidad As Movimiento_Inventario)
            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_MOVIMIENTO_INVENTARIO.SP_ACTUALIZAR_MOVIMIENTO_INVENTARIO", cn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.BindByName = True

                    cmd.Parameters.Add("P_MOV_INV_ID", OracleDbType.Int32).Value = entidad.MovInvId
                    cmd.Parameters.Add("P_INV_PROD_ID", OracleDbType.Int32).Value = entidad.InvProdId
                    cmd.Parameters.Add("P_TIPO_MOV", OracleDbType.Varchar2).Value = entidad.TipoMov
                    cmd.Parameters.Add("P_CANTIDAD", OracleDbType.Int32).Value = entidad.Cantidad

                    If String.IsNullOrWhiteSpace(entidad.Motivo) Then
                        cmd.Parameters.Add("P_MOTIVO", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("P_MOTIVO", OracleDbType.Varchar2).Value = entidad.Motivo
                    End If

                    If entidad.ReferenciaId.HasValue Then
                        cmd.Parameters.Add("P_REFERENCIA_ID", OracleDbType.Int32).Value = entidad.ReferenciaId.Value
                    Else
                        cmd.Parameters.Add("P_REFERENCIA_ID", OracleDbType.Int32).Value = DBNull.Value
                    End If

                    cmd.Parameters.Add("P_MOV_AT", OracleDbType.TimeStamp).Value = entidad.MovAt

                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Sub Eliminar(ByVal id As Integer)
            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_MOVIMIENTO_INVENTARIO.SP_ELIMINAR_MOVIMIENTO_INVENTARIO", cn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.BindByName = True

                    cmd.Parameters.Add("P_MOV_INV_ID", OracleDbType.Int32).Value = id
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Function ObtenerPorId(ByVal id As Integer) As Movimiento_Inventario
            Dim entidad As Movimiento_Inventario = Nothing

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_MOVIMIENTO_INVENTARIO.SP_OBTENER_MOVIMIENTO_INVENTARIO", cn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.BindByName = True

                    cmd.Parameters.Add("P_MOV_INV_ID", OracleDbType.Int32).Value = id
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        If dr.Read() Then
                            entidad = MapearMovimientoInventario(dr)
                        End If
                    End Using
                End Using
            End Using

            Return entidad
        End Function

        Public Function Listar() As List(Of Movimiento_Inventario)
            Dim lista As New List(Of Movimiento_Inventario)()

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_MOVIMIENTO_INVENTARIO.SP_LISTAR_MOVIMIENTO_INVENTARIO", cn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.BindByName = True

                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        While dr.Read()
                            lista.Add(MapearMovimientoInventario(dr))
                        End While
                    End Using
                End Using
            End Using

            Return lista
        End Function

        Public Function Buscar(ByVal criterio As String, ByVal valor As String) As List(Of Movimiento_Inventario)
            Dim lista As New List(Of Movimiento_Inventario)()

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_MOVIMIENTO_INVENTARIO.SP_BUSCAR_MOVIMIENTO_INVENTARIO", cn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.BindByName = True

                    cmd.Parameters.Add("P_CRITERIO", OracleDbType.Varchar2).Value = criterio
                    cmd.Parameters.Add("P_VALOR", OracleDbType.Varchar2).Value = valor
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        While dr.Read()
                            lista.Add(MapearMovimientoInventario(dr))
                        End While
                    End Using
                End Using
            End Using

            Return lista
        End Function

        Private Function MapearMovimientoInventario(ByVal dr As OracleDataReader) As Movimiento_Inventario
            Dim entidad As New Movimiento_Inventario()

            If TieneColumna(dr, "MOV_INV_ID") AndAlso Not IsDBNull(dr("MOV_INV_ID")) Then
                entidad.MovInvId = Convert.ToInt32(dr("MOV_INV_ID"))
            End If

            If TieneColumna(dr, "INV_PROD_ID") AndAlso Not IsDBNull(dr("INV_PROD_ID")) Then
                entidad.InvProdId = Convert.ToInt32(dr("INV_PROD_ID"))
            End If

            If TieneColumna(dr, "TIPO_MOV") AndAlso Not IsDBNull(dr("TIPO_MOV")) Then
                entidad.TipoMov = dr("TIPO_MOV").ToString()
            Else
                entidad.TipoMov = String.Empty
            End If

            If TieneColumna(dr, "CANTIDAD") AndAlso Not IsDBNull(dr("CANTIDAD")) Then
                entidad.Cantidad = Convert.ToInt32(dr("CANTIDAD"))
            End If

            If TieneColumna(dr, "MOTIVO") AndAlso Not IsDBNull(dr("MOTIVO")) Then
                entidad.Motivo = dr("MOTIVO").ToString()
            Else
                entidad.Motivo = String.Empty
            End If

            If TieneColumna(dr, "REFERENCIA_ID") AndAlso Not IsDBNull(dr("REFERENCIA_ID")) Then
                entidad.ReferenciaId = Convert.ToInt32(dr("REFERENCIA_ID"))
            Else
                entidad.ReferenciaId = Nothing
            End If

            If TieneColumna(dr, "MOV_AT") AndAlso Not IsDBNull(dr("MOV_AT")) Then
                entidad.MovAt = Convert.ToDateTime(dr("MOV_AT"))
            End If

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

            If TieneColumna(dr, "ESTADO") AndAlso Not IsDBNull(dr("ESTADO")) Then
                entidad.Estado = dr("ESTADO").ToString()
            Else
                entidad.Estado = String.Empty
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