Imports Oracle.ManagedDataAccess.Client
Imports System.Data
Imports alpes.Entidades

Namespace Datos

    Public Class RutaEntregaDatos

        Private ReadOnly _conexion As alpes.Datos.ConexionOracle

        Public Sub New()
            _conexion = New alpes.Datos.ConexionOracle()
        End Sub

        Public Function Insertar(rutaEntrega As RutaEntrega) As Integer
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_RUTA_ENTREGA.SP_INSERTAR_RUTA_ENTREGA", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_VEHICULO_ID",  OracleDbType.Int32).Value    = rutaEntrega.VehiculoId
                    cmd.Parameters.Add("P_FECHA_RUTA",   OracleDbType.Date).Value     = rutaEntrega.FechaRuta
                    cmd.Parameters.Add("P_DESCRIPCION",  OracleDbType.Varchar2).Value = If(String.IsNullOrWhiteSpace(rutaEntrega.Descripcion), DBNull.Value, rutaEntrega.Descripcion)
                    cmd.Parameters.Add("P_ESTADO_RUTA",  OracleDbType.Varchar2).Value = rutaEntrega.EstadoRuta

                    Dim pOut As New OracleParameter("P_RUTA_ENTREGA_ID", OracleDbType.Int32)
                    pOut.Direction = ParameterDirection.Output
                    cmd.Parameters.Add(pOut)

                    conn.Open()
                    cmd.ExecuteNonQuery()
                    Return Convert.ToInt32(pOut.Value.ToString())
                End Using
            End Using
        End Function

        Public Sub Actualizar(rutaEntrega As RutaEntrega)
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_RUTA_ENTREGA.SP_ACTUALIZAR_RUTA_ENTREGA", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_RUTA_ENTREGA_ID", OracleDbType.Int32).Value    = rutaEntrega.RutaEntregaId
                    cmd.Parameters.Add("P_VEHICULO_ID",     OracleDbType.Int32).Value    = rutaEntrega.VehiculoId
                    cmd.Parameters.Add("P_FECHA_RUTA",      OracleDbType.Date).Value     = rutaEntrega.FechaRuta
                    cmd.Parameters.Add("P_DESCRIPCION",     OracleDbType.Varchar2).Value = If(String.IsNullOrWhiteSpace(rutaEntrega.Descripcion), DBNull.Value, rutaEntrega.Descripcion)
                    cmd.Parameters.Add("P_ESTADO_RUTA",     OracleDbType.Varchar2).Value = rutaEntrega.EstadoRuta

                    conn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Sub Eliminar(rutaEntregaId As Integer)
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_RUTA_ENTREGA.SP_ELIMINAR_RUTA_ENTREGA", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_RUTA_ENTREGA_ID", OracleDbType.Int32).Value = rutaEntregaId
                    conn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Function Listar() As DataTable
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_RUTA_ENTREGA.SP_LISTAR_RUTA_ENTREGA", conn)
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
                Using cmd As New OracleCommand("PKG_RUTA_ENTREGA.SP_BUSCAR_RUTA_ENTREGA", conn)
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
