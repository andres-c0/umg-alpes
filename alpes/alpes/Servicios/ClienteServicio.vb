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

    End Class
End Namespace