Option Strict On
Option Explicit On

Imports System.Data
Imports Oracle.ManagedDataAccess.Client
Imports Alpes.Datos.Conexion
Imports Alpes.Entidades.Ventas

Namespace Repositorios
    Public Class Estado_OrdenDatos

        Private ReadOnly _conexionOracle As ConexionOracle

        Public Sub New()
            _conexionOracle = New ConexionOracle()
        End Sub

        Public Function Insertar(ByVal entidad As Estado_Orden) As Integer
            Dim idGenerado As Integer = 0

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_ESTADO_ORDEN.SP_INSERTAR", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_CODIGO", OracleDbType.Varchar2).Value = entidad.Codigo

                    If String.IsNullOrWhiteSpace(entidad.Descripcion) Then
                        cmd.Parameters.Add("P_DESCRIPCION", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("P_DESCRIPCION", OracleDbType.Varchar2).Value = entidad.Descripcion
                    End If

                    If String.IsNullOrWhiteSpace(entidad.Estado) Then
                        cmd.Parameters.Add("P_ESTADO", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("P_ESTADO", OracleDbType.Varchar2).Value = entidad.Estado
                    End If

                    cmd.Parameters.Add("P_ID", OracleDbType.Int32).Direction = ParameterDirection.Output

                    cmd.ExecuteNonQuery()

                    idGenerado = Convert.ToInt32(cmd.Parameters("P_ID").Value.ToString())
                End Using
            End Using

            Return idGenerado
        End Function

        Public Sub Actualizar(ByVal entidad As Estado_Orden)
            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_ESTADO_ORDEN.SP_ACTUALIZAR", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_ESTADO_ORDEN_ID", OracleDbType.Int32).Value = entidad.EstadoOrdenId
                    cmd.Parameters.Add("P_CODIGO", OracleDbType.Varchar2).Value = entidad.Codigo

                    If String.IsNullOrWhiteSpace(entidad.Descripcion) Then
                        cmd.Parameters.Add("P_DESCRIPCION", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("P_DESCRIPCION", OracleDbType.Varchar2).Value = entidad.Descripcion
                    End If

                    cmd.Parameters.Add("P_ESTADO", OracleDbType.Varchar2).Value = entidad.Estado

                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Sub Eliminar(ByVal id As Integer)
            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_ESTADO_ORDEN.SP_ELIMINAR", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_ID", OracleDbType.Int32).Value = id

                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Function ObtenerPorId(ByVal id As Integer) As Estado_Orden
            Dim entidad As Estado_Orden = Nothing

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_ESTADO_ORDEN.SP_OBTENER", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_ID", OracleDbType.Int32).Value = id
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        If dr.Read() Then
                            entidad = MapearEstadoOrden(dr)
                        End If
                    End Using
                End Using
            End Using

            Return entidad
        End Function

        Public Function Listar() As List(Of Estado_Orden)
            Dim lista As New List(Of Estado_Orden)()

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_ESTADO_ORDEN.SP_LISTAR", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        While dr.Read()
                            lista.Add(MapearEstadoOrden(dr))
                        End While
                    End Using
                End Using
            End Using

            Return lista
        End Function

        Public Function Buscar(ByVal valor As String) As List(Of Estado_Orden)
            Dim lista As New List(Of Estado_Orden)()

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_ESTADO_ORDEN.SP_BUSCAR", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_VALOR", OracleDbType.Varchar2).Value = valor
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        While dr.Read()
                            lista.Add(MapearEstadoOrden(dr))
                        End While
                    End Using
                End Using
            End Using

            Return lista
        End Function

        Private Function MapearEstadoOrden(ByVal dr As OracleDataReader) As Estado_Orden
            Dim entidad As New Estado_Orden()

            entidad.EstadoOrdenId = Convert.ToInt32(dr("ESTADO_ORDEN_ID"))
            entidad.Codigo = dr("CODIGO").ToString()
            entidad.Descripcion = If(IsDBNull(dr("DESCRIPCION")), Nothing, dr("DESCRIPCION").ToString())
            entidad.CreatedAt = If(IsDBNull(dr("CREATED_AT")), CType(Nothing, DateTime?), Convert.ToDateTime(dr("CREATED_AT")))
            entidad.UpdatedAt = If(IsDBNull(dr("UPDATED_AT")), CType(Nothing, DateTime?), Convert.ToDateTime(dr("UPDATED_AT")))
            entidad.Estado = If(IsDBNull(dr("ESTADO")), Nothing, dr("ESTADO").ToString())

            Return entidad
        End Function

    End Class
End Namespace