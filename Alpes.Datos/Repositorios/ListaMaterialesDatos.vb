Option Strict On
Option Explicit On

Imports System.Collections.Generic
Imports System.Data
Imports Oracle.ManagedDataAccess.Client
Imports Alpes.Datos.Conexion
Imports Alpes.Entidades.Produccion

Namespace Repositorios
    Public Class ListaMaterialesDatos

        Private ReadOnly _conexionOracle As ConexionOracle

        Public Sub New()
            _conexionOracle = New ConexionOracle()
        End Sub

        Public Function Insertar(ByVal entidad As ListaMateriales) As Integer
            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using transaccion As OracleTransaction = conexion.BeginTransaction()
                    Using cmd As New OracleCommand("PKG_LISTA_MATERIALES.SP_INSERTAR", conexion)
                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Transaction = transaccion

                        cmd.Parameters.Add("P_PRODUCTO_ID", OracleDbType.Int32).Value = entidad.ProductoId

                        Dim pVersion As New OracleParameter("P_VERSION", OracleDbType.Varchar2)
                        pVersion.Value = If(String.IsNullOrWhiteSpace(entidad.Version), CType(DBNull.Value, Object), entidad.Version)
                        cmd.Parameters.Add(pVersion)

                        Dim pVigenciaInicio As New OracleParameter("P_VIGENCIA_INICIO", OracleDbType.Date)
                        pVigenciaInicio.Value = If(entidad.VigenciaInicio.HasValue, CType(entidad.VigenciaInicio.Value, Object), DBNull.Value)
                        cmd.Parameters.Add(pVigenciaInicio)

                        Dim pVigenciaFin As New OracleParameter("P_VIGENCIA_FIN", OracleDbType.Date)
                        pVigenciaFin.Value = If(entidad.VigenciaFin.HasValue, CType(entidad.VigenciaFin.Value, Object), DBNull.Value)
                        cmd.Parameters.Add(pVigenciaFin)

                        Dim pEstado As New OracleParameter("P_ESTADO", OracleDbType.Varchar2)
                        pEstado.Value = If(String.IsNullOrWhiteSpace(entidad.Estado), CType(DBNull.Value, Object), entidad.Estado)
                        cmd.Parameters.Add(pEstado)

                        Dim pId As New OracleParameter("P_ID", OracleDbType.Int32)
                        pId.Direction = ParameterDirection.Output
                        cmd.Parameters.Add(pId)

                        cmd.ExecuteNonQuery()
                        transaccion.Commit()

                        Return Convert.ToInt32(pId.Value.ToString())
                    End Using
                End Using
            End Using
        End Function

        Public Function Actualizar(ByVal entidad As ListaMateriales) As Boolean
            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using transaccion As OracleTransaction = conexion.BeginTransaction()
                    Using cmd As New OracleCommand("PKG_LISTA_MATERIALES.SP_ACTUALIZAR", conexion)
                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Transaction = transaccion

                        cmd.Parameters.Add("P_LISTA_MATERIALES_ID", OracleDbType.Int32).Value = entidad.ListaMaterialesId
                        cmd.Parameters.Add("P_PRODUCTO_ID", OracleDbType.Int32).Value = entidad.ProductoId

                        Dim pVersion As New OracleParameter("P_VERSION", OracleDbType.Varchar2)
                        pVersion.Value = If(String.IsNullOrWhiteSpace(entidad.Version), CType(DBNull.Value, Object), entidad.Version)
                        cmd.Parameters.Add(pVersion)

                        Dim pVigenciaInicio As New OracleParameter("P_VIGENCIA_INICIO", OracleDbType.Date)
                        pVigenciaInicio.Value = If(entidad.VigenciaInicio.HasValue, CType(entidad.VigenciaInicio.Value, Object), DBNull.Value)
                        cmd.Parameters.Add(pVigenciaInicio)

                        Dim pVigenciaFin As New OracleParameter("P_VIGENCIA_FIN", OracleDbType.Date)
                        pVigenciaFin.Value = If(entidad.VigenciaFin.HasValue, CType(entidad.VigenciaFin.Value, Object), DBNull.Value)
                        cmd.Parameters.Add(pVigenciaFin)

                        Dim pEstado As New OracleParameter("P_ESTADO", OracleDbType.Varchar2)
                        pEstado.Value = If(String.IsNullOrWhiteSpace(entidad.Estado), CType(DBNull.Value, Object), entidad.Estado)
                        cmd.Parameters.Add(pEstado)

                        cmd.ExecuteNonQuery()
                        transaccion.Commit()

                        Return True
                    End Using
                End Using
            End Using
        End Function

        Public Function Eliminar(ByVal listaMaterialesId As Integer) As Boolean
            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using transaccion As OracleTransaction = conexion.BeginTransaction()
                    Using cmd As New OracleCommand("PKG_LISTA_MATERIALES.SP_ELIMINAR", conexion)
                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Transaction = transaccion

                        cmd.Parameters.Add("P_ID", OracleDbType.Int32).Value = listaMaterialesId

                        cmd.ExecuteNonQuery()
                        transaccion.Commit()

                        Return True
                    End Using
                End Using
            End Using
        End Function

        Public Function ObtenerPorId(ByVal listaMaterialesId As Integer) As ListaMateriales
            Dim entidad As ListaMateriales = Nothing

            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using cmd As New OracleCommand("PKG_LISTA_MATERIALES.SP_OBTENER", conexion)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_ID", OracleDbType.Int32).Value = listaMaterialesId
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        If dr.Read() Then
                            entidad = MapearListaMateriales(dr)
                        End If
                    End Using
                End Using
            End Using

            Return entidad
        End Function

        Public Function Listar() As List(Of ListaMateriales)
            Dim lista As New List(Of ListaMateriales)()

            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using cmd As New OracleCommand("PKG_LISTA_MATERIALES.SP_LISTAR", conexion)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        While dr.Read()
                            lista.Add(MapearListaMateriales(dr))
                        End While
                    End Using
                End Using
            End Using

            Return lista
        End Function

        Public Function Buscar(ByVal valor As String) As List(Of ListaMateriales)
            Dim lista As New List(Of ListaMateriales)()

            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using cmd As New OracleCommand("PKG_LISTA_MATERIALES.SP_BUSCAR", conexion)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_VALOR", OracleDbType.Varchar2).Value = valor
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        While dr.Read()
                            lista.Add(MapearListaMateriales(dr))
                        End While
                    End Using
                End Using
            End Using

            Return lista
        End Function

        Private Function MapearListaMateriales(ByVal dr As OracleDataReader) As ListaMateriales
            Dim entidad As New ListaMateriales()

            entidad.ListaMaterialesId = Convert.ToInt32(dr("LISTA_MATERIALES_ID"))
            entidad.ProductoId = Convert.ToInt32(dr("PRODUCTO_ID"))
            entidad.Version = If(dr("VERSION") Is DBNull.Value, String.Empty, dr("VERSION").ToString())

            If dr("VIGENCIA_INICIO") Is DBNull.Value Then
                entidad.VigenciaInicio = Nothing
            Else
                entidad.VigenciaInicio = Convert.ToDateTime(dr("VIGENCIA_INICIO"))
            End If

            If dr("VIGENCIA_FIN") Is DBNull.Value Then
                entidad.VigenciaFin = Nothing
            Else
                entidad.VigenciaFin = Convert.ToDateTime(dr("VIGENCIA_FIN"))
            End If

            If dr("CREATED_AT") Is DBNull.Value Then
                entidad.CreatedAt = Nothing
            Else
                entidad.CreatedAt = Convert.ToDateTime(dr("CREATED_AT"))
            End If

            If dr("UPDATED_AT") Is DBNull.Value Then
                entidad.UpdatedAt = Nothing
            Else
                entidad.UpdatedAt = Convert.ToDateTime(dr("UPDATED_AT"))
            End If

            entidad.Estado = If(dr("ESTADO") Is DBNull.Value, String.Empty, dr("ESTADO").ToString())

            Return entidad
        End Function

    End Class
End Namespace