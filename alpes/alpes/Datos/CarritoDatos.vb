Imports Oracle.ManagedDataAccess.Client
Imports System.Data
Imports alpes.Entidades

Namespace Datos

    Public Class CarritoDatos

        Private ReadOnly _conexion As alpes.Datos.ConexionOracle

        Public Sub New()
            _conexion = New alpes.Datos.ConexionOracle()
        End Sub

        Public Function Insertar(carrito As Carrito) As Integer
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_CARRITO.SP_INSERTAR_CARRITO", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_CLI_ID", OracleDbType.Int32).Value = carrito.CliId
                    cmd.Parameters.Add("P_ESTADO_CARRITO", OracleDbType.Varchar2).Value = carrito.EstadoCarrito
                    cmd.Parameters.Add("P_ULTIMO_CALCULO_AT", OracleDbType.TimeStamp).Value =
                        If(carrito.UltimoCalculoAt Is Nothing, DBNull.Value, carrito.UltimoCalculoAt)

                    Dim pOut As New OracleParameter("P_CARRITO_ID", OracleDbType.Int32)
                    pOut.Direction = ParameterDirection.Output
                    cmd.Parameters.Add(pOut)

                    conn.Open()
                    cmd.ExecuteNonQuery()
                    Return Convert.ToInt32(pOut.Value.ToString())
                End Using
            End Using
        End Function

        Public Sub Actualizar(carrito As Carrito)
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_CARRITO.SP_ACTUALIZAR_CARRITO", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_CARRITO_ID", OracleDbType.Int32).Value = carrito.CarritoId
                    cmd.Parameters.Add("P_CLI_ID", OracleDbType.Int32).Value = carrito.CliId
                    cmd.Parameters.Add("P_ESTADO_CARRITO", OracleDbType.Varchar2).Value = carrito.EstadoCarrito
                    cmd.Parameters.Add("P_ULTIMO_CALCULO_AT", OracleDbType.TimeStamp).Value =
                        If(carrito.UltimoCalculoAt Is Nothing, DBNull.Value, carrito.UltimoCalculoAt)

                    conn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Sub Eliminar(carritoId As Integer)
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_CARRITO.SP_ELIMINAR_CARRITO", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_CARRITO_ID", OracleDbType.Int32).Value = carritoId
                    conn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Function Listar() As DataTable
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_CARRITO.SP_LISTAR_CARRITO", conn)
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
                Using cmd As New OracleCommand("PKG_CARRITO.SP_BUSCAR_CARRITO", conn)
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

    End Class

End Namespace
