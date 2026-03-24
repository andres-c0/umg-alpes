Imports Oracle.ManagedDataAccess.Client
Imports System.Data
Imports alpes.Entidades

Namespace Datos

    Public Class CargoDatos

        Private ReadOnly _conexion As alpes.Datos.ConexionOracle

        Public Sub New()
            _conexion = New alpes.Datos.ConexionOracle()
        End Sub

        Public Function Insertar(cargo As Cargo) As Integer

            Using conn = _conexion.ObtenerConexion()

                Using cmd As New OracleCommand("PKG_CARGO.SP_INSERTAR_CARGO", conn)

                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_NOMBRE", OracleDbType.Varchar2).Value = cargo.Nombre
                    cmd.Parameters.Add("P_DESCRIPCION", OracleDbType.Varchar2).Value = cargo.Descripcion

                    Dim pOut As New OracleParameter("P_CARGO_ID", OracleDbType.Int32)
                    pOut.Direction = ParameterDirection.Output
                    cmd.Parameters.Add(pOut)

                    conn.Open()
                    cmd.ExecuteNonQuery()

                    Return Convert.ToInt32(pOut.Value.ToString())

                End Using
            End Using

        End Function


        Public Sub Actualizar(cargo As Cargo)

            Using conn = _conexion.ObtenerConexion()

                Using cmd As New OracleCommand("PKG_CARGO.SP_ACTUALIZAR_CARGO", conn)

                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_CARGO_ID", OracleDbType.Int32).Value = cargo.CargoId
                    cmd.Parameters.Add("P_NOMBRE", OracleDbType.Varchar2).Value = cargo.Nombre
                    cmd.Parameters.Add("P_DESCRIPCION", OracleDbType.Varchar2).Value = cargo.Descripcion

                    conn.Open()
                    cmd.ExecuteNonQuery()

                End Using
            End Using

        End Sub


        Public Sub Eliminar(cargoId As Integer)

            Using conn = _conexion.ObtenerConexion()

                Using cmd As New OracleCommand("PKG_CARGO.SP_ELIMINAR_CARGO", conn)

                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_CARGO_ID", OracleDbType.Int32).Value = cargoId

                    conn.Open()
                    cmd.ExecuteNonQuery()

                End Using
            End Using

        End Sub


        Public Function Listar() As DataTable

            Using conn = _conexion.ObtenerConexion()

                Using cmd As New OracleCommand("PKG_CARGO.SP_LISTAR_CARGOS", conn)

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

    End Class

End Namespace