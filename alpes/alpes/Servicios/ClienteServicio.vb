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

        Public Function Listar() As DataTable
            Return _datos.Listar()
        End Function

        Public Function ObtenerPorId(id As Integer) As DataTable
            Return _datos.ObtenerPorId(id)
        End Function

        Public Sub Actualizar(cliente As Cliente)
            _datos.Actualizar(cliente)
        End Sub

        Public Sub Eliminar(id As Integer)
            _datos.Eliminar(id)
        End Sub

        Public Function Buscar(criterio As String, valor As String) As DataTable
            Return _datos.Buscar(criterio, valor)
        End Function

    End Class
End Namespace
