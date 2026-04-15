Option Strict On
Option Explicit On

Imports System.Collections.Generic
Imports Alpes.Datos.Repositorios
Imports Alpes.Entidades.Compras

Namespace Servicios
    Public Class PagoProveedorServicio

        Private ReadOnly _datos As PagoProveedorDatos

        Public Sub New()
            _datos = New PagoProveedorDatos()
        End Sub

        Public Function Insertar(ByVal entidad As PagoProveedor) As Integer
            Return _datos.Insertar(entidad)
        End Function

        Public Function Actualizar(ByVal entidad As PagoProveedor) As Boolean
            Return _datos.Actualizar(entidad)
        End Function

        Public Function Eliminar(ByVal pagoProveedorId As Integer) As Boolean
            Return _datos.Eliminar(pagoProveedorId)
        End Function

        Public Function ObtenerPorId(ByVal pagoProveedorId As Integer) As PagoProveedor
            Return _datos.ObtenerPorId(pagoProveedorId)
        End Function

        Public Function Listar() As List(Of PagoProveedor)
            Return _datos.Listar()
        End Function

        Public Function Buscar(ByVal criterio As String, ByVal valor As String) As List(Of PagoProveedor)
            Return _datos.Buscar(criterio, valor)
        End Function

    End Class
End Namespace