Option Strict On
Option Explicit On

Imports System.Collections.Generic
Imports System.Data
Imports Oracle.ManagedDataAccess.Client
Imports Alpes.Datos.Conexion
Imports Alpes.Entidades.Compras

Namespace Repositorios
    Public Class HistoricoPrecioMateriaPrimaProveedorDatos

        Private ReadOnly _conexionOracle As ConexionOracle

        Public Sub New()
            _conexionOracle = New ConexionOracle()
        End Sub

        Public Function Insertar(ByVal entidad As HistoricoPrecioMateriaPrimaProveedor) As Boolean
            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using cmd As New OracleCommand("PKG_HISTORIAL_PRECIO_MP_PROV.SP_INSERTAR_HISTORIAL", conexion)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_PROV_ID", OracleDbType.Int32).Value = entidad.ProvId
                    cmd.Parameters.Add("P_MP_ID", OracleDbType.Int32).Value = entidad.MpId
                    cmd.Parameters.Add("P_PRECIO", OracleDbType.Decimal).Value = entidad.Precio
                    cmd.Parameters.Add("P_VIG_INICIO", OracleDbType.Date).Value = entidad.VigenciaInicio

                    Dim pVigFin As New OracleParameter("P_VIG_FIN", OracleDbType.Date)
                    If entidad.VigenciaFin.HasValue Then
                        pVigFin.Value = entidad.VigenciaFin.Value
                    Else
                        pVigFin.Value = DBNull.Value
                    End If
                    cmd.Parameters.Add(pVigFin)

                    cmd.ExecuteNonQuery()
                    Return True
                End Using
            End Using
        End Function

        Public Function Eliminar(ByVal histMpProvId As Integer) As Boolean
            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using cmd As New OracleCommand("PKG_HISTORIAL_PRECIO_MP_PROV.SP_ELIMINAR_HISTORIAL", conexion)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_HIST_ID", OracleDbType.Int32).Value = histMpProvId

                    cmd.ExecuteNonQuery()
                    Return True
                End Using
            End Using
        End Function

        Public Function Listar() As List(Of HistoricoPrecioMateriaPrimaProveedor)
            Dim lista As New List(Of HistoricoPrecioMateriaPrimaProveedor)()

            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using cmd As New OracleCommand("PKG_HISTORIAL_PRECIO_MP_PROV.SP_LISTAR_HISTORIAL", conexion)
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

        Public Function ListarPorProveedor(ByVal provId As Integer) As List(Of HistoricoPrecioMateriaPrimaProveedor)
            Dim lista As New List(Of HistoricoPrecioMateriaPrimaProveedor)()

            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using cmd As New OracleCommand("PKG_HISTORIAL_PRECIO_MP_PROV.SP_LISTAR_POR_PROVEEDOR", conexion)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_PROV_ID", OracleDbType.Int32).Value = provId
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

        Private Function Mapear(ByVal dr As OracleDataReader) As HistoricoPrecioMateriaPrimaProveedor
            Dim entidad As New HistoricoPrecioMateriaPrimaProveedor()

            entidad.HistMpProvId = Convert.ToInt32(dr("HIST_MP_PROV_ID"))
            entidad.ProvId = Convert.ToInt32(dr("PROV_ID"))
            entidad.MpId = Convert.ToInt32(dr("MP_ID"))
            entidad.Precio = Convert.ToDecimal(dr("PRECIO"))
            entidad.VigenciaInicio = Convert.ToDateTime(dr("VIGENCIA_INICIO"))

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