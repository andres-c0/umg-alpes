Option Strict On
Option Explicit On

Imports System.Data
Imports Oracle.ManagedDataAccess.Client
Imports Alpes.Datos.Conexion
Imports Alpes.Entidades.Compras

Namespace Repositorios
    Public Class Estado_Orden_CompraDatos

        Private ReadOnly _conexionOracle As ConexionOracle

        Public Sub New()
            _conexionOracle = New ConexionOracle()
        End Sub

        Public Function Insertar(ByVal entidad As Estado_Orden_Compra) As Integer
            Dim idGenerado As Integer = 0

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_ESTADO_ORDEN_COMPRA.SP_INSERTAR_ESTADO_ORDEN_COMPRA", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_CODIGO", OracleDbType.Varchar2).Value = entidad.Codigo

                    If String.IsNullOrWhiteSpace(entidad.Descripcion) Then
                        cmd.Parameters.Add("P_DESCRIPCION", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("P_DESCRIPCION", OracleDbType.Varchar2).Value = entidad.Descripcion
                    End If

                    cmd.Parameters.Add("P_ESTADO_OC_ID", OracleDbType.Int32).Direction = ParameterDirection.Output

                    cmd.ExecuteNonQuery()

                    idGenerado = Convert.ToInt32(cmd.Parameters("P_ESTADO_OC_ID").Value.ToString())
                End Using
            End Using

            Return idGenerado
        End Function

        Public Sub Actualizar(ByVal entidad As Estado_Orden_Compra)
            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_ESTADO_ORDEN_COMPRA.SP_ACTUALIZAR_ESTADO_ORDEN_COMPRA", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_ESTADO_OC_ID", OracleDbType.Int32).Value = entidad.EstadoOcId
                    cmd.Parameters.Add("P_CODIGO", OracleDbType.Varchar2).Value = entidad.Codigo

                    If String.IsNullOrWhiteSpace(entidad.Descripcion) Then
                        cmd.Parameters.Add("P_DESCRIPCION", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("P_DESCRIPCION", OracleDbType.Varchar2).Value = entidad.Descripcion
                    End If

                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Sub Eliminar(ByVal id As Integer)
            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_ESTADO_ORDEN_COMPRA.SP_ELIMINAR_ESTADO_ORDEN_COMPRA", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_ESTADO_OC_ID", OracleDbType.Int32).Value = id

                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Function ObtenerPorId(ByVal id As Integer) As Estado_Orden_Compra
            Dim entidad As Estado_Orden_Compra = Nothing

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_ESTADO_ORDEN_COMPRA.SP_OBTENER_ESTADO_ORDEN_COMPRA", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_ESTADO_OC_ID", OracleDbType.Int32).Value = id
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        If dr.Read() Then
                            entidad = MapearEstadoOrdenCompra(dr)
                        End If
                    End Using
                End Using
            End Using

            Return entidad
        End Function

        Public Function Listar() As List(Of Estado_Orden_Compra)
            Dim lista As New List(Of Estado_Orden_Compra)()

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_ESTADO_ORDEN_COMPRA.SP_LISTAR_ESTADOS_ORDEN_COMPRA", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        While dr.Read()
                            lista.Add(MapearEstadoOrdenCompra(dr))
                        End While
                    End Using
                End Using
            End Using

            Return lista
        End Function

        Public Function Buscar(ByVal criterio As String, ByVal valor As String) As List(Of Estado_Orden_Compra)
            Dim lista As New List(Of Estado_Orden_Compra)()

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_ESTADO_ORDEN_COMPRA.SP_BUSCAR_ESTADOS_ORDEN_COMPRA", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_CRITERIO", OracleDbType.Varchar2).Value = criterio
                    cmd.Parameters.Add("P_VALOR", OracleDbType.Varchar2).Value = valor
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        While dr.Read()
                            lista.Add(MapearEstadoOrdenCompra(dr))
                        End While
                    End Using
                End Using
            End Using

            Return lista
        End Function

        Private Function MapearEstadoOrdenCompra(ByVal dr As OracleDataReader) As Estado_Orden_Compra
            Dim entidad As New Estado_Orden_Compra()

            entidad.EstadoOcId = Convert.ToInt32(dr("ESTADO_OC_ID"))
            entidad.Codigo = dr("CODIGO").ToString()
            entidad.Descripcion = If(IsDBNull(dr("DESCRIPCION")), Nothing, dr("DESCRIPCION").ToString())
            entidad.CreatedAt = If(IsDBNull(dr("CREATED_AT")), CType(Nothing, DateTime?), Convert.ToDateTime(dr("CREATED_AT")))
            entidad.UpdatedAt = If(IsDBNull(dr("UPDATED_AT")), CType(Nothing, DateTime?), Convert.ToDateTime(dr("UPDATED_AT")))
            entidad.Estado = If(IsDBNull(dr("ESTADO")), Nothing, dr("ESTADO").ToString())

            Return entidad
        End Function

    End Class
End Namespace