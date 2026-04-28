Option Strict On
Option Explicit On

Imports System.Collections.Generic
Imports System.Data
Imports Oracle.ManagedDataAccess.Client
Imports Alpes.Datos.Conexion
Imports Alpes.Entidades.Compras

Namespace Repositorios
    Public Class ContratoProveedorDatos

        Private ReadOnly _conexionOracle As ConexionOracle

        Public Sub New()
            _conexionOracle = New ConexionOracle()
        End Sub

        Public Function Insertar(ByVal entidad As ContratoProveedor) As Integer
            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using transaccion As OracleTransaction = conexion.BeginTransaction()
                    Using cmd As New OracleCommand("PKG_CONTRATO_PROVEEDOR.SP_INSERTAR_CONTRATO_PROVEEDOR", conexion)
                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Transaction = transaccion

                        cmd.Parameters.Add("P_PROV_ID", OracleDbType.Int32).Value = entidad.ProvId
                        cmd.Parameters.Add("P_TITULO", OracleDbType.Varchar2).Value = entidad.Titulo

                        Dim pVigenciaInicio As New OracleParameter("P_VIGENCIA_INICIO", OracleDbType.Date)
                        If entidad.VigenciaInicio.HasValue Then
                            pVigenciaInicio.Value = entidad.VigenciaInicio.Value
                        Else
                            pVigenciaInicio.Value = DBNull.Value
                        End If
                        cmd.Parameters.Add(pVigenciaInicio)

                        Dim pVigenciaFin As New OracleParameter("P_VIGENCIA_FIN", OracleDbType.Date)
                        If entidad.VigenciaFin.HasValue Then
                            pVigenciaFin.Value = entidad.VigenciaFin.Value
                        Else
                            pVigenciaFin.Value = DBNull.Value
                        End If
                        cmd.Parameters.Add(pVigenciaFin)

                        Dim pUrlDocumento As New OracleParameter("P_URL_DOCUMENTO", OracleDbType.Varchar2)
                        If String.IsNullOrWhiteSpace(entidad.UrlDocumento) Then
                            pUrlDocumento.Value = DBNull.Value
                        Else
                            pUrlDocumento.Value = entidad.UrlDocumento
                        End If
                        cmd.Parameters.Add(pUrlDocumento)

                        Dim pContratoProveedorId As New OracleParameter("P_CONTRATO_PROVEEDOR_ID", OracleDbType.Int32)
                        pContratoProveedorId.Direction = ParameterDirection.Output
                        cmd.Parameters.Add(pContratoProveedorId)

                        cmd.ExecuteNonQuery()
                        transaccion.Commit()

                        Return Convert.ToInt32(pContratoProveedorId.Value.ToString())
                    End Using
                End Using
            End Using
        End Function

        Public Function Actualizar(ByVal entidad As ContratoProveedor) As Boolean
            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using transaccion As OracleTransaction = conexion.BeginTransaction()
                    Using cmd As New OracleCommand("PKG_CONTRATO_PROVEEDOR.SP_ACTUALIZAR_CONTRATO_PROVEEDOR", conexion)
                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Transaction = transaccion

                        cmd.Parameters.Add("P_CONTRATO_PROVEEDOR_ID", OracleDbType.Int32).Value = entidad.ContratoProveedorId
                        cmd.Parameters.Add("P_PROV_ID", OracleDbType.Int32).Value = entidad.ProvId
                        cmd.Parameters.Add("P_TITULO", OracleDbType.Varchar2).Value = entidad.Titulo

                        Dim pVigenciaInicio As New OracleParameter("P_VIGENCIA_INICIO", OracleDbType.Date)
                        If entidad.VigenciaInicio.HasValue Then
                            pVigenciaInicio.Value = entidad.VigenciaInicio.Value
                        Else
                            pVigenciaInicio.Value = DBNull.Value
                        End If
                        cmd.Parameters.Add(pVigenciaInicio)

                        Dim pVigenciaFin As New OracleParameter("P_VIGENCIA_FIN", OracleDbType.Date)
                        If entidad.VigenciaFin.HasValue Then
                            pVigenciaFin.Value = entidad.VigenciaFin.Value
                        Else
                            pVigenciaFin.Value = DBNull.Value
                        End If
                        cmd.Parameters.Add(pVigenciaFin)

                        Dim pUrlDocumento As New OracleParameter("P_URL_DOCUMENTO", OracleDbType.Varchar2)
                        If String.IsNullOrWhiteSpace(entidad.UrlDocumento) Then
                            pUrlDocumento.Value = DBNull.Value
                        Else
                            pUrlDocumento.Value = entidad.UrlDocumento
                        End If
                        cmd.Parameters.Add(pUrlDocumento)

                        cmd.ExecuteNonQuery()
                        transaccion.Commit()

                        Return True
                    End Using
                End Using
            End Using
        End Function

        Public Function Eliminar(ByVal contratoProveedorId As Integer) As Boolean
            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using transaccion As OracleTransaction = conexion.BeginTransaction()
                    Using cmd As New OracleCommand("PKG_CONTRATO_PROVEEDOR.SP_ELIMINAR_CONTRATO_PROVEEDOR", conexion)
                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Transaction = transaccion

                        cmd.Parameters.Add("P_CONTRATO_PROVEEDOR_ID", OracleDbType.Int32).Value = contratoProveedorId

                        cmd.ExecuteNonQuery()
                        transaccion.Commit()

                        Return True
                    End Using
                End Using
            End Using
        End Function

        Public Function ObtenerPorId(ByVal contratoProveedorId As Integer) As ContratoProveedor
            Dim entidad As ContratoProveedor = Nothing

            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using cmd As New OracleCommand("PKG_CONTRATO_PROVEEDOR.SP_OBTENER_CONTRATO_PROVEEDOR", conexion)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_CONTRATO_PROVEEDOR_ID", OracleDbType.Int32).Value = contratoProveedorId
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        If dr.Read() Then
                            entidad = Mapear(dr)
                        End If
                    End Using
                End Using
            End Using

            Return entidad
        End Function

        Public Function Listar() As List(Of ContratoProveedor)
            Dim lista As New List(Of ContratoProveedor)()

            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using cmd As New OracleCommand("PKG_CONTRATO_PROVEEDOR.SP_LISTAR_CONTRATOS_PROVEEDOR", conexion)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        While dr.Read()
                            lista.Add(Mapear(dr))
                        End While
                    End Using
                End Using
            End Using

            Return lista
        End Function

        Public Function Buscar(ByVal criterio As String, ByVal valor As String) As List(Of ContratoProveedor)
            Dim lista As New List(Of ContratoProveedor)()

            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using cmd As New OracleCommand("PKG_CONTRATO_PROVEEDOR.SP_BUSCAR_CONTRATOS_PROVEEDOR", conexion)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_CRITERIO", OracleDbType.Varchar2).Value = criterio
                    cmd.Parameters.Add("P_VALOR", OracleDbType.Varchar2).Value = valor
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        While dr.Read()
                            lista.Add(Mapear(dr))
                        End While
                    End Using
                End Using
            End Using

            Return lista
        End Function

        Private Function Mapear(ByVal dr As OracleDataReader) As ContratoProveedor
            Dim entidad As New ContratoProveedor()

            entidad.ContratoProveedorId = Convert.ToInt32(dr("CONTRATO_PROVEEDOR_ID"))

            If TieneColumna(dr, "PROV_ID") AndAlso dr("PROV_ID") IsNot DBNull.Value Then
                entidad.ProvId = Convert.ToInt32(dr("PROV_ID"))
            End If

            entidad.Titulo = dr("TITULO").ToString()

            If TieneColumna(dr, "VIGENCIA_INICIO") AndAlso dr("VIGENCIA_INICIO") IsNot DBNull.Value Then
                entidad.VigenciaInicio = Convert.ToDateTime(dr("VIGENCIA_INICIO"))
            Else
                entidad.VigenciaInicio = Nothing
            End If

            If TieneColumna(dr, "VIGENCIA_FIN") AndAlso dr("VIGENCIA_FIN") IsNot DBNull.Value Then
                entidad.VigenciaFin = Convert.ToDateTime(dr("VIGENCIA_FIN"))
            Else
                entidad.VigenciaFin = Nothing
            End If

            If TieneColumna(dr, "URL_DOCUMENTO") AndAlso dr("URL_DOCUMENTO") IsNot DBNull.Value Then
                entidad.UrlDocumento = dr("URL_DOCUMENTO").ToString()
            Else
                entidad.UrlDocumento = String.Empty
            End If

            entidad.Estado = If(dr("ESTADO") Is DBNull.Value, String.Empty, dr("ESTADO").ToString())

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