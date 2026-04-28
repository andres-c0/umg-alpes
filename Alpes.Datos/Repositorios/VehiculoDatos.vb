Option Strict On
Option Explicit On

Imports System
Imports System.Collections.Generic
Imports System.Data
Imports Oracle.ManagedDataAccess.Client
Imports Alpes.Datos.Conexion
Imports Alpes.Entidades.Logistica

Namespace Repositorios
    Public Class VehiculoDatos

        Private ReadOnly _conexionOracle As ConexionOracle

        Public Sub New()
            _conexionOracle = New ConexionOracle()
        End Sub

        Public Function Insertar(ByVal entidad As Vehiculo) As Integer
            Dim idGenerado As Integer = 0

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_VEHICULO.SP_INSERTAR_VEHICULO", cn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.BindByName = True

                    cmd.Parameters.Add("P_PLACA", OracleDbType.Varchar2).Value = entidad.Placa

                    If String.IsNullOrWhiteSpace(entidad.Tipo) Then
                        cmd.Parameters.Add("P_TIPO", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("P_TIPO", OracleDbType.Varchar2).Value = entidad.Tipo
                    End If

                    If entidad.CapacidadKg.HasValue Then
                        cmd.Parameters.Add("P_CAPACIDAD_KG", OracleDbType.Decimal).Value = entidad.CapacidadKg.Value
                    Else
                        cmd.Parameters.Add("P_CAPACIDAD_KG", OracleDbType.Decimal).Value = DBNull.Value
                    End If

                    If entidad.CapacidadM3.HasValue Then
                        cmd.Parameters.Add("P_CAPACIDAD_M3", OracleDbType.Decimal).Value = entidad.CapacidadM3.Value
                    Else
                        cmd.Parameters.Add("P_CAPACIDAD_M3", OracleDbType.Decimal).Value = DBNull.Value
                    End If

                    cmd.Parameters.Add("P_ACTIVO", OracleDbType.Int32).Value = entidad.Activo
                    cmd.Parameters.Add("P_VEHICULO_ID", OracleDbType.Int32).Direction = ParameterDirection.Output

                    cmd.ExecuteNonQuery()
                    idGenerado = Convert.ToInt32(cmd.Parameters("P_VEHICULO_ID").Value.ToString())
                End Using
            End Using

            Return idGenerado
        End Function

        Public Sub Actualizar(ByVal entidad As Vehiculo)
            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_VEHICULO.SP_ACTUALIZAR_VEHICULO", cn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.BindByName = True

                    cmd.Parameters.Add("P_VEHICULO_ID", OracleDbType.Int32).Value = entidad.VehiculoId
                    cmd.Parameters.Add("P_PLACA", OracleDbType.Varchar2).Value = entidad.Placa

                    If String.IsNullOrWhiteSpace(entidad.Tipo) Then
                        cmd.Parameters.Add("P_TIPO", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("P_TIPO", OracleDbType.Varchar2).Value = entidad.Tipo
                    End If

                    If entidad.CapacidadKg.HasValue Then
                        cmd.Parameters.Add("P_CAPACIDAD_KG", OracleDbType.Decimal).Value = entidad.CapacidadKg.Value
                    Else
                        cmd.Parameters.Add("P_CAPACIDAD_KG", OracleDbType.Decimal).Value = DBNull.Value
                    End If

                    If entidad.CapacidadM3.HasValue Then
                        cmd.Parameters.Add("P_CAPACIDAD_M3", OracleDbType.Decimal).Value = entidad.CapacidadM3.Value
                    Else
                        cmd.Parameters.Add("P_CAPACIDAD_M3", OracleDbType.Decimal).Value = DBNull.Value
                    End If

                    cmd.Parameters.Add("P_ACTIVO", OracleDbType.Int32).Value = entidad.Activo

                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Sub Eliminar(ByVal id As Integer)
            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_VEHICULO.SP_ELIMINAR_VEHICULO", cn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.BindByName = True

                    cmd.Parameters.Add("P_VEHICULO_ID", OracleDbType.Int32).Value = id
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Function ObtenerPorId(ByVal id As Integer) As Vehiculo
            Dim entidad As Vehiculo = Nothing

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_VEHICULO.SP_OBTENER_VEHICULO", cn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.BindByName = True

                    cmd.Parameters.Add("P_VEHICULO_ID", OracleDbType.Int32).Value = id
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        If dr.Read() Then
                            entidad = MapearVehiculo(dr)
                        End If
                    End Using
                End Using
            End Using

            Return entidad
        End Function

        Public Function Listar() As List(Of Vehiculo)
            Dim lista As New List(Of Vehiculo)()

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_VEHICULO.SP_LISTAR_VEHICULOS", cn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.BindByName = True

                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        While dr.Read()
                            lista.Add(MapearVehiculo(dr))
                        End While
                    End Using
                End Using
            End Using

            Return lista
        End Function

        Public Function Buscar(ByVal criterio As String, ByVal valor As String) As List(Of Vehiculo)
            Dim lista As New List(Of Vehiculo)()

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_VEHICULO.SP_BUSCAR_VEHICULOS", cn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.BindByName = True

                    cmd.Parameters.Add("P_CRITERIO", OracleDbType.Varchar2).Value = criterio
                    cmd.Parameters.Add("P_VALOR", OracleDbType.Varchar2).Value = valor
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        While dr.Read()
                            lista.Add(MapearVehiculo(dr))
                        End While
                    End Using
                End Using
            End Using

            Return lista
        End Function

        Private Function MapearVehiculo(ByVal dr As OracleDataReader) As Vehiculo
            Dim entidad As New Vehiculo()

            If Not IsDBNull(dr("VEHICULO_ID")) Then
                entidad.VehiculoId = Convert.ToInt32(dr("VEHICULO_ID"))
            End If

            entidad.Placa = If(IsDBNull(dr("PLACA")), String.Empty, dr("PLACA").ToString())
            entidad.Tipo = If(IsDBNull(dr("TIPO")), String.Empty, dr("TIPO").ToString())

            If Not IsDBNull(dr("CAPACIDAD_KG")) Then
                entidad.CapacidadKg = Convert.ToDecimal(dr("CAPACIDAD_KG"))
            Else
                entidad.CapacidadKg = Nothing
            End If

            If Not IsDBNull(dr("CAPACIDAD_M3")) Then
                entidad.CapacidadM3 = Convert.ToDecimal(dr("CAPACIDAD_M3"))
            Else
                entidad.CapacidadM3 = Nothing
            End If

            If Not IsDBNull(dr("ACTIVO")) Then
                entidad.Activo = Convert.ToInt32(dr("ACTIVO"))
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