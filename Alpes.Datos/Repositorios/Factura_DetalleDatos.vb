Option Strict On
Option Explicit On

Imports System.Data
Imports Oracle.ManagedDataAccess.Client
Imports Alpes.Datos.Conexion
Imports Alpes.Entidades.Ventas

Namespace Repositorios
    Public Class Factura_DetalleDatos

        Private ReadOnly _conexionOracle As ConexionOracle

        Public Sub New()
            _conexionOracle = New ConexionOracle()
        End Sub

        Public Function Insertar(ByVal entidad As Factura_Detalle) As Integer
            Dim idGenerado As Integer = 0

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_FACTURA_DETALLE.SP_INSERTAR_FACTURA_DETALLE", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_FACTURA_ID", OracleDbType.Int32).Value = entidad.FacturaId
                    cmd.Parameters.Add("P_PRODUCTO_ID", OracleDbType.Int32).Value = entidad.ProductoId
                    cmd.Parameters.Add("P_CANTIDAD", OracleDbType.Int32).Value = entidad.Cantidad
                    cmd.Parameters.Add("P_PRECIO_UNITARIO_SNAPSHOT", OracleDbType.Decimal).Value = entidad.PrecioUnitarioSnapshot
                    cmd.Parameters.Add("P_TOTAL_LINEA", OracleDbType.Decimal).Value = entidad.TotalLinea
                    cmd.Parameters.Add("P_FACTURA_DET_ID", OracleDbType.Int32).Direction = ParameterDirection.Output

                    cmd.ExecuteNonQuery()
                    idGenerado = Convert.ToInt32(cmd.Parameters("P_FACTURA_DET_ID").Value.ToString())
                End Using
            End Using

            Return idGenerado
        End Function

        Public Sub Actualizar(ByVal entidad As Factura_Detalle)
            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_FACTURA_DETALLE.SP_ACTUALIZAR_FACTURA_DETALLE", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_FACTURA_DET_ID", OracleDbType.Int32).Value = entidad.FacturaDetId
                    cmd.Parameters.Add("P_FACTURA_ID", OracleDbType.Int32).Value = entidad.FacturaId
                    cmd.Parameters.Add("P_PRODUCTO_ID", OracleDbType.Int32).Value = entidad.ProductoId
                    cmd.Parameters.Add("P_CANTIDAD", OracleDbType.Int32).Value = entidad.Cantidad
                    cmd.Parameters.Add("P_PRECIO_UNITARIO_SNAPSHOT", OracleDbType.Decimal).Value = entidad.PrecioUnitarioSnapshot
                    cmd.Parameters.Add("P_TOTAL_LINEA", OracleDbType.Decimal).Value = entidad.TotalLinea

                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Sub Eliminar(ByVal id As Integer)
            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_FACTURA_DETALLE.SP_ELIMINAR_FACTURA_DETALLE", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_FACTURA_DET_ID", OracleDbType.Int32).Value = id
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Function ObtenerPorId(ByVal id As Integer) As Factura_Detalle
            Dim entidad As Factura_Detalle = Nothing

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_FACTURA_DETALLE.SP_OBTENER_FACTURA_DETALLE", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_FACTURA_DET_ID", OracleDbType.Int32).Value = id
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        If dr.Read() Then
                            entidad = MapearFacturaDetalle(dr)
                        End If
                    End Using
                End Using
            End Using

            Return entidad
        End Function

        Public Function Listar() As List(Of Factura_Detalle)
            Dim lista As New List(Of Factura_Detalle)()

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_FACTURA_DETALLE.SP_LISTAR_FACTURA_DETALLE", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        While dr.Read()
                            lista.Add(MapearFacturaDetalle(dr))
                        End While
                    End Using
                End Using
            End Using

            Return lista
        End Function

        Public Function Buscar(ByVal criterio As String, ByVal valor As String) As List(Of Factura_Detalle)
            Dim lista As New List(Of Factura_Detalle)()

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_FACTURA_DETALLE.SP_BUSCAR_FACTURA_DETALLE", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_CRITERIO", OracleDbType.Varchar2).Value = criterio
                    cmd.Parameters.Add("P_VALOR", OracleDbType.Varchar2).Value = valor
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        While dr.Read()
                            lista.Add(MapearFacturaDetalle(dr))
                        End While
                    End Using
                End Using
            End Using

            Return lista
        End Function

        Private Function MapearFacturaDetalle(ByVal dr As OracleDataReader) As Factura_Detalle
            Dim entidad As New Factura_Detalle()

            entidad.FacturaDetId = Convert.ToInt32(dr("FACTURA_DET_ID"))
            entidad.FacturaId = Convert.ToInt32(dr("FACTURA_ID"))
            entidad.ProductoId = Convert.ToInt32(dr("PRODUCTO_ID"))
            entidad.Cantidad = Convert.ToInt32(dr("CANTIDAD"))
            entidad.TotalLinea = Convert.ToDecimal(dr("TOTAL_LINEA"))
            entidad.Estado = dr("ESTADO").ToString()

            If TieneColumna(dr, "PRECIO_UNITARIO_SNAPSHOT") AndAlso Not IsDBNull(dr("PRECIO_UNITARIO_SNAPSHOT")) Then
                entidad.PrecioUnitarioSnapshot = Convert.ToDecimal(dr("PRECIO_UNITARIO_SNAPSHOT"))
            Else
                entidad.PrecioUnitarioSnapshot = 0D
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

            Return entidad
        End Function

        Private Function TieneColumna(ByVal dr As OracleDataReader, ByVal nombreColumna As String) As Boolean
            For i As Integer = 0 To dr.FieldCount - 1
                If dr.GetName(i).Equals(nombreColumna, StringComparison.OrdinalIgnoreCase) Then
                    Return True
                End If
            Next
            Return False
        End Function

    End Class
End Namespace