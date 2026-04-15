Option Strict On
Option Explicit On

Imports System
Imports System.Collections.Generic
Imports System.Data
Imports Oracle.ManagedDataAccess.Client
Imports Alpes.Datos.Conexion
Imports Alpes.Entidades.Inventario

Namespace Repositorios
    Public Class ProductoDatos

        Private ReadOnly _conexionOracle As ConexionOracle

        Public Sub New()
            _conexionOracle = New ConexionOracle()
        End Sub

        Public Function Insertar(ByVal entidad As Producto) As Integer
            Dim idGenerado As Integer = 0

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_PRODUCTO.SP_INSERTAR_PRODUCTO", cn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.BindByName = True

                    If String.IsNullOrWhiteSpace(entidad.Referencia) Then
                        cmd.Parameters.Add("P_REFERENCIA", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("P_REFERENCIA", OracleDbType.Varchar2).Value = entidad.Referencia
                    End If

                    cmd.Parameters.Add("P_NOMBRE", OracleDbType.Varchar2).Value = entidad.Nombre

                    If String.IsNullOrWhiteSpace(entidad.Descripcion) Then
                        cmd.Parameters.Add("P_DESCRIPCION", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("P_DESCRIPCION", OracleDbType.Varchar2).Value = entidad.Descripcion
                    End If

                    cmd.Parameters.Add("P_TIPO", OracleDbType.Varchar2).Value = entidad.Tipo

                    If String.IsNullOrWhiteSpace(entidad.Material) Then
                        cmd.Parameters.Add("P_MATERIAL", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("P_MATERIAL", OracleDbType.Varchar2).Value = entidad.Material
                    End If

                    cmd.Parameters.Add("P_ALTO_CM", OracleDbType.Decimal).Value = entidad.AltoCm.Value
                    cmd.Parameters.Add("P_ANCHO_CM", OracleDbType.Decimal).Value = entidad.AnchoCm.Value
                    cmd.Parameters.Add("P_PROFUNDIDAD_CM", OracleDbType.Decimal).Value = entidad.ProfundidadCm.Value

                    If String.IsNullOrWhiteSpace(entidad.Color) Then
                        cmd.Parameters.Add("P_COLOR", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("P_COLOR", OracleDbType.Varchar2).Value = entidad.Color
                    End If

                    cmd.Parameters.Add("P_PESO_GRAMOS", OracleDbType.Decimal).Value = entidad.PesoGramos.Value

                    If String.IsNullOrWhiteSpace(entidad.ImagenUrl) Then
                        cmd.Parameters.Add("P_IMAGEN_URL", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("P_IMAGEN_URL", OracleDbType.Varchar2).Value = entidad.ImagenUrl
                    End If

                    If entidad.UnidadMedidaId.HasValue Then
                        cmd.Parameters.Add("P_UNIDAD_MEDIDA_ID", OracleDbType.Int32).Value = entidad.UnidadMedidaId.Value
                    Else
                        cmd.Parameters.Add("P_UNIDAD_MEDIDA_ID", OracleDbType.Int32).Value = DBNull.Value
                    End If

                    cmd.Parameters.Add("P_CATEGORIA_ID", OracleDbType.Int32).Value = entidad.CategoriaId

                    If String.IsNullOrWhiteSpace(entidad.LoteProducto) Then
                        cmd.Parameters.Add("P_LOTE_PRODUCTO", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("P_LOTE_PRODUCTO", OracleDbType.Varchar2).Value = entidad.LoteProducto
                    End If

                    cmd.Parameters.Add("P_PRODUCTO_ID", OracleDbType.Int32).Direction = ParameterDirection.Output

                    cmd.ExecuteNonQuery()
                    idGenerado = Convert.ToInt32(cmd.Parameters("P_PRODUCTO_ID").Value.ToString())
                End Using
            End Using

            Return idGenerado
        End Function

        Public Sub Actualizar(ByVal entidad As Producto)
            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_PRODUCTO.SP_ACTUALIZAR_PRODUCTO", cn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.BindByName = True

                    cmd.Parameters.Add("P_PRODUCTO_ID", OracleDbType.Int32).Value = entidad.ProductoId

                    If String.IsNullOrWhiteSpace(entidad.Referencia) Then
                        cmd.Parameters.Add("P_REFERENCIA", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("P_REFERENCIA", OracleDbType.Varchar2).Value = entidad.Referencia
                    End If

                    cmd.Parameters.Add("P_NOMBRE", OracleDbType.Varchar2).Value = entidad.Nombre

                    If String.IsNullOrWhiteSpace(entidad.Descripcion) Then
                        cmd.Parameters.Add("P_DESCRIPCION", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("P_DESCRIPCION", OracleDbType.Varchar2).Value = entidad.Descripcion
                    End If

                    cmd.Parameters.Add("P_TIPO", OracleDbType.Varchar2).Value = entidad.Tipo

                    If String.IsNullOrWhiteSpace(entidad.Material) Then
                        cmd.Parameters.Add("P_MATERIAL", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("P_MATERIAL", OracleDbType.Varchar2).Value = entidad.Material
                    End If

                    cmd.Parameters.Add("P_ALTO_CM", OracleDbType.Decimal).Value = entidad.AltoCm.Value
                    cmd.Parameters.Add("P_ANCHO_CM", OracleDbType.Decimal).Value = entidad.AnchoCm.Value
                    cmd.Parameters.Add("P_PROFUNDIDAD_CM", OracleDbType.Decimal).Value = entidad.ProfundidadCm.Value

                    If String.IsNullOrWhiteSpace(entidad.Color) Then
                        cmd.Parameters.Add("P_COLOR", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("P_COLOR", OracleDbType.Varchar2).Value = entidad.Color
                    End If

                    cmd.Parameters.Add("P_PESO_GRAMOS", OracleDbType.Decimal).Value = entidad.PesoGramos.Value

                    If String.IsNullOrWhiteSpace(entidad.ImagenUrl) Then
                        cmd.Parameters.Add("P_IMAGEN_URL", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("P_IMAGEN_URL", OracleDbType.Varchar2).Value = entidad.ImagenUrl
                    End If

                    If entidad.UnidadMedidaId.HasValue Then
                        cmd.Parameters.Add("P_UNIDAD_MEDIDA_ID", OracleDbType.Int32).Value = entidad.UnidadMedidaId.Value
                    Else
                        cmd.Parameters.Add("P_UNIDAD_MEDIDA_ID", OracleDbType.Int32).Value = DBNull.Value
                    End If

                    cmd.Parameters.Add("P_CATEGORIA_ID", OracleDbType.Int32).Value = entidad.CategoriaId

                    If String.IsNullOrWhiteSpace(entidad.LoteProducto) Then
                        cmd.Parameters.Add("P_LOTE_PRODUCTO", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("P_LOTE_PRODUCTO", OracleDbType.Varchar2).Value = entidad.LoteProducto
                    End If

                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Sub Eliminar(ByVal id As Integer)
            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_PRODUCTO.SP_ELIMINAR_PRODUCTO", cn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.BindByName = True

                    cmd.Parameters.Add("P_PRODUCTO_ID", OracleDbType.Int32).Value = id
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Function ObtenerPorId(ByVal id As Integer) As Producto
            Dim entidad As Producto = Nothing

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_PRODUCTO.SP_OBTENER_PRODUCTO", cn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.BindByName = True

                    cmd.Parameters.Add("P_PRODUCTO_ID", OracleDbType.Int32).Value = id
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        If dr.Read() Then
                            entidad = MapearProducto(dr)
                        End If
                    End Using
                End Using
            End Using

            Return entidad
        End Function

        Public Function Listar() As List(Of Producto)
            Dim lista As New List(Of Producto)()

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_PRODUCTO.SP_LISTAR_PRODUCTOS", cn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.BindByName = True

                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        While dr.Read()
                            lista.Add(MapearProducto(dr))
                        End While
                    End Using
                End Using
            End Using

            Return lista
        End Function

        Public Function Buscar(ByVal criterio As String, ByVal valor As String) As List(Of Producto)
            Dim lista As New List(Of Producto)()

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_PRODUCTO.SP_BUSCAR_PRODUCTOS", cn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.BindByName = True

                    cmd.Parameters.Add("P_CRITERIO", OracleDbType.Varchar2).Value = criterio
                    cmd.Parameters.Add("P_VALOR", OracleDbType.Varchar2).Value = valor
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        While dr.Read()
                            lista.Add(MapearProducto(dr))
                        End While
                    End Using
                End Using
            End Using

            Return lista
        End Function

        Private Function MapearProducto(ByVal dr As OracleDataReader) As Producto
            Dim entidad As New Producto()

            If TieneColumna(dr, "PRODUCTO_ID") AndAlso Not IsDBNull(dr("PRODUCTO_ID")) Then
                entidad.ProductoId = Convert.ToInt32(dr("PRODUCTO_ID"))
            End If

            If TieneColumna(dr, "REFERENCIA") AndAlso Not IsDBNull(dr("REFERENCIA")) Then
                entidad.Referencia = dr("REFERENCIA").ToString()
            Else
                entidad.Referencia = String.Empty
            End If

            If TieneColumna(dr, "NOMBRE") AndAlso Not IsDBNull(dr("NOMBRE")) Then
                entidad.Nombre = dr("NOMBRE").ToString()
            Else
                entidad.Nombre = String.Empty
            End If

            If TieneColumna(dr, "DESCRIPCION") AndAlso Not IsDBNull(dr("DESCRIPCION")) Then
                entidad.Descripcion = dr("DESCRIPCION").ToString()
            Else
                entidad.Descripcion = String.Empty
            End If

            If TieneColumna(dr, "TIPO") AndAlso Not IsDBNull(dr("TIPO")) Then
                entidad.Tipo = dr("TIPO").ToString()
            Else
                entidad.Tipo = String.Empty
            End If

            If TieneColumna(dr, "MATERIAL") AndAlso Not IsDBNull(dr("MATERIAL")) Then
                entidad.Material = dr("MATERIAL").ToString()
            Else
                entidad.Material = String.Empty
            End If

            If TieneColumna(dr, "ALTO_CM") AndAlso Not IsDBNull(dr("ALTO_CM")) Then
                entidad.AltoCm = Convert.ToDecimal(dr("ALTO_CM"))
            Else
                entidad.AltoCm = Nothing
            End If

            If TieneColumna(dr, "ANCHO_CM") AndAlso Not IsDBNull(dr("ANCHO_CM")) Then
                entidad.AnchoCm = Convert.ToDecimal(dr("ANCHO_CM"))
            Else
                entidad.AnchoCm = Nothing
            End If

            If TieneColumna(dr, "PROFUNDIDAD_CM") AndAlso Not IsDBNull(dr("PROFUNDIDAD_CM")) Then
                entidad.ProfundidadCm = Convert.ToDecimal(dr("PROFUNDIDAD_CM"))
            Else
                entidad.ProfundidadCm = Nothing
            End If

            If TieneColumna(dr, "COLOR") AndAlso Not IsDBNull(dr("COLOR")) Then
                entidad.Color = dr("COLOR").ToString()
            Else
                entidad.Color = String.Empty
            End If

            If TieneColumna(dr, "PESO_GRAMOS") AndAlso Not IsDBNull(dr("PESO_GRAMOS")) Then
                entidad.PesoGramos = Convert.ToDecimal(dr("PESO_GRAMOS"))
            Else
                entidad.PesoGramos = Nothing
            End If

            If TieneColumna(dr, "IMAGEN_URL") AndAlso Not IsDBNull(dr("IMAGEN_URL")) Then
                entidad.ImagenUrl = dr("IMAGEN_URL").ToString()
            Else
                entidad.ImagenUrl = String.Empty
            End If

            If TieneColumna(dr, "UNIDAD_MEDIDA_ID") AndAlso Not IsDBNull(dr("UNIDAD_MEDIDA_ID")) Then
                entidad.UnidadMedidaId = Convert.ToInt32(dr("UNIDAD_MEDIDA_ID"))
            Else
                entidad.UnidadMedidaId = Nothing
            End If

            If TieneColumna(dr, "CATEGORIA_ID") AndAlso Not IsDBNull(dr("CATEGORIA_ID")) Then
                entidad.CategoriaId = Convert.ToInt32(dr("CATEGORIA_ID"))
            End If

            If TieneColumna(dr, "LOTE_PRODUCTO") AndAlso Not IsDBNull(dr("LOTE_PRODUCTO")) Then
                entidad.LoteProducto = dr("LOTE_PRODUCTO").ToString()
            Else
                entidad.LoteProducto = String.Empty
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