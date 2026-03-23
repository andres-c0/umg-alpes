Imports Oracle.ManagedDataAccess.Client
Imports System.Data
Imports alpes.Entidades

Namespace Datos

    Public Class CategoriaDatos

        Private ReadOnly _conexion As ConexionOracle

        Public Sub New()
            _conexion = New ConexionOracle()
        End Sub

        Public Function Insertar(categoria As Categoria) As Integer
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_CATEGORIA.SP_INSERTAR_CATEGORIA", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_NOMBRE",             OracleDbType.Varchar2).Value = categoria.Nombre
                    cmd.Parameters.Add("P_DESCRIPCION",        OracleDbType.Varchar2).Value = If(String.IsNullOrWhiteSpace(categoria.Descripcion), DBNull.Value, categoria.Descripcion)
                    cmd.Parameters.Add("P_CATEGORIA_PADRE_ID", OracleDbType.Int32).Value    = If(categoria.CategoriaPadreId.HasValue, categoria.CategoriaPadreId.Value, DBNull.Value)

                    Dim pOut As New OracleParameter("P_CATEGORIA_ID", OracleDbType.Int32)
                    pOut.Direction = ParameterDirection.Output
                    cmd.Parameters.Add(pOut)

                    conn.Open()
                    cmd.ExecuteNonQuery()
                    Return Convert.ToInt32(pOut.Value.ToString())
                End Using
            End Using
        End Function

        Public Sub Actualizar(categoria As Categoria)
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_CATEGORIA.SP_ACTUALIZAR_CATEGORIA", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_CATEGORIA_ID",       OracleDbType.Int32).Value    = categoria.CategoriaId
                    cmd.Parameters.Add("P_NOMBRE",             OracleDbType.Varchar2).Value = categoria.Nombre
                    cmd.Parameters.Add("P_DESCRIPCION",        OracleDbType.Varchar2).Value = If(String.IsNullOrWhiteSpace(categoria.Descripcion), DBNull.Value, categoria.Descripcion)
                    cmd.Parameters.Add("P_CATEGORIA_PADRE_ID", OracleDbType.Int32).Value    = If(categoria.CategoriaPadreId.HasValue, categoria.CategoriaPadreId.Value, DBNull.Value)

                    conn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Sub Eliminar(categoriaId As Integer)
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_CATEGORIA.SP_ELIMINAR_CATEGORIA", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_CATEGORIA_ID", OracleDbType.Int32).Value = categoriaId
                    conn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Function Listar() As DataTable
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_CATEGORIA.SP_LISTAR_CATEGORIAS", conn)
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
                Using cmd As New OracleCommand("PKG_CATEGORIA.SP_BUSCAR_CATEGORIAS", conn)
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
