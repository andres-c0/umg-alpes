Imports Oracle.ManagedDataAccess.Client
Imports System.Data
Imports alpes.Entidades

Namespace Datos

    Public Class EstadoProduccionDatos

        Private ReadOnly _conexion As alpes.Datos.ConexionOracle

        Public Sub New()
            _conexion = New alpes.Datos.ConexionOracle()
        End Sub

        Public Function Insertar(estadoProduccion As EstadoProduccion) As Integer
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_ESTADO_PRODUCCION.SP_INSERTAR_ESTADO_PRODUCCION", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_CODIGO",      OracleDbType.Varchar2).Value = estadoProduccion.Codigo
                    cmd.Parameters.Add("P_DESCRIPCION", OracleDbType.Varchar2).Value = If(String.IsNullOrWhiteSpace(estadoProduccion.Descripcion), DBNull.Value, estadoProduccion.Descripcion)

                    Dim pOut As New OracleParameter("P_ESTADO_PRODUCCION_ID", OracleDbType.Int32)
                    pOut.Direction = ParameterDirection.Output
                    cmd.Parameters.Add(pOut)

                    conn.Open()
                    cmd.ExecuteNonQuery()
                    Return Convert.ToInt32(pOut.Value.ToString())
                End Using
            End Using
        End Function

        Public Sub Actualizar(estadoProduccion As EstadoProduccion)
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_ESTADO_PRODUCCION.SP_ACTUALIZAR_ESTADO_PRODUCCION", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_ESTADO_PRODUCCION_ID", OracleDbType.Int32).Value    = estadoProduccion.EstadoProduccionId
                    cmd.Parameters.Add("P_CODIGO",               OracleDbType.Varchar2).Value = estadoProduccion.Codigo
                    cmd.Parameters.Add("P_DESCRIPCION",          OracleDbType.Varchar2).Value = If(String.IsNullOrWhiteSpace(estadoProduccion.Descripcion), DBNull.Value, estadoProduccion.Descripcion)

                    conn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Sub Eliminar(estadoProduccionId As Integer)
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_ESTADO_PRODUCCION.SP_ELIMINAR_ESTADO_PRODUCCION", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_ESTADO_PRODUCCION_ID", OracleDbType.Int32).Value = estadoProduccionId
                    conn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Function Listar() As DataTable
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_ESTADO_PRODUCCION.SP_LISTAR_ESTADO_PRODUCCION", conn)
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
                Using cmd As New OracleCommand("PKG_ESTADO_PRODUCCION.SP_BUSCAR_ESTADO_PRODUCCION", conn)
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
