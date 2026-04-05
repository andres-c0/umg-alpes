Option Strict On
Option Explicit On

Imports System.Data
Imports Oracle.ManagedDataAccess.Client
Imports Alpes.Datos.Conexion
Imports Alpes.Entidades.Ventas

Namespace Repositorios
    Public Class PromocionDatos

        Private ReadOnly _conexionOracle As ConexionOracle

        Public Sub New()
            _conexionOracle = New ConexionOracle()
        End Sub

        Public Function Insertar(ByVal entidad As Promocion) As Integer
            Using conn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_PROMOCION.SP_INSERTAR_PROMOCION", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_TIPO_PROMOCION_ID", entidad.tipo_promocion_id)
                    cmd.Parameters.Add("P_NOMBRE", entidad.nombre)
                    cmd.Parameters.Add("P_DESCRIPCION", entidad.descripcion)
                    cmd.Parameters.Add("P_VIGENCIA_INICIO", entidad.vigencia_inicio)
                    cmd.Parameters.Add("P_VIGENCIA_FIN", If(entidad.vigencia_fin.HasValue, CType(entidad.vigencia_fin.Value, Object), DBNull.Value))
                    cmd.Parameters.Add("P_PRIORIDAD", entidad.prioridad)

                    cmd.Parameters.Add("P_PROMOCION_ID", OracleDbType.Int32).Direction = ParameterDirection.Output

                    conn.Open()
                    cmd.ExecuteNonQuery()

                    Return Convert.ToInt32(cmd.Parameters("P_PROMOCION_ID").Value)
                End Using
            End Using
        End Function

        Public Sub Actualizar(ByVal entidad As Promocion)
            Using conn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_PROMOCION.SP_ACTUALIZAR_PROMOCION", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_PROMOCION_ID", entidad.promocion_id)
                    cmd.Parameters.Add("P_TIPO_PROMOCION_ID", entidad.tipo_promocion_id)
                    cmd.Parameters.Add("P_NOMBRE", entidad.nombre)
                    cmd.Parameters.Add("P_DESCRIPCION", entidad.descripcion)
                    cmd.Parameters.Add("P_VIGENCIA_INICIO", entidad.vigencia_inicio)
                    cmd.Parameters.Add("P_VIGENCIA_FIN", If(entidad.vigencia_fin.HasValue, CType(entidad.vigencia_fin.Value, Object), DBNull.Value))
                    cmd.Parameters.Add("P_PRIORIDAD", entidad.prioridad)

                    conn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Sub Eliminar(ByVal id As Integer)
            Using conn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_PROMOCION.SP_ELIMINAR_PROMOCION", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_PROMOCION_ID", id)

                    conn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Function ObtenerPorId(ByVal id As Integer) As Promocion
            Using conn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_PROMOCION.SP_OBTENER_PROMOCION", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_PROMOCION_ID", id)
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    conn.Open()

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        If dr.Read() Then
                            Return Mapear(dr)
                        End If
                    End Using
                End Using
            End Using

            Return Nothing
        End Function

        Public Function Listar() As List(Of Promocion)
            Dim lista As New List(Of Promocion)()

            Using conn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_PROMOCION.SP_LISTAR_PROMOCIONES", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    conn.Open()

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        While dr.Read()
                            lista.Add(Mapear(dr))
                        End While
                    End Using
                End Using
            End Using

            Return lista
        End Function

        Public Function Buscar(ByVal criterio As String, ByVal valor As String) As List(Of Promocion)
            Dim lista As New List(Of Promocion)()

            Using conn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_PROMOCION.SP_BUSCAR_PROMOCIONES", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_CRITERIO", criterio)
                    cmd.Parameters.Add("P_VALOR", valor)
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    conn.Open()

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        While dr.Read()
                            lista.Add(Mapear(dr))
                        End While
                    End Using
                End Using
            End Using

            Return lista
        End Function

        Private Function Mapear(ByVal dr As OracleDataReader) As Promocion
            Dim entidad As New Promocion()

            If TieneColumna(dr, "PROMOCION_ID") AndAlso Not IsDBNull(dr("PROMOCION_ID")) Then
                entidad.promocion_id = Convert.ToInt32(dr("PROMOCION_ID"))
            End If

            If TieneColumna(dr, "TIPO_PROMOCION_ID") AndAlso Not IsDBNull(dr("TIPO_PROMOCION_ID")) Then
                entidad.tipo_promocion_id = Convert.ToInt32(dr("TIPO_PROMOCION_ID"))
            End If

            If TieneColumna(dr, "TIPO_PROMOCION") AndAlso Not IsDBNull(dr("TIPO_PROMOCION")) Then
                entidad.tipo_promocion = dr("TIPO_PROMOCION").ToString()
            End If

            If TieneColumna(dr, "NOMBRE") AndAlso Not IsDBNull(dr("NOMBRE")) Then
                entidad.nombre = dr("NOMBRE").ToString()
            End If

            If TieneColumna(dr, "DESCRIPCION") AndAlso Not IsDBNull(dr("DESCRIPCION")) Then
                entidad.descripcion = dr("DESCRIPCION").ToString()
            End If

            If TieneColumna(dr, "VIGENCIA_INICIO") AndAlso Not IsDBNull(dr("VIGENCIA_INICIO")) Then
                entidad.vigencia_inicio = Convert.ToDateTime(dr("VIGENCIA_INICIO"))
            End If

            If TieneColumna(dr, "VIGENCIA_FIN") AndAlso Not IsDBNull(dr("VIGENCIA_FIN")) Then
                entidad.vigencia_fin = Convert.ToDateTime(dr("VIGENCIA_FIN"))
            End If

            If TieneColumna(dr, "PRIORIDAD") AndAlso Not IsDBNull(dr("PRIORIDAD")) Then
                entidad.prioridad = Convert.ToInt32(dr("PRIORIDAD"))
            End If

            If TieneColumna(dr, "CREATED_AT") AndAlso Not IsDBNull(dr("CREATED_AT")) Then
                entidad.created_at = Convert.ToDateTime(dr("CREATED_AT"))
            End If

            If TieneColumna(dr, "UPDATED_AT") AndAlso Not IsDBNull(dr("UPDATED_AT")) Then
                entidad.updated_at = Convert.ToDateTime(dr("UPDATED_AT"))
            End If

            If TieneColumna(dr, "ESTADO") AndAlso Not IsDBNull(dr("ESTADO")) Then
                entidad.estado = dr("ESTADO").ToString()
            End If

            Return entidad
        End Function

        Private Function TieneColumna(ByVal dr As OracleDataReader, ByVal nombreColumna As String) As Boolean
            For i As Integer = 0 To dr.FieldCount - 1
                If String.Equals(dr.GetName(i), nombreColumna, StringComparison.OrdinalIgnoreCase) Then
                    Return True
                End If
            Next
            Return False
        End Function

    End Class
End Namespace