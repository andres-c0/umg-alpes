Option Strict On
Option Explicit On

Imports System.Collections.Generic
Imports System.Data
Imports Oracle.ManagedDataAccess.Client
Imports Alpes.Datos.Conexion
Imports Alpes.Entidades.Produccion

Namespace Repositorios
    Public Class ListaMaterialesDetalleDatos

        Private ReadOnly _conexionOracle As ConexionOracle

        Public Sub New()
            _conexionOracle = New ConexionOracle()
        End Sub

        Public Function Insertar(ByVal entidad As ListaMaterialesDetalle) As Integer
            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using transaccion As OracleTransaction = conexion.BeginTransaction()
                    Using cmd As New OracleCommand("PKG_LISTA_MATERIALES_DETALLE.SP_INSERTAR", conexion)
                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Transaction = transaccion

                        cmd.Parameters.Add("P_LISTA_MATERIALES_ID", OracleDbType.Int32).Value = entidad.ListaMaterialesId
                        cmd.Parameters.Add("P_MP_ID", OracleDbType.Int32).Value = entidad.MpId
                        cmd.Parameters.Add("P_CANTIDAD_REQUERIDA", OracleDbType.Decimal).Value = entidad.CantidadRequerida

                        Dim pMermaPct As New OracleParameter("P_MERMA_PCT", OracleDbType.Decimal)
                        If entidad.MermaPct.HasValue Then
                            pMermaPct.Value = entidad.MermaPct.Value
                        Else
                            pMermaPct.Value = DBNull.Value
                        End If
                        cmd.Parameters.Add(pMermaPct)

                        Dim pEstado As New OracleParameter("P_ESTADO", OracleDbType.Varchar2)
                        If String.IsNullOrWhiteSpace(entidad.Estado) Then
                            pEstado.Value = "ACTIVO"
                        Else
                            pEstado.Value = entidad.Estado
                        End If
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

        Public Function Actualizar(ByVal entidad As ListaMaterialesDetalle) As Boolean
            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using transaccion As OracleTransaction = conexion.BeginTransaction()
                    Using cmd As New OracleCommand("PKG_LISTA_MATERIALES_DETALLE.SP_ACTUALIZAR", conexion)
                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Transaction = transaccion

                        cmd.Parameters.Add("P_LISTA_MATERIALES_DET_ID", OracleDbType.Int32).Value = entidad.ListaMaterialesDetId
                        cmd.Parameters.Add("P_LISTA_MATERIALES_ID", OracleDbType.Int32).Value = entidad.ListaMaterialesId
                        cmd.Parameters.Add("P_MP_ID", OracleDbType.Int32).Value = entidad.MpId
                        cmd.Parameters.Add("P_CANTIDAD_REQUERIDA", OracleDbType.Decimal).Value = entidad.CantidadRequerida

                        Dim pMermaPct As New OracleParameter("P_MERMA_PCT", OracleDbType.Decimal)
                        If entidad.MermaPct.HasValue Then
                            pMermaPct.Value = entidad.MermaPct.Value
                        Else
                            pMermaPct.Value = DBNull.Value
                        End If
                        cmd.Parameters.Add(pMermaPct)

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

        Public Function Eliminar(ByVal listaMaterialesDetId As Integer) As Boolean
            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using transaccion As OracleTransaction = conexion.BeginTransaction()
                    Using cmd As New OracleCommand("PKG_LISTA_MATERIALES_DETALLE.SP_ELIMINAR", conexion)
                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Transaction = transaccion

                        cmd.Parameters.Add("P_ID", OracleDbType.Int32).Value = listaMaterialesDetId

                        cmd.ExecuteNonQuery()
                        transaccion.Commit()

                        Return True
                    End Using
                End Using
            End Using
        End Function

        Public Function ObtenerPorId(ByVal listaMaterialesDetId As Integer) As ListaMaterialesDetalle
            Dim entidad As ListaMaterialesDetalle = Nothing

            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using cmd As New OracleCommand("PKG_LISTA_MATERIALES_DETALLE.SP_OBTENER", conexion)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_ID", OracleDbType.Int32).Value = listaMaterialesDetId
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

        Public Function Listar() As List(Of ListaMaterialesDetalle)
            Dim lista As New List(Of ListaMaterialesDetalle)()

            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using cmd As New OracleCommand("PKG_LISTA_MATERIALES_DETALLE.SP_LISTAR", conexion)
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

        Private Function Mapear(ByVal dr As OracleDataReader) As ListaMaterialesDetalle
            Dim entidad As New ListaMaterialesDetalle()

            entidad.ListaMaterialesDetId = Convert.ToInt32(dr("LISTA_MATERIALES_DET_ID"))
            entidad.ListaMaterialesId = Convert.ToInt32(dr("LISTA_MATERIALES_ID"))
            entidad.MpId = Convert.ToInt32(dr("MP_ID"))
            entidad.CantidadRequerida = Convert.ToDecimal(dr("CANTIDAD_REQUERIDA"))

            If dr("MERMA_PCT") Is DBNull.Value Then
                entidad.MermaPct = Nothing
            Else
                entidad.MermaPct = Convert.ToDecimal(dr("MERMA_PCT"))
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