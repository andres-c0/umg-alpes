Option Strict On
Option Explicit On

Imports System.Collections.Generic
Imports System.Data
Imports Oracle.ManagedDataAccess.Client
Imports Alpes.Datos.Conexion
Imports Alpes.Entidades.Compras

Namespace Repositorios
    Public Class ExpedienteProveedorDatos

        Private ReadOnly _conexionOracle As ConexionOracle

        Public Sub New()
            _conexionOracle = New ConexionOracle()
        End Sub

        Public Function Insertar(ByVal entidad As ExpedienteProveedor) As Integer
            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using transaccion As OracleTransaction = conexion.BeginTransaction()
                    Using cmd As New OracleCommand("PKG_EXPEDIENTE_PROVEEDOR.SP_INSERTAR_EXPEDIENTE_PROVEEDOR", conexion)
                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Transaction = transaccion

                        cmd.Parameters.Add("P_PROV_ID", OracleDbType.Int32).Value = entidad.ProvId
                        cmd.Parameters.Add("P_TIPO_DOCUMENTO", OracleDbType.Varchar2).Value = entidad.TipoDocumento
                        cmd.Parameters.Add("P_URL_DOCUMENTO", OracleDbType.Varchar2).Value = entidad.UrlDocumento

                        Dim pFechaDocumento As New OracleParameter("P_FECHA_DOCUMENTO", OracleDbType.Date)
                        If entidad.FechaDocumento.HasValue Then
                            pFechaDocumento.Value = entidad.FechaDocumento.Value
                        Else
                            pFechaDocumento.Value = DBNull.Value
                        End If
                        cmd.Parameters.Add(pFechaDocumento)

                        Dim pExpedienteProveedorId As New OracleParameter("P_EXPEDIENTE_PROVEEDOR_ID", OracleDbType.Int32)
                        pExpedienteProveedorId.Direction = ParameterDirection.Output
                        cmd.Parameters.Add(pExpedienteProveedorId)

                        cmd.ExecuteNonQuery()
                        transaccion.Commit()

                        Return Convert.ToInt32(pExpedienteProveedorId.Value.ToString())
                    End Using
                End Using
            End Using
        End Function

        Public Function Actualizar(ByVal entidad As ExpedienteProveedor) As Boolean
            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using transaccion As OracleTransaction = conexion.BeginTransaction()
                    Using cmd As New OracleCommand("PKG_EXPEDIENTE_PROVEEDOR.SP_ACTUALIZAR_EXPEDIENTE_PROVEEDOR", conexion)
                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Transaction = transaccion

                        cmd.Parameters.Add("P_EXPEDIENTE_PROVEEDOR_ID", OracleDbType.Int32).Value = entidad.ExpedienteProveedorId
                        cmd.Parameters.Add("P_PROV_ID", OracleDbType.Int32).Value = entidad.ProvId
                        cmd.Parameters.Add("P_TIPO_DOCUMENTO", OracleDbType.Varchar2).Value = entidad.TipoDocumento
                        cmd.Parameters.Add("P_URL_DOCUMENTO", OracleDbType.Varchar2).Value = entidad.UrlDocumento

                        Dim pFechaDocumento As New OracleParameter("P_FECHA_DOCUMENTO", OracleDbType.Date)
                        If entidad.FechaDocumento.HasValue Then
                            pFechaDocumento.Value = entidad.FechaDocumento.Value
                        Else
                            pFechaDocumento.Value = DBNull.Value
                        End If
                        cmd.Parameters.Add(pFechaDocumento)

                        cmd.ExecuteNonQuery()
                        transaccion.Commit()

                        Return True
                    End Using
                End Using
            End Using
        End Function

        Public Function Eliminar(ByVal expedienteProveedorId As Integer) As Boolean
            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using transaccion As OracleTransaction = conexion.BeginTransaction()
                    Using cmd As New OracleCommand("PKG_EXPEDIENTE_PROVEEDOR.SP_ELIMINAR_EXPEDIENTE_PROVEEDOR", conexion)
                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Transaction = transaccion

                        cmd.Parameters.Add("P_EXPEDIENTE_PROVEEDOR_ID", OracleDbType.Int32).Value = expedienteProveedorId

                        cmd.ExecuteNonQuery()
                        transaccion.Commit()

                        Return True
                    End Using
                End Using
            End Using
        End Function

        Public Function ObtenerPorId(ByVal expedienteProveedorId As Integer) As ExpedienteProveedor
            Dim entidad As ExpedienteProveedor = Nothing

            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using cmd As New OracleCommand("PKG_EXPEDIENTE_PROVEEDOR.SP_OBTENER_EXPEDIENTE_PROVEEDOR", conexion)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_EXPEDIENTE_PROVEEDOR_ID", OracleDbType.Int32).Value = expedienteProveedorId
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        If dr.Read() Then
                            entidad = MapearObtener(dr)
                        End If
                    End Using
                End Using
            End Using

            Return entidad
        End Function

        Public Function Listar() As List(Of ExpedienteProveedor)
            Dim lista As New List(Of ExpedienteProveedor)()

            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using cmd As New OracleCommand("PKG_EXPEDIENTE_PROVEEDOR.SP_LISTAR_EXPEDIENTES_PROVEEDOR", conexion)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        While dr.Read()
                            lista.Add(MapearListado(dr))
                        End While
                    End Using
                End Using
            End Using

            Return lista
        End Function

        Public Function Buscar(ByVal criterio As String, ByVal valor As String) As List(Of ExpedienteProveedor)
            Dim lista As New List(Of ExpedienteProveedor)()

            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using cmd As New OracleCommand("PKG_EXPEDIENTE_PROVEEDOR.SP_BUSCAR_EXPEDIENTES_PROVEEDOR", conexion)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_CRITERIO", OracleDbType.Varchar2).Value = criterio
                    cmd.Parameters.Add("P_VALOR", OracleDbType.Varchar2).Value = valor
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        While dr.Read()
                            lista.Add(MapearListado(dr))
                        End While
                    End Using
                End Using
            End Using

            Return lista
        End Function

        Private Function MapearObtener(ByVal dr As OracleDataReader) As ExpedienteProveedor
            Dim entidad As New ExpedienteProveedor()

            entidad.ExpedienteProveedorId = Convert.ToInt32(dr("EXPEDIENTE_PROVEEDOR_ID"))
            entidad.ProvId = Convert.ToInt32(dr("PROV_ID"))
            entidad.TipoDocumento = dr("TIPO_DOCUMENTO").ToString()
            entidad.UrlDocumento = dr("URL_DOCUMENTO").ToString()

            If dr("FECHA_DOCUMENTO") Is DBNull.Value Then
                entidad.FechaDocumento = Nothing
            Else
                entidad.FechaDocumento = Convert.ToDateTime(dr("FECHA_DOCUMENTO"))
            End If

            entidad.Estado = If(dr("ESTADO") Is DBNull.Value, String.Empty, dr("ESTADO").ToString())

            Return entidad
        End Function

        Private Function MapearListado(ByVal dr As OracleDataReader) As ExpedienteProveedor
            Dim entidad As New ExpedienteProveedor()

            entidad.ExpedienteProveedorId = Convert.ToInt32(dr("EXPEDIENTE_PROVEEDOR_ID"))

            If TieneColumna(dr, "PROV_ID") AndAlso dr("PROV_ID") IsNot DBNull.Value Then
                entidad.ProvId = Convert.ToInt32(dr("PROV_ID"))
            End If

            If TieneColumna(dr, "TIPO_DOCUMENTO") AndAlso dr("TIPO_DOCUMENTO") IsNot DBNull.Value Then
                entidad.TipoDocumento = dr("TIPO_DOCUMENTO").ToString()
            Else
                entidad.TipoDocumento = String.Empty
            End If

            If TieneColumna(dr, "URL_DOCUMENTO") AndAlso dr("URL_DOCUMENTO") IsNot DBNull.Value Then
                entidad.UrlDocumento = dr("URL_DOCUMENTO").ToString()
            Else
                entidad.UrlDocumento = String.Empty
            End If

            If TieneColumna(dr, "FECHA_DOCUMENTO") AndAlso dr("FECHA_DOCUMENTO") IsNot DBNull.Value Then
                entidad.FechaDocumento = Convert.ToDateTime(dr("FECHA_DOCUMENTO"))
            Else
                entidad.FechaDocumento = Nothing
            End If

            If TieneColumna(dr, "ESTADO") AndAlso dr("ESTADO") IsNot DBNull.Value Then
                entidad.Estado = dr("ESTADO").ToString()
            Else
                entidad.Estado = String.Empty
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