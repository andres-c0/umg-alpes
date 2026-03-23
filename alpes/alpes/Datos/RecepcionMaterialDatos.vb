Imports Oracle.ManagedDataAccess.Client
Imports System.Data
Imports alpes.Entidades

Namespace Datos
    Public Class RecepcionMaterialDatos
        Private ReadOnly _conexion As ConexionOracle

        Public Sub New()
            _conexion = New ConexionOracle()
        End Sub

        Public Function Insertar(rm As RecepcionMaterial) As Integer
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_RECEPCION_MATERIAL.SP_INSERTAR_RECEPCION_MATERIAL", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_ORDEN_COMPRA_ID", OracleDbType.Int32).Value    = rm.OrdenCompraId
                    cmd.Parameters.Add("P_FECHA_RECEPCION", OracleDbType.Date).Value     = rm.FechaRecepcion
                    cmd.Parameters.Add("P_EMP_ID_RECIBE",   OracleDbType.Int32).Value    = If(rm.EmpIdRecibe = 0, DBNull.Value, rm.EmpIdRecibe)
                    cmd.Parameters.Add("P_OBSERVACIONES",   OracleDbType.Varchar2).Value = If(String.IsNullOrWhiteSpace(rm.Observaciones), DBNull.Value, rm.Observaciones)
                    Dim pOut As New OracleParameter("P_RECEPCION_MATERIAL_ID", OracleDbType.Int32)
                    pOut.Direction = ParameterDirection.Output
                    cmd.Parameters.Add(pOut)
                    conn.Open()
                    cmd.ExecuteNonQuery()
                    Return Convert.ToInt32(pOut.Value.ToString())
                End Using
            End Using
        End Function

        Public Sub Actualizar(rm As RecepcionMaterial)
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_RECEPCION_MATERIAL.SP_ACTUALIZAR_RECEPCION_MATERIAL", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_RECEPCION_MATERIAL_ID", OracleDbType.Int32).Value    = rm.RecepcionMaterialId
                    cmd.Parameters.Add("P_ORDEN_COMPRA_ID",       OracleDbType.Int32).Value    = rm.OrdenCompraId
                    cmd.Parameters.Add("P_FECHA_RECEPCION",       OracleDbType.Date).Value     = rm.FechaRecepcion
                    cmd.Parameters.Add("P_EMP_ID_RECIBE",         OracleDbType.Int32).Value    = If(rm.EmpIdRecibe = 0, DBNull.Value, rm.EmpIdRecibe)
                    cmd.Parameters.Add("P_OBSERVACIONES",         OracleDbType.Varchar2).Value = If(String.IsNullOrWhiteSpace(rm.Observaciones), DBNull.Value, rm.Observaciones)
                    conn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Sub Eliminar(recepcionMaterialId As Integer)
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_RECEPCION_MATERIAL.SP_ELIMINAR_RECEPCION_MATERIAL", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_RECEPCION_MATERIAL_ID", OracleDbType.Int32).Value = recepcionMaterialId
                    conn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Function Listar() As DataTable
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_RECEPCION_MATERIAL.SP_LISTAR_RECEPCIONES_MATERIAL", conn)
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
                Using cmd As New OracleCommand("PKG_RECEPCION_MATERIAL.SP_BUSCAR_RECEPCIONES_MATERIAL", conn)
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
