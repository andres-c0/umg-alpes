Imports Oracle.ManagedDataAccess.Client
Imports System.Data
Imports alpes.Entidades

Namespace Datos

    Public Class UnidadMedidaDatos

        Private ReadOnly _conexion As alpes.Datos.ConexionOracle

        Public Sub New()
            _conexion = New alpes.Datos.ConexionOracle()
        End Sub

        Public Function Insertar(unidadMedida As UnidadMedida) As Integer
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_UNIDAD_MEDIDA.SP_INSERTAR_UNIDAD_MEDIDA", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_CODIGO", OracleDbType.Varchar2).Value = unidadMedida.Codigo
                    cmd.Parameters.Add("P_NOMBRE", OracleDbType.Varchar2).Value = unidadMedida.Nombre

                    Dim pOut As New OracleParameter("P_UNIDAD_MEDIDA_ID", OracleDbType.Int32)
                    pOut.Direction = ParameterDirection.Output
                    cmd.Parameters.Add(pOut)

                    conn.Open()
                    cmd.ExecuteNonQuery()
                    Return Convert.ToInt32(pOut.Value.ToString())
                End Using
            End Using
        End Function

        Public Sub Actualizar(unidadMedida As UnidadMedida)
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_UNIDAD_MEDIDA.SP_ACTUALIZAR_UNIDAD_MEDIDA", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_UNIDAD_MEDIDA_ID", OracleDbType.Int32).Value    = unidadMedida.UnidadMedidaId
                    cmd.Parameters.Add("P_CODIGO",           OracleDbType.Varchar2).Value = unidadMedida.Codigo
                    cmd.Parameters.Add("P_NOMBRE",           OracleDbType.Varchar2).Value = unidadMedida.Nombre

                    conn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Sub Eliminar(unidadMedidaId As Integer)
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_UNIDAD_MEDIDA.SP_ELIMINAR_UNIDAD_MEDIDA", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_UNIDAD_MEDIDA_ID", OracleDbType.Int32).Value = unidadMedidaId
                    conn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Function Listar() As DataTable
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_UNIDAD_MEDIDA.SP_LISTAR_UNIDAD_MEDIDA", conn)
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
                Using cmd As New OracleCommand("PKG_UNIDAD_MEDIDA.SP_BUSCAR_UNIDAD_MEDIDA", conn)
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
