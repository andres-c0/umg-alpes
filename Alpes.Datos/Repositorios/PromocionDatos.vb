Option Strict On
Option Explicit On

Imports System
Imports System.Collections.Generic
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
            Dim idGenerado As Integer = 0

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_PROMOCION.SP_INSERTAR_PROMOCION", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_TIPO_PROMOCION_ID", OracleDbType.Int32).Value = entidad.TipoPromocionId
                    cmd.Parameters.Add("P_NOMBRE", OracleDbType.Varchar2).Value = entidad.Nombre

                    If String.IsNullOrWhiteSpace(entidad.Descripcion) Then
                        cmd.Parameters.Add("P_DESCRIPCION", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("P_DESCRIPCION", OracleDbType.Varchar2).Value = entidad.Descripcion
                    End If

                    cmd.Parameters.Add("P_VIGENCIA_INICIO", OracleDbType.Date).Value = entidad.VigenciaInicio
                    cmd.Parameters.Add("P_VIGENCIA_FIN", OracleDbType.Date).Value = entidad.VigenciaFin
                    cmd.Parameters.Add("P_PRIORIDAD", OracleDbType.Int32).Value = entidad.Prioridad

                    cmd.Parameters.Add("P_PROMOCION_ID", OracleDbType.Int32).Direction = ParameterDirection.Output

                    cmd.ExecuteNonQuery()

                    idGenerado = Convert.ToInt32(cmd.Parameters("P_PROMOCION_ID").Value.ToString())
                End Using
            End Using

            Return idGenerado
        End Function

        Public Sub Actualizar(ByVal entidad As Promocion)
            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_PROMOCION.SP_ACTUALIZAR_PROMOCION", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_PROMOCION_ID", OracleDbType.Int32).Value = entidad.PromocionId
                    cmd.Parameters.Add("P_TIPO_PROMOCION_ID", OracleDbType.Int32).Value = entidad.TipoPromocionId
                    cmd.Parameters.Add("P_NOMBRE", OracleDbType.Varchar2).Value = entidad.Nombre

                    If String.IsNullOrWhiteSpace(entidad.Descripcion) Then
                        cmd.Parameters.Add("P_DESCRIPCION", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("P_DESCRIPCION", OracleDbType.Varchar2).Value = entidad.Descripcion
                    End If

                    cmd.Parameters.Add("P_VIGENCIA_INICIO", OracleDbType.Date).Value = entidad.VigenciaInicio
                    cmd.Parameters.Add("P_VIGENCIA_FIN", OracleDbType.Date).Value = entidad.VigenciaFin
                    cmd.Parameters.Add("P_PRIORIDAD", OracleDbType.Int32).Value = entidad.Prioridad

                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Sub Eliminar(ByVal id As Integer)
            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_PROMOCION.SP_ELIMINAR_PROMOCION", cn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_PROMOCION_ID", OracleDbType.Int32).Value = id
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Function ObtenerPorId(ByVal id As Integer) As Promocion
            Dim entidad As Promocion = Nothing

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_PROMOCION.SP_OBTENER_PROMOCION", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_PROMOCION_ID", OracleDbType.Int32).Value = id
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        If dr.Read() Then
                            entidad = MapearPromocion(dr)
                        End If
                    End Using
                End Using
            End Using

            Return entidad
        End Function

        Public Function Listar() As List(Of Promocion)
            Dim lista As New List(Of Promocion)()

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_PROMOCION.SP_LISTAR_PROMOCIONES", cn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        While dr.Read()
                            lista.Add(MapearPromocion(dr))
                        End While
                    End Using
                End Using
            End Using

            Return lista
        End Function

        Public Function Buscar(ByVal criterio As String, ByVal valor As String) As List(Of Promocion)
            Dim lista As New List(Of Promocion)()

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_PROMOCION.SP_BUSCAR_PROMOCIONES", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_CRITERIO", OracleDbType.Varchar2).Value = criterio
                    cmd.Parameters.Add("P_VALOR", OracleDbType.Varchar2).Value = valor
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        While dr.Read()
                            lista.Add(MapearPromocion(dr))
                        End While
                    End Using
                End Using
            End Using

            Return lista
        End Function

        Private Function MapearPromocion(ByVal dr As OracleDataReader) As Promocion
            Dim entidad As New Promocion()

            entidad.PromocionId = LeerEntero(dr, "PROMOCION_ID")
            entidad.TipoPromocionId = LeerEnteroNullable(dr, "TIPO_PROMOCION_ID")
            entidad.TipoPromocion = LeerTexto(dr, "TIPO_PROMOCION")
            entidad.Nombre = LeerTexto(dr, "NOMBRE")
            entidad.Descripcion = LeerTexto(dr, "DESCRIPCION")
            entidad.VigenciaInicio = LeerFecha(dr, "VIGENCIA_INICIO")
            entidad.VigenciaFin = LeerFecha(dr, "VIGENCIA_FIN")
            entidad.Prioridad = LeerEntero(dr, "PRIORIDAD")
            entidad.CreatedAt = LeerFechaNullable(dr, "CREATED_AT")
            entidad.UpdatedAt = LeerFechaNullable(dr, "UPDATED_AT")
            entidad.Estado = LeerTexto(dr, "ESTADO")

            Return entidad
        End Function

        Private Function LeerTexto(ByVal dr As OracleDataReader, ByVal columna As String) As String
            If TieneColumna(dr, columna) AndAlso Not dr.IsDBNull(dr.GetOrdinal(columna)) Then
                Return dr(columna).ToString()
            End If

            Return String.Empty
        End Function

        Private Function LeerEntero(ByVal dr As OracleDataReader, ByVal columna As String) As Integer
            If TieneColumna(dr, columna) AndAlso Not dr.IsDBNull(dr.GetOrdinal(columna)) Then
                Return Convert.ToInt32(dr(columna))
            End If

            Return 0
        End Function

        Private Function LeerEnteroNullable(ByVal dr As OracleDataReader, ByVal columna As String) As Integer?
            If TieneColumna(dr, columna) AndAlso Not dr.IsDBNull(dr.GetOrdinal(columna)) Then
                Return Convert.ToInt32(dr(columna))
            End If

            Return Nothing
        End Function

        Private Function LeerFecha(ByVal dr As OracleDataReader, ByVal columna As String) As DateTime
            If TieneColumna(dr, columna) AndAlso Not dr.IsDBNull(dr.GetOrdinal(columna)) Then
                Return Convert.ToDateTime(dr(columna))
            End If

            Return DateTime.MinValue
        End Function

        Private Function LeerFechaNullable(ByVal dr As OracleDataReader, ByVal columna As String) As DateTime?
            If TieneColumna(dr, columna) AndAlso Not dr.IsDBNull(dr.GetOrdinal(columna)) Then
                Return Convert.ToDateTime(dr(columna))
            End If

            Return Nothing
        End Function

        Private Function TieneColumna(ByVal dr As OracleDataReader, ByVal nombreColumna As String) As Boolean
            For i As Integer = 0 To dr.FieldCount - 1
                If dr.GetName(i).ToUpper() = nombreColumna.ToUpper() Then
                    Return True
                End If
            Next

            Return False
        End Function

    End Class
End Namespace