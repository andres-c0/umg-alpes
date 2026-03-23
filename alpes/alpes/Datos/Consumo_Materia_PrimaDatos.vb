Imports Oracle.ManagedDataAccess.Client
Imports System.Data
Imports alpes.Entidades

Namespace Datos

    Public Class ConsumoMateriaPrimaDatos

        Private ReadOnly _conexion As ConexionOracle

        Public Sub New()
            _conexion = New ConexionOracle()
        End Sub

        Public Function Insertar(entidad As ConsumoMateriaPrima) As Integer

            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_CONSUMO_MATERIA_PRIMA.SP_INSERTAR_CONSUMO_MATERIA_PRIMA", conn)

                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_OP_ID", OracleDbType.Int32).Value = entidad.OpId
                    cmd.Parameters.Add("P_MP_ID", OracleDbType.Int32).Value = entidad.MpId
                    cmd.Parameters.Add("P_CANTIDAD_CONSUMIDA", OracleDbType.Decimal).Value = entidad.CantidadConsumida
                    cmd.Parameters.Add("P_FECHA_CONSUMO", OracleDbType.Date).Value = entidad.FechaConsumo
                    cmd.Parameters.Add("P_USUARIO_REGISTRO", OracleDbType.Varchar2).Value = entidad.UsuarioRegistro

                    Dim pOut As New OracleParameter("P_CONS_MP_ID", OracleDbType.Int32)
                    pOut.Direction = ParameterDirection.Output

                    cmd.Parameters.Add(pOut)

                    conn.Open()
                    cmd.ExecuteNonQuery()

                    Return Convert.ToInt32(pOut.Value.ToString())

                End Using
            End Using

        End Function


        Public Sub Actualizar(entidad As ConsumoMateriaPrima)

            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_CONSUMO_MATERIA_PRIMA.SP_ACTUALIZAR_CONSUMO_MATERIA_PRIMA", conn)

                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_CONS_MP_ID", OracleDbType.Int32).Value = entidad.ConsMpId
                    cmd.Parameters.Add("P_OP_ID", OracleDbType.Int32).Value = entidad.OpId
                    cmd.Parameters.Add("P_MP_ID", OracleDbType.Int32).Value = entidad.MpId
                    cmd.Parameters.Add("P_CANTIDAD_CONSUMIDA", OracleDbType.Decimal).Value = entidad.CantidadConsumida
                    cmd.Parameters.Add("P_FECHA_CONSUMO", OracleDbType.Date).Value = entidad.FechaConsumo
                    cmd.Parameters.Add("P_USUARIO_REGISTRO", OracleDbType.Varchar2).Value = entidad.UsuarioRegistro

                    conn.Open()
                    cmd.ExecuteNonQuery()

                End Using
            End Using

        End Sub


        Public Sub Eliminar(consMpId As Integer)

            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_CONSUMO_MATERIA_PRIMA.SP_ELIMINAR_CONSUMO_MATERIA_PRIMA", conn)

                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_CONS_MP_ID", OracleDbType.Int32).Value = consMpId

                    conn.Open()
                    cmd.ExecuteNonQuery()

                End Using
            End Using

        End Sub


        Public Function Listar() As DataTable

            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_CONSUMO_MATERIA_PRIMA.SP_LISTAR_CONSUMOS_MATERIA_PRIMA", conn)

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
                Using cmd As New OracleCommand("PKG_CONSUMO_MATERIA_PRIMA.SP_BUSCAR_CONSUMOS_MATERIA_PRIMA", conn)

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


        Public Function ObtenerPorId(consMpId As Integer) As DataTable

            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_CONSUMO_MATERIA_PRIMA.SP_OBTENER_CONSUMO_MATERIA_PRIMA", conn)

                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_CONS_MP_ID", OracleDbType.Int32).Value = consMpId
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