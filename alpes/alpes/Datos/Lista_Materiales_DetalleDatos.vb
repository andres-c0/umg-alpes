Imports Oracle.ManagedDataAccess.Client
Imports System.Data
Imports alpes.Entidades

Namespace Datos
    Public Class ListaMaterialesDetalleDatos

        Private ReadOnly _conexion As alpes.Datos.ConexionOracle

        Public Sub New()
            _conexion = New alpes.Datos.ConexionOracle()
        End Sub

        Public Function Insertar(entidad As ListaMaterialesDetalle) As Integer
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_LISTA_MATERIALES_DETALLE.SP_INSERTAR_LISTA_MATERIALES_DETALLE", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_LM_ID", OracleDbType.Int32).Value = entidad.LmId
                    cmd.Parameters.Add("P_MP_ID", OracleDbType.Int32).Value = entidad.MpId
                    cmd.Parameters.Add("P_CANTIDAD_REQUERIDA", OracleDbType.Decimal).Value = entidad.CantidadRequerida
                    cmd.Parameters.Add("P_UNIDAD_MEDIDA", OracleDbType.Varchar2).Value = entidad.UnidadMedida
                    cmd.Parameters.Add("P_DESCRIPCION", OracleDbType.Varchar2).Value = If(String.IsNullOrWhiteSpace(entidad.Descripcion), DBNull.Value, entidad.Descripcion)

                    Dim pOut As New OracleParameter("P_LMD_ID", OracleDbType.Int32)
                    pOut.Direction = ParameterDirection.Output
                    cmd.Parameters.Add(pOut)

                    conn.Open()
                    cmd.ExecuteNonQuery()

                    Return Convert.ToInt32(pOut.Value.ToString())
                End Using
            End Using
        End Function

        Public Sub Actualizar(entidad As ListaMaterialesDetalle)
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_LISTA_MATERIALES_DETALLE.SP_ACTUALIZAR_LISTA_MATERIALES_DETALLE", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_LMD_ID", OracleDbType.Int32).Value = entidad.LmdId
                    cmd.Parameters.Add("P_LM_ID", OracleDbType.Int32).Value = entidad.LmId
                    cmd.Parameters.Add("P_MP_ID", OracleDbType.Int32).Value = entidad.MpId
                    cmd.Parameters.Add("P_CANTIDAD_REQUERIDA", OracleDbType.Decimal).Value = entidad.CantidadRequerida
                    cmd.Parameters.Add("P_UNIDAD_MEDIDA", OracleDbType.Varchar2).Value = entidad.UnidadMedida
                    cmd.Parameters.Add("P_DESCRIPCION", OracleDbType.Varchar2).Value = If(String.IsNullOrWhiteSpace(entidad.Descripcion), DBNull.Value, entidad.Descripcion)

                    conn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Sub Eliminar(lmdId As Integer)
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_LISTA_MATERIALES_DETALLE.SP_ELIMINAR_LISTA_MATERIALES_DETALLE", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_LMD_ID", OracleDbType.Int32).Value = lmdId

                    conn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Function Listar() As DataTable
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_LISTA_MATERIALES_DETALLE.SP_LISTAR_LISTAS_MATERIALES_DETALLE", conn)
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
                Using cmd As New OracleCommand("PKG_LISTA_MATERIALES_DETALLE.SP_BUSCAR_LISTAS_MATERIALES_DETALLE", conn)
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

        Public Function ObtenerPorId(lmdId As Integer) As DataTable
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_LISTA_MATERIALES_DETALLE.SP_OBTENER_LISTA_MATERIALES_DETALLE", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_LMD_ID", OracleDbType.Int32).Value = lmdId
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