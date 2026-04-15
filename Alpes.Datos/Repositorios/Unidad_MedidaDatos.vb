Option Strict On
Option Explicit On

Imports System.Data
Imports Oracle.ManagedDataAccess.Client
Imports Alpes.Datos.Conexion
Imports Alpes.Entidades.Inventario

Namespace Repositorios
    Public Class Unidad_MedidaDatos

        Private ReadOnly _conexionOracle As ConexionOracle

        Public Sub New()
            _conexionOracle = New ConexionOracle()
        End Sub

        Public Function Insertar(ByVal entidad As Unidad_Medida) As Integer
            Dim idGenerado As Integer = 0

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_UNIDAD_MEDIDA.SP_INSERTAR_UNIDAD_MEDIDA", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_CODIGO", OracleDbType.Varchar2).Value = entidad.Codigo
                    cmd.Parameters.Add("P_NOMBRE", OracleDbType.Varchar2).Value = entidad.Nombre
                    cmd.Parameters.Add("P_UNIDAD_MEDIDA_ID", OracleDbType.Int32).Direction = ParameterDirection.Output

                    cmd.ExecuteNonQuery()

                    idGenerado = Convert.ToInt32(cmd.Parameters("P_UNIDAD_MEDIDA_ID").Value.ToString())
                End Using
            End Using

            Return idGenerado
        End Function

        Public Sub Actualizar(ByVal entidad As Unidad_Medida)
            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_UNIDAD_MEDIDA.SP_ACTUALIZAR_UNIDAD_MEDIDA", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_UNIDAD_MEDIDA_ID", OracleDbType.Int32).Value = entidad.UnidadMedidaId
                    cmd.Parameters.Add("P_CODIGO", OracleDbType.Varchar2).Value = entidad.Codigo
                    cmd.Parameters.Add("P_NOMBRE", OracleDbType.Varchar2).Value = entidad.Nombre

                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Sub Eliminar(ByVal id As Integer)
            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_UNIDAD_MEDIDA.SP_ELIMINAR_UNIDAD_MEDIDA", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_UNIDAD_MEDIDA_ID", OracleDbType.Int32).Value = id

                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Function ObtenerPorId(ByVal id As Integer) As Unidad_Medida
            Dim entidad As Unidad_Medida = Nothing

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_UNIDAD_MEDIDA.SP_OBTENER_UNIDAD_MEDIDA", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_UNIDAD_MEDIDA_ID", OracleDbType.Int32).Value = id
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        If dr.Read() Then
                            entidad = MapearUnidadMedida(dr)
                        End If
                    End Using
                End Using
            End Using

            Return entidad
        End Function

        Public Function Listar() As List(Of Unidad_Medida)
            Dim lista As New List(Of Unidad_Medida)()

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_UNIDAD_MEDIDA.SP_LISTAR_UNIDAD_MEDIDA", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        While dr.Read()
                            lista.Add(MapearUnidadMedida(dr))
                        End While
                    End Using
                End Using
            End Using

            Return lista
        End Function

        Public Function Buscar(ByVal criterio As String, ByVal valor As String) As List(Of Unidad_Medida)
            Dim lista As New List(Of Unidad_Medida)()

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_UNIDAD_MEDIDA.SP_BUSCAR_UNIDAD_MEDIDA", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_CRITERIO", OracleDbType.Varchar2).Value = criterio
                    cmd.Parameters.Add("P_VALOR", OracleDbType.Varchar2).Value = valor
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        While dr.Read()
                            lista.Add(MapearUnidadMedida(dr))
                        End While
                    End Using
                End Using
            End Using

            Return lista
        End Function

        Private Function MapearUnidadMedida(ByVal dr As OracleDataReader) As Unidad_Medida
            Dim entidad As New Unidad_Medida()

            entidad.UnidadMedidaId = Convert.ToInt32(dr("UNIDAD_MEDIDA_ID"))
            entidad.Codigo = dr("CODIGO").ToString()
            entidad.Nombre = dr("NOMBRE").ToString()
            entidad.CreatedAt = If(IsDBNull(dr("CREATED_AT")), CType(Nothing, DateTime?), Convert.ToDateTime(dr("CREATED_AT")))
            entidad.UpdatedAt = If(IsDBNull(dr("UPDATED_AT")), CType(Nothing, DateTime?), Convert.ToDateTime(dr("UPDATED_AT")))
            entidad.Estado = If(IsDBNull(dr("ESTADO")), Nothing, dr("ESTADO").ToString())

            Return entidad
        End Function

    End Class
End Namespace