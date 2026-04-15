Option Strict On
Option Explicit On

Imports System.Collections.Generic
Imports System.Data
Imports Oracle.ManagedDataAccess.Client
Imports Alpes.Datos.Conexion
Imports Alpes.Entidades.Produccion

Namespace Repositorios
    Public Class ConsumoMateriaPrimaDatos

        Private ReadOnly _conexionOracle As ConexionOracle

        Public Sub New()
            _conexionOracle = New ConexionOracle()
        End Sub

        Public Function Insertar(ByVal entidad As ConsumoMateriaPrima) As Integer
            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using transaccion As OracleTransaction = conexion.BeginTransaction()
                    Using cmd As New OracleCommand("PKG_CONSUMO_MATERIA_PRIMA.SP_INSERTAR_CONSUMO_MATERIA_PRIMA", conexion)
                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Transaction = transaccion

                        cmd.Parameters.Add("P_ORDEN_PRODUCCION_ID", OracleDbType.Int32).Value = entidad.OrdenProduccionId
                        cmd.Parameters.Add("P_MP_ID", OracleDbType.Int32).Value = entidad.MpId
                        cmd.Parameters.Add("P_CANTIDAD", OracleDbType.Decimal).Value = entidad.Cantidad
                        cmd.Parameters.Add("P_CONSUMO_AT", OracleDbType.TimeStamp).Value = entidad.ConsumoAt

                        Dim pEstado As New OracleParameter("P_ESTADO", OracleDbType.Varchar2)
                        If String.IsNullOrWhiteSpace(entidad.Estado) Then
                            pEstado.Value = "ACTIVO"
                        Else
                            pEstado.Value = entidad.Estado
                        End If
                        cmd.Parameters.Add(pEstado)

                        Dim pId As New OracleParameter("P_CONSUMO_ID", OracleDbType.Int32)
                        pId.Direction = ParameterDirection.Output
                        cmd.Parameters.Add(pId)

                        cmd.ExecuteNonQuery()
                        transaccion.Commit()

                        Return Convert.ToInt32(pId.Value.ToString())
                    End Using
                End Using
            End Using
        End Function

        Public Function Actualizar(ByVal entidad As ConsumoMateriaPrima) As Boolean
            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using transaccion As OracleTransaction = conexion.BeginTransaction()
                    Using cmd As New OracleCommand("PKG_CONSUMO_MATERIA_PRIMA.SP_ACTUALIZAR_CONSUMO_MATERIA_PRIMA", conexion)
                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Transaction = transaccion

                        cmd.Parameters.Add("P_CONSUMO_ID", OracleDbType.Int32).Value = entidad.ConsumoId
                        cmd.Parameters.Add("P_ORDEN_PRODUCCION_ID", OracleDbType.Int32).Value = entidad.OrdenProduccionId
                        cmd.Parameters.Add("P_MP_ID", OracleDbType.Int32).Value = entidad.MpId
                        cmd.Parameters.Add("P_CANTIDAD", OracleDbType.Decimal).Value = entidad.Cantidad
                        cmd.Parameters.Add("P_CONSUMO_AT", OracleDbType.TimeStamp).Value = entidad.ConsumoAt

                        Dim pEstado As New OracleParameter("P_ESTADO", OracleDbType.Varchar2)
                        If String.IsNullOrWhiteSpace(entidad.Estado) Then
                            pEstado.Value = "ACTIVO"
                        Else
                            pEstado.Value = entidad.Estado
                        End If
                        cmd.Parameters.Add(pEstado)

                        cmd.ExecuteNonQuery()
                        transaccion.Commit()

                        Return True
                    End Using
                End Using
            End Using
        End Function

        Public Function Eliminar(ByVal consumoId As Integer) As Boolean
            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using transaccion As OracleTransaction = conexion.BeginTransaction()
                    Using cmd As New OracleCommand("PKG_CONSUMO_MATERIA_PRIMA.SP_ELIMINAR_CONSUMO_MATERIA_PRIMA", conexion)
                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Transaction = transaccion

                        cmd.Parameters.Add("P_CONSUMO_ID", OracleDbType.Int32).Value = consumoId

                        cmd.ExecuteNonQuery()
                        transaccion.Commit()

                        Return True
                    End Using
                End Using
            End Using
        End Function

        Public Function ObtenerPorId(ByVal consumoId As Integer) As ConsumoMateriaPrima
            Dim entidad As ConsumoMateriaPrima = Nothing

            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using cmd As New OracleCommand("PKG_CONSUMO_MATERIA_PRIMA.SP_OBTENER_CONSUMO_MATERIA_PRIMA", conexion)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_CONSUMO_ID", OracleDbType.Int32).Value = consumoId
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

        Public Function Listar() As List(Of ConsumoMateriaPrima)
            Dim lista As New List(Of ConsumoMateriaPrima)()

            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using cmd As New OracleCommand("PKG_CONSUMO_MATERIA_PRIMA.SP_LISTAR_CONSUMO_MATERIA_PRIMA", conexion)
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

        Public Function Buscar(ByVal criterio As String, ByVal valor As String) As List(Of ConsumoMateriaPrima)
            Dim lista As New List(Of ConsumoMateriaPrima)()

            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using cmd As New OracleCommand("PKG_CONSUMO_MATERIA_PRIMA.SP_BUSCAR_CONSUMO_MATERIA_PRIMA", conexion)
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

        Private Function Mapear(ByVal dr As OracleDataReader) As ConsumoMateriaPrima
            Dim entidad As New ConsumoMateriaPrima()

            entidad.ConsumoId = Convert.ToInt32(dr("CONSUMO_ID"))
            entidad.OrdenProduccionId = Convert.ToInt32(dr("ORDEN_PRODUCCION_ID"))
            entidad.MpId = Convert.ToInt32(dr("MP_ID"))
            entidad.Cantidad = Convert.ToDecimal(dr("CANTIDAD"))
            entidad.ConsumoAt = Convert.ToDateTime(dr("CONSUMO_AT"))

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