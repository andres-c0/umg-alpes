Option Strict On
Option Explicit On

Imports System
Imports System.Collections.Generic
Imports System.Data
Imports Oracle.ManagedDataAccess.Client
Imports Alpes.Datos.Conexion
Imports Alpes.Entidades.Compras

Namespace Repositorios
    Public Class ProveedorDatos

        Private ReadOnly _conexionOracle As ConexionOracle

        Public Sub New()
            _conexionOracle = New ConexionOracle()
        End Sub

        Public Function Insertar(ByVal entidad As Proveedor) As Integer
            Dim idGenerado As Integer = 0

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_PROVEEDOR.SP_INSERTAR_PROVEEDOR", cn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.BindByName = True

                    cmd.Parameters.Add("P_RAZON_SOCIAL", OracleDbType.Varchar2).Value = entidad.RazonSocial
                    cmd.Parameters.Add("P_NIT", OracleDbType.Varchar2).Value = entidad.Nit

                    If String.IsNullOrWhiteSpace(entidad.Email) Then
                        cmd.Parameters.Add("P_EMAIL", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("P_EMAIL", OracleDbType.Varchar2).Value = entidad.Email
                    End If

                    If String.IsNullOrWhiteSpace(entidad.Telefono) Then
                        cmd.Parameters.Add("P_TELEFONO", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("P_TELEFONO", OracleDbType.Varchar2).Value = entidad.Telefono
                    End If

                    If String.IsNullOrWhiteSpace(entidad.Direccion) Then
                        cmd.Parameters.Add("P_DIRECCION", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("P_DIRECCION", OracleDbType.Varchar2).Value = entidad.Direccion
                    End If

                    If String.IsNullOrWhiteSpace(entidad.Ciudad) Then
                        cmd.Parameters.Add("P_CIUDAD", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("P_CIUDAD", OracleDbType.Varchar2).Value = entidad.Ciudad
                    End If

                    If String.IsNullOrWhiteSpace(entidad.Pais) Then
                        cmd.Parameters.Add("P_PAIS", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("P_PAIS", OracleDbType.Varchar2).Value = entidad.Pais
                    End If

                    cmd.Parameters.Add("P_PROV_ID", OracleDbType.Int32).Direction = ParameterDirection.Output

                    cmd.ExecuteNonQuery()
                    idGenerado = Convert.ToInt32(cmd.Parameters("P_PROV_ID").Value.ToString())
                End Using
            End Using

            Return idGenerado
        End Function

        Public Sub Actualizar(ByVal entidad As Proveedor)
            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_PROVEEDOR.SP_ACTUALIZAR_PROVEEDOR", cn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.BindByName = True

                    cmd.Parameters.Add("P_PROV_ID", OracleDbType.Int32).Value = entidad.ProvId
                    cmd.Parameters.Add("P_RAZON_SOCIAL", OracleDbType.Varchar2).Value = entidad.RazonSocial
                    cmd.Parameters.Add("P_NIT", OracleDbType.Varchar2).Value = entidad.Nit

                    If String.IsNullOrWhiteSpace(entidad.Email) Then
                        cmd.Parameters.Add("P_EMAIL", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("P_EMAIL", OracleDbType.Varchar2).Value = entidad.Email
                    End If

                    If String.IsNullOrWhiteSpace(entidad.Telefono) Then
                        cmd.Parameters.Add("P_TELEFONO", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("P_TELEFONO", OracleDbType.Varchar2).Value = entidad.Telefono
                    End If

                    If String.IsNullOrWhiteSpace(entidad.Direccion) Then
                        cmd.Parameters.Add("P_DIRECCION", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("P_DIRECCION", OracleDbType.Varchar2).Value = entidad.Direccion
                    End If

                    If String.IsNullOrWhiteSpace(entidad.Ciudad) Then
                        cmd.Parameters.Add("P_CIUDAD", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("P_CIUDAD", OracleDbType.Varchar2).Value = entidad.Ciudad
                    End If

                    If String.IsNullOrWhiteSpace(entidad.Pais) Then
                        cmd.Parameters.Add("P_PAIS", OracleDbType.Varchar2).Value = DBNull.Value
                    Else
                        cmd.Parameters.Add("P_PAIS", OracleDbType.Varchar2).Value = entidad.Pais
                    End If

                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Sub Eliminar(ByVal id As Integer)
            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_PROVEEDOR.SP_ELIMINAR_PROVEEDOR", cn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.BindByName = True

                    cmd.Parameters.Add("P_PROV_ID", OracleDbType.Int32).Value = id
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        Public Function ObtenerPorId(ByVal id As Integer) As Proveedor
            Dim entidad As Proveedor = Nothing

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_PROVEEDOR.SP_OBTENER_PROVEEDOR", cn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.BindByName = True

                    cmd.Parameters.Add("P_PROV_ID", OracleDbType.Int32).Value = id
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        If dr.Read() Then
                            entidad = MapearProveedor(dr)
                        End If
                    End Using
                End Using
            End Using

            Return entidad
        End Function

        Public Function Listar() As List(Of Proveedor)
            Dim lista As New List(Of Proveedor)()

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_PROVEEDOR.SP_LISTAR_PROVEEDORES", cn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.BindByName = True

                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        While dr.Read()
                            lista.Add(MapearProveedor(dr))
                        End While
                    End Using
                End Using
            End Using

            Return lista
        End Function

        Public Function Buscar(ByVal criterio As String, ByVal valor As String) As List(Of Proveedor)
            Dim lista As New List(Of Proveedor)()

            Using cn As OracleConnection = _conexionOracle.ObtenerConexion()
                Using cmd As New OracleCommand("PKG_PROVEEDOR.SP_BUSCAR_PROVEEDORES", cn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.BindByName = True

                    cmd.Parameters.Add("P_CRITERIO", OracleDbType.Varchar2).Value = criterio
                    cmd.Parameters.Add("P_VALOR", OracleDbType.Varchar2).Value = valor
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output

                    Using dr As OracleDataReader = cmd.ExecuteReader()
                        While dr.Read()
                            lista.Add(MapearProveedor(dr))
                        End While
                    End Using
                End Using
            End Using

            Return lista
        End Function

        Private Function MapearProveedor(ByVal dr As OracleDataReader) As Proveedor
            Dim entidad As New Proveedor()

            If TieneColumna(dr, "PROV_ID") AndAlso Not IsDBNull(dr("PROV_ID")) Then
                entidad.ProvId = Convert.ToInt32(dr("PROV_ID"))
            End If

            entidad.RazonSocial = If(TieneColumna(dr, "RAZON_SOCIAL") AndAlso Not IsDBNull(dr("RAZON_SOCIAL")), dr("RAZON_SOCIAL").ToString(), String.Empty)
            entidad.Nit = If(TieneColumna(dr, "NIT") AndAlso Not IsDBNull(dr("NIT")), dr("NIT").ToString(), String.Empty)
            entidad.Email = If(TieneColumna(dr, "EMAIL") AndAlso Not IsDBNull(dr("EMAIL")), dr("EMAIL").ToString(), String.Empty)
            entidad.Telefono = If(TieneColumna(dr, "TELEFONO") AndAlso Not IsDBNull(dr("TELEFONO")), dr("TELEFONO").ToString(), String.Empty)
            entidad.Direccion = If(TieneColumna(dr, "DIRECCION") AndAlso Not IsDBNull(dr("DIRECCION")), dr("DIRECCION").ToString(), String.Empty)
            entidad.Ciudad = If(TieneColumna(dr, "CIUDAD") AndAlso Not IsDBNull(dr("CIUDAD")), dr("CIUDAD").ToString(), String.Empty)
            entidad.Pais = If(TieneColumna(dr, "PAIS") AndAlso Not IsDBNull(dr("PAIS")), dr("PAIS").ToString(), String.Empty)
            entidad.Estado = If(TieneColumna(dr, "ESTADO") AndAlso Not IsDBNull(dr("ESTADO")), dr("ESTADO").ToString(), String.Empty)

            If TieneColumna(dr, "CREATED_AT") AndAlso Not IsDBNull(dr("CREATED_AT")) Then
                entidad.CreatedAt = Convert.ToDateTime(dr("CREATED_AT"))
            Else
                entidad.CreatedAt = Nothing
            End If

            If TieneColumna(dr, "UPDATED_AT") AndAlso Not IsDBNull(dr("UPDATED_AT")) Then
                entidad.UpdatedAt = Convert.ToDateTime(dr("UPDATED_AT"))
            Else
                entidad.UpdatedAt = Nothing
            End If

            Return entidad
        End Function

        Private Function TieneColumna(ByVal dr As IDataRecord, ByVal nombreColumna As String) As Boolean
            For i As Integer = 0 To dr.FieldCount - 1
                If String.Equals(dr.GetName(i), nombreColumna, StringComparison.OrdinalIgnoreCase) Then
                    Return True
                End If
            Next

            Return False
        End Function

    End Class
End Namespace