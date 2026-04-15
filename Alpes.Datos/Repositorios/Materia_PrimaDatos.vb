Option Strict On
Option Explicit On

Imports System
Imports System.Collections.Generic
Imports System.Data
Imports Oracle.ManagedDataAccess.Client
Imports Alpes.Datos.Conexion
Imports Alpes.Entidades.Produccion

Namespace Repositorios
    Public Class Materia_PrimaDatos

        Private ReadOnly _conexionOracle As ConexionOracle

        Public Sub New()
            _conexionOracle = New ConexionOracle()
        End Sub

        Public Function Insertar(ByVal entidad As Materia_Prima) As Integer
            Dim idGenerado As Integer = 0

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_MATERIA_PRIMA.SP_INSERTAR", cn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.BindByName = True

                    cmd.Parameters.Add("P_CODIGO", OracleDbType.Varchar2).Value = entidad.Codigo
                    cmd.Parameters.Add("P_NOMBRE", OracleDbType.Varchar2).Value = entidad.Nombre
                    cmd.Parameters.Add("P_UNIDAD_MEDIDA_ID", OracleDbType.Int32).Value = entidad.UnidadMedidaId

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

        Public Sub Actualizar(ByVal entidad As Materia_Prima)
            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_MATERIA_PRIMA.SP_ACTUALIZAR", cn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.BindByName = True

                    cmd.Parameters.Add("P_MP_ID", OracleDbType.Int32).Value = entidad.MpId
                    cmd.Parameters.Add("P_CODIGO", OracleDbType.Varchar2).Value = entidad.Codigo
                    cmd.Parameters.Add("P_NOMBRE", OracleDbType.Varchar2).Value = entidad.Nombre
                    cmd.Parameters.Add("P_UNIDAD_MEDIDA_ID", OracleDbType.Int32).Value = entidad.UnidadMedidaId

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

                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Sub Eliminar(ByVal id As Integer)
            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_MATERIA_PRIMA.SP_ELIMINAR", cn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.BindByName = True

                    cmd.Parameters.Add("P_ID", OracleDbType.Int32).Value = id
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Function ObtenerPorId(ByVal id As Integer) As Materia_Prima
            Dim entidad As Materia_Prima = Nothing

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_MATERIA_PRIMA.SP_OBTENER", cn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.BindByName = True

                    cmd.Parameters.Add("P_ID", OracleDbType.Int32).Value = id
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        If dr.Read() Then
                            entidad = MapearMateriaPrima(dr)
                        End If
                    End Using
                End Using
            End Using

            Return entidad
        End Function

        Public Function Listar() As List(Of Materia_Prima)
            Dim lista As New List(Of Materia_Prima)()

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_MATERIA_PRIMA.SP_LISTAR", cn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.BindByName = True

                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        While dr.Read()
                            lista.Add(MapearMateriaPrima(dr))
                        End While
                    End Using
                End Using
            End Using

            Return lista
        End Function

        Public Function Buscar(ByVal valor As String) As List(Of Materia_Prima)
            Dim lista As New List(Of Materia_Prima)()

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_MATERIA_PRIMA.SP_BUSCAR", cn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.BindByName = True

                    cmd.Parameters.Add("P_VALOR", OracleDbType.Varchar2).Value = valor
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        While dr.Read()
                            lista.Add(MapearMateriaPrima(dr))
                        End While
                    End Using
                End Using
            End Using

            Return lista
        End Function

        Private Function MapearMateriaPrima(ByVal dr As OracleDataReader) As Materia_Prima
            Dim entidad As New Materia_Prima()

            If TieneColumna(dr, "MP_ID") AndAlso Not IsDBNull(dr("MP_ID")) Then
                entidad.MpId = Convert.ToInt32(dr("MP_ID"))
            End If

            If TieneColumna(dr, "CODIGO") AndAlso Not IsDBNull(dr("CODIGO")) Then
                entidad.Codigo = dr("CODIGO").ToString()
            Else
                entidad.Codigo = String.Empty
            End If

            If TieneColumna(dr, "NOMBRE") AndAlso Not IsDBNull(dr("NOMBRE")) Then
                entidad.Nombre = dr("NOMBRE").ToString()
            Else
                entidad.Nombre = String.Empty
            End If

            If TieneColumna(dr, "UNIDAD_MEDIDA_ID") AndAlso Not IsDBNull(dr("UNIDAD_MEDIDA_ID")) Then
                entidad.UnidadMedidaId = Convert.ToInt32(dr("UNIDAD_MEDIDA_ID"))
            End If

            If TieneColumna(dr, "DESCRIPCION") AndAlso Not IsDBNull(dr("DESCRIPCION")) Then
                entidad.Descripcion = dr("DESCRIPCION").ToString()
            Else
                entidad.Descripcion = String.Empty
            End If

            If TieneColumna(dr, "CREATED_AT") AndAlso Not IsDBNull(dr("CREATED_AT")) Then
                entidad.CreatedAt = Convert.ToDateTime(dr("CREATED_AT"))
            Else
                entidad.CreatedAt = Nothing
            End If

            If TieneColumna(dr, "UPDATED_AT") AndAlso Not IsDBNull(dr("UPDATED_AT")) Then
                entidad.UpdatedAt = Convert.ToDateTime(dr("UPDATED_AT"))
            Else
                entidad.UpdatedAt = Nothing
            End If

            If TieneColumna(dr, "ESTADO") AndAlso Not IsDBNull(dr("ESTADO")) Then
                entidad.Estado = dr("ESTADO").ToString()
            Else
                entidad.Estado = String.Empty
            End If

            Return entidad
        End Function

        Private Function TieneColumna(ByVal dr As IDataRecord, ByVal nombreColumna As String) As Boolean
            For i As Integer = 0 To dr.FieldCount - 1
                If String.Equals(dr.GetName(i), nombreColumna, StringComparison.OrdinalIgnoreCase) Then
                    Return True
                End If
            Next

            Return False
        End Function

    End Class
End Namespace