Imports Oracle.ManagedDataAccess.Client
Imports System.Data
Imports alpes.Entidades

Namespace Datos

Public Class MetodoPagoDatos

    Private ReadOnly _conexion As ConexionOracle

    Public Sub New()
        _conexion = New ConexionOracle()
    End Sub



Public Function Insertar(entidad As MetodoPago) As Integer

    Using conn = _conexion.ObtenerConexion()

        Using cmd As New OracleCommand("PKG_METODO_PAGO.SP_INSERTAR", conn)

            cmd.CommandType = CommandType.StoredProcedure

            cmd.Parameters.Add("P_NOMBRE", OracleDbType.Varchar2).Value =
                If(entidad.Nombre Is Nothing, DBNull.Value, entidad.Nombre)

            cmd.Parameters.Add("P_DESCRIPCION", OracleDbType.Varchar2).Value =
                If(entidad.Descripcion Is Nothing, DBNull.Value, entidad.Descripcion)

            Dim pOut As New OracleParameter("P_ID", OracleDbType.Int32)
            pOut.Direction = ParameterDirection.Output
            cmd.Parameters.Add(pOut)

            conn.Open()
            cmd.ExecuteNonQuery()

            Return Convert.ToInt32(pOut.Value.ToString())

        End Using

    End Using

End Function



Public Sub Actualizar(entidad As MetodoPago)

    Using conn = _conexion.ObtenerConexion()

        Using cmd As New OracleCommand("PKG_METODO_PAGO.SP_ACTUALIZAR", conn)

            cmd.CommandType = CommandType.StoredProcedure

            cmd.Parameters.Add("P_ID", OracleDbType.Int32).Value = entidad.MetodoPagoId

            cmd.Parameters.Add("P_NOMBRE", OracleDbType.Varchar2).Value =
                If(entidad.Nombre Is Nothing, DBNull.Value, entidad.Nombre)

            cmd.Parameters.Add("P_DESCRIPCION", OracleDbType.Varchar2).Value =
                If(entidad.Descripcion Is Nothing, DBNull.Value, entidad.Descripcion)

            conn.Open()
            cmd.ExecuteNonQuery()

        End Using

    End Using

End Sub



Public Sub Eliminar(id As Integer)

    Using conn = _conexion.ObtenerConexion()

        Using cmd As New OracleCommand("PKG_METODO_PAGO.SP_ELIMINAR", conn)

            cmd.CommandType = CommandType.StoredProcedure

            cmd.Parameters.Add("P_ID", OracleDbType.Int32).Value = id

            conn.Open()
            cmd.ExecuteNonQuery()

        End Using

    End Using

End Sub



Public Function Listar() As DataTable

    Using conn = _conexion.ObtenerConexion()

        Using cmd As New OracleCommand("PKG_METODO_PAGO.SP_LISTAR", conn)

            cmd.CommandType = CommandType.StoredProcedure

            cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction =
                ParameterDirection.Output

            Using da As New OracleDataAdapter(cmd)

                Dim dt As New DataTable()
                da.Fill(dt)
                Return dt

            End Using

        End Using

    End Using

End Function



Public Function Buscar(nombre As String) As DataTable

    Using conn = _conexion.ObtenerConexion()

        Using cmd As New OracleCommand("PKG_METODO_PAGO.SP_BUSCAR", conn)

            cmd.CommandType = CommandType.StoredProcedure

            cmd.Parameters.Add("P_NOMBRE", OracleDbType.Varchar2).Value =
                If(nombre Is Nothing, DBNull.Value, nombre)

            cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction =
                ParameterDirection.Output

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