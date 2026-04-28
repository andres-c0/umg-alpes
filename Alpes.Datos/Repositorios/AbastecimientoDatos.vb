Option Strict On
Option Explicit On

Imports System.Collections.Generic
Imports System.Data
Imports Oracle.ManagedDataAccess.Client
Imports Alpes.Datos.Conexion
Imports Alpes.Entidades.Compras

Namespace Repositorios
    Public Class AbastecimientoDatos

        Private ReadOnly _conexionOracle As ConexionOracle

        Public Sub New()
            _conexionOracle = New ConexionOracle()
        End Sub

        Public Function Insertar(ByVal entidad As Abastecimiento) As Integer
            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using transaccion As OracleTransaction = conexion.BeginTransaction()
                    Using cmd As New OracleCommand("PKG_ABASTECIMIENTO.SP_INSERTAR_ABASTECIMIENTO", conexion)
                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Transaction = transaccion

                        cmd.Parameters.Add("P_MP_ID", OracleDbType.Int32).Value = entidad.MpId
                        cmd.Parameters.Add("P_CANTIDAD_SUGERIDA", OracleDbType.Decimal).Value = entidad.CantidadSugerida

                        Dim pMotivo As New OracleParameter("P_MOTIVO", OracleDbType.Varchar2)
                        If String.IsNullOrWhiteSpace(entidad.Motivo) Then
                            pMotivo.Value = DBNull.Value
                        Else
                            pMotivo.Value = entidad.Motivo
                        End If
                        cmd.Parameters.Add(pMotivo)

                        Dim pId As New OracleParameter("P_ABASTECIMIENTO_ID", OracleDbType.Int32)
                        pId.Direction = ParameterDirection.Output
                        cmd.Parameters.Add(pId)

                        cmd.ExecuteNonQuery()
                        transaccion.Commit()

                        Return Convert.ToInt32(pId.Value.ToString())
                    End Using
                End Using
            End Using
        End Function

        Public Function Actualizar(ByVal entidad As Abastecimiento) As Boolean
            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using transaccion As OracleTransaction = conexion.BeginTransaction()
                    Using cmd As New OracleCommand("PKG_ABASTECIMIENTO.SP_ACTUALIZAR_ABASTECIMIENTO", conexion)
                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Transaction = transaccion

                        cmd.Parameters.Add("P_ABASTECIMIENTO_ID", OracleDbType.Int32).Value = entidad.AbastecimientoId
                        cmd.Parameters.Add("P_MP_ID", OracleDbType.Int32).Value = entidad.MpId
                        cmd.Parameters.Add("P_CANTIDAD_SUGERIDA", OracleDbType.Decimal).Value = entidad.CantidadSugerida

                        Dim pMotivo As New OracleParameter("P_MOTIVO", OracleDbType.Varchar2)
                        If String.IsNullOrWhiteSpace(entidad.Motivo) Then
                            pMotivo.Value = DBNull.Value
                        Else
                            pMotivo.Value = entidad.Motivo
                        End If
                        cmd.Parameters.Add(pMotivo)

                        cmd.ExecuteNonQuery()
                        transaccion.Commit()

                        Return True
                    End Using
                End Using
            End Using
        End Function

        Public Function Eliminar(ByVal abastecimientoId As Integer) As Boolean
            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using transaccion As OracleTransaction = conexion.BeginTransaction()
                    Using cmd As New OracleCommand("PKG_ABASTECIMIENTO.SP_ELIMINAR_ABASTECIMIENTO", conexion)
                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Transaction = transaccion

                        cmd.Parameters.Add("P_ABASTECIMIENTO_ID", OracleDbType.Int32).Value = abastecimientoId

                        cmd.ExecuteNonQuery()
                        transaccion.Commit()

                        Return True
                    End Using
                End Using
            End Using
        End Function

        Public Function ObtenerPorId(ByVal abastecimientoId As Integer) As Abastecimiento
            Dim entidad As Abastecimiento = Nothing

            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using cmd As New OracleCommand("PKG_ABASTECIMIENTO.SP_OBTENER_ABASTECIMIENTO", conexion)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_ABASTECIMIENTO_ID", OracleDbType.Int32).Value = abastecimientoId
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

        Public Function Listar() As List(Of Abastecimiento)
            Dim lista As New List(Of Abastecimiento)()

            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using cmd As New OracleCommand("PKG_ABASTECIMIENTO.SP_LISTAR_ABASTECIMIENTOS", conexion)
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

        Public Function Buscar(ByVal criterio As String, ByVal valor As String) As List(Of Abastecimiento)
            Dim lista As New List(Of Abastecimiento)()

            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using cmd As New OracleCommand("PKG_ABASTECIMIENTO.SP_BUSCAR_ABASTECIMIENTOS", conexion)
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

        Private Function MapearObtener(ByVal dr As OracleDataReader) As Abastecimiento
            Dim entidad As New Abastecimiento()

            entidad.AbastecimientoId = Convert.ToInt32(dr("ABASTECIMIENTO_ID"))
            entidad.MpId = Convert.ToInt32(dr("MP_ID"))
            entidad.MateriaPrimaNombre = If(dr("MATERIA_PRIMA_NOMBRE") Is DBNull.Value, String.Empty, dr("MATERIA_PRIMA_NOMBRE").ToString())
            entidad.CantidadSugerida = Convert.ToDecimal(dr("CANTIDAD_SUGERIDA"))
            entidad.Motivo = If(dr("MOTIVO") Is DBNull.Value, String.Empty, dr("MOTIVO").ToString())
            entidad.Estado = If(dr("ESTADO") Is DBNull.Value, String.Empty, dr("ESTADO").ToString())

            Return entidad
        End Function

        Private Function MapearListado(ByVal dr As OracleDataReader) As Abastecimiento
            Dim entidad As New Abastecimiento()

            entidad.AbastecimientoId = Convert.ToInt32(dr("ABASTECIMIENTO_ID"))
            entidad.MateriaPrimaNombre = If(dr("MATERIA_PRIMA_NOMBRE") Is DBNull.Value, String.Empty, dr("MATERIA_PRIMA_NOMBRE").ToString())
            entidad.CantidadSugerida = Convert.ToDecimal(dr("CANTIDAD_SUGERIDA"))
            entidad.Motivo = If(dr("MOTIVO") Is DBNull.Value, String.Empty, dr("MOTIVO").ToString())
            entidad.Estado = If(dr("ESTADO") Is DBNull.Value, String.Empty, dr("ESTADO").ToString())

            Return entidad
        End Function

    End Class
End Namespace