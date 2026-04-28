Option Strict On
Option Explicit On

Imports System.Collections.Generic
Imports System.Data
Imports Oracle.ManagedDataAccess.Client
Imports Alpes.Datos.Conexion
Imports Alpes.Entidades.Compras

Namespace Repositorios
    Public Class CuentaPagarProveedorDatos

        Private ReadOnly _conexionOracle As ConexionOracle

        Public Sub New()
            _conexionOracle = New ConexionOracle()
        End Sub

        Public Function Insertar(ByVal entidad As CuentaPagarProveedor) As Integer
            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using transaccion As OracleTransaction = conexion.BeginTransaction()
                    Using cmd As New OracleCommand("PKG_CUENTA_PAGAR_PROVEEDOR.SP_INSERTAR_CUENTA_PAGAR_PROVEEDOR", conexion)
                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Transaction = transaccion

                        cmd.Parameters.Add("P_PROV_ID", OracleDbType.Int32).Value = entidad.ProvId
                        cmd.Parameters.Add("P_ORDEN_COMPRA_ID", OracleDbType.Int32).Value = entidad.OrdenCompraId
                        cmd.Parameters.Add("P_SALDO", OracleDbType.Decimal).Value = entidad.Saldo

                        Dim pFechaVencimiento As New OracleParameter("P_FECHA_VENCIMIENTO", OracleDbType.Date)
                        If entidad.FechaVencimiento.HasValue Then
                            pFechaVencimiento.Value = entidad.FechaVencimiento.Value
                        Else
                            pFechaVencimiento.Value = DBNull.Value
                        End If
                        cmd.Parameters.Add(pFechaVencimiento)

                        Dim pEstadoCp As New OracleParameter("P_ESTADO_CP", OracleDbType.Varchar2)
                        If String.IsNullOrWhiteSpace(entidad.EstadoCp) Then
                            pEstadoCp.Value = DBNull.Value
                        Else
                            pEstadoCp.Value = entidad.EstadoCp
                        End If
                        cmd.Parameters.Add(pEstadoCp)

                        Dim pId As New OracleParameter("P_CUENTA_PAGAR_ID", OracleDbType.Int32)
                        pId.Direction = ParameterDirection.Output
                        cmd.Parameters.Add(pId)

                        cmd.ExecuteNonQuery()
                        transaccion.Commit()

                        Return Convert.ToInt32(pId.Value.ToString())
                    End Using
                End Using
            End Using
        End Function

        Public Function Actualizar(ByVal entidad As CuentaPagarProveedor) As Boolean
            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using transaccion As OracleTransaction = conexion.BeginTransaction()
                    Using cmd As New OracleCommand("PKG_CUENTA_PAGAR_PROVEEDOR.SP_ACTUALIZAR_CUENTA_PAGAR_PROVEEDOR", conexion)
                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Transaction = transaccion

                        cmd.Parameters.Add("P_CUENTA_PAGAR_ID", OracleDbType.Int32).Value = entidad.CuentaPagarId
                        cmd.Parameters.Add("P_PROV_ID", OracleDbType.Int32).Value = entidad.ProvId
                        cmd.Parameters.Add("P_ORDEN_COMPRA_ID", OracleDbType.Int32).Value = entidad.OrdenCompraId
                        cmd.Parameters.Add("P_SALDO", OracleDbType.Decimal).Value = entidad.Saldo

                        Dim pFechaVencimiento As New OracleParameter("P_FECHA_VENCIMIENTO", OracleDbType.Date)
                        If entidad.FechaVencimiento.HasValue Then
                            pFechaVencimiento.Value = entidad.FechaVencimiento.Value
                        Else
                            pFechaVencimiento.Value = DBNull.Value
                        End If
                        cmd.Parameters.Add(pFechaVencimiento)

                        Dim pEstadoCp As New OracleParameter("P_ESTADO_CP", OracleDbType.Varchar2)
                        If String.IsNullOrWhiteSpace(entidad.EstadoCp) Then
                            pEstadoCp.Value = DBNull.Value
                        Else
                            pEstadoCp.Value = entidad.EstadoCp
                        End If
                        cmd.Parameters.Add(pEstadoCp)

                        cmd.ExecuteNonQuery()
                        transaccion.Commit()

                        Return True
                    End Using
                End Using
            End Using
        End Function

        Public Function Eliminar(ByVal cuentaPagarId As Integer) As Boolean
            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using transaccion As OracleTransaction = conexion.BeginTransaction()
                    Using cmd As New OracleCommand("PKG_CUENTA_PAGAR_PROVEEDOR.SP_ELIMINAR_CUENTA_PAGAR_PROVEEDOR", conexion)
                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Transaction = transaccion

                        cmd.Parameters.Add("P_CUENTA_PAGAR_ID", OracleDbType.Int32).Value = cuentaPagarId

                        cmd.ExecuteNonQuery()
                        transaccion.Commit()

                        Return True
                    End Using
                End Using
            End Using
        End Function

        Public Function ObtenerPorId(ByVal cuentaPagarId As Integer) As CuentaPagarProveedor
            Dim entidad As CuentaPagarProveedor = Nothing

            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using cmd As New OracleCommand("PKG_CUENTA_PAGAR_PROVEEDOR.SP_OBTENER_CUENTA_PAGAR_PROVEEDOR", conexion)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_CUENTA_PAGAR_ID", OracleDbType.Int32).Value = cuentaPagarId
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

        Public Function Listar() As List(Of CuentaPagarProveedor)
            Dim lista As New List(Of CuentaPagarProveedor)()

            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using cmd As New OracleCommand("PKG_CUENTA_PAGAR_PROVEEDOR.SP_LISTAR_CUENTAS_PAGAR_PROVEEDOR", conexion)
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

        Public Function Buscar(ByVal criterio As String, ByVal valor As String) As List(Of CuentaPagarProveedor)
            Dim lista As New List(Of CuentaPagarProveedor)()

            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using cmd As New OracleCommand("PKG_CUENTA_PAGAR_PROVEEDOR.SP_BUSCAR_CUENTAS_PAGAR_PROVEEDOR", conexion)
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

        Private Function MapearObtener(ByVal dr As OracleDataReader) As CuentaPagarProveedor
            Dim entidad As New CuentaPagarProveedor()

            entidad.CuentaPagarId = Convert.ToInt32(dr("CUENTA_PAGAR_ID"))
            entidad.ProvId = Convert.ToInt32(dr("PROV_ID"))
            entidad.RazonSocial = If(dr("RAZON_SOCIAL") Is DBNull.Value, String.Empty, dr("RAZON_SOCIAL").ToString())
            entidad.OrdenCompraId = Convert.ToInt32(dr("ORDEN_COMPRA_ID"))
            entidad.NumOc = If(dr("NUM_OC") Is DBNull.Value, String.Empty, dr("NUM_OC").ToString())
            entidad.Saldo = Convert.ToDecimal(dr("SALDO"))

            If dr("FECHA_VENCIMIENTO") Is DBNull.Value Then
                entidad.FechaVencimiento = Nothing
            Else
                entidad.FechaVencimiento = Convert.ToDateTime(dr("FECHA_VENCIMIENTO"))
            End If

            entidad.EstadoCp = If(dr("ESTADO_CP") Is DBNull.Value, String.Empty, dr("ESTADO_CP").ToString())
            entidad.Estado = If(dr("ESTADO") Is DBNull.Value, String.Empty, dr("ESTADO").ToString())

            Return entidad
        End Function

        Private Function MapearListado(ByVal dr As OracleDataReader) As CuentaPagarProveedor
            Dim entidad As New CuentaPagarProveedor()

            entidad.CuentaPagarId = Convert.ToInt32(dr("CUENTA_PAGAR_ID"))
            entidad.RazonSocial = If(dr("RAZON_SOCIAL") Is DBNull.Value, String.Empty, dr("RAZON_SOCIAL").ToString())
            entidad.NumOc = If(dr("NUM_OC") Is DBNull.Value, String.Empty, dr("NUM_OC").ToString())
            entidad.Saldo = Convert.ToDecimal(dr("SALDO"))

            If dr("FECHA_VENCIMIENTO") Is DBNull.Value Then
                entidad.FechaVencimiento = Nothing
            Else
                entidad.FechaVencimiento = Convert.ToDateTime(dr("FECHA_VENCIMIENTO"))
            End If

            entidad.EstadoCp = If(dr("ESTADO_CP") Is DBNull.Value, String.Empty, dr("ESTADO_CP").ToString())
            entidad.Estado = If(dr("ESTADO") Is DBNull.Value, String.Empty, dr("ESTADO").ToString())

            Return entidad
        End Function

    End Class
End Namespace