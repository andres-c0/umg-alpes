Imports Oracle.ManagedDataAccess.Client
Imports System.Data
Imports alpes.Entidades

Namespace Datos
    Public Class InventarioMateriaPrimaDatos

        Private ReadOnly _conexion As alpes.Datos.ConexionOracle

        Public Sub New()
            _conexion = New alpes.Datos.ConexionOracle()
        End Sub

        Public Function Insertar(entidad As InventarioMateriaPrima) As Integer
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_INVENTARIO_MATERIA_PRIMA.SP_INSERTAR_INVENTARIO_MATERIA_PRIMA", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_MP_ID", OracleDbType.Int32).Value = entidad.MpId
                    cmd.Parameters.Add("P_CANTIDAD_ACTUAL", OracleDbType.Decimal).Value = entidad.CantidadActual
                    cmd.Parameters.Add("P_STOCK_MINIMO", OracleDbType.Decimal).Value = entidad.StockMinimo
                    cmd.Parameters.Add("P_UBICACION", OracleDbType.Varchar2).Value = entidad.Ubicacion
                    cmd.Parameters.Add("P_FECHA_ULT_INGRESO", OracleDbType.Date).Value = entidad.FechaUltIngreso
                    cmd.Parameters.Add("P_ESTADO", OracleDbType.Varchar2).Value = entidad.Estado

                    Dim pOut As New OracleParameter("P_INV_MP_ID", OracleDbType.Int32)
                    pOut.Direction = ParameterDirection.Output
                    cmd.Parameters.Add(pOut)

                    conn.Open()
                    cmd.ExecuteNonQuery()

                    Return Convert.ToInt32(pOut.Value.ToString())
                End Using
            End Using
        End Function

        Public Sub Actualizar(entidad As InventarioMateriaPrima)
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_INVENTARIO_MATERIA_PRIMA.SP_ACTUALIZAR_INVENTARIO_MATERIA_PRIMA", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_INV_MP_ID", OracleDbType.Int32).Value = entidad.InvMpId
                    cmd.Parameters.Add("P_MP_ID", OracleDbType.Int32).Value = entidad.MpId
                    cmd.Parameters.Add("P_CANTIDAD_ACTUAL", OracleDbType.Decimal).Value = entidad.CantidadActual
                    cmd.Parameters.Add("P_STOCK_MINIMO", OracleDbType.Decimal).Value = entidad.StockMinimo
                    cmd.Parameters.Add("P_UBICACION", OracleDbType.Varchar2).Value = entidad.Ubicacion
                    cmd.Parameters.Add("P_FECHA_ULT_INGRESO", OracleDbType.Date).Value = entidad.FechaUltIngreso
                    cmd.Parameters.Add("P_ESTADO", OracleDbType.Varchar2).Value = entidad.Estado

                    conn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Sub Eliminar(invMpId As Integer)
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_INVENTARIO_MATERIA_PRIMA.SP_ELIMINAR_INVENTARIO_MATERIA_PRIMA", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_INV_MP_ID", OracleDbType.Int32).Value = invMpId

                    conn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Function Listar() As DataTable
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_INVENTARIO_MATERIA_PRIMA.SP_LISTAR_INVENTARIOS_MATERIA_PRIMA", conn)
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
                Using cmd As New OracleCommand("PKG_INVENTARIO_MATERIA_PRIMA.SP_BUSCAR_INVENTARIOS_MATERIA_PRIMA", conn)
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

        Public Function ObtenerPorId(invMpId As Integer) As DataTable
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_INVENTARIO_MATERIA_PRIMA.SP_OBTENER_INVENTARIO_MATERIA_PRIMA", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_INV_MP_ID", OracleDbType.Int32).Value = invMpId
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