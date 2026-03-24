Imports Oracle.ManagedDataAccess.Client
Imports System.Data
Imports alpes.Entidades

Namespace Datos

    Public Class DepartamentoDatos

        Private ReadOnly _conexion As alpes.Datos.ConexionOracle

        Public Sub New()
            _conexion = New alpes.Datos.ConexionOracle()
        End Sub


        Public Function Insertar(departamento As Departamento) As Integer

            Using conn = _conexion.ObtenerConexion()

                Using cmd As New OracleCommand("PKG_DEPARTAMENTO.SP_INSERTAR_DEPARTAMENTO", conn)

                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_NOMBRE", OracleDbType.Varchar2).Value = departamento.Nombre
                    cmd.Parameters.Add("P_DESCRIPCION", OracleDbType.Varchar2).Value = departamento.Descripcion

                    Dim pOut As New OracleParameter("P_DEPTO_ID", OracleDbType.Int32)
                    pOut.Direction = ParameterDirection.Output
                    cmd.Parameters.Add(pOut)

                    conn.Open()
                    cmd.ExecuteNonQuery()

                    Return Convert.ToInt32(pOut.Value.ToString())

                End Using
            End Using

        End Function


        Public Sub Actualizar(departamento As Departamento)

            Using conn = _conexion.ObtenerConexion()

                Using cmd As New OracleCommand("PKG_DEPARTAMENTO.SP_ACTUALIZAR_DEPARTAMENTO", conn)

                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_DEPTO_ID", OracleDbType.Int32).Value = departamento.DeptoId
                    cmd.Parameters.Add("P_NOMBRE", OracleDbType.Varchar2).Value = departamento.Nombre
                    cmd.Parameters.Add("P_DESCRIPCION", OracleDbType.Varchar2).Value = departamento.Descripcion

                    conn.Open()
                    cmd.ExecuteNonQuery()

                End Using
            End Using

        End Sub


        Public Sub Eliminar(deptoId As Integer)

            Using conn = _conexion.ObtenerConexion()

                Using cmd As New OracleCommand("PKG_DEPARTAMENTO.SP_ELIMINAR_DEPARTAMENTO", conn)

                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_DEPTO_ID", OracleDbType.Int32).Value = deptoId

                    conn.Open()
                    cmd.ExecuteNonQuery()

                End Using
            End Using

        End Sub


        Public Function Listar() As DataTable

            Using conn = _conexion.ObtenerConexion()

                Using cmd As New OracleCommand("PKG_DEPARTAMENTO.SP_LISTAR_DEPARTAMENTOS", conn)

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