Option Strict On
Option Explicit On

Imports System.Data
Imports Oracle.ManagedDataAccess.Client
Imports Alpes.Datos.Conexion
Imports Alpes.Entidades.Ventas

Namespace Repositorios
    Public Class Cuotas_PagoDatos

        Private ReadOnly _conexionOracle As ConexionOracle

        Public Sub New()
            _conexionOracle = New ConexionOracle()
        End Sub

        Public Function Insertar(ByVal entidad As Cuotas_Pago) As Integer
            Dim idGenerado As Integer = 0

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_CUOTAS_PAGO.SP_INSERTAR", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_PAGO_ID", OracleDbType.Int32).Value = entidad.PagoId
                    cmd.Parameters.Add("P_NUM_CUOTA", OracleDbType.Int32).Value = entidad.NumCuota
                    cmd.Parameters.Add("P_MONTO_CUOTA", OracleDbType.Decimal).Value = entidad.MontoCuota

                    If entidad.FechaVencimiento.HasValue Then
                        cmd.Parameters.Add("P_FECHA_VENCIMIENTO", OracleDbType.Date).Value = entidad.FechaVencimiento.Value
                    Else
                        cmd.Parameters.Add("P_FECHA_VENCIMIENTO", OracleDbType.Date).Value = DBNull.Value
                    End If

                    If entidad.FechaPago.HasValue Then
                        cmd.Parameters.Add("P_FECHA_PAGO", OracleDbType.Date).Value = entidad.FechaPago.Value
                    Else
                        cmd.Parameters.Add("P_FECHA_PAGO", OracleDbType.Date).Value = DBNull.Value
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

        Public Sub Actualizar(ByVal entidad As Cuotas_Pago)
            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_CUOTAS_PAGO.SP_ACTUALIZAR", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_CUOTA_ID", OracleDbType.Int32).Value = entidad.CuotaId
                    cmd.Parameters.Add("P_PAGO_ID", OracleDbType.Int32).Value = entidad.PagoId
                    cmd.Parameters.Add("P_NUM_CUOTA", OracleDbType.Int32).Value = entidad.NumCuota
                    cmd.Parameters.Add("P_MONTO_CUOTA", OracleDbType.Decimal).Value = entidad.MontoCuota

                    If entidad.FechaVencimiento.HasValue Then
                        cmd.Parameters.Add("P_FECHA_VENCIMIENTO", OracleDbType.Date).Value = entidad.FechaVencimiento.Value
                    Else
                        cmd.Parameters.Add("P_FECHA_VENCIMIENTO", OracleDbType.Date).Value = DBNull.Value
                    End If

                    If entidad.FechaPago.HasValue Then
                        cmd.Parameters.Add("P_FECHA_PAGO", OracleDbType.Date).Value = entidad.FechaPago.Value
                    Else
                        cmd.Parameters.Add("P_FECHA_PAGO", OracleDbType.Date).Value = DBNull.Value
                    End If

                    cmd.Parameters.Add("P_ESTADO", OracleDbType.Varchar2).Value = entidad.Estado

                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Sub Eliminar(ByVal id As Integer)
            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_CUOTAS_PAGO.SP_ELIMINAR", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_ID", OracleDbType.Int32).Value = id
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Function ObtenerPorId(ByVal id As Integer) As Cuotas_Pago
            Dim entidad As Cuotas_Pago = Nothing

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_CUOTAS_PAGO.SP_OBTENER", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_ID", OracleDbType.Int32).Value = id
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        If dr.Read() Then
                            entidad = MapearCuotasPago(dr)
                        End If
                    End Using
                End Using
            End Using

            Return entidad
        End Function

        Public Function Listar() As List(Of Cuotas_Pago)
            Dim lista As New List(Of Cuotas_Pago)()

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_CUOTAS_PAGO.SP_LISTAR", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        While dr.Read()
                            lista.Add(MapearCuotasPago(dr))
                        End While
                    End Using
                End Using
            End Using

            Return lista
        End Function

        Public Function Buscar(ByVal valor As Integer) As List(Of Cuotas_Pago)
            Dim lista As New List(Of Cuotas_Pago)()

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_CUOTAS_PAGO.SP_BUSCAR", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_VALOR", OracleDbType.Int32).Value = valor
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        While dr.Read()
                            lista.Add(MapearCuotasPago(dr))
                        End While
                    End Using
                End Using
            End Using

            Return lista
        End Function

        Private Function MapearCuotasPago(ByVal dr As OracleDataReader) As Cuotas_Pago
            Dim entidad As New Cuotas_Pago()

            entidad.CuotaId = Convert.ToInt32(dr("cuota_id"))
            entidad.PagoId = Convert.ToInt32(dr("pago_id"))
            entidad.NumCuota = Convert.ToInt32(dr("num_cuota"))
            entidad.MontoCuota = Convert.ToDecimal(dr("monto_cuota"))

            If IsDBNull(dr("fecha_vencimiento")) Then
                entidad.FechaVencimiento = Nothing
            Else
                entidad.FechaVencimiento = Convert.ToDateTime(dr("fecha_vencimiento"))
            End If

            If IsDBNull(dr("fecha_pago")) Then
                entidad.FechaPago = Nothing
            Else
                entidad.FechaPago = Convert.ToDateTime(dr("fecha_pago"))
            End If

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