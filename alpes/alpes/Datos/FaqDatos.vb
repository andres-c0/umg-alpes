Imports Oracle.ManagedDataAccess.Client
Imports System.Data
Imports alpes.Entidades

Namespace Datos

    Public Class FaqDatos

        Private ReadOnly _conexion As alpes.Datos.ConexionOracle

        Public Sub New()
            _conexion = New alpes.Datos.ConexionOracle()
        End Sub

        Public Function Insertar(faq As Faq) As Integer
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_FAQ.SP_INSERTAR_FAQ", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_PREGUNTA", OracleDbType.Varchar2).Value = faq.Pregunta
                    cmd.Parameters.Add("P_RESPUESTA", OracleDbType.Varchar2).Value = faq.Respuesta
                    cmd.Parameters.Add("P_ORDEN", OracleDbType.Int32).Value = faq.Orden

                    Dim pOut As New OracleParameter("P_FAQ_ID", OracleDbType.Int32)
                    pOut.Direction = ParameterDirection.Output
                    cmd.Parameters.Add(pOut)

                    conn.Open()
                    cmd.ExecuteNonQuery()
                    Return Convert.ToInt32(pOut.Value.ToString())
                End Using
            End Using
        End Function

        Public Sub Actualizar(faq As Faq)
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_FAQ.SP_ACTUALIZAR_FAQ", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_FAQ_ID", OracleDbType.Int32).Value = faq.FaqId
                    cmd.Parameters.Add("P_PREGUNTA", OracleDbType.Varchar2).Value = faq.Pregunta
                    cmd.Parameters.Add("P_RESPUESTA", OracleDbType.Varchar2).Value = faq.Respuesta
                    cmd.Parameters.Add("P_ORDEN", OracleDbType.Int32).Value = faq.Orden

                    conn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Sub Eliminar(faqId As Integer)
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_FAQ.SP_ELIMINAR_FAQ", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_FAQ_ID", OracleDbType.Int32).Value = faqId
                    conn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Function Listar() As DataTable
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_FAQ.SP_LISTAR_FAQ", conn)
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
                Using cmd As New OracleCommand("PKG_FAQ.SP_BUSCAR_FAQ", conn)
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
