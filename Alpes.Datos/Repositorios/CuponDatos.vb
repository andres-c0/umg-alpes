Option Strict On
Option Explicit On

Imports System.Data
Imports Oracle.ManagedDataAccess.Client
Imports Alpes.Datos.Conexion
Imports Alpes.Entidades.Marketing

Namespace Repositorios
    Public Class CuponDatos

        Private ReadOnly _conexionOracle As ConexionOracle

        Public Sub New()
            _conexionOracle = New ConexionOracle()
        End Sub

        Public Function Insertar(ByVal entidad As Cupon) As Integer
            Dim idGenerado As Integer = 0

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_CUPON.SP_INSERTAR_CUPON", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_CODIGO", OracleDbType.Varchar2).Value = entidad.Codigo

                    If String.IsNullOrWhiteSpace(entidad.Descripcion) Then
                        cmd.Parameters.Add("P_DESCRIPCION", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("P_DESCRIPCION", OracleDbType.Varchar2).Value = entidad.Descripcion
                    End If

                    cmd.Parameters.Add("P_VIGENCIA_INICIO", OracleDbType.Date).Value = entidad.VigenciaInicio
                    cmd.Parameters.Add("P_VIGENCIA_FIN", OracleDbType.Date).Value = entidad.VigenciaFin

                    If entidad.LimiteUsoTotal.HasValue Then
                        cmd.Parameters.Add("P_LIMITE_USO_TOTAL", OracleDbType.Int32).Value = entidad.LimiteUsoTotal.Value
                    Else
                        cmd.Parameters.Add("P_LIMITE_USO_TOTAL", OracleDbType.Int32).Value = DBNull.Value
                    End If

                    If entidad.LimiteUsoPorCliente.HasValue Then
                        cmd.Parameters.Add("P_LIMITE_USO_POR_CLIENTE", OracleDbType.Int32).Value = entidad.LimiteUsoPorCliente.Value
                    Else
                        cmd.Parameters.Add("P_LIMITE_USO_POR_CLIENTE", OracleDbType.Int32).Value = DBNull.Value
                    End If

                    cmd.Parameters.Add("P_USOS_ACTUALES", OracleDbType.Int32).Value = entidad.UsosActuales
                    cmd.Parameters.Add("P_CUPON_ID", OracleDbType.Int32).Direction = ParameterDirection.Output

                    cmd.ExecuteNonQuery()

                    idGenerado = Convert.ToInt32(cmd.Parameters("P_CUPON_ID").Value.ToString())
                End Using
            End Using

            Return idGenerado
        End Function

        Public Sub Actualizar(ByVal entidad As Cupon)
            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_CUPON.SP_ACTUALIZAR_CUPON", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_CUPON_ID", OracleDbType.Int32).Value = entidad.CuponId
                    cmd.Parameters.Add("P_CODIGO", OracleDbType.Varchar2).Value = entidad.Codigo

                    If String.IsNullOrWhiteSpace(entidad.Descripcion) Then
                        cmd.Parameters.Add("P_DESCRIPCION", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("P_DESCRIPCION", OracleDbType.Varchar2).Value = entidad.Descripcion
                    End If

                    cmd.Parameters.Add("P_VIGENCIA_INICIO", OracleDbType.Date).Value = entidad.VigenciaInicio
                    cmd.Parameters.Add("P_VIGENCIA_FIN", OracleDbType.Date).Value = entidad.VigenciaFin

                    If entidad.LimiteUsoTotal.HasValue Then
                        cmd.Parameters.Add("P_LIMITE_USO_TOTAL", OracleDbType.Int32).Value = entidad.LimiteUsoTotal.Value
                    Else
                        cmd.Parameters.Add("P_LIMITE_USO_TOTAL", OracleDbType.Int32).Value = DBNull.Value
                    End If

                    If entidad.LimiteUsoPorCliente.HasValue Then
                        cmd.Parameters.Add("P_LIMITE_USO_POR_CLIENTE", OracleDbType.Int32).Value = entidad.LimiteUsoPorCliente.Value
                    Else
                        cmd.Parameters.Add("P_LIMITE_USO_POR_CLIENTE", OracleDbType.Int32).Value = DBNull.Value
                    End If

                    cmd.Parameters.Add("P_USOS_ACTUALES", OracleDbType.Int32).Value = entidad.UsosActuales

                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Sub Eliminar(ByVal id As Integer)
            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_CUPON.SP_ELIMINAR_CUPON", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_CUPON_ID", OracleDbType.Int32).Value = id

                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Function ObtenerPorId(ByVal id As Integer) As Cupon
            Dim entidad As Cupon = Nothing

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_CUPON.SP_OBTENER_CUPON", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_CUPON_ID", OracleDbType.Int32).Value = id
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        If dr.Read() Then
                            entidad = MapearCupon(dr)
                        End If
                    End Using
                End Using
            End Using

            Return entidad
        End Function

        Public Function Listar() As List(Of Cupon)
            Dim lista As New List(Of Cupon)()

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_CUPON.SP_LISTAR_CUPONES", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        While dr.Read()
                            lista.Add(MapearCupon(dr))
                        End While
                    End Using
                End Using
            End Using

            Return lista
        End Function

        Public Function Buscar(ByVal criterio As String, ByVal valor As String) As List(Of Cupon)
            Dim lista As New List(Of Cupon)()

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_CUPON.SP_BUSCAR_CUPONES", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_CRITERIO", OracleDbType.Varchar2).Value = criterio
                    cmd.Parameters.Add("P_VALOR", OracleDbType.Varchar2).Value = valor
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        While dr.Read()
                            lista.Add(MapearCupon(dr))
                        End While
                    End Using
                End Using
            End Using

            Return lista
        End Function

        Private Function MapearCupon(ByVal dr As OracleDataReader) As Cupon
            Dim entidad As New Cupon()

            entidad.CuponId = Convert.ToInt32(dr("CUPON_ID"))
            entidad.Codigo = dr("CODIGO").ToString()
            entidad.Descripcion = If(IsDBNull(dr("DESCRIPCION")), Nothing, dr("DESCRIPCION").ToString())
            entidad.VigenciaInicio = Convert.ToDateTime(dr("VIGENCIA_INICIO"))
            entidad.VigenciaFin = Convert.ToDateTime(dr("VIGENCIA_FIN"))
            entidad.LimiteUsoTotal = If(IsDBNull(dr("LIMITE_USO_TOTAL")), CType(Nothing, Integer?), Convert.ToInt32(dr("LIMITE_USO_TOTAL")))
            entidad.LimiteUsoPorCliente = If(IsDBNull(dr("LIMITE_USO_POR_CLIENTE")), CType(Nothing, Integer?), Convert.ToInt32(dr("LIMITE_USO_POR_CLIENTE")))
            entidad.UsosActuales = Convert.ToInt32(dr("USOS_ACTUALES"))
            entidad.CreatedAt = If(IsDBNull(dr("CREATED_AT")), CType(Nothing, DateTime?), Convert.ToDateTime(dr("CREATED_AT")))
            entidad.UpdatedAt = If(IsDBNull(dr("UPDATED_AT")), CType(Nothing, DateTime?), Convert.ToDateTime(dr("UPDATED_AT")))
            entidad.Estado = If(IsDBNull(dr("ESTADO")), Nothing, dr("ESTADO").ToString())

            Return entidad
        End Function

    End Class
End Namespace