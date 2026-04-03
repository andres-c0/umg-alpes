Option Strict On
Option Explicit On

Imports System.Data
Imports Oracle.ManagedDataAccess.Client
Imports Alpes.Datos.Conexion
Imports Alpes.Entidades.Ventas

Namespace Repositorios
    Public Class FacturaDatos

        Private ReadOnly _conexionOracle As ConexionOracle

        Public Sub New()
            _conexionOracle = New ConexionOracle()
        End Sub

        Public Function Insertar(ByVal entidad As Factura) As Integer
            Using conn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_FACTURA.SP_INSERTAR_FACTURA", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_ORDEN_VENTA_ID", entidad.orden_venta_id)
                    cmd.Parameters.Add("P_NUM_FACTURA", entidad.num_factura)
                    cmd.Parameters.Add("P_FECHA_EMISION", entidad.fecha_emision)
                    cmd.Parameters.Add("P_NIT_FACTURACION", entidad.nit_facturacion)
                    cmd.Parameters.Add("P_DIRECCION_FACTURACION_SNAPSHOT", entidad.direccion_facturacion_snapshot)
                    cmd.Parameters.Add("P_TOTAL_FACTURA_SNAPSHOT", entidad.total_factura_snapshot)

                    cmd.Parameters.Add("P_FACTURA_ID", OracleDbType.Int32).Direction = ParameterDirection.Output

                    Try
                        conn.Open()
                        cmd.ExecuteNonQuery()
                        Return Convert.ToInt32(cmd.Parameters("P_FACTURA_ID").Value)
                    Catch ex As OracleException
                        Throw New Exception(ex.Message)
                    End Try
                End Using
            End Using
        End Function

        Public Sub Actualizar(ByVal entidad As Factura)
            Using conn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_FACTURA.SP_ACTUALIZAR_FACTURA", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_FACTURA_ID", entidad.factura_id)
                    cmd.Parameters.Add("P_ORDEN_VENTA_ID", entidad.orden_venta_id)
                    cmd.Parameters.Add("P_NUM_FACTURA", entidad.num_factura)
                    cmd.Parameters.Add("P_FECHA_EMISION", entidad.fecha_emision)
                    cmd.Parameters.Add("P_NIT_FACTURACION", entidad.nit_facturacion)
                    cmd.Parameters.Add("P_DIRECCION_FACTURACION_SNAPSHOT", entidad.direccion_facturacion_snapshot)
                    cmd.Parameters.Add("P_TOTAL_FACTURA_SNAPSHOT", entidad.total_factura_snapshot)

                    Try
                        conn.Open()
                        cmd.ExecuteNonQuery()
                    Catch ex As OracleException
                        Throw New Exception(ex.Message)
                    End Try
                End Using
            End Using
        End Sub

        Public Sub Eliminar(ByVal id As Integer)
            Using conn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_FACTURA.SP_ELIMINAR_FACTURA", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_FACTURA_ID", id)

                    Try
                        conn.Open()
                        cmd.ExecuteNonQuery()
                    Catch ex As OracleException
                        Throw New Exception(ex.Message)
                    End Try
                End Using
            End Using
        End Sub

        Public Function ObtenerPorId(ByVal id As Integer) As Factura
            Using conn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_FACTURA.SP_OBTENER_FACTURA", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_FACTURA_ID", id)
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    conn.Open()

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        If dr.Read() Then
                            Return Mapear(dr)
                        End If
                    End Using
                End Using
            End Using

            Return Nothing
        End Function

        Public Function Listar() As List(Of Factura)
            Dim lista As New List(Of Factura)()

            Using conn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_FACTURA.SP_LISTAR_FACTURAS", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    conn.Open()

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        While dr.Read()
                            lista.Add(Mapear(dr))
                        End While
                    End Using
                End Using
            End Using

            Return lista
        End Function

        Public Function Buscar(ByVal criterio As String, ByVal valor As String) As List(Of Factura)
            Dim lista As New List(Of Factura)()

            Using conn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_FACTURA.SP_BUSCAR_FACTURAS", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_CRITERIO", criterio)
                    cmd.Parameters.Add("P_VALOR", valor)
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    conn.Open()

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        While dr.Read()
                            lista.Add(Mapear(dr))
                        End While
                    End Using
                End Using
            End Using

            Return lista
        End Function

        Private Function Mapear(ByVal dr As OracleDataReader) As Factura
            Dim entidad As New Factura()

            If TieneColumna(dr, "FACTURA_ID") AndAlso Not IsDBNull(dr("FACTURA_ID")) Then
                entidad.factura_id = Convert.ToInt32(dr("FACTURA_ID"))
            End If

            If TieneColumna(dr, "ORDEN_VENTA_ID") AndAlso Not IsDBNull(dr("ORDEN_VENTA_ID")) Then
                entidad.orden_venta_id = Convert.ToInt32(dr("ORDEN_VENTA_ID"))
            End If

            If TieneColumna(dr, "NUM_FACTURA") AndAlso Not IsDBNull(dr("NUM_FACTURA")) Then
                entidad.num_factura = dr("NUM_FACTURA").ToString()
            End If

            If TieneColumna(dr, "FECHA_EMISION") AndAlso Not IsDBNull(dr("FECHA_EMISION")) Then
                entidad.fecha_emision = Convert.ToDateTime(dr("FECHA_EMISION"))
            End If

            If TieneColumna(dr, "NIT_FACTURACION") AndAlso Not IsDBNull(dr("NIT_FACTURACION")) Then
                entidad.nit_facturacion = dr("NIT_FACTURACION").ToString()
            End If

            If TieneColumna(dr, "DIRECCION_FACTURACION_SNAPSHOT") AndAlso Not IsDBNull(dr("DIRECCION_FACTURACION_SNAPSHOT")) Then
                entidad.direccion_facturacion_snapshot = dr("DIRECCION_FACTURACION_SNAPSHOT").ToString()
            End If

            If TieneColumna(dr, "TOTAL_FACTURA_SNAPSHOT") AndAlso Not IsDBNull(dr("TOTAL_FACTURA_SNAPSHOT")) Then
                entidad.total_factura_snapshot = Convert.ToDecimal(dr("TOTAL_FACTURA_SNAPSHOT"))
            End If

            If TieneColumna(dr, "CREATED_AT") AndAlso Not IsDBNull(dr("CREATED_AT")) Then
                entidad.created_at = Convert.ToDateTime(dr("CREATED_AT"))
            End If

            If TieneColumna(dr, "UPDATED_AT") AndAlso Not IsDBNull(dr("UPDATED_AT")) Then
                entidad.updated_at = Convert.ToDateTime(dr("UPDATED_AT"))
            End If

            If TieneColumna(dr, "ESTADO") AndAlso Not IsDBNull(dr("ESTADO")) Then
                entidad.estado = dr("ESTADO").ToString()
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