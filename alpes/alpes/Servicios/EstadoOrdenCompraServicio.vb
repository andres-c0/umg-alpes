Option Strict On
Option Explicit On

Imports System.Data

Public Class EstadoOrdenCompraServicio

    Private ReadOnly _datos As EstadoOrdenCompraDatos

    Public Sub New()
        _datos = New EstadoOrdenCompraDatos()
    End Sub

    Public Function Insertar(ByVal entidad As EstadoOrdenCompra) As Integer
        Return _datos.Insertar(entidad)
    End Function

    Public Sub Actualizar(ByVal entidad As EstadoOrdenCompra)
        _datos.Actualizar(entidad)
    End Sub

    Public Sub Eliminar(ByVal id As Integer)
        _datos.Eliminar(id)
    End Sub

    Public Function ObtenerPorId(ByVal id As Integer) As DataTable
        Return _datos.ObtenerPorId(id)
    End Function

    Public Function Listar() As DataTable
        Return _datos.Listar()
    End Function

    Public Function Buscar(ByVal criterio As String, ByVal valor As String) As DataTable
        Return _datos.Buscar(criterio, valor)
    End Function

End Class
