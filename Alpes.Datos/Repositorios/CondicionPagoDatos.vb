Option Strict On
Option Explicit On

Imports System.Collections.Generic
Imports System.Data
Imports Oracle.ManagedDataAccess.Client
Imports Alpes.Datos.Conexion
Imports Alpes.Entidades.Compras

Namespace Repositorios
    Public Class CondicionPagoDatos

        Private ReadOnly _conexionOracle As ConexionOracle

        Public Sub New()
            _conexionOracle = New ConexionOracle()
        End Sub

        Public Function Insertar(ByVal entidad As CondicionPago) As Integer
            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using transaccion As OracleTransaction = conexion.BeginTransaction()
                    Using cmd As New OracleCommand("PKG_CONDICION_PAGO.SP_INSERTAR_CONDICION_PAGO", conexion)
                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Transaction = transaccion

                        cmd.Parameters.Add("P_NOMBRE", OracleDbType.Varchar2).Value = entidad.Nombre
                        cmd.Parameters.Add("P_DIAS_CREDITO", OracleDbType.Int32).Value = entidad.DiasCredito

                        Dim pDescripcion As New OracleParameter("P_DESCRIPCION", OracleDbType.Varchar2)
                        If String.IsNullOrWhiteSpace(entidad.Descripcion) Then
                            pDescripcion.Value = DBNull.Value
                        Else
                            pDescripcion.Value = entidad.Descripcion
                        End If
                        cmd.Parameters.Add(pDescripcion)

                        Dim pId As New OracleParameter("P_CONDICION_PAGO_ID", OracleDbType.Int32)
                        pId.Direction = ParameterDirection.Output
                        cmd.Parameters.Add(pId)

                        cmd.ExecuteNonQuery()
                        transaccion.Commit()

                        Return Convert.ToInt32(pId.Value.ToString())
                    End Using
                End Using
            End Using
        End Function

        Public Function Actualizar(ByVal entidad As CondicionPago) As Boolean
            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using transaccion As OracleTransaction = conexion.BeginTransaction()
                    Using cmd As New OracleCommand("PKG_CONDICION_PAGO.SP_ACTUALIZAR_CONDICION_PAGO", conexion)
                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Transaction = transaccion

                        cmd.Parameters.Add("P_CONDICION_PAGO_ID", OracleDbType.Int32).Value = entidad.CondicionPagoId
                        cmd.Parameters.Add("P_NOMBRE", OracleDbType.Varchar2).Value = entidad.Nombre
                        cmd.Parameters.Add("P_DIAS_CREDITO", OracleDbType.Int32).Value = entidad.DiasCredito

                        Dim pDescripcion As New OracleParameter("P_DESCRIPCION", OracleDbType.Varchar2)
                        If String.IsNullOrWhiteSpace(entidad.Descripcion) Then
                            pDescripcion.Value = DBNull.Value
                        Else
                            pDescripcion.Value = entidad.Descripcion
                        End If
                        cmd.Parameters.Add(pDescripcion)

                        cmd.ExecuteNonQuery()
                        transaccion.Commit()

                        Return True
                    End Using
                End Using
            End Using
        End Function

        Public Function Eliminar(ByVal condicionPagoId As Integer) As Boolean
            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using transaccion As OracleTransaction = conexion.BeginTransaction()
                    Using cmd As New OracleCommand("PKG_CONDICION_PAGO.SP_ELIMINAR_CONDICION_PAGO", conexion)
                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Transaction = transaccion

                        cmd.Parameters.Add("P_CONDICION_PAGO_ID", OracleDbType.Int32).Value = condicionPagoId

                        cmd.ExecuteNonQuery()
                        transaccion.Commit()

                        Return True
                    End Using
                End Using
            End Using
        End Function

        Public Function ObtenerPorId(ByVal condicionPagoId As Integer) As CondicionPago
            Dim entidad As CondicionPago = Nothing

            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using cmd As New OracleCommand("PKG_CONDICION_PAGO.SP_OBTENER_CONDICION_PAGO", conexion)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_CONDICION_PAGO_ID", OracleDbType.Int32).Value = condicionPagoId
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

        Public Function Listar() As List(Of CondicionPago)
            Dim lista As New List(Of CondicionPago)()

            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using cmd As New OracleCommand("PKG_CONDICION_PAGO.SP_LISTAR_CONDICIONES_PAGO", conexion)
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

        Public Function Buscar(ByVal criterio As String, ByVal valor As String) As List(Of CondicionPago)
            Dim lista As New List(Of CondicionPago)()

            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using cmd As New OracleCommand("PKG_CONDICION_PAGO.SP_BUSCAR_CONDICIONES_PAGO", conexion)
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

        Private Function Mapear(ByVal dr As OracleDataReader) As CondicionPago
            Dim entidad As New CondicionPago()

            entidad.CondicionPagoId = Convert.ToInt32(dr("CONDICION_PAGO_ID"))
            entidad.Nombre = dr("NOMBRE").ToString()
            entidad.DiasCredito = Convert.ToInt32(dr("DIAS_CREDITO"))
            entidad.Descripcion = If(dr("DESCRIPCION") Is DBNull.Value, String.Empty, dr("DESCRIPCION").ToString())
            entidad.Estado = If(dr("ESTADO") Is DBNull.Value, String.Empty, dr("ESTADO").ToString())

            Return entidad
        End Function

    End Class
End Namespace