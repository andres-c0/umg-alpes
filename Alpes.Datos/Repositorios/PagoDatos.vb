Option Strict On
Option Explicit On

Imports System.Data
Imports Oracle.ManagedDataAccess.Client
Imports Alpes.Datos.Conexion
Imports Alpes.Entidades.Ventas

Namespace Repositorios
    Public Class PagoDatos

        Private ReadOnly _conexionOracle As ConexionOracle

        Public Sub New()
            _conexionOracle = New ConexionOracle()
        End Sub

        Public Function Insertar(ByVal entidad As Pago) As Integer
            Dim idGenerado As Integer = 0

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_PAGO.SP_INSERTAR", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_ORDEN_VENTA_ID", OracleDbType.Int32).Value = entidad.OrdenVentaId
                    cmd.Parameters.Add("P_METODO_PAGO_ID", OracleDbType.Int32).Value = entidad.MetodoPagoId
                    cmd.Parameters.Add("P_MONTO", OracleDbType.Decimal).Value = entidad.Monto

                    If String.IsNullOrWhiteSpace(entidad.EstadoPago) Then
                        cmd.Parameters.Add("P_ESTADO_PAGO", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("P_ESTADO_PAGO", OracleDbType.Varchar2).Value = entidad.EstadoPago
                    End If

                    If String.IsNullOrWhiteSpace(entidad.Referencia) Then
                        cmd.Parameters.Add("P_REFERENCIA", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("P_REFERENCIA", OracleDbType.Varchar2).Value = entidad.Referencia
                    End If

                    cmd.Parameters.Add("P_PAGO_AT", OracleDbType.TimeStamp).Value = entidad.PagoAt

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

        Public Sub Actualizar(ByVal entidad As Pago)
            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_PAGO.SP_ACTUALIZAR", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_PAGO_ID", OracleDbType.Int32).Value = entidad.PagoId
                    cmd.Parameters.Add("P_ORDEN_VENTA_ID", OracleDbType.Int32).Value = entidad.OrdenVentaId
                    cmd.Parameters.Add("P_METODO_PAGO_ID", OracleDbType.Int32).Value = entidad.MetodoPagoId
                    cmd.Parameters.Add("P_MONTO", OracleDbType.Decimal).Value = entidad.Monto

                    If String.IsNullOrWhiteSpace(entidad.EstadoPago) Then
                        cmd.Parameters.Add("P_ESTADO_PAGO", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("P_ESTADO_PAGO", OracleDbType.Varchar2).Value = entidad.EstadoPago
                    End If

                    If String.IsNullOrWhiteSpace(entidad.Referencia) Then
                        cmd.Parameters.Add("P_REFERENCIA", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("P_REFERENCIA", OracleDbType.Varchar2).Value = entidad.Referencia
                    End If

                    cmd.Parameters.Add("P_PAGO_AT", OracleDbType.TimeStamp).Value = entidad.PagoAt
                    cmd.Parameters.Add("P_ESTADO", OracleDbType.Varchar2).Value = entidad.Estado

                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Sub Eliminar(ByVal id As Integer)
            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_PAGO.SP_ELIMINAR", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_ID", OracleDbType.Int32).Value = id
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Function ObtenerPorId(ByVal id As Integer) As Pago
            Dim entidad As Pago = Nothing

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_PAGO.SP_OBTENER", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_ID", OracleDbType.Int32).Value = id
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        If dr.Read() Then
                            entidad = MapearPago(dr)
                        End If
                    End Using
                End Using
            End Using

            Return entidad
        End Function

        Public Function Listar() As List(Of Pago)
            Dim lista As New List(Of Pago)()

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_PAGO.SP_LISTAR", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        While dr.Read()
                            lista.Add(MapearPago(dr))
                        End While
                    End Using
                End Using
            End Using

            Return lista
        End Function

        Public Function Buscar(ByVal valor As String) As List(Of Pago)
            Dim lista As New List(Of Pago)()

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_PAGO.SP_BUSCAR", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_VALOR", OracleDbType.Varchar2).Value = valor
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        While dr.Read()
                            lista.Add(MapearPago(dr))
                        End While
                    End Using
                End Using
            End Using

            Return lista
        End Function

        Private Function MapearPago(ByVal dr As OracleDataReader) As Pago
            Dim entidad As New Pago()

            entidad.PagoId = Convert.ToInt32(dr("pago_id"))
            entidad.OrdenVentaId = Convert.ToInt32(dr("orden_venta_id"))
            entidad.MetodoPagoId = Convert.ToInt32(dr("metodo_pago_id"))
            entidad.Monto = Convert.ToDecimal(dr("monto"))
            entidad.EstadoPago = If(IsDBNull(dr("estado_pago")), String.Empty, dr("estado_pago").ToString())
            entidad.Referencia = If(IsDBNull(dr("referencia")), String.Empty, dr("referencia").ToString())
            entidad.PagoAt = Convert.ToDateTime(dr("pago_at"))
            entidad.CreatedAt = Convert.ToDateTime(dr("created_at"))

            If IsDBNull(dr("updated_at")) Then
                entidad.UpdatedAt = Nothing
            Else
                entidad.UpdatedAt = Convert.ToDateTime(dr("updated_at"))
            End If

            entidad.Estado = dr("estado").ToString()

            Return entidad
        End Function

    End Class
End Namespace