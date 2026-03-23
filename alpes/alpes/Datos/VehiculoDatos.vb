Imports Oracle.ManagedDataAccess.Client
Imports System.Data
Imports alpes.Entidades

Namespace Datos

    Public Class VehiculoDatos

        Private ReadOnly _conexion As ConexionOracle

        Public Sub New()
            _conexion = New ConexionOracle()
        End Sub

        Public Function Insertar(vehiculo As Vehiculo) As Integer
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_VEHICULO.SP_INSERTAR_VEHICULO", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_PLACA",        OracleDbType.Varchar2).Value = vehiculo.Placa
                    cmd.Parameters.Add("P_TIPO",         OracleDbType.Varchar2).Value = If(String.IsNullOrWhiteSpace(vehiculo.Tipo), DBNull.Value, vehiculo.Tipo)
                    cmd.Parameters.Add("P_CAPACIDAD_KG", OracleDbType.Decimal).Value  = If(vehiculo.CapacidadKg.HasValue, vehiculo.CapacidadKg.Value, DBNull.Value)
                    cmd.Parameters.Add("P_CAPACIDAD_M3", OracleDbType.Decimal).Value  = If(vehiculo.CapacidadM3.HasValue, vehiculo.CapacidadM3.Value, DBNull.Value)
                    cmd.Parameters.Add("P_ACTIVO",       OracleDbType.Int32).Value    = vehiculo.Activo

                    Dim pOut As New OracleParameter("P_VEHICULO_ID", OracleDbType.Int32)
                    pOut.Direction = ParameterDirection.Output
                    cmd.Parameters.Add(pOut)

                    conn.Open()
                    cmd.ExecuteNonQuery()
                    Return Convert.ToInt32(pOut.Value.ToString())
                End Using
            End Using
        End Function

        Public Sub Actualizar(vehiculo As Vehiculo)
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_VEHICULO.SP_ACTUALIZAR_VEHICULO", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_VEHICULO_ID",  OracleDbType.Int32).Value    = vehiculo.VehiculoId
                    cmd.Parameters.Add("P_PLACA",        OracleDbType.Varchar2).Value = vehiculo.Placa
                    cmd.Parameters.Add("P_TIPO",         OracleDbType.Varchar2).Value = If(String.IsNullOrWhiteSpace(vehiculo.Tipo), DBNull.Value, vehiculo.Tipo)
                    cmd.Parameters.Add("P_CAPACIDAD_KG", OracleDbType.Decimal).Value  = If(vehiculo.CapacidadKg.HasValue, vehiculo.CapacidadKg.Value, DBNull.Value)
                    cmd.Parameters.Add("P_CAPACIDAD_M3", OracleDbType.Decimal).Value  = If(vehiculo.CapacidadM3.HasValue, vehiculo.CapacidadM3.Value, DBNull.Value)
                    cmd.Parameters.Add("P_ACTIVO",       OracleDbType.Int32).Value    = vehiculo.Activo

                    conn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Sub Eliminar(vehiculoId As Integer)
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_VEHICULO.SP_ELIMINAR_VEHICULO", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_VEHICULO_ID", OracleDbType.Int32).Value = vehiculoId
                    conn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Function Listar() As DataTable
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_VEHICULO.SP_LISTAR_VEHICULOS", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output
                    Using da As New OracleDataAdapter(cmd)
                        Dim dt As New DataTable()
                        da.Fill(dt)
                        Return dt
                    End Using
                End Using
            End Using
        End Function

        Public Function Buscar(criterio As String, valor As String) As DataTable
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_VEHICULO.SP_BUSCAR_VEHICULOS", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_CRITERIO", OracleDbType.Varchar2).Value = criterio
                    cmd.Parameters.Add("P_VALOR",    OracleDbType.Varchar2).Value = valor
                    cmd.Parameters.Add("P_CURSOR",   OracleDbType.RefCursor).Direction = ParameterDirection.Output
                    Using da As New OracleDataAdapter(cmd)
                        Dim dt As New DataTable()
                        da.Fill(dt)
                        Return dt
                    End Using
                End Using
            End Using
        End Function

    End Class

End Namespace
