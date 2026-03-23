Imports Oracle.ManagedDataAccess.Client
Imports System.Data
Imports alpes.Entidades

Namespace Datos
    Public Class HerramientaDatos

        Private ReadOnly _conexion As ConexionOracle

        Public Sub New()
            _conexion = New ConexionOracle()
        End Sub

        Public Function Insertar(entidad As Herramienta) As Integer
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_HERRAMIENTA.SP_INSERTAR_HERRAMIENTA", conn)

                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_CODIGO_HERRAMIENTA", OracleDbType.Varchar2).Value = entidad.CodigoHerramienta
                    cmd.Parameters.Add("P_NOMBRE", OracleDbType.Varchar2).Value = entidad.Nombre
                    cmd.Parameters.Add("P_DESCRIPCION", OracleDbType.Varchar2).Value =
                        If(String.IsNullOrWhiteSpace(entidad.Descripcion), DBNull.Value, entidad.Descripcion)
                    cmd.Parameters.Add("P_ESTADO", OracleDbType.Varchar2).Value = entidad.Estado

                    Dim pOut As New OracleParameter("P_HERR_ID", OracleDbType.Int32)
                    pOut.Direction = ParameterDirection.Output
                    cmd.Parameters.Add(pOut)

                    conn.Open()
                    cmd.ExecuteNonQuery()

                    Return Convert.ToInt32(pOut.Value.ToString())

                End Using
            End Using
        End Function


        Public Sub Actualizar(entidad As Herramienta)

            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_HERRAMIENTA.SP_ACTUALIZAR_HERRAMIENTA", conn)

                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_HERR_ID", OracleDbType.Int32).Value = entidad.HerrId
                    cmd.Parameters.Add("P_CODIGO_HERRAMIENTA", OracleDbType.Varchar2).Value = entidad.CodigoHerramienta
                    cmd.Parameters.Add("P_NOMBRE", OracleDbType.Varchar2).Value = entidad.Nombre
                    cmd.Parameters.Add("P_DESCRIPCION", OracleDbType.Varchar2).Value =
                        If(String.IsNullOrWhiteSpace(entidad.Descripcion), DBNull.Value, entidad.Descripcion)
                    cmd.Parameters.Add("P_ESTADO", OracleDbType.Varchar2).Value = entidad.Estado

                    conn.Open()
                    cmd.ExecuteNonQuery()

                End Using
            End Using

        End Sub


        Public Sub Eliminar(herrId As Integer)

            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_HERRAMIENTA.SP_ELIMINAR_HERRAMIENTA", conn)

                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_HERR_ID", OracleDbType.Int32).Value = herrId

                    conn.Open()
                    cmd.ExecuteNonQuery()

                End Using
            End Using

        End Sub


        Public Function Listar() As DataTable

            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_HERRAMIENTA.SP_LISTAR_HERRAMIENTAS", conn)

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
                Using cmd As New OracleCommand("PKG_HERRAMIENTA.SP_BUSCAR_HERRAMIENTAS", conn)

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


        Public Function ObtenerPorId(herrId As Integer) As DataTable

            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_HERRAMIENTA.SP_OBTENER_HERRAMIENTA", conn)

                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_HERR_ID", OracleDbType.Int32).Value = herrId
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