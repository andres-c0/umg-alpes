Imports Oracle.ManagedDataAccess.Client
Imports System.Data
Imports alpes.Entidades

Namespace Datos

    Public Class HistorialPrecioMPProveedorDatos

        Private ReadOnly _conexion As alpes.Datos.ConexionOracle

        Public Sub New()
            _conexion = New alpes.Datos.ConexionOracle()
        End Sub


        Public Function Insertar(hist As HistorialPrecioMateriaPrimaProveedor) As Integer

            Using conn = _conexion.ObtenerConexion()

                Using cmd As New OracleCommand("PKG_HISTORIAL_PRECIO_MP_PROV.SP_INSERTAR_HISTORIAL_PRECIO", conn)

                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("P_PROVEEDOR_ID", OracleDbType.Int32).Value = hist.ProveedorId
                    cmd.Parameters.Add("P_MATERIA_PRIMA_ID", OracleDbType.Int32).Value = hist.MateriaPrimaId
                    cmd.Parameters.Add("P_PRECIO", OracleDbType.Decimal).Value = hist.Precio
                    cmd.Parameters.Add("P_FECHA", OracleDbType.Date).Value = hist.Fecha

                    Dim pOut As New OracleParameter("P_HISTORIAL_ID", OracleDbType.Int32)
                    pOut.Direction = ParameterDirection.Output
                    cmd.Parameters.Add(pOut)

                    conn.Open()
                    cmd.ExecuteNonQuery()

                    Return Convert.ToInt32(pOut.Value.ToString())

                End Using
            End Using

        End Function

    End Class

End Namespace