Option Strict On
Option Explicit On

Imports System.Collections.Generic
Imports Alpes.Datos.Repositorios
Imports Alpes.Entidades.Inventario

Namespace Servicios
    Public Class ProductoServicio

        Private ReadOnly _datos As ProductoDatos

        Public Sub New()
            _datos = New ProductoDatos()
        End Sub

        Public Function Insertar(ByVal entidad As Producto) As Integer
            Return _datos.Insertar(entidad)
        End Function

        Public Sub Actualizar(ByVal entidad As Producto)
            _datos.Actualizar(entidad)
        End Sub

        Public Sub Eliminar(ByVal id As Integer)
            _datos.Eliminar(id)
        End Sub

        Public Function ObtenerPorId(ByVal id As Integer) As Producto
            Return _datos.ObtenerPorId(id)
        End Function

        Public Function Listar() As List(Of Producto)
            Return _datos.Listar()
        End Function

        Public Function Buscar(ByVal criterio As String, ByVal valor As String) As List(Of Producto)
            Return _datos.Buscar(criterio, valor)
        End Function

    End Class
End Namespace