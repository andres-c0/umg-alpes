Option Strict On
Option Explicit On

Imports System.Collections.Generic
Imports System.Data
Imports Oracle.ManagedDataAccess.Client
Imports Alpes.Datos.Conexion
Imports Alpes.Entidades.Compras

Namespace Repositorios
    Public Class PagoProveedorDatos

        Private ReadOnly _conexionOracle As ConexionOracle

        Public Sub New()
            _conexionOracle = New ConexionOracle()
        End Sub

        Public Function Insertar(ByVal entidad As PagoProveedor) As Integer
            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using transaccion As OracleTransaction = conexion.BeginTransaction()
                    Using cmd As New OracleCommand("PKG_PAGO_PROVEEDOR.SP_INSERTAR_PAGO_PROVEEDOR", conexion)
                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Transaction = transaccion

                        cmd.Parameters.Add("P_CUENTA_PAGAR_ID", OracleDbType.Int32).Value = entidad.CuentaPagarId
                        cmd.Parameters.Add("P_MONTO", OracleDbType.Decimal).Value = entidad.Monto
                        cmd.Parameters.Add("P_FECHA_PAGO", OracleDbType.Date).Value = entidad.FechaPago

                        Dim pReferencia As New OracleParameter("P_REFERENCIA", OracleDbType.Varchar2)
                        If String.IsNullOrWhiteSpace(entidad.Referencia) Then
                            pReferencia.Value = DBNull.Value
                        Else
                            pReferencia.Value = entidad.Referencia
                        End If
                        cmd.Parameters.Add(pReferencia)

                        Dim pPagoProveedorId As New OracleParameter("P_PAGO_PROVEEDOR_ID", OracleDbType.Int32)
                        pPagoProveedorId.Direction = ParameterDirection.Output
                        cmd.Parameters.Add(pPagoProveedorId)

                        cmd.ExecuteNonQuery()
                        transaccion.Commit()

                        Return Convert.ToInt32(pPagoProveedorId.Value.ToString())
                    End Using
                End Using
            End Using
        End Function

        Public Function Actualizar(ByVal entidad As PagoProveedor) As Boolean
            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using transaccion As OracleTransaction = conexion.BeginTransaction()
                    Using cmd As New OracleCommand("PKG_PAGO_PROVEEDOR.SP_ACTUALIZAR_PAGO_PROVEEDOR", conexion)
                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Transaction = transaccion

                        cmd.Parameters.Add("P_PAGO_PROVEEDOR_ID", OracleDbType.Int32).Value = entidad.PagoProveedorId
                        cmd.Parameters.Add("P_CUENTA_PAGAR_ID", OracleDbType.Int32).Value = entidad.CuentaPagarId
                        cmd.Parameters.Add("P_MONTO", OracleDbType.Decimal).Value = entidad.Monto
                        cmd.Parameters.Add("P_FECHA_PAGO", OracleDbType.Date).Value = entidad.FechaPago

                        Dim pReferencia As New OracleParameter("P_REFERENCIA", OracleDbType.Varchar2)
                        If String.IsNullOrWhiteSpace(entidad.Referencia) Then
                            pReferencia.Value = DBNull.Value
                        Else
                            pReferencia.Value = entidad.Referencia
                        End If
                        cmd.Parameters.Add(pReferencia)

                        cmd.ExecuteNonQuery()
                        transaccion.Commit()

                        Return True
                    End Using
                End Using
            End Using
        End Function

        Public Function Eliminar(ByVal pagoProveedorId As Integer) As Boolean
            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using transaccion As OracleTransaction = conexion.BeginTransaction()
                    Using cmd As New OracleCommand("PKG_PAGO_PROVEEDOR.SP_ELIMINAR_PAGO_PROVEEDOR", conexion)
                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Transaction = transaccion

                        cmd.Parameters.Add("P_PAGO_PROVEEDOR_ID", OracleDbType.Int32).Value = pagoProveedorId

                        cmd.ExecuteNonQuery()
                        transaccion.Commit()

                        Return True
                    End Using
                End Using
            End Using
        End Function

        Public Function ObtenerPorId(ByVal pagoProveedorId As Integer) As PagoProveedor
            Dim entidad As PagoProveedor = Nothing

            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using cmd As New OracleCommand("PKG_PAGO_PROVEEDOR.SP_OBTENER_PAGO_PROVEEDOR", conexion)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_PAGO_PROVEEDOR_ID", OracleDbType.Int32).Value = pagoProveedorId
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        If dr.Read() Then
                            entidad = MapearObtener(dr)
                        End If
                    End Using
                End Using
            End Using

            Return entidad
        End Function

        Public Function Listar() As List(Of PagoProveedor)
            Dim lista As New List(Of PagoProveedor)()

            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using cmd As New OracleCommand("PKG_PAGO_PROVEEDOR.SP_LISTAR_PAGOS_PROVEEDOR", conexion)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        While dr.Read()
                            lista.Add(MapearListado(dr))
                        End While
                    End Using
                End Using
            End Using

            Return lista
        End Function

        Public Function Buscar(ByVal criterio As String, ByVal valor As String) As List(Of PagoProveedor)
            Dim lista As New List(Of PagoProveedor)()

            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using cmd As New OracleCommand("PKG_PAGO_PROVEEDOR.SP_BUSCAR_PAGOS_PROVEEDOR", conexion)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_CRITERIO", OracleDbType.Varchar2).Value = criterio
                    cmd.Parameters.Add("P_VALOR", OracleDbType.Varchar2).Value = valor
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        While dr.Read()
                            lista.Add(MapearListado(dr))
                        End While
                    End Using
                End Using
            End Using

            Return lista
        End Function

        Private Function MapearObtener(ByVal dr As OracleDataReader) As PagoProveedor
            Dim entidad As New PagoProveedor()

            entidad.PagoProveedorId = Convert.ToInt32(dr("PAGO_PROVEEDOR_ID"))
            entidad.CuentaPagarId = Convert.ToInt32(dr("CUENTA_PAGAR_ID"))
            entidad.Monto = Convert.ToDecimal(dr("MONTO"))
            entidad.FechaPago = Convert.ToDateTime(dr("FECHA_PAGO"))
            entidad.Referencia = If(dr("REFERENCIA") Is DBNull.Value, String.Empty, dr("REFERENCIA").ToString())
            entidad.Estado = If(dr("ESTADO") Is DBNull.Value, String.Empty, dr("ESTADO").ToString())

            Return entidad
        End Function

        Private Function MapearListado(ByVal dr As OracleDataReader) As PagoProveedor
            Dim entidad As New PagoProveedor()

            entidad.PagoProveedorId = Convert.ToInt32(dr("PAGO_PROVEEDOR_ID"))
            entidad.Monto = Convert.ToDecimal(dr("MONTO"))
            entidad.FechaPago = Convert.ToDateTime(dr("FECHA_PAGO"))
            entidad.Referencia = If(dr("REFERENCIA") Is DBNull.Value, String.Empty, dr("REFERENCIA").ToString())
            entidad.Estado = If(dr("ESTADO") Is DBNull.Value, String.Empty, dr("ESTADO").ToString())

            If TieneColumna(dr, "CUENTA_PAGAR_ID") AndAlso dr("CUENTA_PAGAR_ID") IsNot DBNull.Value Then
                entidad.CuentaPagarId = Convert.ToInt32(dr("CUENTA_PAGAR_ID"))
            End If

            Return entidad
        End Function

        Private Function TieneColumna(ByVal dr As OracleDataReader, ByVal nombreColumna As String) As Boolean
            For i As Integer = 0 To dr.FieldCount - 1
                If String.Equals(dr.GetName(i), nombreColumna, StringComparison.OrdinalIgnoreCase) Then
                    Return True
                End If
            Next

            Return False
        End Function

    End Class
End Namespace