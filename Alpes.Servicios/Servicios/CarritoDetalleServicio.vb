Option Strict On
Option Explicit On

Imports System.Collections.Generic
Imports Alpes.Datos.Repositorios
Imports Alpes.Entidades.Ventas

Namespace Servicios
    Public Class CarritoDetalleServicio

        Private ReadOnly _datos As CarritoDetalleDatos

        Public Sub New()
            _datos = New CarritoDetalleDatos()
        End Sub

        Public Function Insertar(ByVal entidad As CarritoDetalle) As Integer
            Return _datos.Insertar(entidad)
        End Function

        Public Function Actualizar(ByVal entidad As CarritoDetalle) As Boolean
            Return _datos.Actualizar(entidad)
        End Function

        Public Function Eliminar(ByVal carritoDetId As Integer) As Boolean
            Return _datos.Eliminar(carritoDetId)
        End Function

        Public Function ObtenerPorId(ByVal carritoDetId As Integer) As CarritoDetalle
            Return _datos.ObtenerPorId(carritoDetId)
        End Function

        Public Function Listar() As List(Of CarritoDetalle)
            Return _datos.Listar()
        End Function

        Public Function Buscar(ByVal criterio As String, ByVal valor As String) As List(Of CarritoDetalle)
            Return _datos.Buscar(criterio, valor)
        End Function

    End Class
End Namespace