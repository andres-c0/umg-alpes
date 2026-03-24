Imports Oracle.ManagedDataAccess.Client
Imports System.Data
Imports alpes.Entidades

Namespace Datos

    Public Class ProductoDatos

        Private ReadOnly _conexion As alpes.Datos.ConexionOracle

        Public Sub New()
            _conexion = New alpes.Datos.ConexionOracle()
        End Sub

        Public Function Insertar(producto As Producto) As Integer
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_PRODUCTO.SP_INSERTAR_PRODUCTO", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_REFERENCIA",       OracleDbType.Varchar2).Value = If(String.IsNullOrWhiteSpace(producto.Referencia), DBNull.Value, producto.Referencia)
                    cmd.Parameters.Add("P_NOMBRE",           OracleDbType.Varchar2).Value = producto.Nombre
                    cmd.Parameters.Add("P_DESCRIPCION",      OracleDbType.Varchar2).Value = If(String.IsNullOrWhiteSpace(producto.Descripcion), DBNull.Value, producto.Descripcion)
                    cmd.Parameters.Add("P_TIPO",             OracleDbType.Varchar2).Value = producto.Tipo
                    cmd.Parameters.Add("P_MATERIAL",         OracleDbType.Varchar2).Value = If(String.IsNullOrWhiteSpace(producto.Material), DBNull.Value, producto.Material)
                    cmd.Parameters.Add("P_ALTO_CM",          OracleDbType.Decimal).Value  = producto.AltoCm
                    cmd.Parameters.Add("P_ANCHO_CM",         OracleDbType.Decimal).Value  = producto.AnchoCm
                    cmd.Parameters.Add("P_PROFUNDIDAD_CM",   OracleDbType.Decimal).Value  = producto.ProfundidadCm
                    cmd.Parameters.Add("P_COLOR",            OracleDbType.Varchar2).Value = If(String.IsNullOrWhiteSpace(producto.Color), DBNull.Value, producto.Color)
                    cmd.Parameters.Add("P_PESO_GRAMOS",      OracleDbType.Int32).Value    = producto.PesoGramos
                    cmd.Parameters.Add("P_IMAGEN_URL",       OracleDbType.Varchar2).Value = If(String.IsNullOrWhiteSpace(producto.ImagenUrl), DBNull.Value, producto.ImagenUrl)
                    cmd.Parameters.Add("P_UNIDAD_MEDIDA_ID", OracleDbType.Int32).Value    = If(producto.UnidadMedidaId.HasValue, producto.UnidadMedidaId.Value, DBNull.Value)
                    cmd.Parameters.Add("P_CATEGORIA_ID",     OracleDbType.Int32).Value    = producto.CategoriaId
                    cmd.Parameters.Add("P_LOTE_PRODUCTO",    OracleDbType.Varchar2).Value = If(String.IsNullOrWhiteSpace(producto.LoteProducto), DBNull.Value, producto.LoteProducto)

                    Dim pOut As New OracleParameter("P_PRODUCTO_ID", OracleDbType.Int32)
                    pOut.Direction = ParameterDirection.Output
                    cmd.Parameters.Add(pOut)

                    conn.Open()
                    cmd.ExecuteNonQuery()
                    Return Convert.ToInt32(pOut.Value.ToString())
                End Using
            End Using
        End Function

        Public Sub Actualizar(producto As Producto)
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_PRODUCTO.SP_ACTUALIZAR_PRODUCTO", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_PRODUCTO_ID",      OracleDbType.Int32).Value    = producto.ProductoId
                    cmd.Parameters.Add("P_REFERENCIA",       OracleDbType.Varchar2).Value = If(String.IsNullOrWhiteSpace(producto.Referencia), DBNull.Value, producto.Referencia)
                    cmd.Parameters.Add("P_NOMBRE",           OracleDbType.Varchar2).Value = producto.Nombre
                    cmd.Parameters.Add("P_DESCRIPCION",      OracleDbType.Varchar2).Value = If(String.IsNullOrWhiteSpace(producto.Descripcion), DBNull.Value, producto.Descripcion)
                    cmd.Parameters.Add("P_TIPO",             OracleDbType.Varchar2).Value = producto.Tipo
                    cmd.Parameters.Add("P_MATERIAL",         OracleDbType.Varchar2).Value = If(String.IsNullOrWhiteSpace(producto.Material), DBNull.Value, producto.Material)
                    cmd.Parameters.Add("P_ALTO_CM",          OracleDbType.Decimal).Value  = producto.AltoCm
                    cmd.Parameters.Add("P_ANCHO_CM",         OracleDbType.Decimal).Value  = producto.AnchoCm
                    cmd.Parameters.Add("P_PROFUNDIDAD_CM",   OracleDbType.Decimal).Value  = producto.ProfundidadCm
                    cmd.Parameters.Add("P_COLOR",            OracleDbType.Varchar2).Value = If(String.IsNullOrWhiteSpace(producto.Color), DBNull.Value, producto.Color)
                    cmd.Parameters.Add("P_PESO_GRAMOS",      OracleDbType.Int32).Value    = producto.PesoGramos
                    cmd.Parameters.Add("P_IMAGEN_URL",       OracleDbType.Varchar2).Value = If(String.IsNullOrWhiteSpace(producto.ImagenUrl), DBNull.Value, producto.ImagenUrl)
                    cmd.Parameters.Add("P_UNIDAD_MEDIDA_ID", OracleDbType.Int32).Value    = If(producto.UnidadMedidaId.HasValue, producto.UnidadMedidaId.Value, DBNull.Value)
                    cmd.Parameters.Add("P_CATEGORIA_ID",     OracleDbType.Int32).Value    = producto.CategoriaId
                    cmd.Parameters.Add("P_LOTE_PRODUCTO",    OracleDbType.Varchar2).Value = If(String.IsNullOrWhiteSpace(producto.LoteProducto), DBNull.Value, producto.LoteProducto)

                    conn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Sub Eliminar(productoId As Integer)
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_PRODUCTO.SP_ELIMINAR_PRODUCTO", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_PRODUCTO_ID", OracleDbType.Int32).Value = productoId
                    conn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Function Listar() As DataTable
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_PRODUCTO.SP_LISTAR_PRODUCTOS", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output
                    Using da As New OracleDataAdapter(cmd)
                        Dim dt As New DataTable()
                        da.Fill(dt)
                        Return dt
                    End Using
                End Using
            End Using
        End Function

        Public Function Buscar(criterio As String, valor As String) As DataTable
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_PRODUCTO.SP_BUSCAR_PRODUCTOS", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_CRITERIO", OracleDbType.Varchar2).Value = criterio
                    cmd.Parameters.Add("P_VALOR",    OracleDbType.Varchar2).Value = valor
                    cmd.Parameters.Add("P_CURSOR",   OracleDbType.RefCursor).Direction = ParameterDirection.Output
                    Using da As New OracleDataAdapter(cmd)
                        Dim dt As New DataTable()
                        da.Fill(dt)
                        Return dt
                    End Using
                End Using
            End Using
        End Function

    End Class

End Namespace
