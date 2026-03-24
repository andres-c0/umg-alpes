Imports Oracle.ManagedDataAccess.Client
Imports System.Data
Imports alpes.Entidades

Namespace Datos

    Public Class PrecioHistoricoDatos

        Private ReadOnly _conexion As alpes.Datos.ConexionOracle

        Public Sub New()
            _conexion = New alpes.Datos.ConexionOracle()
        End Sub

        Public Function Insertar(precioHistorico As PrecioHistorico) As Integer
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_PRECIO_HISTORICO.SP_INSERTAR_PRECIO_HISTORICO", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_PRODUCTO_ID",     OracleDbType.Int32).Value    = precioHistorico.ProductoId
                    cmd.Parameters.Add("P_PRECIO",          OracleDbType.Decimal).Value  = precioHistorico.Precio
                    cmd.Parameters.Add("P_VIGENCIA_INICIO", OracleDbType.Date).Value     = precioHistorico.VigenciaInicio
                    cmd.Parameters.Add("P_VIGENCIA_FIN",    OracleDbType.Date).Value     = If(precioHistorico.VigenciaFin.HasValue, precioHistorico.VigenciaFin.Value, DBNull.Value)
                    cmd.Parameters.Add("P_MOTIVO",          OracleDbType.Varchar2).Value = If(String.IsNullOrWhiteSpace(precioHistorico.Motivo), DBNull.Value, precioHistorico.Motivo)

                    Dim pOut As New OracleParameter("P_PRECIO_HIST_ID", OracleDbType.Int32)
                    pOut.Direction = ParameterDirection.Output
                    cmd.Parameters.Add(pOut)

                    conn.Open()
                    cmd.ExecuteNonQuery()
                    Return Convert.ToInt32(pOut.Value.ToString())
                End Using
            End Using
        End Function

        Public Sub Actualizar(precioHistorico As PrecioHistorico)
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_PRECIO_HISTORICO.SP_ACTUALIZAR_PRECIO_HISTORICO", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_PRECIO_HIST_ID",  OracleDbType.Int32).Value    = precioHistorico.PrecioHistId
                    cmd.Parameters.Add("P_PRODUCTO_ID",     OracleDbType.Int32).Value    = precioHistorico.ProductoId
                    cmd.Parameters.Add("P_PRECIO",          OracleDbType.Decimal).Value  = precioHistorico.Precio
                    cmd.Parameters.Add("P_VIGENCIA_INICIO", OracleDbType.Date).Value     = precioHistorico.VigenciaInicio
                    cmd.Parameters.Add("P_VIGENCIA_FIN",    OracleDbType.Date).Value     = If(precioHistorico.VigenciaFin.HasValue, precioHistorico.VigenciaFin.Value, DBNull.Value)
                    cmd.Parameters.Add("P_MOTIVO",          OracleDbType.Varchar2).Value = If(String.IsNullOrWhiteSpace(precioHistorico.Motivo), DBNull.Value, precioHistorico.Motivo)

                    conn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Sub Eliminar(precioHistId As Integer)
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_PRECIO_HISTORICO.SP_ELIMINAR_PRECIO_HISTORICO", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_PRECIO_HIST_ID", OracleDbType.Int32).Value = precioHistId
                    conn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Function Listar() As DataTable
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_PRECIO_HISTORICO.SP_LISTAR_PRECIO_HISTORICO", conn)
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
                Using cmd As New OracleCommand("PKG_PRECIO_HISTORICO.SP_BUSCAR_PRECIO_HISTORICO", conn)
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
