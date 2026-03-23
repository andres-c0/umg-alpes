Imports Oracle.ManagedDataAccess.Client
Imports System.Data
Imports alpes.Entidades

Namespace Datos

    Public Class ProduccionResultadosDatos

        Private ReadOnly _conexion As ConexionOracle

        Public Sub New()
            _conexion = New ConexionOracle()
        End Sub

        Public Function Insertar(entidad As ProduccionResultados) As Integer

            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_PRODUCCION_RESULTADOS.SP_INSERTAR_PRODUCCION_RESULTADOS", conn)

                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_OP_ID", OracleDbType.Int32).Value = entidad.OpId
                    cmd.Parameters.Add("P_CANTIDAD_PRODUCIDA", OracleDbType.Decimal).Value = entidad.CantidadProducida
                    cmd.Parameters.Add("P_CANTIDAD_DEFECTUOSA", OracleDbType.Decimal).Value = entidad.CantidadDefectuosa
                    cmd.Parameters.Add("P_FECHA_RESULTADO", OracleDbType.Date).Value = entidad.FechaResultado
                    cmd.Parameters.Add("P_OBSERVACIONES", OracleDbType.Varchar2).Value =
                        If(String.IsNullOrWhiteSpace(entidad.Observaciones), DBNull.Value, entidad.Observaciones)

                    Dim pOut As New OracleParameter("P_PR_ID", OracleDbType.Int32)
                    pOut.Direction = ParameterDirection.Output
                    cmd.Parameters.Add(pOut)

                    conn.Open()
                    cmd.ExecuteNonQuery()

                    Return Convert.ToInt32(pOut.Value.ToString())

                End Using
            End Using

        End Function


        Public Sub Actualizar(entidad As ProduccionResultados)

            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_PRODUCCION_RESULTADOS.SP_ACTUALIZAR_PRODUCCION_RESULTADOS", conn)

                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_PR_ID", OracleDbType.Int32).Value = entidad.PrId
                    cmd.Parameters.Add("P_OP_ID", OracleDbType.Int32).Value = entidad.OpId
                    cmd.Parameters.Add("P_CANTIDAD_PRODUCIDA", OracleDbType.Decimal).Value = entidad.CantidadProducida
                    cmd.Parameters.Add("P_CANTIDAD_DEFECTUOSA", OracleDbType.Decimal).Value = entidad.CantidadDefectuosa
                    cmd.Parameters.Add("P_FECHA_RESULTADO", OracleDbType.Date).Value = entidad.FechaResultado
                    cmd.Parameters.Add("P_OBSERVACIONES", OracleDbType.Varchar2).Value =
                        If(String.IsNullOrWhiteSpace(entidad.Observaciones), DBNull.Value, entidad.Observaciones)

                    conn.Open()
                    cmd.ExecuteNonQuery()

                End Using
            End Using

        End Sub


        Public Sub Eliminar(prId As Integer)

            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_PRODUCCION_RESULTADOS.SP_ELIMINAR_PRODUCCION_RESULTADOS", conn)

                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_PR_ID", OracleDbType.Int32).Value = prId

                    conn.Open()
                    cmd.ExecuteNonQuery()

                End Using
            End Using

        End Sub


        Public Function Listar() As DataTable

            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_PRODUCCION_RESULTADOS.SP_LISTAR_PRODUCCION_RESULTADOS", conn)

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
                Using cmd As New OracleCommand("PKG_PRODUCCION_RESULTADOS.SP_BUSCAR_PRODUCCION_RESULTADOS", conn)

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


        Public Function ObtenerPorId(prId As Integer) As DataTable

            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_PRODUCCION_RESULTADOS.SP_OBTENER_PRODUCCION_RESULTADOS", conn)

                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_PR_ID", OracleDbType.Int32).Value = prId
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