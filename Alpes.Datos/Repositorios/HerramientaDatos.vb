Option Strict On
Option Explicit On

Imports System.Collections.Generic
Imports System.Data
Imports Oracle.ManagedDataAccess.Client
Imports Alpes.Datos.Conexion
Imports Alpes.Entidades.Produccion

Namespace Repositorios
    Public Class HerramientaDatos

        Private ReadOnly _conexionOracle As ConexionOracle

        Public Sub New()
            _conexionOracle = New ConexionOracle()
        End Sub

        Public Function Insertar(ByVal entidad As Herramienta) As Integer
            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using transaccion As OracleTransaction = conexion.BeginTransaction()
                    Using cmd As New OracleCommand("PKG_HERRAMIENTA.SP_INSERTAR", conexion)
                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Transaction = transaccion

                        cmd.Parameters.Add("P_CODIGO", OracleDbType.Varchar2).Value = entidad.Codigo
                        cmd.Parameters.Add("P_NOMBRE", OracleDbType.Varchar2).Value = entidad.Nombre

                        Dim pDescripcion As New OracleParameter("P_DESCRIPCION", OracleDbType.Varchar2)
                        If String.IsNullOrWhiteSpace(entidad.Descripcion) Then
                            pDescripcion.Value = DBNull.Value
                        Else
                            pDescripcion.Value = entidad.Descripcion
                        End If
                        cmd.Parameters.Add(pDescripcion)

                        Dim pFechaCompra As New OracleParameter("P_FECHA_COMPRA", OracleDbType.Date)
                        If entidad.FechaCompra.HasValue Then
                            pFechaCompra.Value = entidad.FechaCompra.Value
                        Else
                            pFechaCompra.Value = DBNull.Value
                        End If
                        cmd.Parameters.Add(pFechaCompra)

                        Dim pEstado As New OracleParameter("P_ESTADO", OracleDbType.Varchar2)
                        If String.IsNullOrWhiteSpace(entidad.Estado) Then
                            pEstado.Value = DBNull.Value
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

        Public Function Actualizar(ByVal entidad As Herramienta) As Boolean
            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using transaccion As OracleTransaction = conexion.BeginTransaction()
                    Using cmd As New OracleCommand("PKG_HERRAMIENTA.SP_ACTUALIZAR", conexion)
                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Transaction = transaccion

                        cmd.Parameters.Add("P_HERRAMIENTA_ID", OracleDbType.Int32).Value = entidad.HerramientaId
                        cmd.Parameters.Add("P_CODIGO", OracleDbType.Varchar2).Value = entidad.Codigo
                        cmd.Parameters.Add("P_NOMBRE", OracleDbType.Varchar2).Value = entidad.Nombre

                        Dim pDescripcion As New OracleParameter("P_DESCRIPCION", OracleDbType.Varchar2)
                        If String.IsNullOrWhiteSpace(entidad.Descripcion) Then
                            pDescripcion.Value = DBNull.Value
                        Else
                            pDescripcion.Value = entidad.Descripcion
                        End If
                        cmd.Parameters.Add(pDescripcion)

                        Dim pFechaCompra As New OracleParameter("P_FECHA_COMPRA", OracleDbType.Date)
                        If entidad.FechaCompra.HasValue Then
                            pFechaCompra.Value = entidad.FechaCompra.Value
                        Else
                            pFechaCompra.Value = DBNull.Value
                        End If
                        cmd.Parameters.Add(pFechaCompra)

                        Dim pEstado As New OracleParameter("P_ESTADO", OracleDbType.Varchar2)
                        If String.IsNullOrWhiteSpace(entidad.Estado) Then
                            pEstado.Value = DBNull.Value
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

        Public Function Eliminar(ByVal herramientaId As Integer) As Boolean
            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using transaccion As OracleTransaction = conexion.BeginTransaction()
                    Using cmd As New OracleCommand("PKG_HERRAMIENTA.SP_ELIMINAR", conexion)
                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Transaction = transaccion

                        cmd.Parameters.Add("P_ID", OracleDbType.Int32).Value = herramientaId

                        cmd.ExecuteNonQuery()
                        transaccion.Commit()

                        Return True
                    End Using
                End Using
            End Using
        End Function

        Public Function ObtenerPorId(ByVal herramientaId As Integer) As Herramienta
            Dim entidad As Herramienta = Nothing

            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using cmd As New OracleCommand("PKG_HERRAMIENTA.SP_OBTENER", conexion)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_ID", OracleDbType.Int32).Value = herramientaId
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

        Public Function Listar() As List(Of Herramienta)
            Dim lista As New List(Of Herramienta)()

            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using cmd As New OracleCommand("PKG_HERRAMIENTA.SP_LISTAR", conexion)
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

        Public Function Buscar(ByVal valor As String) As List(Of Herramienta)
            Dim lista As New List(Of Herramienta)()

            Using conexion As OracleConnection = _conexionOracle.ObtenerConexion()
                If conexion.State = ConnectionState.Closed Then
                    conexion.Open()
                End If

                Using cmd As New OracleCommand("PKG_HERRAMIENTA.SP_BUSCAR", conexion)
                    cmd.CommandType = CommandType.StoredProcedure

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

        Private Function Mapear(ByVal dr As OracleDataReader) As Herramienta
            Dim entidad As New Herramienta()

            entidad.HerramientaId = Convert.ToInt32(dr("HERRAMIENTA_ID"))
            entidad.Codigo = dr("CODIGO").ToString()
            entidad.Nombre = dr("NOMBRE").ToString()
            entidad.Descripcion = If(dr("DESCRIPCION") Is DBNull.Value, String.Empty, dr("DESCRIPCION").ToString())

            If dr("FECHA_COMPRA") Is DBNull.Value Then
                entidad.FechaCompra = Nothing
            Else
                entidad.FechaCompra = Convert.ToDateTime(dr("FECHA_COMPRA"))
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