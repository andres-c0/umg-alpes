Imports System.Data
Imports alpes.Datos
Imports alpes.Entidades

Namespace Servicios
    Public Class ProveedorServicio

        Private ReadOnly _datos As ProveedorDatos

        Public Sub New()
            _datos = New ProveedorDatos()
        End Sub

        Public Function Insertar(proveedor As Proveedor) As Integer
            Return _datos.Insertar(proveedor)
        End Function

        Public Sub Actualizar(proveedor As Proveedor)
            _datos.Actualizar(proveedor)
        End Sub

        Public Sub Eliminar(provId As Integer)
            _datos.Eliminar(provId)
        End Sub

        Public Function Listar() As DataTable
            Return _datos.Listar()
        End Function

        Public Function Buscar(criterio As String, valor As String) As DataTable
            Return _datos.Buscar(criterio, valor)
        End Function

    End Class
End Namespace
