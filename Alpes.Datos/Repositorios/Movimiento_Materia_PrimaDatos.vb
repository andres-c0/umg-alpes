Option Strict On
Option Explicit On

Imports System
Imports System.Collections.Generic
Imports System.Data
Imports Oracle.ManagedDataAccess.Client
Imports Alpes.Datos.Conexion
Imports Alpes.Entidades.Produccion

Namespace Repositorios
    Public Class Movimiento_Materia_PrimaDatos

        Private ReadOnly _conexionOracle As ConexionOracle

        Public Sub New()
            _conexionOracle = New ConexionOracle()
        End Sub

        Public Function Insertar(ByVal entidad As Movimiento_Materia_Prima) As Integer
            Dim idGenerado As Integer = 0

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_MOVIMIENTO_MATERIAS_PRIMA.SP_INSERTAR", cn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.BindByName = True

                    cmd.Parameters.Add("P_INV_MP_ID", OracleDbType.Int32).Value = entidad.InvMpId
                    cmd.Parameters.Add("P_TIPO_MOV", OracleDbType.Varchar2).Value = entidad.TipoMov
                    cmd.Parameters.Add("P_CANTIDAD", OracleDbType.Decimal).Value = entidad.Cantidad

                    If String.IsNullOrWhiteSpace(entidad.Motivo) Then
                        cmd.Parameters.Add("P_MOTIVO", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("P_MOTIVO", OracleDbType.Varchar2).Value = entidad.Motivo
                    End If

                    cmd.Parameters.Add("P_MOV_AT", OracleDbType.TimeStamp).Value = entidad.MovAt

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

        Public Sub Actualizar(ByVal entidad As Movimiento_Materia_Prima)
            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_MOVIMIENTO_MATERIAS_PRIMA.SP_ACTUALIZAR", cn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.BindByName = True

                    cmd.Parameters.Add("P_MOV_MP_ID", OracleDbType.Int32).Value = entidad.MovMpId
                    cmd.Parameters.Add("P_INV_MP_ID", OracleDbType.Int32).Value = entidad.InvMpId
                    cmd.Parameters.Add("P_TIPO_MOV", OracleDbType.Varchar2).Value = entidad.TipoMov
                    cmd.Parameters.Add("P_CANTIDAD", OracleDbType.Decimal).Value = entidad.Cantidad

                    If String.IsNullOrWhiteSpace(entidad.Motivo) Then
                        cmd.Parameters.Add("P_MOTIVO", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("P_MOTIVO", OracleDbType.Varchar2).Value = entidad.Motivo
                    End If

                    cmd.Parameters.Add("P_MOV_AT", OracleDbType.TimeStamp).Value = entidad.MovAt

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
                Using cmd As New OracleCommand("PKG_MOVIMIENTO_MATERIAS_PRIMA.SP_ELIMINAR", cn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.BindByName = True

                    cmd.Parameters.Add("P_ID", OracleDbType.Int32).Value = id
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Function ObtenerPorId(ByVal id As Integer) As Movimiento_Materia_Prima
            Dim entidad As Movimiento_Materia_Prima = Nothing

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_MOVIMIENTO_MATERIAS_PRIMA.SP_OBTENER", cn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.BindByName = True

                    cmd.Parameters.Add("P_ID", OracleDbType.Int32).Value = id
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        If dr.Read() Then
                            entidad = MapearMovimientoMateriaPrima(dr)
                        End If
                    End Using
                End Using
            End Using

            Return entidad
        End Function

        Public Function Listar() As List(Of Movimiento_Materia_Prima)
            Dim lista As New List(Of Movimiento_Materia_Prima)()

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_MOVIMIENTO_MATERIAS_PRIMA.SP_LISTAR", cn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.BindByName = True

                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        While dr.Read()
                            lista.Add(MapearMovimientoMateriaPrima(dr))
                        End While
                    End Using
                End Using
            End Using

            Return lista
        End Function

        Public Function Buscar(ByVal valor As String) As List(Of Movimiento_Materia_Prima)
            Dim lista As New List(Of Movimiento_Materia_Prima)()

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_MOVIMIENTO_MATERIAS_PRIMA.SP_BUSCAR", cn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.BindByName = True

                    cmd.Parameters.Add("P_VALOR", OracleDbType.Varchar2).Value = valor
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        While dr.Read()
                            lista.Add(MapearMovimientoMateriaPrima(dr))
                        End While
                    End Using
                End Using
            End Using

            Return lista
        End Function

        Private Function MapearMovimientoMateriaPrima(ByVal dr As OracleDataReader) As Movimiento_Materia_Prima
            Dim entidad As New Movimiento_Materia_Prima()

            If Not IsDBNull(dr("MOV_MP_ID")) Then
                entidad.MovMpId = Convert.ToInt32(dr("MOV_MP_ID"))
            End If

            If Not IsDBNull(dr("INV_MP_ID")) Then
                entidad.InvMpId = Convert.ToInt32(dr("INV_MP_ID"))
            End If

            entidad.TipoMov = If(IsDBNull(dr("TIPO_MOV")), String.Empty, dr("TIPO_MOV").ToString())

            If Not IsDBNull(dr("CANTIDAD")) Then
                entidad.Cantidad = Convert.ToDecimal(dr("CANTIDAD"))
            End If

            entidad.Motivo = If(IsDBNull(dr("MOTIVO")), String.Empty, dr("MOTIVO").ToString())

            If Not IsDBNull(dr("MOV_AT")) Then
                entidad.MovAt = Convert.ToDateTime(dr("MOV_AT"))
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