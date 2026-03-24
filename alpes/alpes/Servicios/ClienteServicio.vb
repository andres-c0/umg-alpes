Imports System.Data
Imports alpes.Datos
Imports alpes.Entidades

Namespace Servicios
    Public Class ClienteServicio

        Private ReadOnly _datos As ClienteDatos

        Public Sub New()
            _datos = New ClienteDatos()
        End Sub

        Public Function Insertar(cliente As Cliente) As Integer
            Return _datos.Insertar(cliente)
        End Function

        Public Sub Actualizar(cliente As Cliente)
            _datos.Actualizar(cliente)
        End Sub

        Public Sub Eliminar(ByVal cliId As Integer)
            _datos.Eliminar(cliId)
        End Sub

        Public Function ObtenerPorId(ByVal cliId As Integer) As DataTable
            Return _datos.ObtenerPorId(cliId)
        End Function

        Public Function Listar() As DataTable
            Return _datos.Listar()
        End Function

        Public Function Buscar(ByVal criterio As String, ByVal valor As String) As DataTable
            Return _datos.Buscar(criterio, valor)
        End Function

    End Class
End Namespace
