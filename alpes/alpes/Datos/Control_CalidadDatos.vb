Imports Oracle.ManagedDataAccess.Client
Imports System.Data
Imports alpes.Entidades

Namespace Datos

    Public Class ControlCalidadDatos

        Private ReadOnly _conexion As ConexionOracle

        Public Sub New()
            _conexion = New ConexionOracle()
        End Sub

        Public Function Insertar(entidad As ControlCalidad) As Integer

            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_CONTROL_CALIDAD.SP_INSERTAR_CONTROL_CALIDAD", conn)

                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_OP_ID", OracleDbType.Int32).Value = entidad.OpId
                    cmd.Parameters.Add("P_FECHA_CONTROL", OracleDbType.Date).Value = entidad.FechaControl
                    cmd.Parameters.Add("P_RESULTADO", OracleDbType.Varchar2).Value = entidad.Resultado
                    cmd.Parameters.Add("P_OBSERVACIONES", OracleDbType.Varchar2).Value =
                        If(String.IsNullOrWhiteSpace(entidad.Observaciones), DBNull.Value, entidad.Observaciones)
                    cmd.Parameters.Add("P_RESPONSABLE", OracleDbType.Varchar2).Value = entidad.Responsable

                    Dim pOut As New OracleParameter("P_CC_ID", OracleDbType.Int32)
                    pOut.Direction = ParameterDirection.Output
                    cmd.Parameters.Add(pOut)

                    conn.Open()
                    cmd.ExecuteNonQuery()

                    Return Convert.ToInt32(pOut.Value.ToString())

                End Using
            End Using

        End Function


        Public Sub Actualizar(entidad As ControlCalidad)

            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_CONTROL_CALIDAD.SP_ACTUALIZAR_CONTROL_CALIDAD", conn)

                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_CC_ID", OracleDbType.Int32).Value = entidad.CcId
                    cmd.Parameters.Add("P_OP_ID", OracleDbType.Int32).Value = entidad.OpId
                    cmd.Parameters.Add("P_FECHA_CONTROL", OracleDbType.Date).Value = entidad.FechaControl
                    cmd.Parameters.Add("P_RESULTADO", OracleDbType.Varchar2).Value = entidad.Resultado
                    cmd.Parameters.Add("P_OBSERVACIONES", OracleDbType.Varchar2).Value =
                        If(String.IsNullOrWhiteSpace(entidad.Observaciones), DBNull.Value, entidad.Observaciones)
                    cmd.Parameters.Add("P_RESPONSABLE", OracleDbType.Varchar2).Value = entidad.Responsable

                    conn.Open()
                    cmd.ExecuteNonQuery()

                End Using
            End Using

        End Sub


        Public Sub Eliminar(ccId As Integer)

            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_CONTROL_CALIDAD.SP_ELIMINAR_CONTROL_CALIDAD", conn)

                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_CC_ID", OracleDbType.Int32).Value = ccId

                    conn.Open()
                    cmd.ExecuteNonQuery()

                End Using
            End Using

        End Sub


        Public Function Listar() As DataTable

            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_CONTROL_CALIDAD.SP_LISTAR_CONTROLES_CALIDAD", conn)

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
                Using cmd As New OracleCommand("PKG_CONTROL_CALIDAD.SP_BUSCAR_CONTROLES_CALIDAD", conn)

                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_CRITERIO", OracleDbType.Varchar2).Value = criterio
                    cmd.Parameters.Add("P_VALOR", OracleDbType.Varchar2).Value = valor
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using da As New OracleDataAdapter(cmd)

                        Dim dt As New DataTable()
                        da.Fill(dt)

                        Return dt

                    End Using

                End Using
            End Using

        End Function


        Public Function ObtenerPorId(ccId As Integer) As DataTable

            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_CONTROL_CALIDAD.SP_OBTENER_CONTROL_CALIDAD", conn)

                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_CC_ID", OracleDbType.Int32).Value = ccId
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

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