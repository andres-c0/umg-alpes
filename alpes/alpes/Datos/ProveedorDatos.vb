Imports Oracle.ManagedDataAccess.Client
Imports System.Data
Imports alpes.Entidades

Namespace Datos
    Public Class ProveedorDatos
        Private ReadOnly _conexion As alpes.Datos.ConexionOracle

        Public Sub New()
            _conexion = New alpes.Datos.ConexionOracle()
        End Sub

        Public Function Insertar(proveedor As Proveedor) As Integer
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_PROVEEDOR.SP_INSERTAR_PROVEEDOR", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_RAZON_SOCIAL", OracleDbType.Varchar2).Value = proveedor.RazonSocial
                    cmd.Parameters.Add("P_NIT",          OracleDbType.Varchar2).Value = proveedor.Nit
                    cmd.Parameters.Add("P_EMAIL",        OracleDbType.Varchar2).Value = If(String.IsNullOrWhiteSpace(proveedor.Email),    DBNull.Value, proveedor.Email)
                    cmd.Parameters.Add("P_TELEFONO",     OracleDbType.Varchar2).Value = If(String.IsNullOrWhiteSpace(proveedor.Telefono), DBNull.Value, proveedor.Telefono)
                    cmd.Parameters.Add("P_DIRECCION",    OracleDbType.Varchar2).Value = If(String.IsNullOrWhiteSpace(proveedor.Direccion),DBNull.Value, proveedor.Direccion)
                    cmd.Parameters.Add("P_CIUDAD",       OracleDbType.Varchar2).Value = If(String.IsNullOrWhiteSpace(proveedor.Ciudad),   DBNull.Value, proveedor.Ciudad)
                    cmd.Parameters.Add("P_PAIS",         OracleDbType.Varchar2).Value = If(String.IsNullOrWhiteSpace(proveedor.Pais),     DBNull.Value, proveedor.Pais)
                    Dim pOut As New OracleParameter("P_PROV_ID", OracleDbType.Int32)
                    pOut.Direction = ParameterDirection.Output
                    cmd.Parameters.Add(pOut)
                    conn.Open()
                    cmd.ExecuteNonQuery()
                    Return Convert.ToInt32(pOut.Value.ToString())
                End Using
            End Using
        End Function

        Public Sub Actualizar(proveedor As Proveedor)
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_PROVEEDOR.SP_ACTUALIZAR_PROVEEDOR", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_PROV_ID",      OracleDbType.Int32).Value    = proveedor.ProvId
                    cmd.Parameters.Add("P_RAZON_SOCIAL", OracleDbType.Varchar2).Value = proveedor.RazonSocial
                    cmd.Parameters.Add("P_NIT",          OracleDbType.Varchar2).Value = proveedor.Nit
                    cmd.Parameters.Add("P_EMAIL",        OracleDbType.Varchar2).Value = If(String.IsNullOrWhiteSpace(proveedor.Email),    DBNull.Value, proveedor.Email)
                    cmd.Parameters.Add("P_TELEFONO",     OracleDbType.Varchar2).Value = If(String.IsNullOrWhiteSpace(proveedor.Telefono), DBNull.Value, proveedor.Telefono)
                    cmd.Parameters.Add("P_DIRECCION",    OracleDbType.Varchar2).Value = If(String.IsNullOrWhiteSpace(proveedor.Direccion),DBNull.Value, proveedor.Direccion)
                    cmd.Parameters.Add("P_CIUDAD",       OracleDbType.Varchar2).Value = If(String.IsNullOrWhiteSpace(proveedor.Ciudad),   DBNull.Value, proveedor.Ciudad)
                    cmd.Parameters.Add("P_PAIS",         OracleDbType.Varchar2).Value = If(String.IsNullOrWhiteSpace(proveedor.Pais),     DBNull.Value, proveedor.Pais)
                    conn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Sub Eliminar(provId As Integer)
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_PROVEEDOR.SP_ELIMINAR_PROVEEDOR", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("P_PROV_ID", OracleDbType.Int32).Value = provId
                    conn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Function Listar() As DataTable
            Using conn = _conexion.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_PROVEEDOR.SP_LISTAR_PROVEEDORES", conn)
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
                Using cmd As New OracleCommand("PKG_PROVEEDOR.SP_BUSCAR_PROVEEDORES", conn)
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
