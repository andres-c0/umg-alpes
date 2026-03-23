Imports Oracle.ManagedDataAccess.Client
Imports System.Data
Imports alpes.Entidades

Namespace Datos
    Public Class ListaMaterialesDatos

        Private ReadOnly _conexion As ConexionOracle

        Public Sub New()
            _conexion = New ConexionOracle()
        End Sub

        Public Function Insertar(entidad As ListaMateriales) As Integer
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_LISTA_MATERIALES.SP_INSERTAR_LISTA_MATERIALES", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_CODIGO_LISTA", OracleDbType.Varchar2).Value = entidad.CodigoLista
                    cmd.Parameters.Add("P_PRODUCTO_ID", OracleDbType.Int32).Value = entidad.ProductoId
                    cmd.Parameters.Add("P_VERSION", OracleDbType.Int32).Value = entidad.Version
                    cmd.Parameters.Add("P_FECHA_VIGENCIA", OracleDbType.Date).Value = entidad.FechaVigencia
                    cmd.Parameters.Add("P_ESTADO", OracleDbType.Varchar2).Value = entidad.Estado
                    cmd.Parameters.Add("P_OBSERVACIONES", OracleDbType.Varchar2).Value = If(String.IsNullOrWhiteSpace(entidad.Observaciones), DBNull.Value, entidad.Observaciones)

                    Dim pOut As New OracleParameter("P_LM_ID", OracleDbType.Int32)
                    pOut.Direction = ParameterDirection.Output
                    cmd.Parameters.Add(pOut)

                    conn.Open()
                    cmd.ExecuteNonQuery()

                    Return Convert.ToInt32(pOut.Value.ToString())
                End Using
            End Using
        End Function

        Public Sub Actualizar(entidad As ListaMateriales)
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_LISTA_MATERIALES.SP_ACTUALIZAR_LISTA_MATERIALES", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_LM_ID", OracleDbType.Int32).Value = entidad.LmId
                    cmd.Parameters.Add("P_CODIGO_LISTA", OracleDbType.Varchar2).Value = entidad.CodigoLista
                    cmd.Parameters.Add("P_PRODUCTO_ID", OracleDbType.Int32).Value = entidad.ProductoId
                    cmd.Parameters.Add("P_VERSION", OracleDbType.Int32).Value = entidad.Version
                    cmd.Parameters.Add("P_FECHA_VIGENCIA", OracleDbType.Date).Value = entidad.FechaVigencia
                    cmd.Parameters.Add("P_ESTADO", OracleDbType.Varchar2).Value = entidad.Estado
                    cmd.Parameters.Add("P_OBSERVACIONES", OracleDbType.Varchar2).Value = If(String.IsNullOrWhiteSpace(entidad.Observaciones), DBNull.Value, entidad.Observaciones)

                    conn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Sub Eliminar(lmId As Integer)
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_LISTA_MATERIALES.SP_ELIMINAR_LISTA_MATERIALES", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_LM_ID", OracleDbType.Int32).Value = lmId

                    conn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Function Listar() As DataTable
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_LISTA_MATERIALES.SP_LISTAR_LISTAS_MATERIALES", conn)
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
                Using cmd As New OracleCommand("PKG_LISTA_MATERIALES.SP_BUSCAR_LISTAS_MATERIALES", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_CRITERIO", OracleDbType.Varchar2).Value = criterio
                    cmd.Parameters.Add("P_VALOR", OracleDbType.Varchar2).Value = valor
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using da As New OracleDataAdapter(cmd)
                        Dim dt As New DataTable()
                        da.Fill(dt)
                        Return dt
                    End Using
                End Using
            End Using
        End Function

        Public Function ObtenerPorId(lmId As Integer) As DataTable
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_LISTA_MATERIALES.SP_OBTENER_LISTA_MATERIALES", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_LM_ID", OracleDbType.Int32).Value = lmId
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

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